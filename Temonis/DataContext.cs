namespace Temonis
{
    public class DataContext : BindableBase
    {
        private string _latestTimeString = "接続しています...";

        public string LatestTimeString
        {
            get => _latestTimeString;
            set => SetProperty(ref _latestTimeString, value);
        }

        public Kyoshin.DataContext Kyoshin { get; set; } = new Kyoshin.DataContext();

        public Eew.DataContext Eew { get; set; } = new Eew.DataContext();

        public EqInfo.DataContext EqInfo { get; set; } = new EqInfo.DataContext();
    }
}
