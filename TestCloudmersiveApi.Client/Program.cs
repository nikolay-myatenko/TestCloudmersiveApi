using Cloudmersive.APIClient.NETCore.VirusScan.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VirusScanning.Services.Cloudmersive;
using VirusScanning.Services.Models;

namespace TestCloudmersiveApi.Client
{
    internal class Program
    {
        private const string API_KEY = "***API_KEY***"; // Cloudmersive API Key
        private const string SCAN_FILES_ROOT_PATH = @"D:\Desktop\"; // Path to the folder to scan

        static void Main(string[] args)
        {
            Console.WriteLine("File Scanner Started");
            Console.WriteLine(Environment.NewLine);

            Scenario_1();

            Console.WriteLine("File Scanner Finished");
            Console.ReadLine();
        }

        /// <summary>
        ///     Read all files from a folder and subfolders and scan them for viruses through Cloudmersive API client.
        /// </summary>
        static void Scenario_1()
        {
            var virusScanningService = new VirusScanningService(API_KEY);
            var filesToScan = Directory.GetFiles(SCAN_FILES_ROOT_PATH, "*", SearchOption.AllDirectories);
            var measurements = new Dictionary<string, TimeSpan>();
            var counter = 0;
            var counterPassed = 0;
            var counterFailed = 0;
            long totalSizeScanned = 0;

            Console.WriteLine($"Scanning files for viruses from the folder: {SCAN_FILES_ROOT_PATH}");
            Console.WriteLine($"Files to scan: {filesToScan.Count()}");
            Console.WriteLine(Environment.NewLine);

            foreach (var file in filesToScan)
            {
                var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                var fileInfo = new FileInfo(file);
                var stopwatch = new Stopwatch();

                stopwatch.Start();
                var (scanResult, virusScanResult) = virusScanningService.ScanFile(fileStream);
                stopwatch.Stop();

                Console.WriteLine(
                    $"{counter++} => File \"{fileInfo.Name}\" (Size: {string.Format("{0:0.##}", FileSizeInKB(fileInfo.Length))} KB) => " +
                    $"The scan took {stopwatch.Elapsed.TotalMilliseconds} ms." +
                    $"{GetScanResultInfo(scanResult, virusScanResult)}");

                if (scanResult.Status == ScanStatus.Passed) counterPassed++;
                else counterFailed++;

                if (!measurements.ContainsKey(fileInfo.Name)) measurements.Add(fileInfo.Name, stopwatch.Elapsed);

                totalSizeScanned += fileInfo.Length;
            }

            Console.WriteLine("--- Scanning Measurements ---");
            Console.WriteLine($"Files scanned: {counter}");
            Console.WriteLine($"Total size scanned: {string.Format("{0:0.##}", FileSizeInMB(totalSizeScanned))} MB");
            Console.WriteLine($"Passed: {counterPassed}");
            Console.WriteLine($"Failed: {counterFailed}");
            Console.WriteLine($"Average scan time: {measurements.Values.ToList().Average(i => i.TotalMilliseconds)} ms");
            Console.WriteLine($"Max scan time: {measurements.Max(i => i.Value.TotalMilliseconds)} ms ({measurements.Aggregate((l, r) => l.Value > r.Value ? l : r).Key})");
            Console.WriteLine($"Min scan time: {measurements.Min(i => i.Value.TotalMilliseconds)} ms ({measurements.Aggregate((l, r) => l.Value < r.Value ? l : r).Key})");
            Console.WriteLine(Environment.NewLine);
        }

        static string GetScanResultInfo(ScanResult scanResult, VirusScanResult virusScanResult)
        {
            if (scanResult.Status == ScanStatus.Failed)
                return
                    $"{Environment.NewLine}" +
                    $"{scanResult.Message}" +
                    $"{Environment.NewLine}";

            return
                $"{Environment.NewLine}" +
                $"Result: {virusScanResult.CleanResult}{Environment.NewLine}" +
                $"{GetScanResultIssues(virusScanResult)}";
        }

        static string GetScanResultIssues(VirusScanResult virusScanResult)
            => virusScanResult?.CleanResult ?? false
                ? string.Empty
                : $"{string.Join(",", virusScanResult.FoundViruses.Select(i => new { Issue = i.FileName + " " + i.VirusName }).ToList())}{Environment.NewLine}";

        static double FileSizeInKB(double fileSizeInBytes) => fileSizeInBytes / 1024;

        static double FileSizeInMB(double fileSizeInBytes) => fileSizeInBytes / (1024 * 1024);
    }
}
