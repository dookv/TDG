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
    public class AncientImp : Mob
    {

        public AncientImp(Vector2 POS, Vector2 FRAMES, int OWNERID) 
            : base("2d\\Mobs\\AncientImp", POS, new Vector2(45, 45), FRAMES, OWNERID)
        {
            speed = 2.0f;
            health = 2;
            attackRange = 400;
        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {

            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }
        public override void AI(Player ENEMY, SquareGrid GRID)
        {
            if (ENEMY.hero != null && (Globals.GetDistance(pos, ENEMY.hero.pos) < attackRange * 0.9f || isAttacking))
            {
                isAttacking = true;
                attackTimer.UpdateTimer();

                if (attackTimer.Test())
                {
                    GameGlobals.PassProjectile(new AcidSplash(new Vector2(pos.X, pos.Y), this, new Vector2(ENEMY.hero.pos.X, ENEMY.hero.pos.Y)));
                    attackTimer.ResetToZero();
                    isAttacking = false;
                }
            }
            else
            {
                base.AI(ENEMY, GRID);//walk towards the hero via the a*grid
            }



            
        }


        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}
