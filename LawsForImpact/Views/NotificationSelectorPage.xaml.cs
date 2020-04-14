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

        void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ItemsListView.SelectedItem != null)
            {
                //determines globally what the notification topic chosen was

                var item = e.SelectedItem as Item;
                SendSelectedItem(item);
                
                

                // Todo  add detail under title of whether notification turned off or on, what day, etcs
                ItemsListView.SelectedItem = null;
            }
        }
        private Item sendSelectedItem;
        private void SendSelectedItem(Item item)
        {
             item = sendSelectedItem;
        } 
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //Item itemReceived = sendSelectedItem;
           
            //Global.notifTitle = itemReceived.Text;
        }

    }
}