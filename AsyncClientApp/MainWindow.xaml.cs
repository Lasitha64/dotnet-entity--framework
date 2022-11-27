using BankAppBusineesTier;
using Super_Awseome_Library_Project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncClientApp
{
    public delegate DataStruct Search(string value);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface dataif;
        private string searchValue;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/BankService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            dataif = foobFactory.CreateChannel();
            //Also, tell me how many entries are in the DB.
        }

        private void Button1_Click_1(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;

            //On click, Get the index....
            try
            {
                index = Int32.Parse(InputNum.Text);
                if (index >= 0 && index <= 999)
                {
                    //Then, run our RPC function, using the out mode parameters...
                    dataif.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out Bitmap icon);
                    //And now, set the values in the GUI!
                    fname_tb.Text = fName;
                    lname_tb.Text = lName;
                    bal_tb.Text = bal.ToString("C");
                    accno_tb.Text = acct.ToString();
                    pin_tb.Text = pin.ToString("D4");

                    PictureBox.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    icon.Dispose();
                }
                else
                {
                    this.TotalNum.Content = "Please Enter a number from 0 to 999";
                }
            }
            catch (System.FormatException fe)
            {
                this.TotalNum.Content = fe.Message;
            }
            catch (System.ArgumentException ae)
            {
                this.TotalNum.Content = ae.Message;
            }



        }
        private void GetCount(object sender, RoutedEventArgs e)
        {
            this.TotalNum.Content = "NO of counts :" + dataif.GetNumEntries().ToString();
        }

        private async void Search_Btn_Click(object sender, RoutedEventArgs e)
        {
            Search_Btn.IsEnabled = false;
            Button1.IsEnabled = false;
            Search_Pb.Visibility = Visibility.Visible;
            searchValue = SearchText.Text;
            try
            {
                Task<DataStruct> task = new Task<DataStruct>(SearchDB);
                task.Start();
                this.TotalNum.Content = "Searching starts...........";
                DataStruct dataStruct = await task;
                UpdateGui(dataStruct);
                this.TotalNum.Content = "Searching ends............";

            }
            catch (FaultException<SearchTextNotFound> exception)
            {
                TotalNum.Content = exception.Detail.TextNotFound;
            }
            Search_Pb.Visibility = Visibility.Hidden;
            Search_Btn.IsEnabled = true;
            Button1.IsEnabled = true;
            
   

        }

        private DataStruct SearchDB()
        {
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;


            dataif.GetValuesForSearch(searchValue, out acct, out pin, out bal, out fName, out lName, out Bitmap icon);
            if (acct != 0)
            {
                DataStruct dataStruct = new DataStruct();
                dataStruct.acctNo = acct;
                dataStruct.balance = bal;
                dataStruct.pin = pin;
                dataStruct.firstName = fName;
                dataStruct.lastName = lName;
                dataStruct.icon = icon;
                return dataStruct;
            }
            return null;
        }

        private void UpdateGui(DataStruct dataStruct)
        {
            fname_tb.Text = dataStruct.firstName;
            lname_tb.Text = dataStruct.lastName;
            accno_tb.Text = dataStruct.acctNo.ToString();
            pin_tb.Text = dataStruct.pin.ToString("D4");
            bal_tb.Text = dataStruct.balance.ToString("C");
            PictureBox.Source = Imaging.CreateBitmapSourceFromHBitmap(dataStruct.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            dataStruct.icon.Dispose();
        }
    }

}
