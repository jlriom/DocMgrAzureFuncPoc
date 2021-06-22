using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Photos.AnalyzerService.Abstractions;

namespace Photos
{
    public class PhotosAnalyzer
    {
        private readonly IAnalyzerService _analyzerService;

        public PhotosAnalyzer(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        public async Task<dynamic> Run([ActivityTrigger] List<byte> image)
        {
            return await _analyzerService.AnalyzeAsync(image.ToArray());
        }
    }
}
