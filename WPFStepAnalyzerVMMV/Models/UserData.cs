using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace WPFStepAnalyzerVMMV.Models
{
    public class UserData : INotifyPropertyChanged
    {
        public Dictionary<string, List<User>> dataOnDays = new Dictionary<string, List<User>>();
        public UserData(List<string> fileNames)
        {
            readData(fileNames);
        }
        public void readData(List<string> allfiles)
        {
            var countExcludedFile = 0;
            var countExcludedDataUsers = 0;
            //int[] allDaysNum = new int[allfiles.Count];
            List<int> allDaysNum = new List<int>();
            Regex regex = new Regex(@"\\day\d*\.json");
            for (int i = 0; i < allfiles.Count; i++)
            {
                MatchCollection matches = regex.Matches(allfiles[i]);
                try
                {
                    allDaysNum.Add(Convert.ToInt32(matches[0].Value.Substring(@"\day".Length, matches[0].Value.IndexOf('.') - @"\day".Length)));
                }
                catch (Exception)
                {
                    countExcludedFile++;
                    allfiles.RemoveAt(i);
                }
            }
            allDaysNum.Sort();
            for (int i = 0; i < allfiles.Count; i++)
            {
                allfiles[i] = allfiles[i].Substring(0, allfiles[i].LastIndexOf('\\')) + $"\\day{allDaysNum[i]}.json";
            }
            for (int i = 0; i < allfiles.Count; i++)
            {
                try
                {
                    string jsonAll = System.IO.File.ReadAllText(allfiles[i]);
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonAll.ToString());
                    for (int k = 0; k < users.Count; k++)
                    {
                        if (dataOnDays.ContainsKey(users[k].user))
                        {
                            dataOnDays[users[k].user].Add(users[k]);
                        }
                        else
                        {
                            dataOnDays.Add(users[k].user, new List<User>() { users[k] });
                        }
                    }
                }
                catch (Exception)
                {
                    countExcludedDataUsers++;
                }
                
            }
            if(countExcludedDataUsers!=0 || countExcludedFile!=0)
                MessageBox.Show($"{countExcludedFile} invalid file(s)\n{countExcludedDataUsers} invalid data in file(s)");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<UserStatistics> getUserStatisticsForObservableCollection()
        {
            ObservableCollection<UserStatistics> collections = new ObservableCollection<UserStatistics>();
            foreach (var item in dataOnDays)
            {                
                collections.Add(new UserStatistics(item.Value));
            }
            return collections;
        }
    }
}
