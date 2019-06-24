using System;
using System.Collections.Specialized;
using System.Linq;

namespace App.Models
{
    public class CustomRule
    {
        private const string AttributeSetting = "attribute";
        private const string PatternSetting = "pattern";
        private const string ValidatorSetting = "validator";

        public CustomRule(NameValueCollection configCollection)
        {
            Attribute = configCollection[configCollection.AllKeys.FirstOrDefault(k => k.Equals(AttributeSetting))];
            Pattern = configCollection[configCollection.AllKeys.FirstOrDefault(k => k.Equals(PatternSetting))];
            Validator = configCollection[configCollection.AllKeys.FirstOrDefault(k => k.Equals(ValidatorSetting))];

            var isPersistent = true;

            if (string.IsNullOrEmpty(Attribute))
            {
                Log.Error($"{AttributeSetting} setting in custom rules config section is missing");
                isPersistent = false;
            }

            if (string.IsNullOrEmpty(Pattern))
            {
                Log.Error($"{PatternSetting} setting in custom rules config section is missing");
                isPersistent = false;
            }

            if (string.IsNullOrEmpty(Validator))
            {
                Log.Error($"{ValidatorSetting} setting in custom rules config section is missing");
                isPersistent = false;
            }

            if (!isPersistent)
                throw new InvalidOperationException("Custom rules object is not parsed from app.config file");
        }

        public string Attribute { get; }

        public string Pattern { get; }

        public string Validator { get; }

        public bool IsValid(string attributeName, string attributeValue)
        {
            // Check if attribute is under custom rules
            // if not return
            if (!attributeName.Equals(Attribute))
                return true;

            // Check if attribute is match pattern
            if (!attributeValue.Contains(Pattern))
                return true;

            // Check if value is one we are looking for
            return attributeValue.Contains(Validator);
        }
    }
}
