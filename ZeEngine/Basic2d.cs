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
    public class Basic2d
    {
        public float rot;
      
        public Vector2 pos, dims, frameSize;
        public Texture2D myModel;
        public string modelStr;

        public Basic2d(string PATH, Vector2 POS, Vector2 DIMS)
        {
            pos = new Vector2(POS.X, POS.Y);//here we set the input parameters to the variables we defined above
            dims = new Vector2(DIMS.X, DIMS.Y);
            rot = 0.0f;
            modelStr = PATH;
            myModel = Globals.content.Load<Texture2D>(modelStr);//we cn now load anything via the path we bass into basic2d. content.Load<Texture2D>(); Here texture2d is the type we ant to load.
        }

        public virtual void Update(Vector2 OFFSET)
        {

        }

        public virtual bool Hover(Vector2 OFFSET)
        {
            return HoverImg(OFFSET);
        }
        
        public virtual bool HoverImg(Vector2 OFFSET)
        {
            Vector2 mousePos = new Vector2(Globals.mouse.newMouse.X, Globals.mouse.newMouse.Y);//just easier to read by putting it in a variable
            //So basically what we are doing here is comparing the mouse value.x to a value that calculatesthe width and pos of an drawn image.
            //Example: mouse pos X = 80, we draw the image at X 100, the image width (X) is 40. The image is drawn from the center so it is if(80 >= 100 - 20->(40 / 2)
            //We do this for all the side, up, down, left, right(button example )
            if (mousePos.X >= (pos.X + OFFSET.X) - dims.X / 2 && mousePos.X <= (pos.X + OFFSET.X) + dims.X / 2 && mousePos.Y >= (pos.Y + OFFSET.Y) - dims.Y / 2 && mousePos.Y <= (pos.Y + OFFSET.Y) + dims.Y / 2)//idk man 
            {
                return true;
            }

            return false;
        }


        public virtual bool HoverFirst(Vector2 OFFSET)
        {
            Vector2 mousePos = new Vector2(Globals.mouse.firstMousePos.X, Globals.mouse.firstMousePos.Y);
            
            if (mousePos.X >= (pos.X + OFFSET.X) - dims.X / 2 && mousePos.X <= (pos.X + OFFSET.X) + dims.X / 2 && mousePos.Y >= (pos.Y + OFFSET.Y) - dims.Y / 2 && mousePos.Y <= (pos.Y + OFFSET.Y) + dims.Y / 2)//idk man 
            {
                return true;
            }

            return false;
        }

        public virtual void Draw(Vector2 OFFSET)//center the image to the middle
        {                                       //The offset allows us to draw the image somewhere other that its positions says to be, except for menu's it's actually rare that you draw something where it's position thinks it is.
                                                //lets say the screen shifts off to the left now the original position is fucked. so the offset allows you to pass in the worlds offset, which will take care of the latter problem.

            if (myModel != null)//If a model is given, call the globals spritebatch.draw(myModel,) with the other nececarry input parameters that the draw function requires.
            {                   //Use the required pos and dims we get from the basic2d input parameters todeclare the pos and dims for the rectangle
                Globals.spriteBatch.Draw(myModel, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), null, Color.White, rot, new Vector2(myModel.Bounds.Width/2, myModel.Bounds.Height/2), new SpriteEffects(), 0);
            }
        }


        public virtual void Draw(Vector2 OFFSET, Color COLOR)
        {                                      

            if (myModel != null)
            {                  
                Globals.spriteBatch.Draw(myModel, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), null, COLOR, rot, new Vector2(myModel.Bounds.Width / 2, myModel.Bounds.Height / 2), new SpriteEffects(), 0);
            }
        }


        //overloaded, so we can put in a custom origin instead of the middle of the image (which is the original draw in this class)
        public virtual void Draw(Vector2 OFFSET, Vector2 ORIGIN, Color COLOR)//overloaded, center image to the specified origin vector x,y
        {
            if (myModel != null)
            {
                Globals.spriteBatch.Draw(myModel, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), null, COLOR, rot, new Vector2(ORIGIN.X, ORIGIN.Y), new SpriteEffects(), 0);
            }
        }


    }
}
