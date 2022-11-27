using ApiClasses;
using ConsoleAppBank;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class ValuesController : ApiController
    {
        

        private DataServerInterface server;
        ChannelFactory<DataServerInterface> dataFactory;
        NetTcpBinding tcp = new NetTcpBinding();
        string URL = "net.tcp://localhost:8100/DataService";


        // GET api/values
        public string Get()
        {
            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            server = dataFactory.CreateChannel();
            return server.GetNumEntries().ToString();
        }

    }
}
