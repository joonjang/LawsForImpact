using LawsForImpact.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

        // todo add properties about whether its a principle, law, or rule
    }
}
