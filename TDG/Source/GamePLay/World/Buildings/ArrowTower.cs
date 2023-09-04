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
    public class ArrowTower : Building
    {
        int range;
        McTimer shootTimer = new McTimer(1200);

        public ArrowTower(Vector2 POS, int OWNERID, Vector2 FRAMES) 
            : base("2d\\WorldItems\\ArrowTower", POS, new Vector2(24, 60), FRAMES, OWNERID)//64, 160
        {
            range = 500;
            health = 10;
            healthMax = health;

            hitDistance = 35.0f;

        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)//override?
        {
            shootTimer.UpdateTimer();
            if (shootTimer.Test())
            {
                FireArrow(ENEMY);
                shootTimer.ResetToZero();
            }
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public virtual void FireArrow(Player ENEMY)//ai of the tower, i guess
        {
            float closestDist = range, currentDist = 0;
            Unit closest = null;

            for (int i = 0; i < ENEMY.units.Count; i++)//looping for all units in enemy
            {
                currentDist = Globals.GetDistance(pos, ENEMY.units[i].pos);//get all enemy units distance from the tower, pos is always the current pos of the thing we are drawing, (go down the inherritance tree to basic2d, there is pos)

                if (currentDist <= closestDist)//closestDist < currentDist
                {
                    closestDist = currentDist;//closest distance
                    closest = ENEMY.units[i];//closest enemy
                }
            
            }


            if (closest != null)
            {
                GameGlobals.PassProjectile(new Arrow(new Vector2(pos.X, pos.Y), this, new Vector2(closest.pos.X, closest.pos.Y)));
            }
        }


        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {

            base.Draw(OFFSET);
        }
    }
}
