using Microsoft.EntityFrameworkCore;
using WebApi.Shared.Model.Contact;

namespace WebApi.DataAccess.EntityFrameworkCore.DataModel
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Contact> ContactInfo { get; set; }
    }
}
