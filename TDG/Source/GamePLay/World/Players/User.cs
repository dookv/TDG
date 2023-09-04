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
    public class User : Player
    {

        public User(int ID, XElement DATA) :base(ID, DATA)
        {
            /*hero = new Hero("2d\\CharacterTD", new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(100, 100), id);
            buildings.Add(new Tower(new Vector2(Globals.screenWidth / 2, Globals.screenHeight - 40), id));*/
        } 

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);


            if (Globals.keyboard.GetSinglePress("T"))//D1 is the number 1
            {
                if (gold >= 10)
                {
                    Vector2 tempLoc = GRID.GetSlotFromPixel(new Vector2(hero.pos.X, hero.pos.Y - 30), Vector2.Zero);//location on the grid, not the pos of the art
                                                                                                                    //.when we are clicking on the screen we are in 2d screenSpace(0,0 to whatever your resolution is) whe ahve to offset the mouse to put it into gameSpace, that is why we dont account offset here, 
                    GridLocation loc = GRID.GetSlotFromLocation(tempLoc);

                    if (loc != null && !loc.filled && !loc.impassable)
                    {
                        loc.SetToFilled(false);
                        Building tempBuilding = new ArrowTower(new Vector2(0, 0), id, new Vector2(1, 1));
                        tempBuilding.pos = GRID.GetPosFromLoc(tempLoc) + GRID.slotDims / 2 + new Vector2(0, -tempBuilding.dims.Y * .25f);
                        GameGlobals.PassBuilding(tempBuilding);
                        gold -= 10;
                    }
                    //GameGlobals.PassBuilding(new ArrowTower(new Vector2(pos.X, pos.Y - 30), ownerId, new Vector2(1, 1)));//we give 1,1 because it doesnt have an animation, so the sheet is 1,1
                }

            }
        }

    }
}
