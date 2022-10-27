using Cloudmersive.APIClient.NETCore.VirusScan.Api;
using Cloudmersive.APIClient.NETCore.VirusScan.Client;
using Cloudmersive.APIClient.NETCore.VirusScan.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using VirusScanning.Services.Models;

namespace VirusScanning.Services.Cloudmersive
{
    public class VirusScanningService
    {
        private ScanApi _scanApi;

        public VirusScanningService(string apiKey)
        {
            Configuration.Default.AddApiKey("Apikey", apiKey);
            _scanApi = new ScanApi();
        }

        public (ScanResult, VirusScanResult) ScanFile(Stream fileInput)
        {
            try
            {
                var result = _scanApi.ScanFile(fileInput);

                return (new ScanResult { Status = ScanStatus.Passed }, result);
            }
            catch (Exception e)
            {
                return (new ScanResult { Status = ScanStatus.Failed, Message = e.Message }, null);
            }
        }

        public Task<VirusScanResult> ScanFileAsync(Stream fileInput)
        {
            throw new NotImplementedException();
        }

        public VirusScanAdvancedResult ScanFileAdvanced(Stream fileInput)
        {
            throw new NotImplementedException();
        }

        public Task<VirusScanAdvancedResult> ScanFileAdvancedAsync(Stream fileInput)
        {
            throw new NotImplementedException();
        }

        public Task<WebsiteScanResult> ScanWebSiteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CloudStorageVirusScanResult> ScanCloudStorageAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CloudStorageAdvancedVirusScanResult> ScanCloudStorageAdvancedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
