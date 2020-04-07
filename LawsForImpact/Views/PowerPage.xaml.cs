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

		private SQLiteConnection _sqLiteConnection;

		

		public PowerPage()
		{

			InitializeComponent();
			// BindingContext hooks xaml binding to this class
			BindingContext = new PowerViewModel();
			// sets Global.selectDescription to headerTitle through HeaderTitle
			// needs to go through HeaderTitle to call OnPropertyChanged method 
			// not too sure why OnPropertyChanged is required but it makes it work
			

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			RefreshListView();
		}
		
		private async void RefreshListView()
		{
			try
			{
				// where the database gets populated
				_sqLiteConnection = await DependencyService.Get<ISQLite>().GetConnection();
				
				switch (Global.selectedTitle)
				{
					case "Power":
						var listDataPower = _sqLiteConnection.Table<Power>().ToList();
						MyListView.ItemsSource = listDataPower;
						break;
					case "Mastery":
						var listDataMastery = _sqLiteConnection.Table<Mastery>().ToList();
						MyListView.ItemsSource = listDataMastery;
						break;
					case "War":
						var listDataWar = _sqLiteConnection.Table<War>().ToList();
						MyListView.ItemsSource = listDataWar;
						break;
					case "Friends":
						var listDataFriends = _sqLiteConnection.Table<Friends>().ToList();
						MyListView.ItemsSource = listDataFriends;
						break;
					case "Human":
						var listDataHuman = _sqLiteConnection.Table<Human>().ToList();
						MyListView.ItemsSource = listDataHuman;
						break;
					case "User":
						var listDataUser = _sqLiteConnection.Table<User>().ToList();
						MyListView.ItemsSource = listDataUser;
						break;

				}
				
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}

		
	}
}