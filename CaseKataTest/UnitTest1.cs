using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaseKataTest
{
    public class UnitTest1
    {
        [Test]
        public async Task WhenHitValues5ThenReceiveValue()
        {
            var _client = new HttpClient();
            
            var response = await _client.GetAsync("http://localhost:5000/api/values/5");
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("value", content);
        }

        [Test]
        public async Task GivenCaseRequestWhenCaseDoesNotExistThenReturnNotFound()
        {
            var _client = new HttpClient();
            
            var response = await _client.GetAsync("http://localhost:5000/api/casefile/30");
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Resource Not Found", content);
        }
    }
}
