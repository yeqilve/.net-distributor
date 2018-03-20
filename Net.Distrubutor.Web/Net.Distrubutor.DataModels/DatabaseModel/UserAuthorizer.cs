using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Distrubutor.DataModels.DatabaseModel
{
    public class UserAuthorizer:BaseDataModel
    {
        public long UserID { get; set; }
        public long SystemID { get; set; }
        public string ModelIDs { get; set; }
    }
}
