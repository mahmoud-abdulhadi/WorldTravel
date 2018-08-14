using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorldTravel.Models;
using WorldTravel.services;
using WorldTravel.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldTravel.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repo;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService,IConfigurationRoot config,
            IWorldRepository repo,
            ILogger<AppController> logger)
        {

            _mailService = mailService;


            _config = config;

            _repo = repo;

            _logger = logger; 

        }

        // GET: /<controller>/
        public IActionResult Index()
        {

            


                return View();

        }
        [Authorize]
        public IActionResult Trips()
        {

            

            return View(); 

        }

        public IActionResult Contact()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com"))
                ModelState.AddModelError("Email", "We Don't Support AOL addresses"); 

            if (ModelState.IsValid)
                {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "Contact Support", model.Message);

                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent"; 
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
