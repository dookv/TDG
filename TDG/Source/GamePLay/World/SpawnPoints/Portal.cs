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
    public class Portal : SpawnPoint
    {
        //public McTimer portalTimer = new McTimer(10000);
        public Portal(Vector2 POS, int OWNERID, XElement DATA, Vector2 FRAMES) 
            : base("2d\\WorldItems\\Portal", POS, new Vector2(90, 90), FRAMES, OWNERID, DATA)
        {
            health = 10;
            killValue = 5;
            healthMax = health;
        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
          
          base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public override void SpawnMob()
        {

            int num = Globals.rand.Next(0, 100 + 1);//the lowest random number (0) + the highest number - 1
            int total = 0;
            Mob tempMob = null;

            for (int i = 0; i < mobChoices.Count; i++)
            {
                total += mobChoices[i].rate;

                if (num < total)
                {
                    Type sType = Type.GetType("TDG." + mobChoices[i].mobStr, true);//string
                    tempMob = (Mob)(Activator.CreateInstance(sType, new Vector2(pos.X, pos.Y), new Vector2(1, 1), ownerId));//we can add a spawnpoint via here or via the delgate AddSpawnPoint

                    break;

                }
            }


            

            if (tempMob != null)
            {
                GameGlobals.PassMob(tempMob);
            }

            
        }

        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            #region shadernotworkingbecausetimer?
            /*            Globals.lightPulseEffect.Parameters["OriginalColor"].SetValue(new Vector3(-1.0f, -1.0f, -1.0f));
                        Globals.lightPulseEffect.Parameters["LightColor"].SetValue(new Vector3(0.5f, 0.0f, 0.5f));
                        Globals.lightPulseEffect.Parameters["RimPower"].SetValue((float)4.0);
                        Globals.lightPulseEffect.Parameters["LightDiameter"].SetValue((float)1.5f);
                        Globals.lightPulseEffect.Parameters["PulsateFrequency"].SetValue((float)0.001f);
                        Globals.lightPulseEffect.Parameters["PulsateAmplitude"].SetValue((float)0.3f);
                        Globals.lightPulseEffect.Parameters["Time"].SetValue((float)portalTimer.Timer);

                        Globals.lightPulseEffect.CurrentTechnique.Passes[0].Apply();*/
            #endregion
            base.Draw(OFFSET);
        }
    }
}
