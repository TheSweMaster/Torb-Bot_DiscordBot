using DiscordBotTest.Models;
using DiscordBotTest.Models.Floorball;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DiscordBotTest.Services
{
    public static class FloorballDataService
    {
        public static List<FloorballScoreTable> GetFloorballScoreTableList()
        {
            const string url = "http://statistik.innebandy.se/ft.aspx?scr=table&ftid=28330";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var valueNodes = doc.DocumentNode.SelectNodes("//*[@id='IbisInfo']/table[2]/tbody/tr/td")
                .Where(x => !x.HasClass("ext")).ToList();

            var floorBallChartList = new List<FloorBallTable>();
            for (int i = 0; i < valueNodes.Count; i += 9)
            {
                floorBallChartList.Add(
                    new FloorBallTable(
                        valueNodes[i].InnerText,
                        valueNodes[i + 1].InnerText,
                        valueNodes[i + 2].InnerText,
                        valueNodes[i + 3].InnerText,
                        valueNodes[i + 4].InnerText,
                        valueNodes[i + 5].InnerText,
                        valueNodes[i + 6].InnerText,
                        valueNodes[i + 7].InnerText,
                        valueNodes[i + 8].InnerText)
                    );
            }

            var floorballScoreTableList = new List<FloorballScoreTable>();
            foreach (var item in floorBallChartList)
            {
                var floorballScoreTable = new FloorballScoreTable(int.Parse(item.Rank.TrimEnd('.')), item.Team, item.Played, int.Parse(item.GoalDifferens), item.Points);
                floorballScoreTableList.Add(floorballScoreTable);
            }

            return floorballScoreTableList;
        }

        public static List<GameRounds> GetGamesRoundList()
        {
            const string baseurl = "http://statistik.innebandy.se/";
            const string url = "http://statistik.innebandy.se/ft.aspx?scr=fixturelist&ftid=28330";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var valueNodes = doc.DocumentNode.SelectNodes("//*[@id='IbisInfo']/table[2]/tbody/tr/td")
                .Where(x => x.ParentNode.HasClass("clTrOdd") || x.ParentNode.HasClass("clTrEven")).ToList();

            var gameRoundsList = new List<GameRounds>();
            for (int i = 0; i < valueNodes.Count; i += 7)
            {
                var link = valueNodes[i + 2].SelectSingleNode($"{valueNodes[i + 2].XPath}/a")?.GetAttributeValue("href", string.Empty);
                var fullLinkToLineup = string.IsNullOrEmpty(link) ? "" : baseurl + link.Replace("&amp;", "&");

                gameRoundsList.Add(
                    new GameRounds(
                        valueNodes[i + 1].InnerText,
                        valueNodes[i + 2].InnerText.Split('-').First().Trim(),
                        valueNodes[i + 2].InnerText.Split('-').Last().Trim(),
                        valueNodes[i + 3].InnerText,
                        valueNodes[i + 4].InnerText,
                        fullLinkToLineup
                        )
                    );
            }
            return gameRoundsList;
        }

        public static List<PlayerLineup> GetPlayerLineupList(List<GameRounds> gameRoundsList)
        {
            var web = new HtmlWeb();

            var playerLineupList = new List<PlayerLineup>();
            foreach (var round in gameRoundsList.Where(x => x.Round == 1 || x.Round == 2 || x.Round == 3))
            {
                Console.WriteLine(round.TeamHome + "-" + round.TeamAway);
                Thread.Sleep(1000); // Do not spam the server plz
                var doc = web.Load(round.LinkToLineup);

                var homeNode = doc.DocumentNode.SelectNodes("//*[@class='clCommonGrid tablesorter noMarginBottom']").FirstOrDefault();
                var htmlAwayNode = doc.DocumentNode.SelectNodes("//*[@class='clCommonGrid tablesorter noMarginBottom']");
                var awayNode = (htmlAwayNode != null && htmlAwayNode.Count == 2) ? htmlAwayNode.LastOrDefault() : null;

                var homeValuesNodes = homeNode?.SelectNodes($"{homeNode.XPath}/tbody/tr/td");
                var awayValuesNodes = awayNode?.SelectNodes($"{awayNode.XPath}/tbody/tr/td");

                if (homeValuesNodes != null)
                {
                    var playerLineup = AddPlayersToLineup(homeValuesNodes, round.Round, HomeOrAway.Hemma, round.TeamHome);

                    playerLineupList.Add(playerLineup);
                }
                else
                {
                    var playerLineup = new PlayerLineup()
                    {
                        Round = round.Round,
                        HomeOrAway = HomeOrAway.Borta,
                        Team = round.TeamHome,
                    };
                    playerLineupList.Add(playerLineup);
                }

                if (awayValuesNodes != null)
                {
                    var playerLineup = AddPlayersToLineup(awayValuesNodes, round.Round, HomeOrAway.Borta, round.TeamAway);
                    playerLineupList.Add(playerLineup);
                }
                else
                {
                    var playerLineup = new PlayerLineup
                    {
                        Round = round.Round,
                        HomeOrAway = HomeOrAway.Borta,
                        Team = round.TeamAway,
                    };
                    playerLineupList.Add(playerLineup);
                }
            }

            return playerLineupList;
        }

        private static PlayerLineup AddPlayersToLineup(HtmlNodeCollection valuesNodes, int round, HomeOrAway homeOrAway, string team)
        {
            var playerLineup = new PlayerLineup();
            for (int i = 0; i < valuesNodes.Count; i += 11)
            {
                playerLineup.Round = round;
                playerLineup.HomeOrAway = homeOrAway;
                playerLineup.Team = team;

                playerLineup.Players.Add(
                    new Player(
                        valuesNodes[i + 2].InnerText,
                        valuesNodes[i].InnerText.TrimEnd('.'),
                        valuesNodes[i + 1].InnerText)
                    );
            }

            var goalies = playerLineup.Players
                .Where(x => x.PlayerPosition == Position.Målvakt.ToString())
                .OrderByDescending(x => x.PlayerNumber).ToList();

            foreach (var item in goalies)
            {
                playerLineup.Players.Remove(item);
                playerLineup.Players.Insert(0, item);
            }

            return playerLineup;
        }
    }
}
