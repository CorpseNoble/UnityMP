using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class PlayerPrefsManager
    {

        private static int _day = 0;
        private static bool _session = false;
        private static DateTime _lastUpdate = DateTime.Now;

        public static int Day { get => _day; set => _day = value; }
        public static bool Session { get => _session; set => _session = value; }
        public static DateTime LastUpdate { get => _lastUpdate; set => _lastUpdate = value; }
    }
}
