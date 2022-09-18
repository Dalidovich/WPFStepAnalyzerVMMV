using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WPFStepAnalyzerVMMV.Models;
using LiveCharts;
using WPFStepAnalyzerVMMV.Commands;
using Newtonsoft.Json;
using System.IO;
using ServiceStack.Text;
using WPFStepAnalyzerVMMV.Dialogs;

namespace WPFStepAnalyzerVMMV.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private UserStatistics _selectedUserStatistics;
        private Chart _chartSelectedUserStatistics;
        Dialog dlg;
        public UserData userData { get; set; }
        private SeriesCollection _seriesViews { get; set; }
        private ObservableCollection<UserStatistics> _usersStatistics { get; set; }
        public ObservableCollection<UserStatistics> UsersStatistics
        {
            get { return _usersStatistics; }
            set
            {
                _usersStatistics = value;
                OnPropertyChanged("UsersStatistics");
            }
        }
        public SeriesCollection SeriesViews
        {
            get { return _seriesViews; }
            set
            {
                _seriesViews = value;
                OnPropertyChanged("SeriesViews");
            }
        }
        public UserStatistics SelectedUserStatistics
        {
            get { return _selectedUserStatistics; }
            set
            {

                OnPropertyChanged("SelectedUserStatistics");
                _selectedUserStatistics = value;
                try
                {
                    if (value == null) { return; }
                    SeriesViews = _chartSelectedUserStatistics.getSeries(userData.dataOnDays[SelectedUserStatistics.userName]);
                }
                catch (Exception){}
                UsersStatistics = UsersStatistics;
            }
        }
        private Command _loadCommand;
        public Command LoadCommand
        {
            get
            {
                return _loadCommand ??
                  (_loadCommand = new Command(obj =>
                  {
                      List<string> fileNames = new List<string>();
                      if (dlg.OpenFileDialog())
                      {
                          fileNames = dlg.FilesPath.ToList();
                      }
                      if (fileNames.Count == 0) { return; }
                      userData = new UserData(fileNames);
                      UsersStatistics = userData.getUserStatisticsForObservableCollection();
                      try
                      {
                          SelectedUserStatistics = UsersStatistics.First();
                          SeriesViews = _chartSelectedUserStatistics.getSeries(userData.dataOnDays[SelectedUserStatistics.userName]);
                      }
                      catch (Exception) { }
                  }));
            }
        }

        private Command _saveXMLCommand;
        public Command SaveXMLCommand
        {
            get
            {
                return _saveXMLCommand ??
                  (_saveXMLCommand = new Command(obj =>
                  {
                      try
                      {
                          var path = SelectedUserStatistics == null ? throw new ArgumentNullException() : $"{SelectedUserStatistics.userName}Data.xml";
                          if (dlg.SaveFileDialog("xml", "Data Files (*.xml)|*.xml", path) == true)
                          {
                              System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(UserStatistics));
                              using (FileStream fs = new FileStream(dlg.FilePath, FileMode.OpenOrCreate))
                              {
                                  xmlSerializer.Serialize(fs, SelectedUserStatistics);
                              }
                              System.Windows.MessageBox.Show("saved XML file");
                          }
                      }
                      catch (Exception e)
                      {
                          System.Windows.MessageBox.Show($"{e.Message}");
                      }
                      
                  }));
            }
        }

        private Command _saveJSONCommand;
        public Command SaveJSONCommand
        {
            get
            {
                return _saveJSONCommand ??
                  (_saveJSONCommand = new Command(obj =>
                  {
                      try
                      {
                          var path = SelectedUserStatistics == null?throw new ArgumentNullException(): $"{SelectedUserStatistics.userName}Data.json";
                          if (dlg.SaveFileDialog("json", "Data Files (*.json)|*.json",path) == true)
                          {
                              dlg.FilePath = dlg.FilePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? dlg.FilePath : dlg.FilePath + ".json";
                              string json = JsonConvert.SerializeObject(SelectedUserStatistics);
                              System.IO.File.WriteAllText(dlg.FilePath, json);
                              System.Windows.MessageBox.Show("saved JSON file");
                          }
                      }
                      catch (Exception e) 
                      {
                          System.Windows.MessageBox.Show($"{e.Message}");
                      }
                  }));
            }
        }
        private Command _saveCSVCommand;
        public Command SaveCSVCommand
        {
            get
            {
                return _saveCSVCommand ??
                  (_saveCSVCommand = new Command(obj =>
                  {
                      try
                      {
                          var path = SelectedUserStatistics == null ? throw new ArgumentNullException() : $"{SelectedUserStatistics.userName}Data.csv";
                          if (dlg.SaveFileDialog("xml", "Data Files (*.xml)|*.xml", path) == true)
                          {
                              dlg.FilePath = dlg.FilePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ? dlg.FilePath : dlg.FilePath + ".csv";
                              var csv = CsvSerializer.SerializeToCsv(new[] { SelectedUserStatistics }).Replace(',', ';');
                              System.IO.File.WriteAllText(dlg.FilePath, csv);
                              System.Windows.MessageBox.Show("saved csv file");
                          }
                      }
                      catch (Exception e)
                      {
                          System.Windows.MessageBox.Show($"{e.Message}");
                      }                      
                  }));
            }
        }
        public ApplicationViewModel()
        {
            this.dlg = new Dialog();
            this._seriesViews = new SeriesCollection();
            this._chartSelectedUserStatistics = new Chart();
            UsersStatistics = new ObservableCollection<UserStatistics>();           
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
