using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Photos.AnalyzerService.Abstractions;

namespace Photos.AnalyzerService
{
    public class ComputerVisionAnalyzerService: IAnalyzerService
    {
        private readonly ComputerVisionClient client;

        public ComputerVisionAnalyzerService(
            ComputerVisionAnalyzerSettings computerVisionAnalyzerSettings
            )
        {
            var visionKey = computerVisionAnalyzerSettings.VisionKey;
            var visionEndpoint = computerVisionAnalyzerSettings.VisionEndpoint;
            client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(visionKey))
            {
                Endpoint = visionEndpoint
            };
        }

        public async Task<dynamic> AnalyzeAsync(byte[] image)
        {
            try
            {
                await using var ms = new MemoryStream(image);
                var imageAnalysis = await client.AnalyzeImageInStreamAsync(ms);
                var result = new
                {
                    metadata = new
                    {
                        width = imageAnalysis.Metadata.Width,
                        height = imageAnalysis.Metadata.Height,
                        format = imageAnalysis.Metadata.Format
                    },
                    categories = imageAnalysis.Categories.Select(c => c.Name).ToArray()
                };
                return result;
            }
            catch (Exception ex)
            {
                // here 
                throw;
            }
        }
    }
}
