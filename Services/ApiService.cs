using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data;
using AppointmentBooking.Data;
using AppointmentBooking.Models;
using System.Net.Http;

namespace AppointmentBooking.Services
{
    public class ApiService
    {
        private readonly HttpClient client;
        public ApiService(HttpClient _client)
        {
            client = _client;
        }
        public async Task<InsureePatientInfo> GetInsureePersonalInfo(string insid)
        {
            /*string login = "baghaudahf";
            string psw = "jJPEQmFdT0zX3KE6S4ct";
            string HKeyword = "remote-user";
            string HValue = "bhfhir";
            */
            string url = "https://imis.hib.gov.np/api/api_fhir/Patient/?identifier=" + insid;
            string login = "baghaudahf";
            string psw = "jJPEQmFdT0zX3KE6S4ct";
            string HKeyword = "remote-user";
            string HValue = "bhfhir";
            InsureePatientInfo data = new InsureePatientInfo();
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{psw}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            client.DefaultRequestHeaders.Add(HKeyword, HValue);
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<InsureePatientInfo>(responseData);
                if (result != null)
                {
                    data = result;
                   
                }
                return data;
            }
            return null;

            
        }
    }
}
