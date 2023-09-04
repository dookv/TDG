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
    public class Unit : AttackableOBject
    {
        protected Vector2 moveTo;//you dont edit his, stay protected
        protected List<Vector2> pathNodes = new List<Vector2>();

        public Skill currentSkill;
        public List<Skill> skills = new List<Skill>();
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();



        public Unit(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int OWNERID) : base(PATH, POS, DIMS, FRAMES, OWNERID)
        {
            moveTo = new Vector2(pos.X, pos.Y);  
        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)//override?
        {
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public virtual void AddToInventory(Object INFO)
        {
            //inventory.Add((InventoryItem)INFO);
            bool added = false;
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].item == null)
                {

                    InventoryItem tempItem = (InventoryItem)INFO;
                    tempItem.slot = inventorySlots[i];

                    inventorySlots[i].item = tempItem;
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                //deal with items not having enough space...
            }
        }

        public virtual List<Vector2> FindPath(SquareGrid GRID, Vector2 ENDSLOT)//outputs a list of vector2
                                                                               //smooth out the a star pathing, skipping nodes for data storage etc
        {
            pathNodes.Clear();
            Vector2 tempStartSlot = GRID.GetSlotFromPixel(pos, Vector2.Zero);//screenspace = the space of the whole screen(EX: your mouse)
                                                                             //gamespace = the game itself because you scroll around, we are already in that space so we dont need to scroll around

            List<Vector2> tempPath = GRID.GetPath(tempStartSlot, ENDSLOT, true);

            if (tempPath == null || tempPath.Count == 0)
            {

            }
            return tempPath;
        }


        public virtual void MoveUnit()
        {
            if (pos.X != moveTo.X || pos.Y != moveTo.Y)
            {
                rot = Globals.RotateTowards(pos, moveTo);
                pos += Globals.RadialMovement(pos, moveTo, speed);
            }
            else if (pathNodes.Count > 0)
            {
                moveTo = pathNodes[0];
                pathNodes.RemoveAt(0);

                pos += Globals.RadialMovement(pos, moveTo, speed);
            }
            
        }



        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}
