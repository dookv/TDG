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
#endregion

namespace ZeEngine
{
    public class QuantityDisplayBar
    {

        public Basic2d bar, barBkg;
        public int border;
        public Color color;

        public QuantityDisplayBar(Vector2 DIMS, int BORDER, Color COLOR) 
        { 
            border = BORDER;
            color = COLOR;

            bar = new Basic2d("2d\\UI\\HealthBar", new Vector2(0, 0), new Vector2(DIMS.X - border * 2, DIMS.Y - border * 2));//we subtract the border * 2  from both the y and x dims of the bar (the height and width of the bar) 
            barBkg = new Basic2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(DIMS.X, DIMS.Y));
        }

        public virtual void Update(float CURRENT, float MAX)//CURRENT is how full the bar is currently, and max is how full the bar can be
        {
            bar.dims = new Vector2(CURRENT/MAX * (barBkg.dims.X - border * 2), bar.dims.Y);//CURRENT/MAX gives us he percentage of how full the bar is.
                                                                                           //If we multiply how full the bar is with the width of the background but we want to subtract the border * 2 (we subtract border*2 for left and right)
        }

        public virtual void Draw(Vector2 OFFSET)
        {

            Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

            barBkg.Draw(OFFSET, new Vector2(0, 0), Color.White);
            //we apply a different effect for the red bar(color)
            Globals.normalEffect.Parameters["filterColor"].SetValue(color.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

            bar.Draw(OFFSET + new Vector2(border, border), new Vector2(0, 0), color);
        }


    }
}
