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
using System.Text;
using System.Net.Http;
using HttpClient = System.Net.Http.HttpClient;
using System.Collections.Generic;

namespace MovieHunter.Views
{
    public sealed partial class LoginPage : Page
    {
        public static string token;
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

        public static string UserHasher(string username, string password)
        {

            //put the string in a buffer, UTF-8 encoded
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



        class LoginResponse
        {
            public string Token { get; set; }
            public string Result { get; set; }
        }

        private async void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string username = inp_Username.Text;
            string password = inp_Password.Password;
            string password_Hashed = UserHasher(username, password);

            User loginInformation = new User()
            {
                UserName = username,
                Password = password_Hashed,
            };

            string loginInformation_Json = JsonConvert.SerializeObject(loginInformation);


            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/Users/login";
                var httpResponse = await client.PostAsync(uri, new StringContent(loginInformation_Json, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(json);
                    if (loginResponse.Token != "Null")
                    {
                        token = loginResponse.Token;
                        Frame.Navigate(typeof(ShellPage));
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch
            {
                //Write error as feedback
            }

        }

        private void Register_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        //When the user clicks enter they will login
        private void OnKeyDownHandler(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login_Click(null, null);
            }
        }
    }
}
