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
    public class EggSac : SpawnPoint
    {
        int maxSpawns, totalSpawns;

        public EggSac(Vector2 POS, int OWNERID, XElement DATA, Vector2 FRAMES) 
            : base("2d\\WorldItems\\EggSac", POS, new Vector2(60, 60), FRAMES, OWNERID, DATA)
        {
            totalSpawns = 0;
            maxSpawns = 3;
            killValue = 2;
            health = 3;
            healthMax = health;
            spawnTimer = new McTimer(3000);//we override spawntimer from the inherrited spawnpoint class here

        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {

            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public override void SpawnMob()
        {
            Mob tempMob = new Spiderling(new Vector2(pos.X, pos.Y), new Vector2(1, 1), ownerId);


            if (tempMob != null)
            {
                totalSpawns++;
                GameGlobals.PassMob(tempMob);
                if (totalSpawns >= maxSpawns)
                {
                    dead = true;
                }
            }
            
        }

        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}
