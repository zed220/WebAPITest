using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebAPIBooksTests {
    public static class ControllerTestingExtensions {
        public static HttpResponseMessage GetMessage(this IHttpActionResult result) => result.ExecuteAsync(new CancellationToken()).Result;
        public static T GetContent<T>(this HttpResponseMessage message) {
            message.AssertStatusCode();
            return message.Content.ReadAsAsync<T>().Result;
        }
        public static byte[] GetByteArrayContent(this HttpResponseMessage message) {
            message.AssertStatusCode();
            return message.Content.ReadAsByteArrayAsync().Result;
        }
        public static void AssertStatusCode(this HttpResponseMessage message) {
            if(!message.IsSuccessStatusCode)
                Assert.Fail($"Bad Request. Code={message.StatusCode}, Text={message.Content?.ReadAsAsync<HttpError>().Result.Message}");
        }
    }
}
