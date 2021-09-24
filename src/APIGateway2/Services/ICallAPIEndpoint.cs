using System.Threading.Tasks;

namespace APIGateway2.Services
{
    public interface ICallAPIEndpoint
    {
        Task<string> CallEndpoint(string url);
    }
}