using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using Hello_SelfHost_WebAPI.Start;

namespace Hello_SelfHost_WebAPI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var runner = new ApiRunner();
            runner.Start();

            MembershipCreateStatus createStatus;
            Membership.CreateUser("admin", "Admin2015!", "admin@admin.com", "Question?", "Answer", true,
                out createStatus);
            Console.WriteLine("Added user admin with password Admin2015!");
            Console.WriteLine("Trying to authenticate with admin/incorrect");
            var result = GetValues("admin", "incorrect");
            Console.WriteLine("Got: " + result.StatusDescription);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Trying to authenticate with admin/Admin2015!");
                result = GetValues("admin", "Admin2015!");
                var readStream = new StreamReader(result.GetResponseStream(), Encoding.UTF8);
                Console.WriteLine("Response: " + readStream.ReadToEnd());
            }

            Console.WriteLine("Press ENTER to exit the app.");
            Console.ReadLine();
            runner.Stop();
        }

        private static HttpWebResponse GetValues(string login, string password)
        {
            HttpWebResponse result = null;
            try
            {
                var request = WebRequest.Create("http://localhost:9000/api/values");
                request.Credentials = new NetworkCredential(login, password);
                result = (HttpWebResponse) (request.GetResponse());
            }
            catch (Exception exception)
            {
                var webException = exception as WebException;
                if (webException != null)
                {
                    var response = webException.Response as HttpWebResponse;
                    if (response != null)
                    {
                        result = response;
                    }
                }
            }
            return result;
        }
    }
}