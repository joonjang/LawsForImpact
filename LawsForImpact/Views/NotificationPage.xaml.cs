using LawsForImpact.Models;
using LawsForImpact.Services;
using LawsForImpact.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LawsForImpact.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    public partial class NotificationPage : ContentPage
    {
		int notificationNumber = -1;

		public NotificationPage()
        {
			BindingContext = new NotificationViewModel();
			InitializeComponent();

			notificationManager = DependencyService.Get<INotificationManager>();
			notificationManager.NotificationReceived += (sender, eventArgs) =>
			{
				var evtData = (NotificationEventArgs)eventArgs;
				ShowNotification(evtData.Title, evtData.Message);
			};
		}

		INotificationManager notificationManager;
		

		private SQLiteConnection _sqLiteConnection;


		protected override void OnAppearing()
		{
			base.OnAppearing();
			RefreshListView();
		}

		List<Power> globalList;
		private async void RefreshListView()
		{
			try
			{
				_sqLiteConnection = await DependencyService.Get<ISQLite>().GetConnection();
				var listData = _sqLiteConnection.Table<Power>().ToList();
				globalList = listData;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}

		void OnScheduleClick(object sender, EventArgs e)
		{
			notificationNumber++;
			var lawNumber = globalList[notificationNumber].Law; // fix this
			var lawTitle = globalList[notificationNumber].Title;

			string title = $"Law {lawNumber}";
			string message = lawTitle;
			notificationManager.ScheduleNotification(title, message);
		}

		void ShowNotification(string title, string message)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				var msg = new Label()
				{
					Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
				};

			});
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{

			await Navigation.PushAsync(new MainPage());
		}
	}
}