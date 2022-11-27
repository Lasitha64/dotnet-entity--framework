using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Super_Awseome_Library_Project
{
    internal class DataGen
    {
        static Random random = new Random();
        private List<Bitmap> icons;

        public DataGen()
        {
            icons = new List<Bitmap>();
            for(var i = 0; i < 10; i++)
            {
                var image = new Bitmap(64, 64);
                for(var j = 0; j < 64; j++)
                {
                    for(var k = 0; k < 64; k++)
                    {
                        image.SetPixel(j, k, Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
                    }
                }
                icons.Add(image);
            }
        }

        private uint GetPIN()
        {
           
            return (uint)random.Next(0000, 9999);
        }

        private int GetBalance()
        {

            return random.Next(000000, 999999);
        }

        private uint GetAcctNo()
        {
           
            return (uint)random.Next(00000000, 99999999);
        }

        private string GetFirstname()
        {
            string[] Fname = { "John", "William", "Peter", "Ben", "Alex","Joe","Ross","Chandler","Smith","Tom" };
            int index = random.Next(0, Fname.Length);
            return Fname[index];

        }

        private string GetLastName()
        {
            string[] Lname = { "Stokes", "Cook", "Warner", "Holmes", "Trott", "Kane", "Root", "Stook", "Paul","Don" };
            int index = random.Next(0, Lname.Length);
            return Lname[index];

        }

        private Bitmap GetIcon()
        {
            return icons[random.Next(icons.Count)];
        }


        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string
lastName, out int balance, out Bitmap icon)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstname();
            lastName = GetLastName();
            balance = GetBalance();
            icon = GetIcon();

        }

    }
}
