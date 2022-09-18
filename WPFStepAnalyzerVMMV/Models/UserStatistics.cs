using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace WPFStepAnalyzerVMMV.Models
{
    public class UserStatistics : INotifyPropertyChanged
    {
        public string userName { get; set; }
        public int avgSteps { get; set; }
        public int bestResult { get; set; }
        public int worseResult { get; set; }
        public int rank;
        public string status;
        public UserStatistics(List<User> list)
        {
            this.userName = list.First().user;
            this.avgSteps = getAvgStepsFromList(list);
            this.bestResult = getBestResult(list);
            this.worseResult = getWorseResult(list);
            this.rank = list.Last().rank;
            this.status= list.Last().status;
        }

        public UserStatistics()
        {
        }

        private int getAvgStepsFromList(List<User> list)
        {
            BigInteger total = list.First().steps;
            for (int i = 1; i < list.Count; i++)
            {
                total += list[i].steps;
            }
            int avgSteps = (int)BigInteger.Divide(total, list.Count);
            return avgSteps;

        }
        private int getBestResult(List<User> list)
        {
            int bestResult = list.First().steps;
            for (int i = 0; i < list.Count; i++)
            {
                bestResult = bestResult < list[i].steps ? list[i].steps : bestResult;
            }
            return bestResult;
        }
        private int getWorseResult(List<User> list)
        {
            int worseResult = list.First().steps;
            for (int i = 0; i < list.Count; i++)
            {
                worseResult = worseResult > list[i].steps ? list[i].steps : worseResult;
            }
            return worseResult;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
