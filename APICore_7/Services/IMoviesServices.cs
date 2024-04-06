using APICore_7.Models;

namespace APICore_7.Services
{
    public interface IMoviesServices
    {
        Task<IEnumerable<MovieDetailsDTO>> GetAll();
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Task<Movie> Update(Movie movie);
        Task<Movie> Delete(Movie movie);
        Task SavaChangesAsync();
    }
}
