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
    public class FlameCircle : Effect2d
    {


        public FlameCircle(Vector2 POS, Vector2 DIMS, int MSEC) 
            : base("2d\\Misc\\GreatFlame", POS, DIMS, new Vector2(1, 1), MSEC)
        {
        
        }

        public override void Update(Vector2 OFFSET)
        {
            rot += (float)Math.PI * 2.0f / 60.0f;//rotations? think of pi

            base.Update(OFFSET);
        }


    }
}
