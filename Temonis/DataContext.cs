using System.ComponentModel;

namespace Temonis
{
    public class DataContext : INotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs LatestTimeStringPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(LatestTimeString));
        private string _latestTimeString = "接続しています...";

        public event PropertyChangedEventHandler PropertyChanged;

        public string LatestTimeString
        {
            get => _latestTimeString;
            set
            {
                _latestTimeString = value;
                PropertyChanged?.Invoke(this, LatestTimeStringPropertyChangedEventArgs);
            }
        }

        public Kyoshin.DataContext Kyoshin { get; set; } = new Kyoshin.DataContext();

        public Eew.DataContext Eew { get; set; } = new Eew.DataContext();

        public EqInfo.DataContext EqInfo { get; set; } = new EqInfo.DataContext();
    }
}
