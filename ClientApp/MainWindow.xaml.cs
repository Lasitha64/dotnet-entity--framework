using System;
using System.Windows;
using System.Windows.Media.Imaging;
using BankAppBusineesTier;
using System.ServiceModel;
using System.Drawing;
using System.Windows.Interop;
using Super_Awseome_Library_Project;
using System.Runtime.Remoting.Messaging;

namespace ClientApp
{
    public delegate DataStruct Search(string value);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface dataif;
        private Search search;


        public MainWindow()
        {
            //This is a factory that generates remote connections to our remote class. This
//is what hides the RPC stuff!

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
            try {
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
            catch(System.FormatException fe)
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

        private void Search_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            Search_Btn.IsEnabled = false;
            Button1.IsEnabled = false;
            ShowProgressBar();

            search = SearchDB;
            AsyncCallback callback;
            callback = this.OnSearchCompletion;
            IAsyncResult result = search.BeginInvoke(SearchText.Text, callback, null);
            

            /* string fName = "", lName = "";
             int bal = 0;
             uint acct = 0, pin = 0;
             try
             {
                 dataif.GetValuesForSearch(SearchText.Text, out acct, out pin, out bal, out fName, out lName, out Bitmap icon);

                 fname_tb.Text = fName;
                 lname_tb.Text = lName;
                 bal_tb.Text = bal.ToString("C");
                 accno_tb.Text = acct.ToString();
                 pin_tb.Text = pin.ToString("D4");

                 PictureBox.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                 icon.Dispose();
             }
             catch(System.ServiceModel.FaultException fe)
             {
                 this.TotalNum.Content = "Search text not found";
             }
             catch(System.ServiceModel.CommunicationObjectFaultedException ofe)
             {
                 this.TotalNum.Content = "Search text not found";
             }*/

        }

        private DataStruct SearchDB(string value)
        {
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            

            dataif.GetValuesForSearch(value, out acct, out pin, out bal, out fName, out lName, out Bitmap icon);
            if(acct != 0)
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

        private void UpdateGUI(DataStruct dataStruct)
        {
                fname_tb.Dispatcher.Invoke(new Action(() =>
                {
                   
                    fname_tb.Text = dataStruct.firstName;

                }));

                lname_tb.Dispatcher.Invoke(new Action(() =>
                {
                    lname_tb.Text = dataStruct.lastName;

                }));

                accno_tb.Dispatcher.Invoke(new Action(() =>
                {
                    accno_tb.Text = dataStruct.acctNo.ToString();

                }));

                pin_tb.Dispatcher.Invoke(new Action(() =>
                {
                    pin_tb.Text = dataStruct.pin.ToString("D4");

                }));

                bal_tb.Dispatcher.Invoke(new Action(() => bal_tb.Text = dataStruct.balance.ToString("C")));

                PictureBox.Dispatcher.Invoke(new Action(() =>
                {
                PictureBox.Source = Imaging.CreateBitmapSourceFromHBitmap(dataStruct.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                dataStruct.icon.Dispose();
                Search_Btn.IsEnabled = true;
                Button1.IsEnabled = true;
                HideProgressBar();
                }));

                TotalNum.Dispatcher.Invoke(new Action(() =>
                {
                    TotalNum.Content = "Search Successful";

                }));





        }

        private void OnSearchCompletion(IAsyncResult asyncResult)
        {
            DataStruct idataStruct = null;
            Search search = null;
            AsyncResult asyncobj = (AsyncResult)asyncResult;
            if(asyncobj.EndInvokeCalled == false)
            {
                
                try
                {
                    search = (Search)asyncobj.AsyncDelegate;
                    idataStruct = search.EndInvoke(asyncobj);
                    UpdateGUI(idataStruct);
                }catch(FaultException<SearchTextNotFound> exception)
                {
                    TotalNum.Dispatcher.Invoke(new Action(() => {
                        TotalNum.Content = exception.Detail.TextNotFound;
                        Search_Btn.IsEnabled = true;
                        Button1.IsEnabled = true;
                        HideProgressBar();
                    }));

                }
                

            }
            asyncobj.AsyncWaitHandle.Close();


        }

        private void HideProgressBar()
        {
            this.Dispatcher.Invoke((Action)(() => {
                Search_Pb.Visibility = Visibility.Hidden;
            }));
        }
        private void ShowProgressBar()
        {
            this.Dispatcher.Invoke((Action)(() => {
                Search_Pb.Visibility = Visibility.Visible;
                Search_Pb.IsIndeterminate = true;
            }));
        }

    }
}
