using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReversiMvcApp.Models
{
    public class GoogleReCaptchaService
    {
        public string site_key = "6Lcz5kAfAAAAAE3cj1smaqhGrvzasd6RkcHN5PQJ";
        public string secret_key = "6Lcz5kAfAAAAAAV7rEozONRFRNm9dnCUWm-ICtsC";
        public GoogleReCaptchaService()
        {

        }

        public virtual async Task<GoogleResp> RecVer(string token)
        {
            GoogleRepatchaData _MyData = new GoogleRepatchaData()
            {
                response = token,
                secret = secret_key
            };

            HttpClient client = new HttpClient();

            var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret_key}&response={token}");

            var capresp = JsonConvert.DeserializeObject<GoogleResp>(response);

            return capresp;
        }
    }

    public class GoogleRepatchaData
    {

        public string response { get; set; }
        public string secret { get; set; }
    }

    public class GoogleResp
    {
        public bool succes { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
