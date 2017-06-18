using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MazeLib;
using MazeGame.Models;

namespace MazeGame
{
    public class GameHub : Hub
    {

        public Maze StartGame(string nameOfGame, int rows, int cols)
        {
            string clientId = Context.ConnectionId;
            Maze maze = ServerModel.GetInstance().StartGame(nameOfGame, rows, cols, clientId);
            return maze;
        }

        public Maze JoinTo(string nameOfGame)
        {
            Maze maze = ServerModel.GetInstance().JoinTo(nameOfGame, Context.ConnectionId);
            return maze;
        }

        public void Play(string direction)
        {
            ServerModel model = ServerModel.GetInstance();
            string playerId = Context.ConnectionId;
            model.Play(direction, playerId);
            string opponentId = model.GetCompetitorOf(playerId);
            // TODO: implement 'moveOtherPlayer' in client side.
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
    }
}