using APICore_7.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICore_7.Services
{
    public class MoviesServices : IMoviesServices
    {
        private readonly ApplicationDbContext _context;

        public MoviesServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDetailsDTO>> GetAll()
        {
            var genres = await _context.Movies
                .Include(g => g.Genres)
                .Select(g=> new MovieDetailsDTO
                {
                    Id = g.Id,
                    Title = g.Title,
                    Rate = g.Rate,
                    Year = g.Year,
                    StoreLine = g.StoreLine,
                    Poster = g.Poster,
                    GenreName = g.Genres.Name
                })
                .ToListAsync();
            return genres;
        }

        public async Task<Movie> GetById(int id)
        {
          var movie =  await _context.Movies
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);
            return movie;
                
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await SavaChangesAsync();
            return movie;
        }

        public async Task<Movie> Update(Movie movie)
        {
            _context.Update(movie);
            await SavaChangesAsync();
            return movie;
        }

        public async Task<Movie> Delete(Movie movie)
        {
            _context.Remove(movie);
            await SavaChangesAsync();
            return movie;
        }

        public async Task SavaChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
