using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MazeLib;
using MazeGame.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.SignalR.Hubs;

namespace MazeGame
{
    [HubName("gameHub")]
    public class GameHub : Hub
    {
        public void StartGame(string nameOfGame, int rows, int cols)
        {
            ServerModel model = ServerModel.GetInstance();
            // current player
            string clientId = Context.ConnectionId;
            Maze maze = model.StartGame(nameOfGame, rows, cols, clientId);
            Clients.Client(clientId).drawBoard("myCanvas", maze.ToJSON(),
                "playerImg", "exitImg", true);
        }

        public void JoinTo(string nameOfGame)
        {
            ServerModel model = ServerModel.GetInstance();
            // current player
            string clientId = Context.ConnectionId;
            Maze maze = model.JoinTo(nameOfGame, Context.ConnectionId);
            // my boards
            Clients.Client(clientId).drawBoard("myCanvas", maze.ToJSON(),
                "playerImg", "exitImg", true);
            Clients.Client(clientId).drawBoard("competitorCanvas", maze.ToJSON(),
                "competitorImg", "exitImg", false);
            // competitor
            string opponentId = model.GetCompetitorOf(clientId);
            Clients.Client(opponentId).drawBoard("competitorCanvas", maze.ToJSON(),
                "competitorImg", "exitImg", false);
            // notify other player
            Clients.Client(opponentId).gameStarted();
        }

        public void Play(string direction)
        {
            ServerModel model = ServerModel.GetInstance();
            string playerId = Context.ConnectionId;
            string gameName = model.Play(ToDirectionStr(direction), playerId);
            string opponentId = model.GetCompetitorOf(playerId);
            Clients.Client(opponentId).moveOtherPlayer(direction);
            if (model.IsPlayerReachedExit(playerId))
            {
                Close(gameName, playerId);
            }
        }

        private void Close(string nameOfGame, string playerId)
        {
            ServerModel model = ServerModel.GetInstance();
            string opponentId = model.GetCompetitorOf(Context.ConnectionId);
            model.Close(nameOfGame);
            // TODO: implement 'closeGame' method in client side.
            Clients.Client(opponentId).closeGame(false);
            Clients.Client(playerId).closeGame(true);
        }


        public void GetAvailablesGame()
        {
            string[] games = ServerModel.GetInstance().GetAvailableGames();
            Clients.Client(Context.ConnectionId).presentAvailableGames(games);
        }

        private static string ToDirectionStr(string direction)
        {
            string directionStr = "unknown";
            switch (direction)
            {
                case "37":
                    directionStr = "left";
                    break;
                case "38":
                    directionStr = "up";
                    break;
                case "39":
                    directionStr = "right";
                    break;
                case "40":
                    directionStr = "down";
                    break;
            }
            return directionStr;
        }

    }
}