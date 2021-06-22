using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Photos;
using Photos.AnalyzerService;
using Photos.AnalyzerService.Abstractions;

[assembly: FunctionsStartup(typeof(StartUp))]

namespace Photos
{
    public class StartUp: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var computerVisionAnalyzerSettings = new ComputerVisionAnalyzerSettings()
            {
                VisionEndpoint = Environment.GetEnvironmentVariable("VisionEndpoint"),
                VisionKey = Environment.GetEnvironmentVariable("VisionKey")
            };

            builder.Services.AddSingleton(computerVisionAnalyzerSettings);
            builder.Services.AddSingleton<IAnalyzerService, ComputerVisionAnalyzerService>();
        }
    }
}