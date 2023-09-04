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
    public class Lootbag : Animated2d
    {
        public List<InventoryItem> items = new List<InventoryItem>();
        public bool done;
        public float speed;
        public float initiateDistance, lootDistance;



        public Lootbag(string PATH, Vector2 POS, List<InventoryItem> ITEMS)
            : base(PATH, POS, new Vector2(40, 40), new Vector2(1, 1), Color.White)
        {
            initiateDistance = 140.0f;
            lootDistance = 25.0f;
            speed = 7.0f;
            done = false;
            if (ITEMS != null)
            {
                items = ITEMS;
            }


        }


        public virtual void Update(Vector2 OFFSET, Player ENEMY)
        {
/*            if (Globals.mouse.LeftClick() && Hover(OFFSET))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    GameGlobals.AddToInventory(items[i]);
                }
                done = true;
            }*/

            if (Globals.GetDistance(pos, ENEMY.hero.pos) < lootDistance)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Globals.soundControl.PlaySound("PickUp");
                    GameGlobals.AddToInventory(items[i]);
                }
                done = true;
            }

            AI(ENEMY);
        }

        public virtual void AI(Player ENEMY)
        {
            if (Globals.GetDistance(pos, ENEMY.hero.pos) < initiateDistance)
            {

                pos += Globals.RadialMovement(pos, ENEMY.hero.pos, speed);
            }
            //we dont need this here because we do it in MoveUnit()
            //rot = Globals.RotateTowards(pos, ENEMY.hero.pos);
        }

        /*        public override void Draw(Vector2 OFFSET)
                {

                }*/

    }
}
