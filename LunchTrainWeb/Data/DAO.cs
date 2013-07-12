using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace LunchTrainWeb.Data
{
    public class DAO : IDAO
    {
        public void VoteForRestaurant(string Username, string Ip, string restaurant)
        {
        }

        public void UnVoteForRestaurant(string Username, string Ip, string restaurant)
        {
            throw new NotImplementedException();
        }

        public IList<RestaurantVotes> GetCurrentVotes()
        {
            return new List<RestaurantVotes>
            {
                new RestaurantVotes {
                    RestaurantName = "Bankok Sala",
                    VoterNames = new List<string> {"Matt", "Will"}
                },
                new RestaurantVotes {
                    RestaurantName = "Basement Burger Bar",
                    VoterNames = new List<string> {"Mohammed"}
                }
            };
        }
    }
}