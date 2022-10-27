using BenchmarkDotNet.Running;
using CloudmersiveBenchmarks;
using System;

namespace BenchmarksRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benchmarks Runner Started");

            BenchmarkRunner.Run<VirusScanningServiceBenchmarks>();

            Console.ReadLine();
        }
    }
}
