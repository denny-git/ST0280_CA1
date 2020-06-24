using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Task_6.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task_6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        string customerID = "cus_HSJgT9ML7AcfO9";
        // GET api/<ValuesController>/
        [HttpGet("plans")]
        public IActionResult GetPlans()
        {
            StripeConfiguration.ApiKey = "";
            List<StoreProduct> storeProductList = new List<StoreProduct>();
            var options = new PriceListOptions { Limit = 100 };
            var priceSrv = new PriceService();
            StripeList<Price> prices = priceSrv.List(options);

            foreach (Price price in prices)
            {
                StoreProduct item = new StoreProduct();
                item.Price = (long)price.UnitAmount;
                item.ProductId = price.ProductId;
                item.PriceId = price.Id;
                var productSrv = new ProductService();

                Product currentProduct = productSrv.Get(price.ProductId);
                item.Name = currentProduct.Name;
                item.Description = currentProduct.Description;
                storeProductList.Add(item);
            }
            Thread.Sleep(2500);
            return Ok(storeProductList);
        }

        [HttpGet("active")]
        public IActionResult ViewActiveSubscriptions()
        {
            StripeConfiguration.ApiKey = "";
            var options = new SubscriptionListOptions
            {
                Customer = customerID,
                Status = "active"
            };
            List<CustomerSubscription> items = new List<CustomerSubscription>();
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = service.List(options);
            foreach (Subscription sub in subscriptions)
            {
                CustomerSubscription s = new CustomerSubscription();

                s.CreatedAt = sub.Created;
                s.PeriodStart = sub.CurrentPeriodStart;
                s.PeriodEnd = sub.CurrentPeriodEnd;
                s.Id = sub.Items.Data[0].Subscription;
                s.Price = (long)sub.Items.Data[0].Price.UnitAmount;
                s.ProductId = sub.Items.Data[0].Price.ProductId;
                s.Interval = sub.Items.Data[0].Plan.Interval;
                s.IntervalCount = sub.Items.Data[0].Plan.IntervalCount;
                if(sub.PauseCollection != null)
                {
                    s.ResumesAt = (DateTime)sub.PauseCollection.ResumesAt;
                } else
                {
                    s.ResumesAt = null;
                }

                var prodSrv = new ProductService();
                Product p = prodSrv.Get(s.ProductId);
                s.ProductName = p.Name;
                items.Add(s);
            }
            Thread.Sleep(2500);

            return Ok(items);
        }

        // POST api/<ValuesController>
        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromForm] string price_id)
        {
            StripeConfiguration.ApiKey = "";
            var options = new SubscriptionCreateOptions
            {
                Customer = "cus_HSJgT9ML7AcfO9",
                Items = new List<SubscriptionItemOptions>
                      {
                        new SubscriptionItemOptions
                        {
                          Price = price_id,
                        },
                      },
            };
            var service = new SubscriptionService();
            try
            {
                Subscription subscription = service.Create(options);
                if (subscription.Status.Equals("active"))
                {
                    var invService = new InvoiceService();
                    Invoice inv = invService.Get(subscription.LatestInvoiceId);
                    Thread.Sleep(2500);
                    return Ok(new { message = "Successfully created subscription, invoice " + subscription.LatestInvoiceId + " was created.", url = inv.HostedInvoiceUrl });
                }
                else
                {
                    Thread.Sleep(2500);
                    return BadRequest(new { message = "Could not create subscription" });
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(2500);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("pause")]
        public IActionResult Pause([FromForm] string subscription_id)
        {
            StripeConfiguration.ApiKey = "";
            DateTime resumeTimestamp = DateTime.UtcNow.AddDays(3);
            var options = new SubscriptionUpdateOptions
            {
                PauseCollection = new SubscriptionPauseCollectionOptions
                {
                    Behavior = "void",
                    ResumesAt = resumeTimestamp,
                }
            };
            var service = new SubscriptionService();
            try
            {
                Subscription sub = service.Update(subscription_id, options);
                Thread.Sleep(2500);
                return Ok(new { message = sub.PauseCollection.ResumesAt });
            } catch(Exception ex)
            {
                Thread.Sleep(2500);
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
