using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Data.SqlClient;

namespace Task_6.Controllers
{
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        readonly string connString = "Data Source=localhost\\sqlexpress;" +
      "database=CSC_Task_6;" +
      "integrated security=true";
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                if (stripeEvent.Type == Events.ChargeFailed)
                {
                    var charge = stripeEvent.Data.Object as Charge;

                    string chargeId = charge.Id;
                    string code = charge.FailureCode;
                    string failureMessage = charge.FailureMessage;
                    
                    SqlConnection conn = new SqlConnection(connString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO FailedCharges VALUES(@charge, @code, @message)", conn);

                    cmd.Parameters.AddWithValue("@charge", chargeId);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@message", failureMessage);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                    return Ok();
                }
                else if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    var charge = stripeEvent.Data.Object as Charge;

                    string chargeId = charge.Id;
                    long amount = charge.Amount;
                    DateTime? created = charge.Created;
                    string customerId = charge.CustomerId;
                    
                    SqlConnection conn = new SqlConnection(connString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO SuccessfulCharges VALUES(@charge, @amount, @created, @customerId)", conn);

                    cmd.Parameters.AddWithValue("@charge", chargeId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@created", created);
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                    return Ok();
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;

                    string customer = subscription.CustomerId;
                    DateTime? created = subscription.Created;
                    string productId = subscription.Plan.ProductId;
                    long? amount = subscription.Plan.Amount;
                    string interval = subscription.Plan.Interval;

                    SqlConnection conn = new SqlConnection(connString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO CreatedSubscriptions VALUES(@customer, @created, @productId, @amount, @interval)", conn);

                    cmd.Parameters.AddWithValue("@customer", customer);
                    cmd.Parameters.AddWithValue("@created", created);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@interval", interval);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return Ok();
                } 
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;

                    string customer = subscription.CustomerId;
                    DateTime? created = subscription.Created;
                    string productId = subscription.Plan.ProductId;
                    long? amount = subscription.Plan.Amount;
                    string interval = subscription.Plan.Interval;

                    SqlConnection conn = new SqlConnection(connString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO DeletedSubscriptions VALUES(@customer, @created, @productId, @amount, @interval)", conn);

                    cmd.Parameters.AddWithValue("@customer", customer);
                    cmd.Parameters.AddWithValue("@created", created);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@interval", interval);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return Ok();
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;

                    string customer = subscription.CustomerId;
                    DateTime? created = subscription.Created;
                    string productId = subscription.Plan.ProductId;
                    long? amount = subscription.Plan.Amount;
                    string interval = subscription.Plan.Interval;

                    SqlConnection conn = new SqlConnection(connString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO UpdatedSubscriptions VALUES(@customer, @created, @productId, @amount, @interval)", conn);

                    cmd.Parameters.AddWithValue("@customer", customer);
                    cmd.Parameters.AddWithValue("@created", created);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@interval", interval);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return Ok();
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    return BadRequest();
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}