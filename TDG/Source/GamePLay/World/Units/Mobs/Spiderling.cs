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
    public class Spiderling : Mob
    {
        public McTimer spawnTimer;

        public Spiderling(Vector2 POS, Vector2 FRAMES, int OWNERID)
            : base("2d\\Mobs\\Spiderling", POS, new Vector2(40, 40), FRAMES, OWNERID)
        {
            speed = 4.0f;
            health = 1;
            healthMax = health;

        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {

            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public override void AI(Player ENEMY, SquareGrid GRID)
        {

            Building temp = null;

            for (int i = 0; i < ENEMY.buildings.Count; i++)
            {
                //eventually i load all classes via files
                if (ENEMY.buildings[i].GetType().ToString() == "TDG.Tower")//This gets the type of building class GetType(), in this case we want tower. It converts the typ[e to string and you equals it against the namespace of the tower . classname
                {
                    temp = ENEMY.buildings[i];
                }
            }
            if (temp != null)
            {
                if (pathNodes == null || pathNodes.Count == 0 && pos.X == moveTo.X && pos.Y == moveTo.Y)
                {
                    pathNodes = FindPath(GRID, GRID.GetSlotFromPixel(temp.pos, Vector2.Zero));//find the path on the given grid, and get the endslot via the pixel (in which slot is the pos of the temp building) pos of the temp building
                    moveTo = pathNodes[0];
                    pathNodes.RemoveAt(0);

                }
                else 
                {
                    MoveUnit();
                    if (Globals.GetDistance(pos, temp.pos) < GRID.slotDims.X * 1.12f)//some padding
                    {
                        temp.GetHit(this, 1);
                        dead = true;
                    }
                }
            }




        }


        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }
    }
}
