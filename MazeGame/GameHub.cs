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
        }

        public void JoinTo(string nameOfGame)
        {
            ServerModel model = ServerModel.GetInstance();
            // current player
            string clientId = Context.ConnectionId;
            Maze maze = model.JoinTo(nameOfGame, Context.ConnectionId);
            Clients.Client(clientId).drawBoard("myCanvas", maze,
                "Views/Images/minion.gif", "Views/Images/Exit.png", true);
            // competitor
            string opponentId = model.GetCompetitorOf(clientId);
            Clients.Client(opponentId).drawBoard("competitorCanvas", maze,
                "Views/Images/pokemon.gif", "Views/Images/Exit.png", false);
        }

        public void Play(string direction)
        {
            ServerModel model = ServerModel.GetInstance();
            string playerId = Context.ConnectionId;
            model.Play(direction, playerId);
            string opponentId = model.GetCompetitorOf(playerId);
            Clients.Client(opponentId).moveOtherPlayer(direction);
        }

        public void Close(string nameOfGame)
        {
            ServerModel model = ServerModel.GetInstance();
            model.Close(nameOfGame);
            string opponentId = model.GetCompetitorOf(Context.ConnectionId);
            // TODO: implement 'closeGame' method in client side.
            Clients.Client(opponentId).closeGame();
        }


        public void GetAvailablesGame()
        {
            string[] games = ServerModel.GetInstance().GetAvailableGames();
            Clients.Client(Context.ConnectionId).presentAvailableGames(games);
        }
    }
}