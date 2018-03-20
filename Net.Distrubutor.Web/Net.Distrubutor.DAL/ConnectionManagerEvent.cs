using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Distrubutor.DAL
{
    public delegate void ConnectionConfiguration();
    

    public abstract class ConnectionManagerEvent
    {
        public event ConnectionConfiguration configrationEvent;

        //refresh the db connection

        public void Invoke()
        {
            //log
            //execute();
            
            configrationEvent();
            //            
        }
    }
}
