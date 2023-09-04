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
    public class Building : AttackableOBject
    {

        public Building(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int OWNERID) 
            : base(PATH, POS, DIMS, FRAMES, OWNERID)
        {


        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)//Override?
        {
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }



        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            Globals.normalEffect.Parameters["xSize"].SetValue((float)myModel.Bounds.Width);//in the fx file xSize etc is a float, so that is why we cast them to float here, these are already concatonated to int so we dont need to do it
            Globals.normalEffect.Parameters["ySize"].SetValue((float)myModel.Bounds.Height);
            Globals.normalEffect.Parameters["xDraw"].SetValue((float)((int)dims.X));//we concatonate it to an int, because pixel shaders dont deal with partial pixels (float values)
            Globals.normalEffect.Parameters["yDraw"].SetValue((float)((int)dims.Y));
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());//in the fx file filterColor is a float4 which is a vector4
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();//this sline sets all the effects

            base.Draw(OFFSET);
        }
    }
}
