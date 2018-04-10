using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Distrubutor.DAL
{
    public delegate void ConnectionConfiguration();
    
    /// <summary>
    /// refresh the all db connection when connect failed
    /// </summary>
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
