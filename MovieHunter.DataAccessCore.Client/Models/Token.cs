using System;
using System.Collections.Generic;
using System.Text;

namespace MovieHunter.DataAccessCore.Client.Models
{
    public class Token
    {
        public int UserId
        {
            get;set;
        }

        public DateTime Timestamp
        {
            get;set;
        }

        public string ValidationToken
        {
            get;set;
        }
    }
}
