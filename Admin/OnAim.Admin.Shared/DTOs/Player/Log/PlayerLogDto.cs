using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnAim.Admin.Shared.DTOs.Player.Log
{
    public class PlayerLogDto
    {
        public int Id { get; set; }
        public string Log { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string PlayerLogType { get; set; }
    }
}
