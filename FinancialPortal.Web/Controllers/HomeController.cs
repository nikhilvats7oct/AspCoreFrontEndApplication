﻿using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IContactLinksService _contactLinks;

        public HomeController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IContactLinksService contactLinks)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _contactLinks = contactLinks;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)

            {
                return RedirectToAction("Index", "MyAccounts");
            }

            // Only used for the load balance ping.
            return View();
        }


        /// <summary>
        /// Caseflow provides link to this action via email.
        /// </summary>
        /// <param name="id">Guid provided generated by caseflow</param>
        /// <returns>Redirects user to login screen</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("Login/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            if (!Guid.TryParse(id, out _))
            {
                return View("Views/Errors/NotFound.cshtml");
            }

            await _contactLinks.Update(id);

            return RedirectToAction("Index", "MyAccounts");
        }
    }
}