using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPlayer.Models
{
    public class UserSettings
    {
        public string RemoteMusicPath { get; set; }
        public DateTimeOffset LastCompletedScanDate { get; set; }
    }
}
