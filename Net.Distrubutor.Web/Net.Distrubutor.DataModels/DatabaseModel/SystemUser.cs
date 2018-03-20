using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Distrubutor.DataModels.DatabaseModel
{
    public class SystemUser : BaseDataModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        
    }
}
