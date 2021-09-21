using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public interface ITokenFactory
    {
        Task<string> GetAccessToken();
    }
}