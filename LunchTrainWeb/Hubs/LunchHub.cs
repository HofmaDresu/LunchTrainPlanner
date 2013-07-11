using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LunchTrainWeb.Data;
using System.IO;

namespace LunchTrainWeb.Hubs
{
    public class LunchHub : Hub
    {
        private IDAO _dao;

        public LunchHub(IDAO dao)
        {
            _dao = dao;
        }

        public void VoteForRestaurant(string name, string suggestion)
        {
            ChangeVote(_dao.VoteForRestaurant, name, suggestion);
        }

        public void UnVoteForRestaurant(string name, string suggestion)
        {
            ChangeVote(_dao.UnVoteForRestaurant, name, suggestion);
        }

        private void ChangeVote(Action<string, string, string> act, string name, string suggestion)
        {
            act(name, GetRemoteAddress(), suggestion);
            RefreshClients();
        }

        private string GetRemoteAddress()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
        }

        private void RefreshClients()
        {
            var sw = GetSerializedVotes();
            Clients.All.RefreshVotes(sw);
        }

        private StringWriter GetSerializedVotes()
        {
            var serializer = new Microsoft.AspNet.SignalR.Json.JsonNetSerializer();
            var sw = new StringWriter();
            serializer.Serialize(_dao.GetCurrentVotes().OrderByDescending(r => r.VoterNames.Count()), sw);
            return sw;
        }
    }
}