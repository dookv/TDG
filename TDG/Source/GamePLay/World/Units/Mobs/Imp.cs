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
    public class Imp : Mob
    {

        public Imp(Vector2 POS, Vector2 FRAMES, int OWNERID) 
            : base("2d\\Mobs\\Imp", POS, new Vector2(40, 40), FRAMES, OWNERID)
        {
            speed = 4.0f;
        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {

            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }


        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}
