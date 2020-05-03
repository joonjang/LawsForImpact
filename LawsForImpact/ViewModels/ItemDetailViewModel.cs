using System;
using System.Collections.Generic;
using System.Linq;
using LawsForImpact.Models;
using LawsForImpact.Services;
using SQLite;
using Xamarin.Forms;

namespace LawsForImpact.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private SQLiteConnection _sqLiteConnection;
        public ItemDetailViewModel()
        {
            Title = "Summary";
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
        private string headerDescription;
        public string HeaderDescription
        {
            get { return headerDescription; }
            set
            {
                headerDescription = value;
                OnPropertyChanged(nameof(HeaderDescription));
            }
        }
        private string lawOrPrinciple;
        public string LawOrPrinciple
        {
            get { return lawOrPrinciple; }
            set
            {
                if (boldHeaderTitle != "User")
                {
                    lawOrPrinciple = boldHeaderTitle + ": " + FindLawOrPrinciple() + " " + value;
                }
                else
                {
                    lawOrPrinciple = "";
                }
                OnPropertyChanged(nameof(LawOrPrinciple));
            }
        }



        private string FindLawOrPrinciple()
        {
            string tmp = "";
            switch (boldHeaderTitle)
            {
                case "Power":
                case "War":
                case "Human":
                    tmp = "Law";
                    break;
                case "Mastery":
                    tmp = "Principle";
                    break;
                case "User":
                case "Friends":
                    tmp = "Rule";
                    break;
            }

            return tmp;
        }

        string boldHeaderTitle = Global.notifCurrentTitle;
        public async void LoadData()
        {
            try
            {
                // where the database gets populated
                _sqLiteConnection = await DependencyService.Get<ISQLite>().GetConnection();
                int numberOfLaws;
                int randLawIndex;
                int lawIndex;
                List<string> listOfOption = new List<string>() { "Power", "Mastery", "War", "Friends", "Human" };

                Random rand = new Random();

                if (_sqLiteConnection.Table<User>().Count() > 2)
                {
                    listOfOption.Add("User");
                }

                int index = rand.Next(listOfOption.Count);

                switch (listOfOption[index])
                {
                    case "Power":
                        numberOfLaws = _sqLiteConnection.Table<Power>().Count();
                        randLawIndex = rand.Next(numberOfLaws);
                        var getSQLElementPower = _sqLiteConnection.Table<Power>().ElementAt(randLawIndex);
                        boldHeaderTitle = "Power";
                        lawIndex = getSQLElementPower.Law;
                        LawOrPrinciple = lawIndex.ToString();
                        HeaderTitle = getSQLElementPower.Title;
                        HeaderDescription = getSQLElementPower.Description;
                        break;
                    case "Mastery":
                        numberOfLaws = _sqLiteConnection.Table<Mastery>().Count();
                        randLawIndex = rand.Next(numberOfLaws);
                        var getSQLElementMastery = _sqLiteConnection.Table<Mastery>().ElementAt(randLawIndex);
                        boldHeaderTitle = "Mastery";
                        lawIndex = getSQLElementMastery.Law;
                        LawOrPrinciple = lawIndex.ToString();
                        HeaderTitle = getSQLElementMastery.Title;
                        HeaderDescription = getSQLElementMastery.Description;
                        break;
                    case "War":
                        numberOfLaws = _sqLiteConnection.Table<War>().Count();
                        randLawIndex = rand.Next(numberOfLaws);
                        var getSQLElementWar = _sqLiteConnection.Table<War>().ElementAt(randLawIndex);
                        boldHeaderTitle = "War";
                        lawIndex = getSQLElementWar.Law;
                        LawOrPrinciple = lawIndex.ToString();
                        HeaderTitle = getSQLElementWar.Title;
                        HeaderDescription = getSQLElementWar.Description;
                        break;
                    case "Friends":
                        numberOfLaws = _sqLiteConnection.Table<Friends>().Count();
                        randLawIndex = rand.Next(numberOfLaws);
                        var getSQLElementFriends = _sqLiteConnection.Table<Friends>().ElementAt(randLawIndex);
                        boldHeaderTitle = "Friends";
                        lawIndex = getSQLElementFriends.Law;
                        LawOrPrinciple = lawIndex.ToString();
                        HeaderTitle = getSQLElementFriends.Title;
                        HeaderDescription = getSQLElementFriends.Description;
                        break;
                    case "Human":
                        numberOfLaws = _sqLiteConnection.Table<Human>().Count();
                        randLawIndex = rand.Next(numberOfLaws);
                        var getSQLElementHuman = _sqLiteConnection.Table<Human>().ElementAt(randLawIndex);
                        boldHeaderTitle = "Human";
                        lawIndex = getSQLElementHuman.Law;
                        LawOrPrinciple = lawIndex.ToString();
                        HeaderTitle = getSQLElementHuman.Title;
                        HeaderDescription = getSQLElementHuman.Description;
                        break;
                    case "User":
                        numberOfLaws = _sqLiteConnection.Table<User>().Count();
                        randLawIndex = rand.Next(numberOfLaws);
                        var getSQLElementUser = _sqLiteConnection.Table<User>().ElementAt(randLawIndex);
                        boldHeaderTitle = "User";
                        lawIndex = getSQLElementUser.Law;
                        LawOrPrinciple = lawIndex.ToString();
                        HeaderTitle = getSQLElementUser.Title;
                        HeaderDescription = getSQLElementUser.Description;
                        break;

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        //private async void LoadData2()
        //{

        //    try
        //    {
        //        _sqLiteConnection = await Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
        //        IEnumerable<IDataTable> tableToEnumerable = new List<IDataTable>();
        //        List<IDataTable> listData;

        //        switch (Global.notifTitle)
        //        {
        //            case "Power":
        //                tableToEnumerable = _sqLiteConnection.Table<Power>().ToList();
        //                break;
        //            case "Mastery":
        //                tableToEnumerable = _sqLiteConnection.Table<Mastery>().ToList();
        //                break;
        //            case "User":
        //                tableToEnumerable = _sqLiteConnection.Table<User>().ToList();
        //                break;
        //            case "War":
        //                tableToEnumerable = _sqLiteConnection.Table<War>().ToList();
        //                break;
        //            case "Friends":
        //                tableToEnumerable = _sqLiteConnection.Table<Friends>().ToList();
        //                break;
        //            case "Human":
        //                tableToEnumerable = _sqLiteConnection.Table<Human>().ToList();
        //                break;
        //        }

        //        listData = tableToEnumerable.ToList();
        //        boldHeaderTitle = Global.notifTitle;
        //        lawIndex = listData[]
        //        LawOrPrinciple = lawIndex.ToString();
        //        HeaderTitle = getSQLElementPower.Title;
        //        HeaderDescription = getSQLElementPower.Description;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}
    }
}
    //var getSQLElementUser = _sqLiteConnection.Table<User>().ElementAt(randLawIndex);
    //boldHeaderTitle = "User";
    //                    lawIndex = getSQLElementUser.Law;
    //                    LawOrPrinciple = lawIndex.ToString();
    //                    HeaderTitle = getSQLElementUser.Title;
    //                    HeaderDescription = getSQLElementUser.Description;