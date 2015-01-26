using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using Hello_SelfHost_WebAPI.Start;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Hello_SelfHost_WebAPI
{
    class BasicTests
    {
        readonly ApiRunner _runner = new ApiRunner();

        [Test]
        public void InitTest()
        {
            _runner.Start();
            _runner.Stop();
            Assert.Throws<InvalidOperationException>(() => _runner.Stop());
            _runner.Start();
            Assert.Throws<InvalidOperationException>(() => _runner.Start());
            _runner.Stop();
        }

        [Test]
        public void AuthenticationTest()
        {
            var expectedJson = JsonConvert.SerializeObject(new[] { "value1", "value2" });
            Membership.DeleteUser("admin");
            _runner.Start();
            MembershipCreateStatus createStatus;
            Membership.CreateUser("admin", "Admin2015!", "admin@admin.com", "Question?", "Answer", true,
                out createStatus);
            Assert.AreEqual(MembershipCreateStatus.Success, createStatus);

            var result = GetValues("admin", "incorrect");
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
            result = GetValues("admin", "Admin2015!");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var readStream = new StreamReader(result.GetResponseStream(), Encoding.UTF8);
            Assert.AreEqual(expectedJson, readStream.ReadToEnd());
            _runner.Stop();
        }

        private static HttpWebResponse GetValues(string login, string password)
        {
            HttpWebResponse result = null;
            try
            {
                var request = WebRequest.Create("http://localhost:9000/api/values");
                request.Credentials = new NetworkCredential(login, password);
                result = (HttpWebResponse)(request.GetResponse());
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