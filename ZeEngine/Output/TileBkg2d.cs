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
    public class TileBkg2d : Basic2d
    {
        public Vector2 bkgDims;

        public TileBkg2d(string PATH, Vector2 POS, Vector2 DIMS, Vector2 BKGDIMS) 
            : base(PATH, POS, new Vector2((float)Math.Floor(DIMS.X), (float)Math.Floor(DIMS.Y)))
        {
            bkgDims = new Vector2((float)Math.Floor(BKGDIMS.X), (float)Math.Floor(BKGDIMS.Y));
        }


        public override void Draw(Vector2 OFFSET)//this will draw background tiles with overflow
                                                 //we dont put this as an overloaded function in basic2d because this does not get used outside this usecase, and we dont want to have and overloaded draw that never gets used,
        {
            float numX = (float)Math.Ceiling(bkgDims.X / dims.X);
            float numY = (float)Math.Ceiling(bkgDims.Y / dims.Y);
            for (int i = 0; i < numX; i++)//math.ceiling almost always gives you a decimal number(is is not needed here, it helps for debugging for some people)
                                                                      //loop on x to see how many times the tile fits the background,
            {
                for (int j = 0; j < numY; j++)
                {

                    if (i < numX-1 && j < numY-1 )
                    {
                        base.Draw(OFFSET + new Vector2(dims.X / 2 + dims.X * i, dims.Y / 2 + dims.Y * j));
                    }
                    else
                    {   //calculate the overflow of the tiles, so when the tiles dims.x cant fit in the background by an equal number, there is overflow, we cna calculate this and fill it up 
                        float xLeft = Math.Min(dims.X, (bkgDims.X - (i * dims.X)));//i*X returns the total space we have used and (bkgDims.X - that number) is the space w have left 
                        float yLeft = Math.Min(dims.Y, (bkgDims.Y - (j * dims.Y)));

                        float xPercentLeft = Math.Min(1, xLeft / dims.X);//1 here is 100% of the tile, xLeft / dims.X is calculating the percentage of the tile that is left
                        float yPercentLeft = Math.Min(1, yLeft / dims.Y);

                        Globals.spriteBatch.Draw(myModel, 
                            new Rectangle((int)(pos.X + OFFSET.X + i * dims.X),
                            (int)(pos.Y + OFFSET.Y + j * dims.Y), 
                            (int)Math.Ceiling(dims.X * xPercentLeft), 
                            (int)Math.Ceiling(dims.Y * yPercentLeft)), 
                            new Rectangle(0, 0, (int)(xPercentLeft * myModel.Bounds.Width), 
                            (int)(yPercentLeft * myModel.Bounds.Height)), 
                            Color.White, 
                            rot, 
                            new Vector2(0, 0), 
                            new SpriteEffects(), 
                            0);//not going to explain dis one .... shits dumb
                    }
                    
                }
            }
        }
    }
}
