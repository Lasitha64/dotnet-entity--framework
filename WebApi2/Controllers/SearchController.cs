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
using System.Web.Http.Description;

namespace WebApi2.Controllers
{
    public class SearchController : ApiController
    {

        private DataServerInterface server;
        ChannelFactory<DataServerInterface> dataFactory;
        NetTcpBinding tcp = new NetTcpBinding();
        string URL = "net.tcp://localhost:8100/DataService";


        // POST: api/Search
        [ResponseType(typeof(DataIntermed))]
        public IHttpActionResult Post([FromBody]SearchData search)
        {

            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            server = dataFactory.CreateChannel();
            

            for (int i = 0; i < server.GetNumEntries(); i++)
            {
                server.GetValuesForEntry(i, out uint se_accNo, out uint se_pin, out int se_bal, out string se_fName, out string se_LName, out Bitmap se_icon);
                if (se_LName.ToLower().Contains(search.searchStr.ToLower()))
                {
                    Bitmap bImage = se_icon;  // Your Bitmap Image
                    System.IO.MemoryStream ms = new MemoryStream();
                    bImage.Save(ms, ImageFormat.Jpeg);
                    byte[] byteImage = ms.ToArray();
                    var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64


                    DataIntermed dataIntermed = new DataIntermed();
                    dataIntermed.acct = se_accNo;
                    dataIntermed.pin = se_pin;
                    dataIntermed.bal = se_bal;
                    dataIntermed.fname = se_fName;
                    dataIntermed.lname = se_LName;
                    dataIntermed.icon = SigBase64;

                    return Ok(dataIntermed);
                }

            }

            return null;
            

        }


    }
}
