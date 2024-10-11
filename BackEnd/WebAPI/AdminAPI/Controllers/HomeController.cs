using Admin.Application.Interfaces;
using Admin.Application.Services;
using Base.Common.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Admin.API.Controllers
{
    [Route("api/v1/test")]
    [ApiVersion("1")]
    public class HomeController : Controller
    {
        private readonly IRabbitMQService _rabbitMQService;
        public HomeController(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpGet]
        [Route("abc")]
        [ApiVersion("1")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<ApiResponse> Index()
        {
            _rabbitMQService.SendMessage("myQueue", "Hello RabbitMQ!");

            // Nhận thông điệp
            _rabbitMQService.ReceiveMessage("myQueue", message =>
            {
                Console.WriteLine($"Received: {message}");
            });

            ApiResponse response;
            return null;

        }
    }
}
