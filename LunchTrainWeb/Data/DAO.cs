using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace LunchTrainWeb.Data
{
    public class DAO : IDAO
    {
        private IRedisClient _client;
        private readonly string RESTAURANT_LIST = "RestaurantList";
        private readonly string BANNED_SET = "BannedUserSet";

        public DAO()
        {
            _client = new RedisClient();
            //Set to 1 to avoid conflicts with any default installs
            //TODO: make configurable
            _client.Db = 1;
        }

        public void VoteForRestaurant(string Username, string Ip, string restaurant)
        {
            AddItemToSetAndSetExpiration(restaurant, GetUniqueUsername(Username, Ip));
            AddItemToSetAndSetExpiration(RESTAURANT_LIST, restaurant);            
        }

        private void AddItemToSetAndSetExpiration(string listName, string item)
        {
            var tomorrow = DateTime.Now.AddDays(1).Date;
            _client.AddItemToSet(listName, item);
            _client.ExpireEntryAt(listName, tomorrow);
        }

        private string GetUniqueUsername(string Username, string Ip)
        {
            return string.Join(":", Username, Ip);
        }

        public void UnVoteForRestaurant(string Username, string Ip, string restaurant)
        {
            _client.RemoveItemFromSet(restaurant, GetUniqueUsername(Username, Ip));
        }

        public IList<RestaurantVotes> GetCurrentVotes()
        {
            var restaurants = _client.GetAllItemsFromSet(RESTAURANT_LIST);
            var allVotes = new List<RestaurantVotes>();
            return restaurants.Select(r => {
                var votes = new RestaurantVotes { RestaurantName = r };
                votes.VoterNames.AddRange(_client.GetAllItemsFromSet(r).Select(u => u.Split(':').First()));
                return votes;
            }).OrderBy(v => v.VoterNames.Count()).ThenBy(v => v.RestaurantName).ToList();
        }


        public void ClearVotes()
        {
            _client.FlushDb();
        }


        public void BanUser(string Username, string Ip)
        {
            AddItemToSetAndSetExpiration(BANNED_SET, GetUniqueUsername(Username, Ip));
        }

        public bool IsUserBanned(string Username, string Ip)
        {
            return _client.SetContainsItem(BANNED_SET, GetUniqueUsername(Username, Ip));
        }
    }
}