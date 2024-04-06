using APICore_7.Models;
using APICore_7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICore_7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {


        private List<string> _allowedExtenssions = new() { ".jpg", ".png" };
        dynamic _allowedImageSize = 1048576;
        private readonly IMoviesServices _moviesServices;
        private readonly IGenresServices _genresServices;

        public MovieController(IMoviesServices moviesServices, IGenresServices genresServices)
        {
            _moviesServices = moviesServices;
            _genresServices = genresServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var Movies = await _moviesServices.GetAll();

            return Ok(Movies);
        }

        [HttpGet("{id}",Name ="GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _moviesServices.GetById(id);

            if (movie is null)
                return NotFound($"No Movies With Found ID {id}");
            return Ok(movie);
        }


        [HttpPost]
        public async Task<IActionResult> addMovie([FromForm]MovieDTO dto)
        {
            if (dto.Poster is null)
                return BadRequest("Poster is Reqiured");

            if (!_allowedExtenssions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are Allowed!");

            if (dto.Poster.Length > _allowedImageSize)
                return BadRequest("Max Allowed Size For Poster is 1MB!");

            var _isValidGenreId = await _genresServices.isValidGenreId(dto.GenreId);
            if (!_isValidGenreId)
                return BadRequest("Invalid Genre ID!");

            using var dataStrem = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStrem);

            Movie movie = new()
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                Poster = dataStrem.ToArray(),
            };

            await _moviesServices.Add(movie);

            return Ok(movie);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id ,[FromForm]MovieDTO dto)
        {
            var movie = await _moviesServices.GetById(id);

            if (movie is null)
                return NotFound($"No Movies With Found ID {id}");


            if (dto.Poster != null)
            {

                if (!_allowedExtenssions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are Allowed!");

                if (dto.Poster.Length > _allowedImageSize)
                    return BadRequest("Max Allowed Size For Poster is 1MB!");

                var _isValidGenreId = await _genresServices.isValidGenreId(dto.GenreId);
                if (!_isValidGenreId)
                    return BadRequest("Invalid Genre ID!");

                using var dataStrem = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStrem);
                movie.Poster = dataStrem.ToArray();
            }

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.StoreLine = dto.StoreLine;
            movie.GenreId = dto.GenreId;

           await _moviesServices.Update(movie);

            return Ok(movie);
            
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _moviesServices.GetById(id);

            if(movie is null)
                return NotFound($"No Movies With Found ID {id}");

            await _moviesServices.Delete(movie);
            return Ok(movie);
        }


    }
}
