using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.DataAccess.EntityFrameworkCore.DataModel;
using WebApi.Shared.Model.Contact;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly ApplicationDBContext _DbContext;

        public AddressBookController(ApplicationDBContext DbContext)
        {
            _DbContext = DbContext;
        }

        /// <summary>
        /// Get All List
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet]
        [Route("getListContacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            var list = await _DbContext.ContactInfo.ToListAsync();
            if (list.Count > 0)
            {
                return Ok(list);
            }
            return BadRequest();
        }


        /// <summary>
        /// Get Contact Details By Id
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getContactById/{Id}")]
        public async Task<IActionResult> GetContactById(int Id)
        {
            var contactDetails = await _DbContext.ContactInfo.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (contactDetails != null)
            {
                return Ok(contactDetails);
            }
            return BadRequest();
        }


        /// <summary>
        /// Add or Edit Contact Details 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("postContactDetails")]
        public async Task<IActionResult> PostContactDetails(Contact contact)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            if (contact.Id == null || contact.Id == 0)
            {
                await _DbContext.ContactInfo.AddAsync(contact);
            }
            else
            {
                var contactDetails = _DbContext.ContactInfo.FirstOrDefault(x => x.Id == contact.Id);
                if (contactDetails != null)
                {
                    contactDetails.FirstName = contact.FirstName;
                    contactDetails.LastName = contact.LastName;
                    contactDetails.PermanentAddress = contact.PermanentAddress;
                    contactDetails.TemporaryAddress = contact.TemporaryAddress;
                    contactDetails.FatherName = contact.FatherName;
                    contactDetails.GrandFatherName = contact.GrandFatherName;
                    contactDetails.MobileNo = contact.MobileNo;
                }
            }
            var result = await _DbContext.SaveChangesAsync();
            if(result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Delete ContactDetails By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteContactDetails/{id}")]
        public async Task<IActionResult> DeleteContactDetails(int id)
        {
            Contact contact = new Contact { Id = id };
            _DbContext.ContactInfo.Remove(contact);
            var result = await _DbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}