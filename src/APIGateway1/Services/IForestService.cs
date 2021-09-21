using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public interface IForestService
    {
        Task<string> CallEndpoint(string url);
    }
}