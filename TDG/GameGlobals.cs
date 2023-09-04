#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ZeEngine;
#endregion

namespace TDG
{
    //this file happens at load time, dont put too much stuff in here

    public class GameGlobals
    {
        public static bool paused = false;
        public static int score = 0;//static keyword makes it public
        public static KeyBindList keyBinds;

        public static PassObject PassProjectile, PassEffect, PassMob, PassGold, PassLootBag, PassSpawnPoint, CheckScroll, PassBuilding, AddToInventory;//call the PassObject delgate in Globals.

    }
}
