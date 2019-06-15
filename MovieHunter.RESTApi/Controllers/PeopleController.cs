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
    public class PeopleController : ControllerBase
    {
        private readonly FredrifoContext _context;

        public PeopleController(FredrifoContext context)
        {
            _context = context;
        }

        // GET: api/People
        /// <summary>
        /// Gets list of all people.
        /// </summary>
        /// <returns>all people</returns>
        [HttpGet]
        public IEnumerable<Person> GetPerson()
        {
            return _context.Person;
        }

        // GET: api/People/5
        /// <summary>
        /// Gets the person by id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returning the person item</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking if the person exists
            var person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            //returning person
            return Ok(person);
        }

        // PUT: api/People/5
        /// <summary>
        /// Updates the person based on the id. It will be changed to the Person object sent in the body
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="person">The person object.</param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson([FromRoute] int id, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking if Route id and Body id is the same
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            //changing the state to modified
            _context.Entry(person).State = EntityState.Modified;

            //trying to save
            try
            {
                await _context.SaveChangesAsync();
            }
            //if unexpected rows have been changed
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People/add
        /// <summary>
        /// Creating a new person in the database.
        /// </summary>
        /// <param name="person">The person object being posted.</param>
        /// <returns> statuscode</returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostPerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Adding new person
            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
        }

        // POST: api/People/search
        /// <summary>
        /// Search for a person 
        /// </summary>
        /// <param name="currentSearch">The current search.</param>
        /// <returns>a list of matching people</returns>
        [HttpPost]
        [Route("search")]
        public IActionResult PostSearchPerson([FromBody] string currentSearch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking if the database contains any rows where the FirstName matches the search string + wildcard
            var list = _context.Person.Where(c => EF.Functions.Like(c.FirstName, currentSearch+"%"));

            //returning list of matching people
            return Ok(list);
        }



        // DELETE: api/People/5
        /// <summary>
        /// Deletes a person based on the id from the route api/People/{id}.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>status code</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //looking for a person matching the id
            var person = await _context.Person.FindAsync(id);

            //if it doesnt exist return
            if (person == null)
            {
                return NotFound();
            }

            //remove person
            _context.Person.Remove(person);

            //save changes
            await _context.SaveChangesAsync();

            //return deleted person with status code 200
            return Ok(person);
        }

        /// <summary>
        /// Checks if the person exists
        /// </summary>
        /// <param name="id">The identifier for the person.</param>
        /// <returns>Boolean if exists</returns>
        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}