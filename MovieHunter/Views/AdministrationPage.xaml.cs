using System;
using System.Collections.ObjectModel;
using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class AdministrationPage : Page
    {
        private AdministrationViewModel ViewModel
        {
            get { return ViewModelLocator.Current.AdministrationViewModel; }
        }

        public AdministrationPage()
        {
            suggestions = new ObservableCollection<string>();
            InitializeComponent();
        }

        private ObservableCollection<String> suggestions;

        private void AutoSuggestBox_Person_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                suggestions.Clear();

                suggestions.Add(sender.Text + "1");

                suggestions.Add(sender.Text + "2");

                suggestions.Add(sender.Text + "3");

                suggestions.Add(sender.Text + "4");

                suggestions.Add(sender.Text + "5");

                suggestions.Add(sender.Text + "6");

                suggestions.Add(sender.Text + "7");

                suggestions.Add(sender.Text + "8");

                suggestions.Add(sender.Text + "9");

                sender.ItemsSource = suggestions;
            }
        }

        private void AutoSuggestBox_Person_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                txtAutoSuggestBox_Person.Text = args.ChosenSuggestion.ToString();
            }
            else
            {
                // Use args.QueryText to determine what to do.
                txtAutoSuggestBox_Person.Text = sender.Text;
            }
        }

        private void AutoSuggestBox_Person_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            txtAutoSuggestBox_Person.Text = "Choosen";
        }
    }
}
