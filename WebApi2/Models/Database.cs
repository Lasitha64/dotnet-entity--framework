using ApiClasses;
using ConsoleAppBank;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WebApi2.Models
{
    public class Database
    {
        private DataServerInterface server;
        ChannelFactory<DataServerInterface> dataFactory;
        NetTcpBinding tcp = new NetTcpBinding();
        string URL = "net.tcp://localhost:8100/DataService";
        public Database()
        {
            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            server = dataFactory.CreateChannel();
        }

        public string GetCount()
        {
            return server.GetNumEntries().ToString();
        }


    }
}