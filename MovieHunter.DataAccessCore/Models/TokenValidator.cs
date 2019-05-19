using System;
using System.Collections.Generic;
using System.Text;

namespace MovieHunter.DataAccessCore.Models
{
    public class TokenValidator
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int UserId { get; set; }
    }
}
