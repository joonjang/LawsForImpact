using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using LawsForImpact.Models;
using LawsForImpact.Services;
using LawsForImpact.ViewModels;

namespace LawsForImpact.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PowerPage : ContentPage 
	{
		PowerViewModel viewModel;
		public PowerPage()
		{

			InitializeComponent();
			// BindingContext hooks xaml binding to this class
			viewModel = new PowerViewModel();
			// sets Global.selectDescription to headerTitle through HeaderTitle
			// needs to go through HeaderTitle to call OnPropertyChanged method 
			// not too sure why OnPropertyChanged is required but it makes it work

			BindingContext = viewModel;

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.RefreshListView();
		}

		// add stuff into User database
		async void AddItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
		}



	}
}