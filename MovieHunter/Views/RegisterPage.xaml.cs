using System;
using System.Net.Http;
using System.Text;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class RegisterPage : Page
    {
        private RegisterViewModel ViewModel
        {
            get { return ViewModelLocator.Current.RegisterViewModel; }
        }

        /// <summary>Initializes a new instance of the <see cref="RegisterPage"/> class.</summary>
        public RegisterPage()
        {
            InitializeComponent();
        }

        /// <summary>Handles the Click event of the Login control.
        /// Navigates to Login Page</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        /// <summary>Handles the Click event of the Register control.
        /// Reads User input at registers user with a post request
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void Register_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Turning on the loading indicator
            loadingIndicator.IsActive = true;

            string userName = inp_Username.Text;
            string firstName = inp_FirstName.Text;
            string lastName = inp_LastName.Text;
            string password1 = inp_Password1.Password;
            string password2 = inp_Password2.Password;
            string password_Hashed = LoginPage.UserHasher(userName, password1);

            User loginInformation = new User();

            try
            {
                loginInformation = new User()
                {
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    Password = password_Hashed,
                };
            }
            catch
            {
                //Turning off the loading indicator
                loadingIndicator.IsActive = false;

                //If the user object creation failed the registration will be aborted
                return;
                
            }
            

            string loginInformation_Json = JsonConvert.SerializeObject(loginInformation);


            try
            {
                var client = new HttpClient();

                string uri = "http://localhost:59713/api/Users/register";
                await client.PostAsync(uri, new StringContent(loginInformation_Json, Encoding.UTF8, "application/json"));

                Frame.Navigate(typeof(LoginPage));
            }
            catch
            {

            }
            finally
            {
                //Turning off the loading indicator
                loadingIndicator.IsActive = false;
                Frame.Navigate(typeof(LoginPage));
            }
            

            return;
        }

        
        /// <summary>Called when [key down handler]
        ///
        /// When the user clicks enter they will register
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Input.KeyRoutedEventArgs"/> instance containing the event data.</param>
        private void OnKeyDownHandler(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Register_Click(null, null);
            }
        }

    }
}
