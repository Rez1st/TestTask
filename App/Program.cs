using System;
using System.Configuration;
using System.IO;
using App.Services;

namespace App
{
    /// <summary>
    /// As this one seems to be a small app, I didn't use a 3 layer, injection, tests, etc.
    /// Application has 2 modes: manual and auto
    /// Manual means you will execute program from the command line with arguments
    /// Auto means you will paste all file that needs to be processed in one folder
    /// All parameters you can specify in a config file
    /// </summary>
    internal class Program
    {
        private const string PathToFilesVariable = "PathToFiles";
        private const string ShouldReadFilesFromFolder = "ReadFilesFromFolder";

        private static readonly string FilesFolder;
        private static readonly TagFinderService TagFinderService = new TagFinderService();
        // By default - application is in manual mode
        private static readonly bool IsInManualMode = true;

        /// <summary>
        /// Validation before program can Go
        /// </summary>
        static Program()
        {
            Log.Info($"Reading configuration");

            var readFromFolder = ConfigurationManager.AppSettings.Get(ShouldReadFilesFromFolder);

            if (string.IsNullOrEmpty(readFromFolder))
            {
                return;
            }

            if (bool.TryParse(readFromFolder, out var shouldReadFromFolder))
            {
                if (!shouldReadFromFolder)
                    return;

                IsInManualMode = false;
            }

            var filesFolder = ConfigurationManager.AppSettings.Get(PathToFilesVariable);

            if (string.IsNullOrEmpty(filesFolder))
            {
                Log.Warning(PathToFilesVariable + " variable is missing in config file");

            }

            FilesFolder = AppDomain.CurrentDomain.BaseDirectory + filesFolder;

            if (!Directory.Exists(FilesFolder))
            {
                Log.Warning($"Directory {FilesFolder} doesn't exists");
                Log.Warning("Application set to get path to files from user input");
                IsInManualMode = true;
            }
        }

        private static void Main(string[] args)
        {
            if (IsInManualMode)
                ReadFromArgs(args);
            else
                ReadFromFolder();

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static void ReadFromArgs(string[] args)
        {
            Log.Info("Application is in manual mode");

            if (args.Length < 1)
            {
                Log.Error("Arguments are not provided. Closing app.");
                return;
            }

            if (args.Length > 1)
                Log.Error("Too many arguments provided. Can accept only one");

            var filePath = args[0];

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Log.Error("File not exist, provide absolute path to file in format: app.exe C:\\Temp\\fileName.html");
                return;
            }

            if (!Path.GetExtension(filePath).Equals(".html"))
            {
                Log.Error("File extenstion should be html");
                return;
            }

            var responseString = TagFinderService.Locate(filePath);
            OutputResult(responseString, filePath);
        }

        private static void ReadFromFolder()
        {
            Log.Info("Application is in automated mode");

            string[] filePaths = Directory.GetFiles(FilesFolder, "*.html");

            if (filePaths.Length < 1)
            {
                Log.Info("There are no files with .html extenstion");
            }
            else
            {
                Log.Info($"Found {filePaths.Length} .html files for analysis");

                foreach (var filePath in filePaths)
                {
                    var responseString = TagFinderService.Locate(filePath);
                    OutputResult(responseString, filePath);
                }
            }
        }

        private static void OutputResult(string responseString, string filePath)
        {
            Log.Info("Application output:");

            if (string.IsNullOrEmpty(responseString))
                Log.Warning($"Tag not found in file {filePath}");
            else
            {
                Log.Success($"Tag found in file {filePath}");
                Log.Info($"\n\t {responseString}");
            }
        }
    }
}