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
        [HttpGet]
        public IEnumerable<TokenValidator> GetTokenValidator()
        {
            return _context.TokenValidator;
        }

        // GET: api/TokenValidators/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTokenValidator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenValidator = await _context.TokenValidator.FindAsync(id);

            if (tokenValidator == null)
            {
                return NotFound();
            }

            return Ok(tokenValidator);
        }

        // PUT: api/TokenValidators/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTokenValidator([FromRoute] int id, [FromBody] TokenValidator tokenValidator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tokenValidator.TokenId)
            {
                return BadRequest();
            }

            _context.Entry(tokenValidator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
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

            return NoContent();
        }

        // POST: api/TokenValidators
        [HttpPost]
        public async Task<IActionResult> PostTokenValidator([FromBody] TokenValidator tokenValidator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TokenValidator.Add(tokenValidator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTokenValidator", new { id = tokenValidator.TokenId }, tokenValidator);
        }

        // DELETE: api/TokenValidators/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTokenValidator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenValidator = await _context.TokenValidator.FindAsync(id);
            if (tokenValidator == null)
            {
                return NotFound();
            }

            _context.TokenValidator.Remove(tokenValidator);
            await _context.SaveChangesAsync();

            return Ok(tokenValidator);
        }

        private bool TokenValidatorExists(int id)
        {
            return _context.TokenValidator.Any(e => e.TokenId == id);
        }
    }
}