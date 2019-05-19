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

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void Register_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string userName = inp_Username.Text;
            string firstName = inp_FirstName.Text;
            string lastName = inp_LastName.Text;
            string password1 = inp_Password1.Password;
            string password2 = inp_Password2.Password;
            string password_Hashed = LoginPage.UserHasher(userName, password1);

            User loginInformation = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = password_Hashed,
            };

            string loginInformation_Json = JsonConvert.SerializeObject(loginInformation);

            

            var client = new HttpClient();

            string uri = "http://localhost:59713/api/Users/register";
            await client.PostAsync(uri, new StringContent(loginInformation_Json, Encoding.UTF8, "application/json"));

            return;
        }

    }
}
