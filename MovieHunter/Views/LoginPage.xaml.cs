using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MovieHunter.Services;
using MovieHunter.ViewModels;
using Windows.ApplicationModel.Activation;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using MovieHunter.DataAccess.Models;
using Newtonsoft.Json;

namespace MovieHunter.Views
{
    public sealed partial class LoginPage : Page
    {
        private LoginViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LoginViewModel; }
        }

        public ICommand StartCommand { get; set; }

        public void StartViewModel()
        {
            StartCommand = new RelayCommand(OnStart);
        }

        private void OnStart()
        {

        }

        public LoginPage()
        {
            InitializeComponent();
        }

        public string UserHasher(string username, string password)
        {

            // put the string in a buffer, UTF-8 encoded...
            IBuffer preHashed = CryptographicBuffer.ConvertStringToBinary(password + username,
                BinaryStringEncoding.Utf8);

            // hash it...
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer hashed = hasher.HashData(preHashed);

            // format it...
            return CryptographicBuffer.EncodeToBase64String(hashed);
        }


        public static bool isLoggedIn
        {
            get;
            set;
        }

        private async void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            // DisplayAlert for the clicked item
            string passwordHashed = UserHasher(username.Text, password.Password);


            Uri requestUri1 = new Uri("http://localhost:62841/api/UsersController_Json");

            //Send the GET request asynchronously and retrieve the response as a string.
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            //
            try
            {
                //Send the GET request

                HttpResponseMessage result = await httpClient.GetAsync(requestUri1);
                string json = await result.Content.ReadAsStringAsync();
                User[] users = JsonConvert.DeserializeObject<User[]>(json);

                foreach (User user in users)
                {
                    if (user.UserName == username.Text && !isLoggedIn)
                    {
                        Frame.Navigate(typeof(ShellPage));
                        isLoggedIn = true;
                    }
                    
                }


                httpResponse = await httpClient.GetAsync(requestUri1);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                ContentDialog notification = new ContentDialog
                {
                    Title = "Login button clicked",
                    Content = "The username and password combined creates the salted hashvalue: '" +
                    passwordHashed + "'. Every time the user login this value will be checked against the password saved in the database             " + httpResponseBody,
                    CloseButtonText = "Ok"
                };
                ContentDialogResult resulst = await notification.ShowAsync();
            }
            finally
            {
                //courses.Add(httpResponseBody);
                //courses.Add("test1");
                //courses.Add("test2");


                //listView_Students.ItemsSource = students;
            }
            //}

            




            //If login is successfull show mainpage with navigation enabled


        }

    }
}
