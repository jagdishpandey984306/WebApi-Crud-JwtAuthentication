using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApi.Presentation
{
    public interface ICheckToken
    {
        Task GetToken();
    }
}