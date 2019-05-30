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
    public class TokenValidatorsController : ControllerBase
    {
        private readonly fredrifoContext _context;

        public TokenValidatorsController(fredrifoContext context)
        {
            _context = context;
        }

        // GET: api/TokenValidators
        /// <summary>
        /// Gets all the tokens from the database.
        /// </summary>
        /// <returns>Returns TokenValidator object</returns>
        [HttpGet]
        public IEnumerable<TokenValidator> GetTokenValidator()
        {
            return _context.TokenValidator;
        }

        // GET: api/TokenValidators/5
        /// <summary>
        /// Gets the token validator based on id from route.
        /// </summary>
        /// <param name="id">The TokenValidator Id</param>
        /// <returns>Matching TokenValidator</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTokenValidator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get tokenValidator with Id
            var tokenValidator = await _context.TokenValidator.FindAsync(id);

            if (tokenValidator == null)
            {
                return NotFound();
            }

            //Return token
            return Ok(tokenValidator);
        }

        // PUT: api/TokenValidators/5
        /// <summary>
        /// Update the TokenValidator.
        /// </summary>
        /// <param name="id">The identifier of the TokenValidator that will be updated.</param>
        /// <param name="tokenValidator">The token validator that will replace the old one.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTokenValidator([FromRoute] int id, [FromBody] TokenValidator tokenValidator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking if the id matched the object Id
            if (id != tokenValidator.TokenId)
            {
                return BadRequest();
            }

            //Changing state
            _context.Entry(tokenValidator).State = EntityState.Modified;


            //Trying to save
            try
            {
                await _context.SaveChangesAsync();
            }
            //Catches unexpected rows affected
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenValidatorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //Return No Content response 204
            return NoContent();
        }

        // POST: api/TokenValidators
        /// <summary>
        /// Posts a new token validator.
        /// </summary>
        /// <param name="tokenValidator">The token validator.</param>
        /// <returns>Status code</returns>
        [HttpPost]
        public async Task<IActionResult> PostTokenValidator([FromBody] TokenValidator tokenValidator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Adding tokenValidator
            _context.TokenValidator.Add(tokenValidator);

            //Saving changes
            await _context.SaveChangesAsync();

            //return response code 201 (Created response)
            return CreatedAtAction("GetTokenValidator", new { id = tokenValidator.TokenId }, tokenValidator);
        }


        // DELETE: api/TokenValidators/5
        /// <summary>
        /// Deletes the token validator by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>status code with deleted object</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTokenValidator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Looks for matching id
            var tokenValidator = await _context.TokenValidator.FindAsync(id);

            //if no matching id
            if (tokenValidator == null)
            {
                return NotFound();
            }

            //delete mathing id
            _context.TokenValidator.Remove(tokenValidator);

            //save changes
            await _context.SaveChangesAsync();

            return Ok(tokenValidator);
        }

        /// <summary>
        /// Checks if the tokens validator exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true or false</returns>
        private bool TokenValidatorExists(int id)
        {
            return _context.TokenValidator.Any(e => e.TokenId == id);
        }
    }
}