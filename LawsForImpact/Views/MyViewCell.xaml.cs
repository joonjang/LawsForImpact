using LawsForImpact.Models;
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


    //https://stackoverflow.com/questions/48607818/how-to-use-xname-of-items-in-listview-at-codebehind
    public partial class MyViewCell : ViewCell
    {

        public MyViewCell()
        {
            InitializeComponent();
            InitCell();
            FindLawOrPrinciple();
            IsItLawOrPrinciple.Text = LawOrPrinciple;
        }

        public void InitCell()
        {
            //i can access my stack:
            if (Global.selectedTitle == "Personal")
            {
                lawIndexBind.IsVisible = false;

            }
            else
            {
                lawIndexBind.IsVisible = true;
            }

        }

        //Now not for demo but in the real world:
        //We can set content according to your data from ItemsSource
        //This will act when you set your ListView ItemsSource to something valid
        protected override void OnBindingContextChanged()
        {
            SetupCell();
            base.OnBindingContextChanged();
        }

        public void SetupCell()
        {
            //use data from ItemsSource
            var item = BindingContext as PowerPage;
            if (item == null) return;

        }


        private string lawOrPrinciple;
        public string LawOrPrinciple
        {
            get => lawOrPrinciple;
            set
            {
                lawOrPrinciple = value;
                OnPropertyChanged(LawOrPrinciple);
            }
        }
        private void FindLawOrPrinciple()
        {
            string tmp = "";
            switch (Global.selectedTitle)
            {
                case "Power":
                case "War":
                case "Human":
                    tmp = "Law";
                    break;
                case "Mastery":
                    tmp = "Principle";
                    break;
                case "Personal":
                case "Friends":
                    tmp = "Rule";
                    break;
            }

            LawOrPrinciple = tmp;
        }
    }
}