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
    public class ListItemsController : ControllerBase
    {
        private readonly FredrifoContext _context;

        public ListItemsController(FredrifoContext context)
        {
            _context = context;
        }

        // GET: api/ListItems
        /// <summary>Gets all listItems</summary>
        /// <returns>The list of all listitems</returns>
        [HttpGet]
        public IEnumerable<ListItem> GetListItem()
        {
            return _context.ListItem;
        }

        // GET: api/ListItems/5
        /// <summary>Gets the list item that has the id specified in the parameter.</summary>
        /// <param name="id">  list item id</param>
        /// <returns>returns the list item object</returns>
        [HttpGet("{id}")]
        public IActionResult GetListItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listItem = _context.ListItem.Where(c => c.ListId == id);
            //var listItem = await _context.ListItem.FindAsync(id);

            if (listItem == null)
            {
                return NotFound();
            }

            //returning the list item object
            return Ok(listItem);
        }


        // PUT: api/ListItems/5
        /// <summary>  Replacing a listitem with a new object</summary>
        /// <param name="id">  The id of the object that will be replaced</param>
        /// <param name="listItem">  The object that will replace the list</param>
        /// <returns>Returns the result</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListItem([FromRoute] int id, [FromBody] ListItem listItem)
        {
            //checking the state of the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //If the id of the list item that is gonna be replaced isnt the same as the object, return a status code 400 
            if (id != listItem.ListItemId)
            {
                return BadRequest();
            }

            //Sets the state to modified
            _context.Entry(listItem).State = EntityState.Modified;

            //Tries to save changes
            try
            {
                await _context.SaveChangesAsync();
            }

            // checking an unexpected number of rows has been modified
            catch (DbUpdateConcurrencyException)
            {
                if (!ListItemExists(id))
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

        // POST: api/ListItems
        /// <summary> Posting a new ListItem. If it aready exists in the list do nothing.</summary>
        /// <param name="listItem">The list item.</param>
        /// <returns>Status of the post</returns>
        [HttpPost]
        public async Task<IActionResult> PostListItem([FromBody] ListItem listItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Checking if the list exists. It exists only if the Movie ID exists in the the same List its being added to
            var listIdenticals = _context.ListItem.Where(c => ((c.ListId == listItem.ListId)&& (c.MovieId == listItem.MovieId)));
            
            //If there are more than 0 identical listitems in the list
            if (listIdenticals.Count() > 0)
            {
                //Cant be added as it is already in the list
                //Return status code 201 (It was successfull because it is in the list)
                return CreatedAtAction("GetListItem", new { id = listItem.ListItemId }, listItem);
            }
            _context.ListItem.Add(listItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListItem", new { id = listItem.ListItemId }, listItem);
        }

        // DELETE: api/ListItems/5
        /// <summary>Deletes a list item by looking at the id in the url</summary>
        /// <param name="id">  The listItemId of the ListItem that will be deleted</param>
        /// <returns>Status and item deleted</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // checking if the ListItem exists
            var listItem = await _context.ListItem.FindAsync(id);
            if (listItem == null)
            {
                // No listItem exists
                return NotFound();
            }

            //Removing item
            _context.ListItem.Remove(listItem);

            //saving changes
            await _context.SaveChangesAsync();

            return Ok(listItem);
        }

        /// <summary>  Checks if there exists a list item with matching id.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Bool for if it exists</returns>
        private bool ListItemExists(int id)
        {
            return _context.ListItem.Any(e => e.ListItemId == id);
        }
    }
}