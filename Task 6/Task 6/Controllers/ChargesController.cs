using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Task_6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task_6.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChargesController : ControllerBase
    {    
        string customerID = "cus_HSJgT9ML7AcfO9";

        // GET: api/<ChargesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("listcharges")]
        public IActionResult GetCharges()
        {
            StripeConfiguration.ApiKey = "";
            List<CustomerCharge> chargeList = new List<CustomerCharge>();
            var options = new ChargeListOptions
            {
                Limit = 100,
                Customer = customerID
            };
            var service = new ChargeService();
            StripeList<Charge> charges = service.List(options);
            foreach (Charge charge in charges)
            {
                CustomerCharge custCharge = new CustomerCharge();
                custCharge.Amount = charge.Amount;
                custCharge.AmountRefunded = charge.AmountRefunded;
                custCharge.Description = charge.Description;
                custCharge.Id = charge.Id;
                custCharge.ReceiptUrl = charge.ReceiptUrl;
                custCharge.Status = charge.Status;
                custCharge.Refunded = charge.Refunded;
                custCharge.Created = charge.Created;
                chargeList.Add(custCharge);
            }
            Thread.Sleep(2500);
            return Ok(chargeList);
        }

        [HttpPost("refund")]
        public IActionResult Refund([FromForm] string charge_id)
        {
            StripeConfiguration.ApiKey = "";
            var options = new RefundCreateOptions
            {
                Charge = charge_id,
            };
            var service = new RefundService();
            try
            {
                Refund refund = service.Create(options);
                Thread.Sleep(2500);

                return Ok();
            } catch(Exception e)
            {
                Thread.Sleep(2500);
                return BadRequest(e.Message);
            }
            
        }

        // PUT api/<ChargesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChargesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
