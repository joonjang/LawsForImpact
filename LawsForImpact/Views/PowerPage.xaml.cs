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
using System.Collections.ObjectModel;

namespace LawsForImpact.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PowerPage : ContentPage
	{
		//PowerViewModel viewModel;
		//public PowerPage()
		//{

		//	InitializeComponent();
		//	// BindingContext hooks xaml binding to this class
		//	viewModel = new PowerViewModel();
		//	// sets Global.selectDescription to headerTitle through HeaderTitle
		//	// needs to go through HeaderTitle to call OnPropertyChanged method 
		//	// not too sure why OnPropertyChanged is required but it makes it work

		//	BindingContext = viewModel;

		//}

		//protected override void OnAppearing()
		//{
		//	base.OnAppearing();
		//	viewModel.RefreshListView();
		//}

	
		//================== OLD CODE ========================

		private SQLiteConnection _sqLiteConnection;

		public PowerPage()
		{

			InitializeComponent();
			// BindingContext hooks xaml binding to this class
			BindingContext = this;
			// sets Global.selectDescription to headerTitle through HeaderTitle
			// needs to go through HeaderTitle to call OnPropertyChanged method 
			// not too sure why OnPropertyChanged is required but it makes it work
			HeaderTitle = Global.selectedDescription;
			AddUserItem = Global.selectedTitle == "Personal";
			DeleteChecked = false;
			AddUserItem = false;

		}

		// TODO hookk up the headerTitle with xaml
		// look at how MyListView is binded
		// use the MockData info instead
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
					case "Personal":
						var listDataUser = _sqLiteConnection.Table<User>().ToList();
						AddUserItem = true;
						Global.userCount = listDataUser.Count();
						MyListView.ItemsSource = listDataUser;
						break;

				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}


		}



		private bool addUserItem;
		public bool AddUserItem
		{
			get => addUserItem;
			set
			{
				addUserItem = value;
				OnPropertyChanged(nameof(AddUserItem));
			}
		}


		// add stuff into User database
		async void AddItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
		}

		private void Delete_Clicked(object sender, EventArgs e)
		{
			DeleteChecked = true;
		}


		// add to list of deletable items from database
		// makes the cancel and confirm button visible
		private bool deleteChecked;
		public bool DeleteChecked
		{
			get => deleteChecked;
			set
			{
				deleteChecked = value;

				AddUserItem = !value;
				OnPropertyChanged(nameof(DeleteChecked));
			}
		}



		private void Cancel_Clicked(object sender, EventArgs e)
		{
			MyListView.SelectedItem = null;
			DeleteChecked = false;
		}

		private void ConfirmDelete_Clicked(object sender, EventArgs e)
		{
			_sqLiteConnection.Delete(deleteSubject);
			DeleteChecked = false;
			MyListView.SelectedItem = null;
			
			RefreshListView();
		}



		User deleteSubject;
		void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			if (DeleteChecked == false)
			{
				MyListView.SelectedItem = null;
			}
			else
			{
				var itemProperty = MyListView.SelectedItem as User;
				deleteSubject = (from item in _sqLiteConnection.Table<User>()
									  where item.Law == itemProperty.Law
									  select item).FirstOrDefault();

			}

		}


		private string addDeleteList;
		

		public string AddDeleteList
		{
			get => addDeleteList;
			set
			{
				addDeleteList = value;

				OnPropertyChanged(nameof(AddDeleteList));
			}
		}

	}
}