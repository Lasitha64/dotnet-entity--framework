using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Super_Awseome_Library_Project;


namespace ConsoleAppBank
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {
        DatabaseClass databaseClass = new DatabaseClass();
        public DataServer() {


               Console.WriteLine("Constructor Called");           
               
               Console.WriteLine("Number of entries are equal to :" + GetNumEntries());

            for (int i = 0; i < GetNumEntries(); i++)
            {
                GetValuesForEntry(i, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);
                Console.WriteLine("Account Number : " + acctNo);
                Console.WriteLine("PIN : " + pin);
                Console.WriteLine("Account Balance : " + bal);
                Console.WriteLine("First Name : " + fName);
                Console.WriteLine("Last Name : " + lName);
                Console.WriteLine("========================================================");

            }





        }

        public int GetNumEntries()
        {
           return databaseClass.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            if(index < 0 || index >= databaseClass.GetNumRecords())
            {
                Console.WriteLine("index is out of range");
                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault()
                {
                    Issue = "Index was not in range"
                });
            }
            acctNo =  databaseClass.GetAcctNoByIndex(index);
            pin = databaseClass.GetPINByIndex(index);
            bal = databaseClass.GetBalanceByIndex(index);
            fName = databaseClass.GetFirstNameByIndex(index);
            lName = databaseClass.GetLastNameByIndex(index);
            icon = new Bitmap(databaseClass.GetIconByIndex(index));
        }


    }
}
