using System.Security.Claims;
using Book_WebAPI.Models.Domain;

namespace Book_WebAPI.Models.Interfaces
{
    public interface IPublisherRepository
    {
        Task<List<Publisher>> GetPublishersAsync(); 
        Task<Publisher> GetPublisherAsync(int id, bool includeClasss = false); 
        Task<Publisher> AddPublisherAsync(Publisher publisher); 
        Task<Publisher> UpdatePublisherAsync(Publisher publisher); 
        Task<(bool, string)> DeletePublisherAsync(Publisher publisher); 
    }
}
