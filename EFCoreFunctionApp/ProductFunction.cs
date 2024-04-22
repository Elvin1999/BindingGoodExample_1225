using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EFCoreFunctionApp.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCoreFunctionApp
{
    public class ProductFunction
    {
        private const string Route = "Products";
        private readonly AppDbContext _context;

        public ProductFunction(AppDbContext context)
        {
            _context = context;
        }

        [FunctionName("GetProducts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Route)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("Get All Products called");
            var products = await _context.Products.ToListAsync();
            return new OkObjectResult(products);
        }

        [FunctionName("SaveProducts")]
        public async Task<IActionResult> SaveProducts(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Route)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("SaveProduct called");
            string requestBody=await new StreamReader(req.Body).ReadToEndAsync();

            var newProduct=JsonConvert.DeserializeObject<Product>(requestBody);

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return new OkObjectResult(newProduct);

        }
    }
}
