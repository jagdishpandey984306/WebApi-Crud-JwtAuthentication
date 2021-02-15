using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Shared.Model.TokenRequest;

namespace WebApi.Presentation
{
    public class CheckToken : ICheckToken
    {
        TokenRequest token = new TokenRequest();
        private readonly IHttpClientFactory http;
        public IHttpContextAccessor HttpContext { get; }
        public CheckToken(IHttpClientFactory http, IHttpContextAccessor httpContext)
        {
            this.http = http;
            HttpContext = httpContext;
        }
        public async Task GetToken()
        {
            if (HttpContext.HttpContext.Session.GetString("token") == null)
            {
                var client = http.CreateClient("JagdishApi");
                token.username = "webapi";
                token.password = "api1234";
                HttpResponseMessage res = await client.PostAsJsonAsync("api/Token/Login", token);
                if (res.IsSuccessStatusCode)
                {
                    string MyToken = await res.Content.ReadAsStringAsync();
                    HttpContext.HttpContext.Session.SetString("token", MyToken);
                }
            }
        }
    }
}
