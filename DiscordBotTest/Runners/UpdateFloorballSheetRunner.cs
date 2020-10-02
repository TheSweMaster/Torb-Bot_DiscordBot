using DiscordBotTest.Helpers;
using DiscordBotTest.Services;

namespace DiscordBotTest.Runners
{
    public class UpdateFloorballSheetRunner
    {
        public void Run()
        {
            var floorballScoreTableList = FloorballDataService.GetFloorballScoreTableList();
            var gameRoundsList = FloorballDataService.GetGamesRoundList();
            var playerLineupList = FloorballDataService.GetPlayerLineupList(gameRoundsList);

            new GoogleSheetHelper().UpdateFloorballSheets(floorballScoreTableList, gameRoundsList, playerLineupList);
        }
    }
}
