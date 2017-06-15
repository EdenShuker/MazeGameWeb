using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazeGame.Models
{
    public class UserRankings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Rank { get; set; }

        [Range(0, int.MaxValue)]
        public int Wins { get; set; }

        [Range(0, int.MaxValue)]
        public int Losses { get; set; }
    }
}