using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LawsForImpact.Models;
using LawsForImpact.ViewModels;
using LawsForImpact.Services;

namespace LawsForImpact.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailViewModel viewModel;
   

        public ItemDetailPage()
        {
            // starting 'Today' page, loads up a random element from the database
            InitializeComponent();


            viewModel = new ItemDetailViewModel
            {
                HeaderTitle = Global.notifDescription,
                HeaderDescription = Global.notifFullDescrip,
                LawOrPrinciple = Global.notifCurrentIndex.ToString()
            };
            if (viewModel.HeaderTitle == null)
            {
                viewModel.LoadData();
            }
            

            BindingContext = viewModel;
        }
      

        private void Button_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadData();
        }
    }
}