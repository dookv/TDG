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
    public class WorldMap
    {

        public Basic2d bkg;
        public List<Button2d> levels = new List<Button2d>();
        PassObject ChangeGameState;

        public WorldMap(PassObject CHANGEGAMESTATE) 
        {
            ChangeGameState = CHANGEGAMESTATE;
            bkg = new Basic2d("2d\\UI\\WorldMap", new Vector2(Globals.screenWidth/2, Globals.screenHeight/2), new Vector2(Globals.screenWidth, Globals.screenHeight));
            LoadData();
        }


        public virtual void Update()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].Update(Vector2.Zero);
            }
        }

        public virtual void LevelClicked(Object INFO)//if we want to do more than just creating a new level with the PassObject changeGameState
        {
            ChangeGameState(INFO);
        }


        public virtual void LoadData()
        {
            XDocument xml = XDocument.Load("XML\\Levels.xml");//load in file
            List<XElement> levelList = (from t in xml.Descendants("Level")
                                            select t).ToList<XElement>();


            for (int i = 0;i < levelList.Count;i++)  
            {
                levels.Add(new Button2d("2d\\UI\\ButtonSmall", new Vector2(Convert.ToInt32(levelList[i].Element("Pos").Element("x").Value, Globals.culture), Convert.ToInt32(levelList[i].Element("Pos").Element("y").Value, Globals.culture)), new Vector2(40, 40), new Vector2(1, 1), "fonts\\font", levelList[i].Attribute("id").Value, LevelClicked, levelList[i].Attribute("id").Value));
            }
        }


        public virtual void Draw()
        {

            bkg.Draw(Vector2.Zero);

            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].Draw(Vector2.Zero);
            }
        }

    }
}
