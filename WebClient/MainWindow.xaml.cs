using RestSharp;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using ApiClasses;

namespace WebClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        RestClient client = new RestClient("http://localhost:59389/");
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private async void Button1_Click_1(object sender, RoutedEventArgs e)
        {
            Search_Btn.IsEnabled = false;
            Button1.IsEnabled = false;
            Search_Pb.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(InputNum.Text) || SearchText.Text.Contains("Index"))
            {
                TotalNum.Content = "Please Enter a number";
            }
            else
            {
                try
                {
                    //On click, Get the index....
                    int index = Int32.Parse(InputNum.Text);
                    //Then, set up and call the API method...
                    RestRequest request = new RestRequest("api/GetAll/" + index.ToString());
                    RestResponse resp = await client.ExecuteGetAsync(request);
                    //And now use the JSON Deserializer to deseralize our object back to the class
                    //we want
                    ApiClasses.DataIntermed dataIntermed =
                    JsonConvert.DeserializeObject<ApiClasses.DataIntermed>(resp.Content);
                    //And now, set the values in the GUI!
                    fname_tb.Text = dataIntermed.fname;
                    lname_tb.Text = dataIntermed.lname;
                    accno_tb.Text = dataIntermed.acct.ToString();
                    pin_tb.Text = dataIntermed.pin.ToString();
                    bal_tb.Text = dataIntermed.bal.ToString();

                    byte[] byteBuffer = Convert.FromBase64String(dataIntermed.icon);
                    MemoryStream memoryStream = new MemoryStream(byteBuffer);

                    Bitmap bitmap = (Bitmap)Bitmap.FromStream(memoryStream);

                    memoryStream.Close();

                    PictureBox.Source = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    bitmap.Dispose();
                }
                catch (System.ArgumentNullException error)
                {

                    TotalNum.Content = "Index not found";
                }
                

            }
            Search_Pb.Visibility = Visibility.Hidden;
            Search_Btn.IsEnabled = true;
            Button1.IsEnabled = true;
        }

        private async void Search_Btn_Click(object sender, RoutedEventArgs e)
        {
            Search_Btn.IsEnabled = false;
            Button1.IsEnabled = false;
            Search_Pb.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(SearchText.Text) || SearchText.Text.Contains("Enter Last Name"))
            {
                TotalNum.Content = "Please Enter a search text";
            }
            else
            {
                try
                {
                    //Make a search class
                    ApiClasses.SearchData mySearch = new ApiClasses.SearchData();
                    mySearch.searchStr = SearchText.Text;
                    //Build a request with the json in the body
                    RestRequest request = new RestRequest("api/Search", Method.Post).AddJsonBody(JsonConvert.SerializeObject(mySearch));
                    //Do the request
                    RestResponse resp = await client.ExecutePostAsync(request);
                    //Console.WriteLine(resp.StatusCode);
                    //Deserialize the result
                    DataIntermed dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(resp.Content);
                    //aaaaand input the data
                    fname_tb.Text = dataIntermed.fname;
                    lname_tb.Text = dataIntermed.lname;
                    accno_tb.Text = dataIntermed.acct.ToString();
                    pin_tb.Text = dataIntermed.pin.ToString();
                    bal_tb.Text = dataIntermed.bal.ToString();

                    byte[] byteBuffer = Convert.FromBase64String(dataIntermed.icon);
                    MemoryStream memoryStream = new MemoryStream(byteBuffer);

                    Bitmap bitmap = (Bitmap)Bitmap.FromStream(memoryStream);

                    memoryStream.Close();

                    PictureBox.Source = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    bitmap.Dispose();
                }
                catch (System.ArgumentNullException error)
                {

                    TotalNum.Content = "Not found matching entry";
                }

            }
            Search_Btn.IsEnabled = true;
            Button1.IsEnabled = true;
            Search_Pb.Visibility = Visibility.Hidden;


        }

        private void GetCount(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/values");
            RestResponse numOfThings = client.Get(request);
            TotalNum.Content = "Number of items :"+numOfThings.Content;
        }
    }
}
