using ApiClasses;
using ConsoleAppBank;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace WebApi2.Controllers
{
    public class GetAllController : ApiController
    {
        private DataServerInterface server;
        ChannelFactory<DataServerInterface> dataFactory;
        NetTcpBinding tcp = new NetTcpBinding();
        string URL = "net.tcp://localhost:8100/DataService";

        //Set the URL and create the connection!

        // GET: api/GetAll/5
        public IHttpActionResult Get(int id)
        {

            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            server = dataFactory.CreateChannel();

            server.GetValuesForEntry(id, out uint accNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);

            Bitmap bImage = icon;  // Your Bitmap Image
            System.IO.MemoryStream ms = new MemoryStream();
            bImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64


            DataIntermed dataIntermed = new DataIntermed();
            dataIntermed.acct = accNo;
            dataIntermed.pin = pin;
            dataIntermed.bal = bal;
            dataIntermed.fname = fName;
            dataIntermed.lname = lName;
            dataIntermed.icon = SigBase64;

            return Ok(dataIntermed);

        }

       
    }
}
