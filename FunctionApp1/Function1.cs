using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;
using System.Drawing;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("GeneratePdf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get parameter
            string name = req.Query["name"];
            Console.WriteLine(name);
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            
            if (string.IsNullOrEmpty(name))
                return new BadRequestObjectResult("Please pass a name on the query"
                        + " string or in the request body");
            else
            {
               
                // Create Pdf Document
                var pdf = new GcPdfDocument();
                pdf.NewPage().Graphics.DrawString($"Hi there {requestBody}!", new TextFormat(), new PointF(72, 72));
               
                pdf.Save("new.pdf");
                // Save PDF
                var ms = new MemoryStream();
                pdf.Save(ms);
                //pdf.Save(name+".pdf");
                ms.Seek(0, SeekOrigin.Begin);
                // Return file
                FileStreamResult result = new FileStreamResult(ms, "application/pdf");
                pdf.NewPage();
                
             
               return result;

            }
        }
    }
}
