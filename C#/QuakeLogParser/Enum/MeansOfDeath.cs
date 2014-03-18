﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Enum
{
    /// <summary>
    /// Modos de morte
    /// </summary>
    public enum MeansOfDeath
    {
        MOD_UNKNOWN,
        MOD_SHOTGUN,
        MOD_GAUNTLET,
        MOD_MACHINEGUN,
        MOD_GRENADE,
        MOD_GRENADE_SPLASH,
        MOD_ROCKET,
        MOD_ROCKET_SPLASH,
        MOD_PLASMA,
        MOD_PLASMA_SPLASH,
        MOD_RAILGUN,
        MOD_LIGHTNING,
        MOD_BFG,
        MOD_BFG_SPLASH,
        MOD_WATER,
        MOD_SLIME,
        MOD_LAVA,
        MOD_CRUSH,
        MOD_TELEFRAG,
        MOD_FALLING,
        MOD_SUICIDE,
        MOD_TARGET_LASER,
        MOD_TRIGGER_HURT,
    #if MISSIONPACK
        MOD_NAIL,
        MOD_CHAINGUN,
        MOD_PROXIMITY_MINE,
        MOD_KAMIKAZE,
        MOD_JUICED,
    #endif
        MOD_GRAPPLE
    }
}
