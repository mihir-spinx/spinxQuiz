using Newtonsoft.Json;
using System.Collections.Generic;

namespace Spinx.Web.Infrastructure
{
    public class ReCaptcha
    {
        public static string Validate(string encodedResponse, string privateKey)
        {
            var client = new System.Net.WebClient();
            var GoogleReply = client.DownloadString(
                $"https://www.google.com/recaptcha/api/siteverify?secret={privateKey}&response={encodedResponse}");

            var captchaResponse = JsonConvert.DeserializeObject<ReCaptcha>(GoogleReply);
            return captchaResponse.Success;
        }

        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}