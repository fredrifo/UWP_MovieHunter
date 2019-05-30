using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHunter.DataAccessCore.Models;

namespace MovieHunter.RESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly fredrifoContext _context;

        public MoviesController(fredrifoContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        /// <summary>
        /// Gets the all the existing movies
        /// </summary>
        /// <returns>All the existing movies</returns>
        [HttpGet]
        public IEnumerable<Movie> GetMovie()
        {
            return _context.Movie;
        }

        // GET: api/Movies/5
        /// <summary>
        /// Gets all movies matching the searchparameter + wildcard
        /// </summary>
        /// <param name="searchParameter">The search parameter.</param>
        /// <returns>The list of movies</returns>
        [HttpGet("{searchParameter}")]
        public async Task<IActionResult> GetMovie([FromRoute] string searchParameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking if the title of a movie matching the searchparameter + wildcard
            var movie = _context.Movie.Where(c => EF.Functions.Like(c.Title, searchParameter + "%"));


            // var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // GET: api/Movies/object/5
        /// <summary>
        /// Getting a movie based on the movie identifier
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>Movie object</returns>
        [HttpGet("object/{movieId}")]
        public async Task<IActionResult> GetMovie([FromRoute] int movieId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movie.FindAsync(movieId);

            // var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // PUT: api/Movies/5
        /// <summary>
        /// Updates a movie with id from the route. this movie object will be replaced by the Movie object in the body 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="movie">The movie object.</param>
        /// <returns>Statuscode</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie([FromRoute] int id, [FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //If the body and route parameters doesnt match return Bad request.
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            //Chaning the state to modified
            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                //saving changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies/newMovie
        /// <summary>
        /// Posts a new Movie object to the database
        /// </summary>
        /// <param name="movie">The movie object</param>
        /// <returns>Status code</returns>
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Adding movie to database
            _context.Movie.Add(movie);

            //saving changes
            await _context.SaveChangesAsync();

            //returning Status 201
            return CreatedAtAction("GetMovie", new { id = movie.MovieId }, movie);
        }

        // api/Movies/deleteMovie/5
        /// <summary>
        /// Deletes the a movieObject based on the id.
        /// It also deletes associated listitems.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>deleted movie object</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Looking for movie with id
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                //If movie doesnt exist return
                return NotFound();
            }

            //remove from list
            _context.Movie.Remove(movie);

            //Find every listItem that is the same movie
            var listItems = _context.ListItem.Where(c => c.MovieId == movie.MovieId);

            //Deleting all of these movies
            foreach (ListItem l in listItems)
            {
                _context.ListItem.Remove(l);
            }

            //Saving changes
            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        /// <summary>
        /// Checks if Movie exists.
        /// </summary>
        /// <param name="id">movie id</param>
        /// <returns>Boolean if exists</returns>
        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }
    }
}