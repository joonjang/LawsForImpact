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
				_sqLiteConnection = await DependencyService.Get<ISQLite>().GetConnection();
				var listData = _sqLiteConnection.Table<Power>().ToList();
				MyListView.ItemsSource = listData;
				}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}


	}
}