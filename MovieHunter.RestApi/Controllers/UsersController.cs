using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MovieHunter.DataAccessCore.Models;


namespace MovieHunter.RESTApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly fredrifoContext _context;

        public UsersController(fredrifoContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.User;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        [Route("register")]
        public async Task<JsonResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Result = "Failed" } );
            }


            //Creating a new user. This code is not necessary due ti _context.List.Add will automatically create the user if it does not exist
            _context.User.Add(user);


            //Adding the two default lists for "ToWatch" and "Watched".
            _context.List.Add(new List() { ListName = "ToWatch", User = user });
            _context.List.Add(new List() { ListName = "Watched", User = user});


            //Updating the tables
            await _context.SaveChangesAsync();

            //Returning a Json message to the user client
            return Json(new { Result = "Registered" } );
        }

        // POST: api/Users
        [HttpPost]
        [Route("login")]
        public async Task<JsonResult> Post([FromBody] User user)
        {

            var allUsers = _context.User;
            User existingUser = null;

            //Checking if user exist and password is correct
            //Can replace with something like this: var list = _context.List.Where(c => c.UserId == id);
            foreach (User u in allUsers){
                if(u.UserName == user.UserName && u.Password == user.Password)
                {
                    existingUser = u;
                }
            }
            


            if (existingUser == null)
            {
                return Json(new
                {
                    Result = "Failed",
                    Input = "Username: " + user.UserName.ToString() + ", Password: " + user.Password,
                    Token = "Null"
                });
            }

            //Generates a token that will be used for clients to communicate with the server. 
            string token = Validator.GenerateToken(user.UserName, user.Password, DateTime.Now, user.UserId);

            
            //Generates a TokenValidator object that contains all of the information required by the database.
                //Valid from logs whenever a user logs in. ValidTo decides if the token is still valid.
               
            TokenValidator tokenObject = new TokenValidator() { Token = token, UserId = existingUser.UserId, ValidFrom = DateTime.Now, ValidTo = DateTime.Now.AddDays(2) };

            //Adding the new token Object to the database
            //Users can now send in their token string, and the server will know what tables to display based on the userId
            _context.TokenValidator.Add(tokenObject);



            //Updating the database
            await _context.SaveChangesAsync(); 


            //Returns a string with information to the client. The client will save the token for his session.
            return Json(new
            {
                //Tells the userclient that a new user has been created successfully.
                Result = "Success",

                //Returning the input values sent from the user client. This can be removed.
                Input = "Username: " + user.UserName.ToString() + ", Password: " + user.Password,

                //Return a token to the user client. Whenever database actions happen, the user will send this token in to a verificator.
                Token = token
            });
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}