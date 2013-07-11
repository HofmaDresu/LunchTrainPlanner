using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LunchTrainWeb.Data
{
    public class DAO : IDAO
    {
        public void VoteForRestaurant(string Username, string Ip, string restaurant)
        {
            throw new NotImplementedException();
        }

        public void UnVoteForRestaurant(string Username, string Ip, string restaurant)
        {
            throw new NotImplementedException();
        }

        public IList<RestaurantVotes> GetCurrentVotes()
        {
            throw new NotImplementedException();
        }
    }
}