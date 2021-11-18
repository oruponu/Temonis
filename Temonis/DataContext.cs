namespace Temonis;

public class DataContext : BindableBase
{
    private string _latestTimeString = "接続しています...";

    public string LatestTimeString
    {
        get => _latestTimeString;
        set => SetProperty(ref _latestTimeString, value);
    }

    public Kyoshin.DataContext Kyoshin { get; } = new();

    public Eew.DataContext Eew { get; } = new();

    public EqInfo.DataContext EqInfo { get; } = new();
}
