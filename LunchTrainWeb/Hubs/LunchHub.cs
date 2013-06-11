using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LunchTrainWeb.Hubs
{
    public class LunchHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void AddSuggestion(string name, string suggestion)
        {
            Clients.All.AddSuggestionFromUser(name, suggestion.Replace(' ', '_'),  suggestion);
        }

        public void VoteForSuggestion(string name, string suggestion)
        {
            var count = Int32.Parse(suggestion.Split(':').Last());

            Clients.All.AddVoteFromUser(suggestion.Split(':').First().Trim().Replace(' ', '_'), suggestion.Split(':').First().Trim(), count + 1);
            
        }

        public void RemoveVote(string name, string suggestion)
        {

        }
    }
}