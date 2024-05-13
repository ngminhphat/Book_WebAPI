using Microsoft.EntityFrameworkCore;
using Book_WebAPI.Data;
using Book_WebAPI.Models.Domain;
using Book_WebAPI.Models.Interfaces;

namespace Book_WebAPI.Models.Services
{
    public class PublisherRepository: IPublisherRepository
    {
        private readonly AppDbContext _db;
        public PublisherRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Publisher>> GetPublishersAsync()
        {
            try
            {
                return await _db.Publishers.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Publisher> GetPublisherAsync(int id, bool includePublishers)
        {
            try
            {
                if (includePublishers)
                {
                    return await _db.Publishers.Include(b => b.Books).FirstOrDefaultAsync(i => i.PublisherId == id);
                }

                return await _db.Publishers.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Publisher> AddPublisherAsync(Publisher publisher)
        {
            try
            {
                await _db.Publishers.AddAsync(publisher);
                await _db.SaveChangesAsync();
                return await _db.Publishers.FindAsync(publisher.PublisherId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
        {
            try
            {
                _db.Entry(publisher).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return publisher;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeletePublisherAsync(Publisher publisher)
        {
            try
            {
                var dbPublisher = await _db.Publishers.FindAsync(publisher.PublisherId);

                if (dbPublisher == null)
                {
                    return (false, "Publisher could not be found");
                }

                _db.Publishers.Remove(publisher);
                await _db.SaveChangesAsync();

                return (true, "Publisher got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
