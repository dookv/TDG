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
    public class GamePlay
    {
        int playState;
        World world;
        WorldMap worldMap;
        PassObject changeGameState;

        public GamePlay(PassObject CHANGEGAMESTATE) 
        {
            changeGameState = CHANGEGAMESTATE;
            playState = 1;
            ResetWorld(null);
            worldMap = new WorldMap(LoadLevel);
        }

        public virtual void Update()
        {
            if (playState == 0)//when in a menu for example, the playstate can be 1 or something else. for example 1 to stop time and stop the update method
            {
                world.Update();
            }
            if (playState == 1)
            {
                worldMap.Update();
            }

        }

        public virtual void ChangePlayState(Object INFO)
        {
            
            playState = Convert.ToInt32(INFO, Globals.culture);
        }

        public virtual void LoadLevel(Object INFO)
        {
            playState = 0;
            int tempLevel = Convert.ToInt32(INFO, Globals.culture);
            Globals.msgList.Add(new Message(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(200, 60),"Level: " + tempLevel, 3500, Color.Purple, false));
            world = new World(ResetWorld, Convert.ToInt32(INFO, Globals.culture), changeGameState, ChangePlayState);
        }

        public virtual void ResetWorld(Object INFO)//threading and reflection
        {
            int levelId = 1;

            if (world != null)
            {
                levelId = world.levelId;
            }
            world = new World(ResetWorld, levelId, changeGameState, ChangePlayState);//create a new world, reset the world
        }

        public virtual void Draw()
        {
            if (playState == 0)
            {
                world.Draw(Vector2.Zero);
            }
            if (playState == 1)
            {
                worldMap.Draw();
            }

        }


    }
}
