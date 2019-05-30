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
    public class ListsController : ControllerBase
    {
        private readonly fredrifoContext _context;

        public ListsController(fredrifoContext context)
        {
            _context = context;
        }

        // GET: api/Lists
        /// <summary>Gets the all Lists</summary>
        /// <returns>All lists in a list..</returns>
        [HttpGet]
        public IEnumerable<List> GetList()
        {
            return _context.List;
        }

        // GET: api/Lists/userLists

        /// <summary>  Get all userowned lists by checking the token that was sent from the body.</summary>
        /// <param name="token">The token.</param>
        /// <returns>Lists that belong to the token-owner</returns>
        [HttpPost]
        [Route("userLists")]
        public async Task<IActionResult> GetList([FromBody] string token)
        {
            //Since the token is being transferred its using HttpPost. 
            //This is because HttpGet sends the information in the header. 
            //The header is easy to see by people monitoring the network. 

            //checks the modelstate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Gets the id connected to the token
            Nullable<int> id = SharedControllerFuntions.TokenVerificator(token);

            //ID only excists if it gets an int as the return value
            if (id == null)
            {
                return NotFound();
            }

            //Returns the lists that belogs to the token owner (user id)
            var list = _context.List.Where(c => c.UserId == id);

            //Empty list
            if (list == null)
            {
                return NotFound();
            }

            //Returning the lists
            return Ok(list);
        }

        // PUT: api/Lists/5
        /// <summary>  Updates a list [Put]. specify id of the list in parameter. the body must contain the modified List object</summary>
        /// <param name="id">The identifier for the List that will be updated.</param>
        /// <param name="list">  The List object that will replace the old one.</param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutList([FromRoute] int id, [FromBody] List list)
        {
            //Checking modelstate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //cheking if the id and listId is the same
            if (id != list.ListId)
            {
                return BadRequest();
            }

            _context.Entry(list).State = EntityState.Modified;

            //Trying to save the changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //Returning statuscode 204 for No content in response
            return NoContent();
        }

        // POST: api/Lists/
        /// <summary>
        /// Postsing a new list object to the database.
        /// </summary>
        /// <param name="list">The list object</param>
        /// <returns>Status code</returns>
        [HttpPost]
        public async Task<IActionResult> PostList([FromBody] List list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            //adding to the database
            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetList", new { id = list.ListId }, list);
        }

        // DELETE: api/Lists/5
        /// <summary>
        /// Deletes the listobject matching the id in the parameter
        /// </summary>
        /// <param name="id">The ListId.</param>
        /// <returns>Statuscode</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Looking for the id from the parameter
            var list = await _context.List.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            //If the id exists it will be removed
            _context.List.Remove(list);

            //saving changes
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        /// <summary>
        /// Checks if the ListId exists in the database
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>returning a boolean for if it exists</returns>
        private bool ListExists(int id)
        {
            return _context.List.Any(e => e.ListId == id);
        }
    }
}