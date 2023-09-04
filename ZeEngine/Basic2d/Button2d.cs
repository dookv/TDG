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
using System.Security.Cryptography.X509Certificates;
#endregion

namespace ZeEngine
{
    public class Button2d : Animated2d
    {

        public bool isPressed, isHovered;
        public string text;
        public Color hoverColor;

        public SpriteFont font;
        public object info;//pass anything and it will come out of the buton click
        public PassObject ButtonClicked;

        public Button2d(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, string FONTPATH, string TEXT, PassObject BUTTONCLICKED, object INFO)//PassObject BUTTONCLICKED is out delgate and object INFO is what we pass trough that delgate
            : base(PATH, POS, DIMS, FRAMES, Color.White)
        {
            info = INFO;
            text = TEXT;
            ButtonClicked = BUTTONCLICKED;


            if (FONTPATH != "")//if string of fontpath is empty, dont load a font
            {
                font = Globals.content.Load<SpriteFont>(FONTPATH);
            }

            isPressed = false;
            hoverColor = new Color(200, 230, 255);

            CreatePerFrameAnimations();
            frameAnimations = true;//base.frameAnimations = true;
        }

        public override void Update(Vector2 OFFSET)
        {

            if (Hover(OFFSET))//checks if the mous hoves over the drawn button
            {
                isHovered = true;

                if (Globals.mouse.LeftClick())
                {
                    isPressed = true;
                    isHovered = false;
                }
                else if (Globals.mouse.LeftClickRelease())
                {
                    RunBtnClick();
                }

            }
            else
            {
                isHovered = false;
                
            }

            if (!Globals.mouse.LeftClick() && ! Globals.mouse.LeftClickHold())
            {
                isPressed = false; 
            }

            base.Update(OFFSET);
        }

        public virtual void Reset()
        {
            isPressed = false;
            isHovered = false;
        }

        public virtual void RunBtnClick()
        {
            if (ButtonClicked != null)
            {

                ButtonClicked(info);//Lets say for the play button,In main we initialize a new menu, in which we pass this function as a delgate (passObject) to play the game
                                    //ChangeGameState(Object INFO){
                                    //Globals.gameState = Convert.ToInt32(INFO, Globals.culture);
                                    //}
                                    //This passobject (currently the ChangeGameState() function gets put in the new button call in MainMenu. we give 2 parameters, the (in this case, ChangeGameState() function ( BUTTONCLICKED in the consructor of this class) and a value (INFO in the constructor of this button class))
                                    //The value(INFO) is 1 and passobject(BUTTONCLICKED) is ChangeGameState().
                                    //So for the main menu case, this ButtonClicked(info); line is actually ChangeGameState(1)
            }

            Reset();
        }


        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {

            Color tempColor = Color.White;
            if (isPressed)
            {
                tempColor = Color.Gray;
            }
            else if (isHovered)
            {
                tempColor = hoverColor;
            }

            //in this case this shader does fuck all because i am using SamplerState.PointClamp in this project because of pixel art
            Globals.normalEffect.Parameters["xSize"].SetValue((float)myModel.Bounds.Width);//in the fx file xSize etc is a float, so that is why we cast them to float here, these are already concatonated to int so we dont need to do it
            Globals.normalEffect.Parameters["ySize"].SetValue((float)myModel.Bounds.Height);
            Globals.normalEffect.Parameters["xDraw"].SetValue((float)((int)dims.X));//we concatonate it to an int, because pixel shaders dont deal with partial pixels (float values)
            Globals.normalEffect.Parameters["yDraw"].SetValue((float)((int)dims.Y));
            Globals.normalEffect.Parameters["filterColor"].SetValue(tempColor.ToVector4());//in the fx file filterColor is a float4 which is a vector4
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();//this sline sets all the effects
            base.Draw(OFFSET);
            //draw the button text on top of the button

            if (font != null)
            {
                Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);
                Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
                Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
                Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
                Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

                Vector2 strDims = font.MeasureString(text);
                Globals.spriteBatch.DrawString(font, text, pos + OFFSET + new Vector2(-strDims.X / 2, -strDims.Y / 2), Color.White);
            }

           
        }
    }
}
