using DiscordBotTest.Helpers;
using DiscordBotTest.Services;

namespace DiscordBotTest.Runners
{
    public class UpdateFloorballSheetRunner
    {
        public void Run(int selectedRound)
        {
            var floorballScoreTableList = FloorballDataService.GetFloorballScoreTableList();
            var gameRoundsList = FloorballDataService.GetGamesRoundList();
            var playerLineupList = FloorballDataService.GetPlayerLineupList(gameRoundsList, selectedRound);

            new GoogleSheetHelper().UpdateFloorballSheets(floorballScoreTableList, gameRoundsList, playerLineupList);
        }
    }
}
