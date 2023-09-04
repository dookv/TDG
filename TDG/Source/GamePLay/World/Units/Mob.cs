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
    public class Mob : Unit
    {
        public McTimer repathTimer = new McTimer(200), attackTimer = new McTimer(500);//200ms, every 12 frames more time is less resources but less accurate pathing
        public bool currentlyPathing, isAttacking;
        public float attackRange;

        public Mob(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int OWNERID) 
            : base(PATH, POS, DIMS, FRAMES, OWNERID)
        {
            isAttacking = false;
            attackRange = 50;
            currentlyPathing = false;
            speed = 4.0f;
        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)//because we changed the update function un unit from override to virtual
        {
            AI(ENEMY, GRID);
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public virtual void AI(Player ENEMY, SquareGrid GRID)
        {
/*            pos += Globals.RadialMovement(pos, ENEMY.hero.pos, speed);//we dont need this here because we do it in MoveUnit()
              rot = Globals.RotateTowards(pos, ENEMY.hero.pos);*/

            repathTimer.UpdateTimer();

            //no path           <------path is finished--------------------------------->       timer is true
            if (pathNodes == null || pathNodes.Count == 0 && pos.X == moveTo.X && pos.Y == moveTo.Y || repathTimer.Test())//if we have no path, find a path
            {
                if (!currentlyPathing)
                {

                    Task repathTask = new Task(() =>//this will ofload our pathing tasks to our other processors, be careful on mobile the more cpus you engage the more battery you burn
                    {
                        currentlyPathing = true;
                        pathNodes = FindPath(GRID, GRID.GetSlotFromPixel(ENEMY.hero.pos, Vector2.Zero));//find the path on the given grid, and get the endslot via the pixel (in which slot is the pos of the temp building) pos of the temp building
                        moveTo = pathNodes[0];
                        pathNodes.RemoveAt(0);

                        repathTimer.ResetToZero();
                        currentlyPathing = false;
                    });
                    repathTask.Start();
                }


            }
            else//if we ahve a path, move and check for hit
            {
                MoveUnit();
                if (Globals.GetDistance(pos, ENEMY.hero.pos) < GRID.slotDims.X * 1.12f)//some padding
                {
                    ENEMY.hero.GetHit(this, 1);
                    dead = true;
                }
            }
        }

        public override void GetHit(AttackableOBject ATTACKER, float DAMAGE)
        {
            health -= DAMAGE;
            throbing = true;
            throbTimer.ResetToZero();//reset everytime you get hit
            if (health <= 0)
            {
                dead = true;

                GameGlobals.PassGold(new PlayerValuePacket(ATTACKER.ownerId, killValue));
                int num = Globals.rand.Next(0, 2 + 1);

                if (num == 0) 
                {
                    Lootbag tempBag = new Lootbag("2d\\Misc\\LootBag", new Vector2(pos.X, pos.Y), null);
                    tempBag.items.Add(new TestItem());
                    GameGlobals.PassLootBag(tempBag);

                }
            }

        }



        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}
