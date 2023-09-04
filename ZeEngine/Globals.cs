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
{                                              //because we declae these degates outside of the globals class, we dont have to do Globals.PassObject, if we want to acces this delgate.
                                               //(look in GameGlobals, we just call PassObject). This means these are the only 2 delgates we will ever need!
    public delegate void PassObject(object i);//object is anything you want it to be (and as much as you want), it doesnt return anything.
                                              //so i want to pass something somewhere and pass a function somewhere
                                              //see GameGlobals for example
    public delegate object PassObjectAndReturn(object i);//this one is the same, but it returns an object too

    public class Globals
    {
        public static int screenHeight, screenWidth, gameState = 0;
        public static string appDataFilePath;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static McKeyboard keyboard;
        public static Random rand = new Random();
        public static McMouseControl mouse;
        public static SoundControl soundControl;
        public static List<Message> msgList = new List<Message>();
        public static OptionsMenu optionsMenu;//not in engine globals...???
        public static DragAndDropPacket dragAndDropPacket;
        public static Save save;
        public static System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("nl-BE");

        public static Effect normalEffect, throbEffect, waveEffect, lightEffect, lightPulseEffect, normalHighlightEffect;//baseEffect

        public static GameTime gameTime;


        public static float GetDistance(Vector2 pos, Vector2 target)//can also use Vector2.Distance(pos, target);
        {
            return (float)Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2)); //the default distance formula-> square root: (x2 - x1)^2 + (y2 - x1)^2
        }

        public static Vector2 RadialMovement(Vector2 pos, Vector2 focus, float speed)//ai
        {
            float dist = GetDistance(pos, focus);

            if (dist <= speed)//if the distance to the target is smaller than your speed, just go that smaller distance. So if the distance to the player is 3 and speed is 5, go the 3 distance
            {
                return focus - pos;
            }
            else
            {
                return(focus - pos) * speed/dist;
            }
        }


        public static float RotateTowards(Vector2 Pos, Vector2 focus)
        {

            float h, sineTheta, angle;
            if(Pos.Y-focus.Y != 0)
            {
                h = (float)Math.Sqrt(Math.Pow(Pos.X-focus.X, 2) + Math.Pow(Pos.Y-focus.Y, 2));
                sineTheta = (float)(Math.Abs(Pos.Y-focus.Y)/h); //* ((item.Pos.Y-focus.Y)/(Math.Abs(item.Pos.Y-focus.Y))));
            }
            else
            {
                h = Pos.X-focus.X;
                sineTheta = 0;
            }

            angle = (float)Math.Asin(sineTheta);
            //the upper part of the code is the geometry part, so getting the first angle, second part is getting the other part of the rotation
            /*<------------------>*/

            // Drawing diagonial lines here.
            //Quadrant 2
            if(Pos.X-focus.X > 0 && Pos.Y-focus.Y > 0)
            {
                angle = (float)(Math.PI*3/2 + angle);
            }
            //Quadrant 3
            else if(Pos.X-focus.X > 0 && Pos.Y-focus.Y < 0)
            {
                angle = (float)(Math.PI*3/2 - angle);
            }
            //Quadrant 1
            else if(Pos.X-focus.X < 0 && Pos.Y-focus.Y > 0)
            {
                angle = (float)(Math.PI/2 - angle);
            }
            else if(Pos.X-focus.X < 0 && Pos.Y-focus.Y < 0)
            {
                angle = (float)(Math.PI/2 + angle);
            }
            else if(Pos.X-focus.X > 0 && Pos.Y-focus.Y == 0)
            {
                angle = (float)Math.PI*3/2;
            }
            else if(Pos.X-focus.X < 0 && Pos.Y-focus.Y == 0)
            {
                angle = (float)Math.PI/2;
            }
            else if(Pos.X-focus.X == 0 && Pos.Y-focus.Y > 0)
            {
                angle = (float)0;
            }
            else if(Pos.X-focus.X == 0 && Pos.Y-focus.Y < 0)
            {
                angle = (float)Math.PI;
            }

            return angle;
        }
    }
}
