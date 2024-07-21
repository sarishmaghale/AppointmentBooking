using AppointmentBooking.Models;
using AppointmentBooking.Services.Interface;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AppointmentBooking.Services.Repository
{
    public class PaymentService:IPaymentService
    {
        public IHttpClientFactory httpClientFactory; private readonly IUrlHelper _urlHelper;
        public PaymentService(IHttpClientFactory _httpClientFactory, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            }
            httpClientFactory = _httpClientFactory;
            _urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext
            {
                HttpContext = httpContextAccessor.HttpContext,
                RouteData = httpContextAccessor.HttpContext.GetRouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            });
        }
        public string GenerateUniqueId(int id)
        {
            //generate unique Id for transactionId combining BookingId with today Date and time
            string newId = id + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return newId;
        }

        public string GenerateSignature(string message)
        {

            string secret = "8gBm/:&EnhH.1/q"; //secret key provided by esewa
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            var hmac = new HMACSHA256(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(messageBytes);
            string signature = Convert.ToBase64String(hashBytes);
            return signature;
        }

        public async Task<PaymentViewModel> ActivateEsewaPayment(int id, double? amount)
        {
            string purchaseId = GenerateUniqueId(id);
            string message = "total_amount="+amount+",transaction_uuid=" + purchaseId + ",product_code=EPAYTEST";
            string signature = GenerateSignature(message);
            PaymentViewModel payData = new PaymentViewModel()
            {
                amount = amount.ToString(),
                totalAmount = amount.ToString(),
                purchaseId = purchaseId,
                successUrl = _urlHelper.Action("PaymentSuccess", "Appointment", null, _urlHelper.ActionContext.HttpContext.Request.Scheme),
                failureUrl = _urlHelper.Action("AppointmentBooking", "Appointment", new { BookingId = id }, _urlHelper.ActionContext.HttpContext.Request.Scheme),
                signature = signature
            };
            return payData;
        }

        public async Task<OPDBookingViewModel> ValidateEsewaPayment(string encodedData)
        {
            string decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
            // Parse the JSON data if necessary
            JObject jsonData = JObject.Parse(decodedData);         

            // Extract individual values
            string status = jsonData["status"].ToString();
            string signature = jsonData["signature"].ToString();
            string transactionCode = jsonData["transaction_code"].ToString();
            string totalAmount = jsonData["total_amount"].ToString();
            string transactionId = jsonData["transaction_uuid"].ToString();
            string productCode = jsonData["product_code"].ToString();
            string signedFieldNames = "transaction_code,status,total_amount,transaction_uuid,product_code,signed_field_names";
            string message = "transaction_code=" + transactionCode + ",status=" + status + ",total_amount=" + totalAmount + ",transaction_uuid=" + transactionId + ",product_code=" + productCode + ",signed_field_names=" + signedFieldNames;
            //generate signature
            string generatedSignature = GenerateSignature(message);
            if (status == "COMPLETE")
            {
                //verify the integrity of the response body by comparing the signature that we have sent with the signature that you generate
                if (signature == generatedSignature)
                {
                    //get the OPDBookingId 
                    string inputString = transactionId;
                    string[] parts = inputString.Split('-');
                    int getId = Int32.Parse(parts[0]);
                    OPDBookingViewModel model = new OPDBookingViewModel()
                    {
                        OpdbookingId = getId,
                        refid = transactionCode,
                        amount = totalAmount,
                    };
                    //returns data if the transaction successfull
                    return model;
                }
            }
            //returns null if transaction failed
            return null;

        }

        public async Task<string> ActivateKhaltiPayment(int id, double? amount)
        {
            string purchaseOrderid = GenerateUniqueId(id);
            var url = "https://a.khalti.com/api/v2/epayment/initiate/";
            var payload = new
            {
                return_url = _urlHelper.Action("PaymentSuccess", "Appointment", null, _urlHelper.ActionContext.HttpContext.Request.Scheme),
                website_url = _urlHelper.Action("AppointmentBooking", "Appointment", new { BookingId = id }, _urlHelper.ActionContext.HttpContext.Request.Scheme),
                amount = (amount*100).ToString(),
                purchase_order_id = purchaseOrderid,
                purchase_order_name = "OPBooking",
                customer_info = new
                {
                    name = "Ram Bahadur",
                    email = "test@gmail.com",
                    phone = "9800000002",
                }

            };
            var jsonPayLoad = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayLoad, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "key live_secret_key_68791341fdd94846a146f0457ff7b455");
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic jsonObject = JsonConvert.DeserializeObject(responseContent);
            return jsonObject.payment_url;
        }

        public async Task<OPDBookingViewModel> ValidateKhaltiPayment(string pdx, string Id)
        {
            string[] parts = Id.Split('-');
            int BookingId = Int32.Parse(parts[0]);
            var url = "https://a.khalti.com/api/v2/epayment/lookup/";
            var payload = new
            {
                pidx = pdx,
            };// Assuming the API expects a JSON object with a 'pidx' field
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "key live_secret_key_68791341fdd94846a146f0457ff7b455");
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic jsonObject = JsonConvert.DeserializeObject(responseContent);
            string status = jsonObject.status;
            if (status == "Completed")
            {

                OPDBookingViewModel model = new OPDBookingViewModel()
                {
                    refid = jsonObject.transaction_id,
                    amount = jsonObject.total_amount,
                    OpdbookingId = BookingId,
                };
                //returns data if the transaction successfull
                return model;
            }
            //returns null if transaction failed
            return null;
        }
    }
}
