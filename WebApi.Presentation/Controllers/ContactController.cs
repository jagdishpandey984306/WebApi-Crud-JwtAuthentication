using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Shared.Model.Contact;

namespace WebApi.Presentation.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory http;
        HttpClient client;
        public ICheckToken Token { get; }
        public ContactController(IHttpClientFactory http, ICheckToken token)
        {
            this.http = http;
            Token = token;
            client = http.CreateClient("JagdishApi");
        }
       

        public async Task<IActionResult> Index()
        {
            await Token.GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage Response = await client.GetAsync("api/AddressBook/getListContacts");
            if (Response.IsSuccessStatusCode)
            {
                ViewBag.list = Response.Content.ReadAsAsync<List<Contact>>().Result;
                return View();
            }
            return View();

        }

        public async Task<IActionResult> Create()
        {
             return  View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(Contact obj)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage Response = await client.PostAsJsonAsync("api/AddressBook/postContactDetails", obj);
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int Id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage Response = await client.DeleteAsync("api/AddressBook/deleteContactDetails/" + Id);
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ContactGetById(int Id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage Response = await client.GetAsync("api/AddressBook/getContactById/" + Id);
            if (Response.IsSuccessStatusCode)
            {
                var data = Response.Content.ReadAsAsync<Contact>().Result;
                return View("Edit", data);
            }
            return View();
        }
    }
}
