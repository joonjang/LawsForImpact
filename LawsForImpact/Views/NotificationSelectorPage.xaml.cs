using LawsForImpact.Models;
using LawsForImpact.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LawsForImpact.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationSelectorPage : ContentPage
    {
        ItemsViewModel viewModel;
        public NotificationSelectorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ItemsListView.SelectedItem != null)
            {
                await Navigation.PushAsync(new NotificationPage());
                // NEXT TASK, MAKE IT SEND DATA OF WHAT DATABASE IT SENT
                // CONNECT DYNAMIC DATABASE STRING TO NOTIFICATIONPAGE
                // ESSKEETIT
                //  add detail under title of whether notification turned off or on, what day, etcs
                ItemsListView.SelectedItem = null;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}