using System;

namespace GameConnectFour.Model
{
    public class GameHistory
    {
        public int Id { get; set; }
        public DateTime EndedAt { get; set; }
        public string Winner { get; set; } = string.Empty;
    }
}
