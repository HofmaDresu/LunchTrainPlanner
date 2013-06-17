using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPersistence.Implementation
{
    public interface IDataAccess
    {
        void StoreSuggestion(string suggestion);
        int ClearSuggestionSession();
        void removeSuggestion(string suggestion);
        string GetSuggestions();
    }
}
