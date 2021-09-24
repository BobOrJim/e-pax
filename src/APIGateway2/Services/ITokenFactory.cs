using System.Threading.Tasks;

namespace APIGateway2.Services
{
    public interface ITokenFactory
    {
        Task<string> GetAccessToken();
    }
}