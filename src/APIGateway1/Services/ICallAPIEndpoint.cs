using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public interface ICallAPIEndpoint
    {
        Task<string> CallEndpoint(string url);
    }
}