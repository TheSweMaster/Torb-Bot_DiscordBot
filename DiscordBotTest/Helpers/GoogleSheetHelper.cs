using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DiscordBotTest.Models.Floorball;
using System.Linq;

namespace DiscordBotTest.Helpers
{
    public class GoogleSheetHelper
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Allsvenskan Sodra Floorball Excel Sheet Application";

        // https://docs.google.com/spreadsheets/d/1GFiMFafSrCt-OZJqNmAyKsCpF12h-kzIsCO1NrfVqDg/edit 
        const string SpreadsheetId = "1GFiMFafSrCt-OZJqNmAyKsCpF12h-kzIsCO1NrfVqDg";

        string ScoreTableSheet = "Tabell Resultat";
        string GamesRoundSheet = "Matcher Omg. {0}";
        string LineUpSheet = "Lag Omg. {0} {1} {2}";
        private readonly SheetsService _service;

        public GoogleSheetHelper()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string tokenFile = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenFile, true)).Result;
                Console.WriteLine("Credential file saved to: " + tokenFile);
            }

            // Create Google Sheets API service.
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void UpdateFloorballSheets(List<FloorballScoreTable> floorballScoreTableList, List<GameRounds> gameRoundsList, List<PlayerLineup> playerLineupList)
        {
            var spreadSheet = _service.Spreadsheets.Get(SpreadsheetId).Execute();

            if (!spreadSheet.Sheets.Select(s => s.Properties.Title).Contains(ScoreTableSheet))
            {
                AddSheet(ScoreTableSheet);
            }
            else
            {
                ClearSheet(ScoreTableSheet);
            }

            var floorBallScoreTableValues = GetFloorBallScoreTableValues(floorballScoreTableList);
            UpdateSheet(ScoreTableSheet, floorBallScoreTableValues);

            var sheetGameRoundsValuesList = new List<(string sheetName, List<IList<object>> tableValues)>();
            for (int round = 1; round <= gameRoundsList.Select(x => x.Round).Max(); round++)
            {
                var gameRounds = gameRoundsList.Where(x => x.Round == round).ToList();
                var sheetName = string.Format(GamesRoundSheet, round);
                var tableValues = GetGameRoundsValues(gameRounds);

                sheetGameRoundsValuesList.Add((sheetName, tableValues));
            }

            UpdateBulkSheets(sheetGameRoundsValuesList, spreadSheet);

            var sheetValuesPlayerLineupList = new List<(string sheetName, List<IList<object>> tableValues)>();
            foreach (var playerLineup in playerLineupList)
            {
                var sheetName = string.Format(LineUpSheet, playerLineup.Round, playerLineup.Team, playerLineup.HomeOrAway);
                var tableValues = GetPlayerLineupValues(playerLineup);

                sheetValuesPlayerLineupList.Add((sheetName, tableValues));
            }

            UpdateBulkSheets(sheetValuesPlayerLineupList, spreadSheet);
        }

        private void UpdateBulkSheets(List<(string sheetName, List<IList<object>> tableValues)> sheetValuesList, Spreadsheet spreadsheet)
        {
            var addSheetRequests = new List<Request>();
            var clearSheetRanges = new List<string>();
            var valueRanges = new List<ValueRange>();
            foreach (var (sheetName, tableValues) in sheetValuesList)
            {

                if (!spreadsheet.Sheets.Select(s => s.Properties.Title).Contains(sheetName))
                {
                    var addSheetRequest = new Request
                    {
                        AddSheet = new AddSheetRequest
                        {
                            Properties = new SheetProperties { Title = sheetName }
                        }
                    };
                    addSheetRequests.Add(addSheetRequest);
                }
                else
                {
                    clearSheetRanges.Add($"{sheetName}!A1");
                }

                var valueRange = new ValueRange
                {
                    Range = $"{sheetName}!A1",
                    Values = tableValues
                };
                valueRanges.Add(valueRange);
            }

            if (addSheetRequests.Any())
            {
                var addSpreadSheetsRequest = new BatchUpdateSpreadsheetRequest
                {
                    Requests = addSheetRequests
                };
                var createResult = _service.Spreadsheets.BatchUpdate(addSpreadSheetsRequest, SpreadsheetId).Execute();
            }

            if (clearSheetRanges.Any())
            {
                var clearValuesRequest = new BatchClearValuesRequest()
                {
                    Ranges = clearSheetRanges
                };
                var clearResult = _service.Spreadsheets.Values.BatchClear(clearValuesRequest, SpreadsheetId).Execute();
            }

            if (valueRanges.Any())
            {
                var updateValuesRequest = new BatchUpdateValuesRequest()
                {
                    Data = valueRanges,
                    ValueInputOption = ValueInputOption.RAW.ToString()
                };
                var updateResult = _service.Spreadsheets.Values.BatchUpdate(updateValuesRequest, SpreadsheetId).Execute();
            }
        }

        public List<IList<object>> GetPlayerLineupValues(PlayerLineup playerLineup)
        {
            var playerLineupValues = new List<IList<object>>();
            var headerRow = new List<object>
                {
                    "Omgång",
                    "Lag",
                    "Position",
                    "Nummer",
                    "Spelare",
                };

            playerLineupValues.Add(headerRow);
            for (int i = 0; i < playerLineup.Players.Count; i++)
            {
                var tableRow = new List<object>
                {
                    playerLineup.Round,
                    playerLineup.Team,
                    playerLineup.Players[i].PlayerPosition,
                    playerLineup.Players[i].PlayerNumber,
                    playerLineup.Players[i].PlayerName
                };
                playerLineupValues.Add(tableRow);
            }

            return playerLineupValues;
        }

        public List<IList<object>> GetGameRoundsValues(List<GameRounds> gameRoundsList)
        {
            var gameRoundsValues = new List<IList<object>>();
            var headerRow = new List<object>
                {
                    "Omgång",
                    "Lag Hemma",
                    "Lag Borta",
                    "Resultat",
                    "Matchens Domare"
                };
            gameRoundsValues.Add(headerRow);
            for (int i = 0; i < gameRoundsList.Count; i++)
            {
                var tableRow = new List<object>
                {
                    gameRoundsList[i].Round,
                    gameRoundsList[i].TeamHome,
                    gameRoundsList[i].TeamAway,
                    gameRoundsList[i].Result,
                    gameRoundsList[i].Referees
                };
                gameRoundsValues.Add(tableRow);
            }

            return gameRoundsValues;
        }

        public List<IList<object>> GetFloorBallScoreTableValues(List<FloorballScoreTable> floorballScoreTableList)
        {
            var floorBallScoreTableValues = new List<IList<object>>();
            var headerRow = new List<object>
                {
                    "Position",
                    "Lag",
                    "Matcher",
                    "Målskillnad",
                    "Poäng"
                };
            floorBallScoreTableValues.Add(headerRow);
            for (int i = 0; i < floorballScoreTableList.Count; i++)
            {
                var tableRow = new List<object>
                {
                    floorballScoreTableList[i].Position,
                    floorballScoreTableList[i].Team,
                    floorballScoreTableList[i].GamesPlayed,
                    floorballScoreTableList[i].GoalDifferens,
                    floorballScoreTableList[i].Points
                };
                floorBallScoreTableValues.Add(tableRow);
            }

            return floorBallScoreTableValues;
        }

        private void AddSheet(string title)
        {
            var updateSpreadSheet = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request>()
                {
                    new Request{ AddSheet = new AddSheetRequest { Properties = new SheetProperties{ Title = title} } },
                }
            };

            var createResult = _service.Spreadsheets.BatchUpdate(updateSpreadSheet, SpreadsheetId).Execute();
        }

        private void ClearSheet(string title)
        {
            var clearedResult = _service.Spreadsheets.Values.Clear(new ClearValuesRequest(), SpreadsheetId, title).Execute();
        }

        private void UpdateSheet(string title, List<IList<object>> values)
        {
            var updateValues = new BatchUpdateValuesRequest()
            {
                Data = new List<ValueRange>()
                {
                    new ValueRange()
                    {
                        Range = $"{title}!A1",
                        Values = values
                    }
                },
                ValueInputOption = ValueInputOption.RAW.ToString()
            };

            var updateResult = _service.Spreadsheets.Values.BatchUpdate(updateValues, SpreadsheetId).Execute();
        }
    }

    public enum ValueInputOption
    {
        INPUT_VALUE_OPTION_UNSPECIFIED,
        // Default input value. This value must not be used.
        RAW,
        // The values the user has entered will not be parsed and will be stored as-is.
        USER_ENTERED
        // The values will be parsed as if the user typed them into the UI. Numbers will stay as numbers, 
        // but strings may be converted to numbers, dates, etc. following the same rules
        // that are applied when entering text into a cell via the Google Sheets UI.
    }
}