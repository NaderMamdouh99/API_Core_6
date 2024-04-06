using APICore_7.Models;

namespace APICore_7.Services
{
    public interface IGenresServices
    {
        Task<IEnumerable<Genre>> GetAll();

        Task<Genre> GetById(byte id);

        Task<Genre> Add(Genre genre);
        Task<Genre> Update(Genre genre);
 
        Task<Genre> Delete(Genre genre);

        Task SavaChangeAsync();

        Task<bool> isValidGenreId(byte id);


    }
}
