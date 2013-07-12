using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LunchTrainWeb.Data
{
    public class RestaurantVotes
    {
        public string RestaurantName;
        public List<string> VoterNames = new List<string>();
    }
}