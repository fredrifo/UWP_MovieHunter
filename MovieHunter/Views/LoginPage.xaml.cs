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
using System.Threading.Tasks;

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

        /// <summary>Initializes a new instance of the <see cref="LoginPage"/> class.</summary>
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>Creates a hashed password using Username and password</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns> Returns a string with the new hasehed password </returns>
        public static string UserHasher(string username, string password)
        {

            // putting the string in a buffer UTF-8 encoded
            IBuffer preHashed = CryptographicBuffer.ConvertStringToBinary(password + username,
                BinaryStringEncoding.Utf8);

            // hash it using Sha246
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            IBuffer hashed = hasher.HashData(preHashed);

            // formatting it
            return CryptographicBuffer.EncodeToBase64String(hashed);
        }


        /// <summary>
        /// This static isLogged is being used to check if the user is logged in
        /// Can call LoginPage.isLoggedIn to verify
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is logged in; otherwise, <c>false</c>.</value>
        public static bool IsLoggedIn
        {
            get;
            set;
        }



        class LoginResponse
        {
            public string Token { get; set; }
            public string Result { get; set; }
        }

        /// <summary>
        /// Handles the Click event of the Login control.
        /// Reads input username and password. Creating hashvalue for password for added security.
        /// Creates a token that for the clients session (the api can decrypt this to find userId when deciding which table rows to return
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Getting user input
            string username = inp_Username.Text;
            string password = inp_Password.Password;

            //Creates the hased password
            string password_Hashed = UserHasher(username, password);

            //Creating user Object with username and salted password
            User loginInformation = new User()
            {
                UserName = username,
                Password = password_Hashed,
            };


            //Post request to the api that checks if the User object is matching 
            string loginInformation_Json = JsonConvert.SerializeObject(loginInformation);

            //Starting the loading indicator
            loadingIndicator.IsActive = true;

            //Logs in with a HttpPost request to the api
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
                        //Saving the token in a dynamic string so that it is accessible from every page
                        token = loginResponse.Token;

                        //Open the shellpage with menu
                       // navigationFrame.Navigate(typeof(MainPage));
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
                //Stops the loading indicator
                loadingIndicator.IsActive = false;
            }

            //This will happen about the same time the Frame is navigater
            finally
            {
                //Finally makes sure that the loadingindicator will end when the db connection is over
                loadingIndicator.IsActive = false;
            }

        }

        /// <summary>
        /// Handles the Click event of the Register control.
        /// Navigates user to Registration page</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void Register_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        
        /// <summary>
        /// Called when [key down] is clicked.
        /// When the user clicks enter they will login.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Input.KeyRoutedEventArgs"/> instance containing the event data.</param>
        private void OnKeyDownHandler(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login_Click(null, null);
            }
        }
    }
}
