using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTrainWeb.Data
{
    public interface IDAO
    {
        void VoteForRestaurant(string Username, string Ip, string restaurant);
        void UnVoteForRestaurant(string Username, string Ip, string restaurant);
        IList<RestaurantVotes> GetCurrentVotes();
        void ClearVotes();
    }
}
