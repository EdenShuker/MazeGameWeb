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

        /// <summary>
        /// Calls start game method of model and call draw board
        /// method of client.
        /// </summary>
        /// <param name="nameOfGame"> name of game</param>
        /// <param name="rows"> number of rows </param>
        /// <param name="cols"> number of cols </param>
        public void StartGame(string nameOfGame, int rows, int cols)
        {
            ServerModel model = ServerModel.GetInstance();
            // current player
            string clientId = Context.ConnectionId;
            Maze maze = model.StartGame(nameOfGame, rows, cols, clientId);
            //calls draw board function in client
            Clients.Client(clientId).drawBoard("myCanvas", maze.ToJSON(),
                "playerImg", "exitImg", true);
        }


        /// <summary>
        /// Calls join to function of model and call draw board on
        /// clients in game.
        /// </summary>
        /// <param name="nameOfGame">name of game</param>
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


        /// <summary>
        /// Calls play game of model. draw player in opponent board, 
        /// and check if player reached the end.
        /// </summary>
        /// <param name="direction"></param>
        public void Play(string direction)
        {
            ServerModel model = ServerModel.GetInstance();
            string playerId = Context.ConnectionId;
            string gameName = model.Play(ToDirectionStr(direction), playerId);
            string opponentId = model.GetCompetitorOf(playerId);
            // draw new player location in opponent board.
            Clients.Client(opponentId).moveOtherPlayer(direction);
            // check if player reached goal position.
            if (model.IsPlayerReachedExit(playerId))
            {
                Close(gameName, playerId);
            }
        }

        /// <summary>
        /// Calls close game method of model and post results in 
        /// both players.
        /// </summary>
        /// <param name="nameOfGame"></param>
        /// <param name="playerId"></param>
        private void Close(string nameOfGame, string playerId)
        {
            ServerModel model = ServerModel.GetInstance();
            string opponentId = model.GetCompetitorOf(Context.ConnectionId);
            model.Close(nameOfGame);
            // close both players
            Clients.Client(opponentId).closeGame(false);
            Clients.Client(playerId).closeGame(true);
        }


        /// <summary>
        /// Get the list of games from model and call client method
        /// to present it.
        /// </summary>
        public void GetAvailablesGame()
        {
            string[] games = ServerModel.GetInstance().GetAvailableGames();
            Clients.Client(Context.ConnectionId).presentAvailableGames(games);
        }

        /// <summary>
        /// Convert the number represnts the direction into string
        /// with the name of direction: left, right, up, down
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
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