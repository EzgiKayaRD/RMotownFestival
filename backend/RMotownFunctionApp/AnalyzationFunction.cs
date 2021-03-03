using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RMotownFunctionApp
{
    class AnalyzationFunction
    {
        public ComputerVisionClient VisionClient { get; set; }

        private static readonly List<VisualFeatureTypes?> Features =
            new List<VisualFeatureTypes?> { VisualFeatureTypes.Adult };

        public AnalyzationFunction(ComputerVisionClient visionClient)
        {
            VisionClient = visionClient;
        }

        [FunctionName("AnalyzationFunction")]
        public async Task Run([BlobTrigger("picsin/{name}", Connection = "BlobStorageConnection")] byte[] myBlob,
            string name, ILogger log, Binder binder)
        {
            log.LogInformation($"C# Blob trigger AnalyzationFunction Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            ImageAnalysis analysis = await VisionClient.AnalyzeImageInStreamAsync(new MemoryStream(myBlob), Features);
            Attribute[] attributes;
            if(analysis.Adult.IsAdultContent || analysis.Adult.IsGoryContent || analysis.Adult.IsRacyContent)
            {
                log.LogInformation($"Image {name} was detected as adult content");
                log.LogInformation($"Adult content = {analysis.Adult.IsAdultContent}\n" +
                    $"Gory content = {analysis.Adult.IsGoryContent}\n" +
                    $"Racy content = {analysis.Adult.IsRacyContent}");

                attributes = new Attribute[]
                {
                    new BlobAttribute($"picsrejected/{name}", FileAccess.Write),
                    new StorageAccountAttribute("BlobStorageConnection")
                };
            }
            else
            {
                log.LogInformation($"Image {name} is clean");
                attributes = new Attribute[]
                {
                    new BlobAttribute($"festivalpics/{name}", FileAccess.Write),
                    new StorageAccountAttribute("BlobStorageConnection")
                };
            }

            using Stream fileOutputStream = await binder.BindAsync<Stream>(attributes);
            fileOutputStream.Write(myBlob);
        }
    }
}
