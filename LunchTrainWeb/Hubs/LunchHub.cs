using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LunchTrainWeb.Data;
using System.IO;
using System.Threading.Tasks;

namespace LunchTrainWeb.Hubs
{
    public class LunchHub : Hub
    {
        private IDAO _dao;

        public LunchHub(IDAO dao)
        {
            _dao = dao;
        }

        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected();
        }

        public void VoteForRestaurant(string name, string suggestion)
        {
            if (!VerifyUserName(name) || !VerifySuggestion(name, suggestion))
            {
                return;
            }
            ChangeVote(_dao.VoteForRestaurant, name, suggestion);
        }

        public void UnVoteForRestaurant(string name, string suggestion)
        {
            if (!VerifyUserName(name) || !VerifySuggestion(name, suggestion))
            {
                return;
            }
            ChangeVote(_dao.UnVoteForRestaurant, name, suggestion);
        }

        public void GetVotes()
        {
            RefreshClients(Clients.Caller);
        }

        public void RequestClearVotes(string name)
        {
            if(!VerifyUserName(name))
            {
                return;
            }
            if (UserHandler.ConnectedIds.Count > 1)
	        {
                Clients.Others.RequestClear(name);
	        }
            else
            {
                _dao.ClearVotes();
                RefreshClients(Clients.All);
            }
        }

        public void ConfirmClearVotes()
        {
            _dao.ClearVotes();
            RefreshClients(Clients.All);
            Clients.All.CancelClearRequest();
        }

        public void CancelClearVotes()
        {
            Clients.Others.CancelClearRequest();
        }

        private void ChangeVote(Action<string, string, string> act, string name, string suggestion)
        {
            act(name, GetRemoteAddress(), suggestion);
            RefreshClients(Clients.All);
        }

        private string GetRemoteAddress()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
        }

        private void RefreshClients(dynamic clientsToRefresh)
        {
            clientsToRefresh.RefreshVotes(_dao.GetCurrentVotes().OrderByDescending(r => r.VoterNames.Count()).ToList());
        }

        private Boolean VerifyUserName(string submittedUserName)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ActivateBanHammer"].ToLower() == "true")
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
                {
                    var serverUserName = System.Web.HttpContext.Current.User.Identity.Name;
                    if (submittedUserName != serverUserName || _dao.IsUserBanned(serverUserName, GetRemoteAddress()))
                    {
                        BanUser(serverUserName);
                        return false;
                    }
                }
            }
            return true;
        }

        private Boolean VerifySuggestion(string submittedUserName, string suggestion)
        {
            if (suggestion.Length > 16)
            {
                BanUser(submittedUserName);
                return false;
            }
            return true;
        }

        private void BanUser(string serverUserName)
        {
            _dao.BanUser(serverUserName, GetRemoteAddress());
            Clients.Caller.RedirectMe("Home/Ban");
        }
    }

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
}