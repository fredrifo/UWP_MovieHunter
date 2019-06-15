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
    //json output
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly FredrifoContext _context;

        public UsersController(FredrifoContext context)
        {
            _context = context;
        }

        // GET: api/Users
        /// <summary>
        /// Getting all existing users. This should be deleted if the app is published. Huge security flaw.
        /// </summary>
        /// <returns>all users</returns>
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.User;
        }

        // GET: api/Users/5
        /// <summary>
        /// Get user by id. 
        /// </summary>
        /// <param name="id">The User identifier.</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //checks if user exists
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        /// <summary>
        /// Updating the user that has the same id as in route. The User object in the body will replace it.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            //Checking if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //checking if Route And body id is matching.
            if (id != user.UserId)
            {
                return BadRequest();
            }

            //Changing state to modified
            _context.Entry(user).State = EntityState.Modified;

            //Trying to save
            try
            {
                await _context.SaveChangesAsync();
            }
            //catching unexpected amount of rows are affected during save.
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

            //Return status code 204
            return NoContent();
        }

        // POST: api/Users
        /// <summary>
        /// Creates a new users. 
        /// If username exists, do not create. 
        /// Adds the two premade lists: ToWatch and Watched.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<JsonResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Result = "Failed" } );
            }

            //Returns the lists that belogs to the token owner (user id)
            var usersCheckIfExists = _context.User.Where(c => c.UserName == user.UserName);

            if (usersCheckIfExists.Any())
            {
                //User exists
                return Json(new { Result = "User exists" });
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
        /// <summary>
        /// Login as a post request for added security. You should never have username and password in the url header...
        /// Generating a login token (random string). This token is sent to the client. The token along with userId and validTo is added to the database
        /// This way the api can check validity off all tokens and fetch User Ids. This way noone can fetch user information without having a valid token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Json string containing the token</returns>
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
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier for the user that is being deleted.</param>
        /// <returns>Status and deleted user object</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checks if the user exists 
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //Removing user
            _context.User.Remove(user);

            //Saving changes
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// Checks if a user exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Boolean if exist</returns>
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}