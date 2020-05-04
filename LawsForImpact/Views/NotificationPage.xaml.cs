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
	
		public NotificationPage()
        {
			BindingContext = new NotificationViewModel();
			InitializeComponent();

			
		}
		// todo make set time and date show based off check box chosen
		protected override void OnAppearing()
		{
			base.OnAppearing();
		}



	}
}