using APICore_7.Models;
using APICore_7.Services;
using Microsoft.AspNetCore.Mvc;



namespace APICore_7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServices _genresServices;

        public GenresController( IGenresServices genresServices)
        {
            _genresServices = genresServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresServices.GetAll();
            return Ok(genres);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(byte id)
        {
            var genre = await _genresServices.GetById(id);
            if (genre is null)
                return NotFound($"No Genre was Found Id : {id}");
            return Ok(genre);   
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]GenreDTO genreDTO)
        {
            Genre genre = new() { Name = genreDTO.Name };
            await _genresServices.Add(genre);
            return Ok(genre);
        }


        [HttpPut]
        public async Task<IActionResult> Edit(byte id,GenreDTO dTO)
        {
            var genre = await _genresServices.GetById(id);

            if (genre is null)
                return NotFound($"No Genre was Found Id : {id}");
            genre.Name = dTO.Name;
            await _genresServices.Update(genre);

            return Ok(genre);
        }

        [HttpDelete]        
        public async Task<IActionResult> Delete(byte id)
        {
            var genre = await _genresServices.GetById(id);

            if (genre is null)
                return NotFound($"No Genre was Found Id : {id}");
            await _genresServices.Delete(genre);

            return Ok(genre);
        }

    }
}
 