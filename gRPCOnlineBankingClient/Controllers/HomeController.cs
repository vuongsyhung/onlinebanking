using Grpc.Net.Client;
using gRPCOnlineBankingClient.Models;
using gRPCOnlineBankingClient.Protos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace gRPCOnlineBankingClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Customer(int customerId=10) 
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7128");
            //
            var client = new OnlineBanking.OnlineBankingClient(channel);
            var reply = await client.ReadCustomerAsync(new ReadCustomerRequest { CustomerId = customerId });
            Customer customer = new()
            { 
                CustomerId = reply.CustomerId,
                FirstName = reply.FirstName,
                LastName = reply.LastName,
                DateOfBirth = DateTime.Parse(reply.DateOfBirth),
                Email = reply.Email,
                StreesAddress = reply.StreesAddress,
                City = reply.City,
                State = reply.State,
                ZipCode = reply.ZipCode,
                Country = reply.Country,
                Sex = reply.Sex
            };
            //
            return View(customer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
