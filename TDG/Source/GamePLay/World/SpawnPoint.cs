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
    public class SpawnPoint : AttackableOBject
    { 
        public McTimer spawnTimer;
        public List<MobChoice> mobChoices = new List<MobChoice>();
        public SpawnPoint(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int OWNERID, XElement DATA) 
            : base(PATH, POS, DIMS, FRAMES, OWNERID)
        {

            dead = false;
            health = 3;
            healthMax = health;
            spawnTimer = new McTimer(2400);
            LoadData(DATA);
            //hitDistance = 35.0f;

        }

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            spawnTimer.UpdateTimer();
            if (spawnTimer.Test())
            {
                SpawnMob();
                spawnTimer.ResetToZero();
            }
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public virtual void SpawnMob()
        {
            GameGlobals.PassMob(new Imp(new Vector2(pos.X, pos.Y), new Vector2(1, 1), ownerId));
        }


        public virtual void LoadData(XElement DATA)
        {
            if (DATA != null)
            {
                spawnTimer.AddToTimer(Convert.ToInt32(DATA.Element("timerAdd").Value, Globals.culture));
                List<XElement> mobList = (from t in DATA.Descendants("mob")
                                            select t).ToList<XElement>();


                for (int i = 0; i < mobList.Count; i++)
                {
                    //here we select the attribute rae from the XML.
                    //rule of thumb is no more than 3 attributes in 1 tag in xml, id id often an attribute
                    mobChoices.Add(new MobChoice(mobList[i].Value, Convert.ToInt32(mobList[i].Attribute("rate").Value, Globals.culture)));

                }
            }  
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
