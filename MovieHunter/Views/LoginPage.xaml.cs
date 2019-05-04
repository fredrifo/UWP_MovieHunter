using System;

using MovieHunter.ViewModels;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class LoginPage : Page
    {
        private LoginViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LoginViewModel; }
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

        private async void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            // DisplayAlert for the clicked item
            string passwordHashed = UserHasher(username.Text, password.Password);

            ContentDialog notification = new ContentDialog
            {
                Title = "Login button clicked",
                Content = "The username and password combined creates the salted hashvalue: '" +
                passwordHashed + "'. Every time the user login this value will be checked against the password saved in the database",
                CloseButtonText = "Ok"
            };
            ContentDialogResult result = await notification.ShowAsync();

        }
    }
}
