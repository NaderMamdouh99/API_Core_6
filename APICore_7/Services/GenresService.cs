using APICore_7.Models;
using Microsoft.EntityFrameworkCore;

namespace APICore_7.Services
{
    public class GenresService : IGenresServices
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }


        public async Task<Genre> GetById(byte id)
        {
           return await _context.Genres.FirstOrDefaultAsync(g=>g.Id ==id);
        }


        public async Task<Genre> Add(Genre genre)
        {
            await _context.AddAsync(genre);
            await SavaChangeAsync();
            return genre;
        }

        public async Task<Genre> Update(Genre genre)
        {
            _context.Update(genre);
            await SavaChangeAsync();
            return genre;
        }

        public async Task<Genre> Delete(Genre genre)
        {
            
            _context.Remove(genre);
            await SavaChangeAsync();
            return genre;

        }

     
        public async Task SavaChangeAsync()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<bool> isValidGenreId(byte id)
        {
           return await _context.Genres.AnyAsync(g => g.Id == id);
        }
    }
}
