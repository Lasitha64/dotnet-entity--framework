using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppBank;
using System.ServiceModel;
using System.Drawing;
using System.Threading;
using System.Runtime.CompilerServices;
using System.IO;

namespace BankAppBusineesTier
{
    internal class BusinessServer : BusinessServerInterface
    {
        private DataServerInterface server;
        private uint logNumber = 0;
        
        public BusinessServer()
        {
            ChannelFactory<DataServerInterface> dataFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/DataService";
            dataFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            server = dataFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            logNumber = logNumber+1;
            string logDetails = "                   Function : GetNumEntries()                                      \n" +
                ".......................................function executed...........................................\n " +
                "Details : This function calculate the count of all the records in the database and return count    \n"+
                "===================================================================================================\n";
            Log(logDetails);
            return server.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            server.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out icon);
            logNumber = logNumber + 1;
            string logDetails = "                   Function : GetValuesForEntry()                                  \n" +
                ".......................................function executed...........................................\n" +
                "Details : This function will get the index from the user input and display the details of that user\n" +
                "===================================================================================================\n";
            Log(logDetails);
        }

        public void GetValuesForSearch(string searchText, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            logNumber = logNumber + 1;
            string logDetails = "                   Function : GetValuesForSearch()                                 \n" +
                "...................................... function executed.......................................... \n" +
                "Details : This function search user from their lastname as it was match with the given text by user, if matched details will display \n"+
                "===================================================================================================\n"+
                "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Tasks executed :" + logNumber + ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n";
            Log(logDetails);

            acctNo = 0;
            pin = 0;
            bal = 0;
            fName = null;
            lName = null;
            icon = null;

           /* if (searchText.Equals(null))
            {
                Console.WriteLine("String is not matching");
                throw new FaultException<SearchTextNotFound>(new SearchTextNotFound()
                {
                    TextNotFound = "Search text is empty"
                });
            }*/

            int count = server.GetNumEntries();

            for(int i = 0; i<= count-1; i++)
            {
                
                server.GetValuesForEntry(i, out uint se_accNo, out uint se_pin, out int se_bal, out string se_fName, out string se_LName, out Bitmap se_icon);



                if (searchText.Equals(""))
                {
                    throw new FaultException<SearchTextNotFound>(new SearchTextNotFound()
                    {
                        TextNotFound = "Search text is empty"
                    });
                }
                if (se_LName.ToLower().Contains(searchText.ToLower()))
                {
                    acctNo = se_accNo;
                    pin = se_pin;
                    bal = se_bal;
                    fName = se_fName;
                    lName = se_LName;
                    icon = se_icon;
                    break;
                }
                if(i == count-1)
                {
                    //Console.WriteLine(!se_LName.ToLower().Contains(searchText.ToLower()));
                    Console.WriteLine("String is not matching");
                    throw new FaultException<SearchTextNotFound>(new SearchTextNotFound()
                    {
                        TextNotFound = "Search text not found"
                    });
                }


               

            }
            Thread.Sleep(2000);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string LogString)
        {
            Console.Write(LogString);
            
            string loglocation = Directory.GetCurrentDirectory() + @"\logs\logs.txt";
            using (StreamWriter sw = new StreamWriter(loglocation))
            {
                sw.WriteLine(LogString);
                sw.Close();
            }
        }


    }
}
