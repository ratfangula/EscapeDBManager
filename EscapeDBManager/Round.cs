using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeDBManager
{
    public class Round
    {
        public int RoundID { get; set; }
        public int GameID { get; set; }
        public string TeamName { get; set; }
        public string RoomName { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int HintsUsed { get; set; }

        public TimeSpan? PlusTime { get; set; }

    }
}
