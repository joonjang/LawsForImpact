using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LawsForImpact.Models;
using LawsForImpact.Views;
using LawsForImpact.ViewModels;

namespace LawsForImpact.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)] 
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (ItemsListView.SelectedItem != null)
            {
                var item = args.SelectedItem as Item;
                switch (item.Text)
                {
                    case "Law of Power":
                        await Navigation.PushAsync(new PowerPage());
                        break;

                }
                //if (item == null)
                //    return;

                //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }

        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}