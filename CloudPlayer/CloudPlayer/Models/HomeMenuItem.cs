using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPlayer.Models
{
    public enum MenuItemType
    {
        Test,
        Artists,
        Tracks,
        Browse,
        NowPlaying,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
