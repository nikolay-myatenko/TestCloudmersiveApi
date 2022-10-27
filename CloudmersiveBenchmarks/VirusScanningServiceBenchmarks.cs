using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.IO;
using VirusScanning.Services.Cloudmersive;

namespace CloudmersiveBenchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.SlowestToFastest)]
    public class VirusScanningServiceBenchmarks
    {
        private const string API_KEY = "***API_KEY***"; // Cloudmersive API Key
        private const string FILE_PATH = @"***FILE_PATH***"; // File to scan
        private static readonly Stream _fileInput = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read);
        private static readonly VirusScanningService _virusScanningService = new VirusScanningService(API_KEY);

        [Benchmark]
        public void ScanFile()
        {
            _ = _virusScanningService.ScanFile(_fileInput);
        }
    }
}
