using MovieHunter.DataAccessCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHunter.RESTApi.Controllers
{
    public class SharedControllerFuntions
    {


        public static Nullable<int> TokenVerificator(string token)
        {
            fredrifoContext _context = new fredrifoContext();

            //TODO: check if the token is valid  var list = _context.TokenValidator.Where(d => d.Token == token && d.ValidFrom,...);
            var list = _context.TokenValidator.Where(d => d.Token == token);
            User tokenOwner = new User();

            if (list == null)
            {
                //The token does not excist in the database
                return null;
            }

            //The list should contain 1 item. if it contains multiple items it will use the last one.
            foreach (TokenValidator u in list)
            {
                tokenOwner.UserId = u.UserId;
            }

            

            //The token excists. 
            return tokenOwner.UserId;
        }
    }
}
