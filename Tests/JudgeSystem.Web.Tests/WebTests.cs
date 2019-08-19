using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace JudgeSystem.Web.Tests
{
    public class WebTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;

        public WebTests(WebApplicationFactory<Startup> server)
        {
            this.server = server;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Identity/Account/Login")]
        [InlineData("/Identity/Account/Register")]
        [InlineData("/Course/All")]
        public async Task RequestToGivenUrlShoudReturnSuccessStatusCode(string url)
        {
            HttpClient client = server.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/Identity/Account/Manage")]
        [InlineData("/User/MyResults")]
        [InlineData("/Administration/Course/Create")]
        [InlineData("/Student/Profile")]
        [InlineData("/Administration/Contest/Create")]
        [InlineData("/Administration/Student/Create")]
        [InlineData("/Administration/Contest/ActiveContests")]
        [InlineData("/Administration/Contest/All")]
        [InlineData("/Administration/Student/StudentsByClass")]
        public async Task AccessPageWithGivenUrlShoudRedirectToLoginPage(string url)
        {
            HttpClient client = server.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            HttpResponseMessage response = await client.GetAsync(url);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            string expectedUrl = $"/Identity/Account/Login?ReturnUrl={WebUtility.UrlEncode(url)}";
            Assert.Contains(expectedUrl, response.Headers.Location.ToString());
        }
    }
}
