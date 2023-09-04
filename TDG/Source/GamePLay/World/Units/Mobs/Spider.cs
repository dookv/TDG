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
    public class Spider : Mob
    {
        public McTimer spawnTimer;

        public Spider(Vector2 POS, Vector2 FRAMES, int OWNERID) 
            : base("2d\\Mobs\\Spider", POS, new Vector2(60, 60), FRAMES, OWNERID)
        {
            speed = 2.0f;
            health = 3;
            killValue = 3;
            healthMax = health;
            spawnTimer = new McTimer(8000);//8 sec
            spawnTimer.AddToTimer(4000);
        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            spawnTimer.UpdateTimer();
            if (spawnTimer.Test())
            {
                SpawnEggSac();
                spawnTimer.ResetToZero();
            }

            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public virtual void SpawnEggSac()
        {
            GameGlobals.PassSpawnPoint(new EggSac(new Vector2(pos.X, pos.Y), ownerId, null, new Vector2(1, 1)));
        }


        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }
    }
}
