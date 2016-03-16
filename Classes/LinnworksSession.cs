using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LowStockApp.Classes
{
    public class LinnworksSession
    {
        public string Locality;
        public string Server;
        public string Token;
        public string Id;
        public StatusDetails Status = new StatusDetails();
    }

    public class StatusDetails
    {
        public string Reason;
        public string State;
    }
}