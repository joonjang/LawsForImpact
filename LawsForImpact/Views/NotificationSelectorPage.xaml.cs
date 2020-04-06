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
                //determines globally what the notification topic chosen was
                var x = e.SelectedItem as Item;
                Global.selectedTitle = x.Text;

                await Navigation.PushAsync(new NotificationPage());

                // Todo  add detail under title of whether notification turned off or on, what day, etcs
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