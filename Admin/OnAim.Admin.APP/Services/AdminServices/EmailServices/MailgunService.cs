//using OnAim.Admin.APP.Services.Abstract;
//using RestSharp;

//namespace OnAim.Admin.APP.Services.Email
//{
//    public class MailgunService : IEmailService
//    {
//        private readonly string _apiKey;
//        private readonly string _domain;

//        public MailgunService(string apiKey, string domain)
//        {
//            _apiKey = apiKey;
//            _domain = domain;
//        }

//        public async Task SendActivationEmailAsync(string recipientEmail, string subject, string htmlBody)
//        {
//            var client = new RestClient(new Uri($"https://api.mailgun.net/v3/onaim.io"));

//            var request = new RestRequest("messages", Method.Post);
//            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("api:" + "3724298e-46a5c1dc")));

//            request.AddParameter("from", "Excited User <mailgun@" + "onaim.io" + ">");
//            request.AddParameter("to", recipientEmail);
//            request.AddParameter("subject", subject);
//            request.AddParameter("text", htmlBody);

//            var response = await client.ExecuteAsync(request);

//            if (!response.IsSuccessful)
//            {
//                Console.WriteLine($"Error: {response.StatusDescription} - {response.Content}");
//            }
//        }

//        public async Task SendEmailAsync(string toEmail, string subject, string body)
//        {
//            var client = new RestClient(new Uri($"https://api.mailgun.net/v3/onaim.io"));

//            var request = new RestRequest("messages", Method.Post);
//            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("api:" + "3724298e-46a5c1dc")));

//            request.AddParameter("from", "Excited User <mailgun@" + "onaim.io" + ">");
//            request.AddParameter("to", toEmail);
//            request.AddParameter("subject", subject);
//            request.AddParameter("text", body);

//            var response = await client.ExecuteAsync(request);

//            if (!response.IsSuccessful)
//            {
//                Console.WriteLine($"Error: {response.StatusDescription} - {response.Content}");
//            }
//        }
//    }
//}
