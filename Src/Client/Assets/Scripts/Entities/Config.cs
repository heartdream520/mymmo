using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entities
{
    class Config
    {
        public static bool MusicOn { get; internal set; }
        public static bool SoundOn { get; internal set; }
        public static float MusicVolume { get; internal set; }
        public static float SoundVolume { get; internal set; }
    }
}
