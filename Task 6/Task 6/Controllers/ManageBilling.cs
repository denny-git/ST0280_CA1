using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task_6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageBilling : ControllerBase
    {
        // POST api/<PortalController>
        [HttpPost("createsession")]
        public IActionResult Post()
        {
            StripeConfiguration.ApiKey = "";

            var options = new SessionCreateOptions
            {
                Customer = "cus_HSJgT9ML7AcfO9",
                ReturnUrl = "https://localhost:44351/home/subscriptions",
            };
            var service = new SessionService();
            try
            {
                Session s = service.Create(options);
                Thread.Sleep(2500);
                return Ok(new { url = s.Url });
            } catch (Exception ex)
            {
                Thread.Sleep(2500);
                return BadRequest(new { message = ex.Message });
            }
            
        }
    }
}
