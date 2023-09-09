
using MauiMaze.Engine;
using MauiMaze.Services;

namespace MauiMaze;

public partial class GameRecordsPage : ContentPage
{
	private int userID;
	public GameRecordsPage(int userID)
    {
        InitializeComponent();
        List<GameRecord> records = RecordFetcher.fetchRecords(1);
        recordsList.ItemsSource = records;
        this.userID = userID;
    }


}