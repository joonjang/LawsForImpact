using LawsForImpact.Models;
using LawsForImpact.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LawsForImpact.ViewModels
{
	public class PowerViewModel : BaseViewModel
	{
		public PowerViewModel()
		{
			HeaderTitle = Global.selectedDescription;
		}

		private string headerTitle;
		public string HeaderTitle
		{
			get { return headerTitle; }
			set
			{
				headerTitle = value;
				OnPropertyChanged(nameof(HeaderTitle));
			}
		}

		private bool addUserItem;
		public bool AddUserItem
		{
			get => AddUserItem;
			set
			{
				SetProperty(ref addUserItem, value, nameof(AddUserItem));
			}
		}

		private SQLiteConnection _sqLiteConnection;

		public ListView MyListView;


		public async void RefreshListView()
		{
			try
			{
				// where the database gets populated
				_sqLiteConnection = await DependencyService.Get<ISQLite>().GetConnection();

				//todo change Law to principal or rule title accordingly
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
					case "Personal":
						var listDataUser = _sqLiteConnection.Table<User>().ToList();
						MyListView.ItemsSource = listDataUser;
						break;

				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			// todo add properties about whether its a principle, law, or rule
		}
	}
}
