using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using App.Infrastructure;
using App.Models;
using HtmlAgilityPack;

namespace App.Services
{
    public class TagFinderService : ITagFinderService
    {
        private const string TagFinderConfigSection = "TagFinderOptionsGroup/MakeButtonFinderOptions";
        private const string TagTypeConfigSection = "TagFinderOptionsGroup/MakeButtonTagContainer";
        private const string CustomOptionsConfigSection = "TagFinderCustomOptions/";

        private static readonly string[] CustomOptions = { "BootstrapCustomOption" };
        private static ICollection<CustomRule> _customRules;
        private static IDictionary<string, string> _selectorRules;
        private static string _tagType;

        public TagFinderService()
        {
            _tagType = (ConfigurationManager.GetSection(TagTypeConfigSection) as NameValueCollection)?["tag"];

            if (string.IsNullOrEmpty(_tagType))
                throw new ConfigurationErrorsException("Tag type is not defined");

            var tagFinderOptions = ConfigurationManager.GetSection(TagFinderConfigSection) as NameValueCollection;

            if (tagFinderOptions == null || tagFinderOptions.Count < 1)
            {
                throw new InvalidOperationException("Failed to read configuration section");
            }

            _selectorRules = new Dictionary<string, string>();

            foreach (string optionKey in tagFinderOptions.AllKeys)
            {
                _selectorRules.Add(optionKey, tagFinderOptions[optionKey]);
            }

            _customRules = new List<CustomRule>();

            foreach (var customOption in CustomOptions)
            {
                var section = ConfigurationManager.GetSection(CustomOptionsConfigSection + customOption) as NameValueCollection;

                _customRules.Add(new CustomRule(section));
            }
        }

        public string Locate(string filePath)
        {
            HtmlNode foundNode = null;
            var foundNodeCounter = 0;

            var doc = new HtmlDocument();
            doc.Load(filePath);

            var nodes = doc.DocumentNode.Descendants(_tagType);

            foreach (var node in nodes)
            {
                // if there are no attributes - skip
                if (!node.HasAttributes)
                    continue;

                var counter = 0;
                var skipNode = false;

                foreach (var rule in _selectorRules)
                {
                    var nodeAttribute = node.Attributes.FirstOrDefault(a => a.Name.Equals(rule.Key));

                    if (nodeAttribute == null)
                        continue;

                    if (!CustomRulePassed(nodeAttribute.Value, nodeAttribute.Name))
                        skipNode = true;

                    if (nodeAttribute.Value.Contains(rule.Value))
                        counter++;
                }

                if (!skipNode && counter > 0 && counter > foundNodeCounter)
                {
                    foundNodeCounter = counter;
                    foundNode = node;
                }
            }

            if (foundNode == null)
                return string.Empty;

            var nodeAttributes = string.Join(", ", foundNode.Attributes.Select(attribute => $"{attribute.Name} = {attribute.Value}"));
            // Used xpath for output string, as it is used not for navigation
            var nodeXPath = foundNode.XPath;

            return $"Attributes {nodeAttributes}\n\t XPath {nodeXPath}";
        }

        private bool CustomRulePassed(string attributeValue, string attributeName)
        {
            bool isValid = true;

            foreach (var customRule in _customRules)
            {
                if (!customRule.IsValid(attributeName, attributeValue))
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}