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
    public class GenresController : ControllerBase
    {
        private readonly FredrifoContext _context;

        public GenresController(FredrifoContext context)
        {
            _context = context;
        }

        // GET: api/Genres
        /// <summary>Gets all of the genres</summary>
        /// <returns>all genres</returns>
        [HttpGet]
        public IEnumerable<Genre> GetGenre()
        {
            return _context.Genre;
        }

        // GET: api/Genres/5
        /// <summary>Gets the genre with matching genreId</summary>
        /// <param name="id">  genre Id</param>
        /// <returns>Returns genre</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = await _context.Genre.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        // PUT: api/Genres/5
        /// <summary>Puts the genre.
        /// Replacing genre with new genre</summary>
        /// <param name="id">  The id that will be replaced</param>
        /// <param name="genre">  The genre that will replace the old one</param>
        /// <returns>Response code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre([FromRoute] int id, [FromBody] Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genre.GenreId)
            {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        /// <summary>  Posts a new genre</summary>
        /// <param name="genre">The genre that will be Created</param>
        /// <returns>
        ///   <para>Response code. and the newly created genre</para>
        /// </returns>
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> PostGenre([FromBody] Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Genre.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.GenreId }, genre);
        }

        // POST: api/Genres/search
        /// <summary>
        ///   <para>Post request on the route .../search/</para>
        ///   <para> Checks if the parameter currentSearch is matching any genres in the database</para>
        /// </summary>
        /// <param name="currentSearch">The search.parameter</param>
        /// <returns>A list of all matching genres</returns>
        [HttpPost]
        [Route("search")]
        public IActionResult PostSearchGenre([FromBody] string currentSearch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Checks if a genreName exists with the search parameter + wildcard
            var list = _context.Genre.Where(c => EF.Functions.Like(c.GenreName, currentSearch + "%"));
            return Ok(list);
        }

        // DELETE: api/Genres/5
        /// <summary>  Deletes a genre object based on the id parameter</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>the genre that got removed</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = await _context.Genre.FindAsync(id);

            //if the genre doesnt exist
            if (genre == null)
            {
                //return without deleting anything
                return NotFound();
            }

            //Removing the genre
            _context.Genre.Remove(genre);

            //saving changes
            await _context.SaveChangesAsync();

            //returning the deleted genre
            return Ok(genre);
        }

        /// <summary>  Checks if there exists a genre with a certain ID</summary>
        /// <param name="id">  genre id</param>
        /// <returns>Boolean representing if the genre exists</returns>
        private bool GenreExists(int id)
        {
            //true if genre id exists
            return _context.Genre.Any(e => e.GenreId == id);
        }
    }
}