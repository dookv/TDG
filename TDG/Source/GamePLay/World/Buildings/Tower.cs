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
    public class Tower : Building
    {

        public Tower(Vector2 POS, Vector2 FRAMES, int OWNERID) 
            : base("2d\\WorldItems\\Tower", POS, new Vector2(128, 320), FRAMES, OWNERID)
        {
            health = 20;
            healthMax = health;

            hitDistance = 35.0f;

        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)//override?
        {
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }



        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}
