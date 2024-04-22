using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TestBinding
{
    public static class Function1
    {
        [FunctionName("Function1")]
        [return: Table("Products", Connection = "MyAzureStorage")]
        public static async Task<Product> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody=await new StreamReader(req.Body).ReadToEndAsync();
            Product newProduct=JsonConvert.DeserializeObject<Product>(requestBody);
            return newProduct;
        }

        [FunctionName("Function2")]
        [return: Queue("testqueue", Connection = "MyAzureStorage")]
        public static async Task<string> Run2(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
           
            return requestBody;
        }
    }
}
