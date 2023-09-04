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
    public class AttackableOBject : Animated2d//now all attackable objects are scene items this creates a bug where the character animation is not playing and everything is drawn doubble,
                                              //to prevent this inherrit Animated2d instead of SceneItem,remove levelDrawmanager from the base update
                                              //and in Player remove all LEVELDRAWMANAGER.Remove(spawnPoints[i]); from all the different update loops,
    {
        public bool dead, throbing, hover;
        public int ownerId, killValue;
        public float speed, hitDistance, health, healthMax;
        public McTimer throbTimer = new McTimer(1000);

        public AttackableOBject(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int OWNERID) 
            : base(PATH, POS, DIMS, FRAMES, Color.White)//give frames in hero
        {

            speed = 40.0f;//default speed for a thing that inherrits this unit class, if not overridden by the derrived class
            dead = false;
            throbing = false;
            hover = false;
            health = 1;
            killValue = 1;
            healthMax = health;
            hitDistance = 25.0f;
            ownerId = OWNERID;
        }

        public virtual void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            hover = false;
            if (Hover(OFFSET))
            {
                hover = true;
            }

            if (throbing)
            {
                throbTimer.UpdateTimer();
                if (throbTimer.Test())
                {
                    throbing = false;
                    throbTimer.ResetToZero();
                }
            }

            base.Update(OFFSET);//, LEVELDRAWMANAGER
        }

        public virtual void GetHit(AttackableOBject ATTACKER,float DAMAGE)
        {
            health -= DAMAGE;
            throbing = true;
            throbTimer.ResetToZero();//reset everytime you get hit
            if (health <= 0)
            {
                dead = true;

                GameGlobals.PassGold(new PlayerValuePacket(ATTACKER.ownerId, killValue));
            }

        }

        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {


            if (hover)
            {
/*                Globals.normalHighlightEffect.Parameters["xSize"].SetValue((float)myModel.Bounds.Width);
                Globals.normalHighlightEffect.Parameters["ySize"].SetValue((float)myModel.Bounds.Height);
                Globals.normalHighlightEffect.Parameters["xDraw"].SetValue((float)((int)dims.X));
                Globals.normalHighlightEffect.Parameters["yDraw"].SetValue((float)((int)dims.Y));
                Globals.normalHighlightEffect.Parameters["len1"].SetValue(1);
                Globals.normalHighlightEffect.Parameters["len2"].SetValue(3);
                Globals.normalHighlightEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                Globals.normalHighlightEffect.CurrentTechnique.Passes[0].Apply();
                base.Draw(OFFSET);
*/
            }
            if (throbing)
            {
                //idea for a shader, the shader targets all pixels of an item, whta if we were to draw an item with low opacity pixel borders (that you dont really initially see),
                //but when the shader is applied, the color changes thus the border becoming visisble
                Globals.throbEffect.Parameters["SINLOC"].SetValue((float)Math.Sin(((float)throbTimer.Timer / (float)throbTimer.MSec + (float)Math.PI / 2) * ((float)Math.PI * 3)));
                Globals.throbEffect.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
                Globals.throbEffect.CurrentTechnique.Passes[0].Apply();//whatever shader you call CurrentTechnique.Passes[0].Apply(); on is the active shader



                //this is the wave shder i wrote, needs some work tho, it only works with movement
                /*                Globals.waveEffect.Parameters["Amplitude"].SetValue((float)0.02f);
                                Globals.waveEffect.Parameters["Frequency"].SetValue((float)5.0f);
                                Globals.waveEffect.Parameters["Speed"].SetValue((float)0.1f);
                                Globals.waveEffect.CurrentTechnique.Passes[0].Apply();
                                base.Draw(OFFSET); include base draw if you want to draw it right behinf an item for a cool wavey border effect*/

                //static light shader
                /*                Globals.lightEffect.Parameters["LightColor"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));
                                Globals.lightEffect.Parameters["RimIntensity"].SetValue((float)2f);
                                Globals.lightEffect.Parameters["RimPower"].SetValue((float)4.0);
                                Globals.lightEffect.Parameters["LightRadius"].SetValue((float)0.6f);
                                Globals.lightEffect.CurrentTechnique.Passes[0].Apply();*/

                //pulse light shader
                /*                Globals.lightPulseEffect.Parameters["OriginalColor"].SetValue(new Vector3(-1.0f, -1.0f, 1.0f));
                                Globals.lightPulseEffect.Parameters["LightColor"].SetValue(new Vector3(0.5f, 0.0f, 0.5f));
                                Globals.lightPulseEffect.Parameters["RimPower"].SetValue((float)4.0);
                                Globals.lightPulseEffect.Parameters["LightDiameter"].SetValue((float)1.4f);
                                Globals.lightPulseEffect.Parameters["PulsateFrequency"].SetValue((float)0.005f);
                                Globals.lightPulseEffect.Parameters["PulsateAmplitude"].SetValue((float)2.0f);
                                Globals.lightPulseEffect.Parameters["Time"].SetValue((float)throbTimer.Timer);

                                Globals.lightPulseEffect.CurrentTechnique.Passes[0].Apply();*/

            }
            else 
            {
                //in this case this shader does fuck all because i am using SamplerState.PointClamp in this project because of pixel art
                Globals.normalEffect.Parameters["xSize"].SetValue((float)myModel.Bounds.Width);//in the fx file xSize etc is a float, so that is why we cast them to float here, these are already concatonated to int so we dont need to do it
                Globals.normalEffect.Parameters["ySize"].SetValue((float)myModel.Bounds.Height);
                Globals.normalEffect.Parameters["xDraw"].SetValue((float)((int)dims.X));//we concatonate it to an int, because pixel shaders dont deal with partial pixels (float values)
                Globals.normalEffect.Parameters["yDraw"].SetValue((float)((int)dims.Y));
                Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());//in the fx file filterColor is a float4 which is a vector4
                Globals.normalEffect.CurrentTechnique.Passes[0].Apply();//this sline sets all the effects
            }

            base.Draw(OFFSET);
        }
    }
}
