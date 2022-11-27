using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Super_Awseome_Library_Project
{
    public class DatabaseClass
    {
        List<DataStruct> dataStruct;
        public DatabaseClass()
        {        
            dataStruct = new List<Super_Awseome_Library_Project.DataStruct>();

            int arr = 20;
            DataGen[] dg = new DataGen[arr];
            DataStruct[] ds = new DataStruct[arr];

            for (int i = 0; i < arr; i++)
            {
                dg[i] = new DataGen();
                

                dg[i].GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string
    lastName, out int balance, out Bitmap icon);

                ds[i] = new DataStruct();

                ds[i].pin = pin;
                ds[i].acctNo = acctNo;
                ds[i].firstName = firstName;
                ds[i].lastName = lastName;
                ds[i].balance = balance;
                ds[i].icon = icon;

                dataStruct.Add(ds[i]);
            
            }

                
            
 
        }

        public uint GetAcctNoByIndex(int index) {
            uint accNo = dataStruct.ElementAt(index).acctNo;
            return accNo;
        }
        public uint GetPINByIndex(int index) {
            uint pin = dataStruct.ElementAt(index).pin;
            return pin;
        }
        public string GetFirstNameByIndex(int index) {
            string fname = dataStruct.ElementAt(index).firstName;
            return fname;
        }
        public string GetLastNameByIndex(int index) {
            string lname = dataStruct.ElementAt(index).lastName;
            return lname;
        }

        public int GetBalanceByIndex(int index)
        {
            int balance = dataStruct.ElementAt(index).balance;
            return balance;
        }

        public Bitmap GetIconByIndex(int index) { 
            Bitmap icon = dataStruct.ElementAt(index).icon;
            return icon;
        }

        public int GetNumRecords()
        {
            int count = dataStruct.Count();
            return count;
        }
    }



}
