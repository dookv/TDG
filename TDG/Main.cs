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
using System.IO;
#endregion

namespace TDG
{
    public class Main : Game
    {
        bool lockUpdate;
        private GraphicsDeviceManager _graphics;
        GamePlay gamePlay;
        Basic2d cursor;
        MainMenu mainMenu;

        public Main()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.HardwareModeSwitch = false;//fullscreen issue?
            _graphics.SynchronizeWithVerticalRetrace = true;//vsync?

            Globals.appDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);//basically C:\Users\dookv\AppData\Roaming  this is the place where you saves are stored, saves store sepearte from you game
            // IsMouseVisible = true;
            lockUpdate = false;
        }

        protected override void Initialize()
        {
            /*            Globals.screenWidth = 2560;//1600
                        Globals.screenHeight = 1440;//900*/

            Globals.screenWidth = 1600;//1600
            Globals.screenHeight = 900;//900
            _graphics.PreferredBackBufferWidth = Globals.screenWidth;
            _graphics.PreferredBackBufferHeight = Globals.screenHeight;
            _graphics.SynchronizeWithVerticalRetrace = true;//vsync?
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = this.Content;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.save = new Save(1, "TDG");//load/initialize the ssave right at the start
            Globals.optionsMenu = new OptionsMenu(ApplyOptions);
            SetFullScreen();
            Globals.keyboard = new McKeyboard();
            Globals.mouse = new McMouseControl();
            cursor = new Basic2d("2d\\Cursor", new Vector2(0, 0), new Vector2(30, 30));//load a new basic2d (cursor) with the needed input parameters,
                                                                                       //the (original) POS of the mouse can be 0,0 here becasue we use the OFFSET, in the cursor.draw() method, ( new Vector2(Globals.mouse.newMousePos.X, Globals.mouse.newMouse.Y) ) as position.
                                                                                       //So the original position of the cursor is 0,0(top left corner of the screen), and the offset will put it to the needeed place (the updated position of the mouse)

            Globals.normalEffect = Globals.content.Load<Effect>("effects\\Shaders\\NormalFlat");//when you have more shaders, you will have to load them at seperate times, instaed of all of them at laod time
            Globals.throbEffect = Globals.content.Load<Effect>("effects\\Shaders\\Throb");
            Globals.waveEffect = Globals.content.Load<Effect>("effects\\Shaders\\Wave");
            Globals.lightEffect = Globals.content.Load<Effect>("effects\\Shaders\\Light");
            Globals.lightPulseEffect = Globals.content.Load<Effect>("effects\\Shaders\\LightPulse");
            Globals.normalHighlightEffect = Globals.content.Load<Effect>("effects\\Shaders\\NormalHighlight");


            if (File.Exists(Globals.appDataFilePath + "\\" + Globals.save.gameName + "\\XML\\KeyBinds.xml"))
            {
                GameGlobals.keyBinds = new KeyBindList(Globals.save.GetFile("XML\\KeyBinds.xml"));
            }
            else
            {
                //make file
                XDocument keyBindXML = XDocument.Parse("<Root><Keys><Key n=\"Move Right\"><value>D</value></Key><Key n=\"Move Left\"><value>A</value></Key><Key n=\"Move Up\"><value>W</value></Key><Key n=\"Move Down\"><value>S</value></Key></Keys></Root>");
                //save file
                Globals.save.HandleSaveFormates(keyBindXML, "KeyBinds.xml");
                //load file
                GameGlobals.keyBinds = new KeyBindList(Globals.save.GetFile("XML\\KeyBinds.xml"));

            }
            mainMenu = new MainMenu(ChangeGameState, ExitGame);//these two functions are passed in to the delgates, 
            gamePlay = new GamePlay(ChangeGameState);

            Globals.soundControl = new SoundControl("2d\\Misc\\RomanBuilderMusic_Loop");
            Globals.soundControl.AddSound("Shoot", "2d\\Misc\\ArrowFire", 0.5f);
            Globals.soundControl.AddSound("FireBlaze", "2d\\Misc\\FireSound", 0.5f);
            Globals.soundControl.AddSound("PickUp", "2d\\Misc\\EarthShoot", 0.5f);
            Globals.soundControl.AddSound("Hit", "2d\\Misc\\EarthHit", 0.5f);

            Globals.dragAndDropPacket = new DragAndDropPacket(new Vector2(40, 40));

        }

        //allows the game to run logic such as updating the world,
        //checking for collisions, gathering input, and playing audio
        //gameTime provides a shapshot of timing values. It gives you the number of miliseconds since the last frame. So when you are running at 30 fps and it should be 33 milliseconds per frame,
        //but then you drop a frame, the next time it comes back with 66 milliseconds
        protected override void Update(GameTime gameTime)
        {
/*            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/
            Globals.gameTime = gameTime;//always get the newest frame updated in gametime
/*            _graphics.SynchronizeWithVerticalRetrace = false; //Vsync
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);
            _graphics.SynchronizeWithVerticalRetrace = true;*/
            Globals.keyboard.Update();
            Globals.mouse.Update();

            lockUpdate = false;

            for (int i = 0; i < Globals.msgList.Count; i++)
            {
                Globals.msgList[i].Update();
                if (!Globals.msgList[i].done)
                {
                    if (Globals.msgList[i].lockScreen)//if lockscreen is true for a message in the list
                    {
                        lockUpdate = true;
                    }

                }
                else
                {
                    Globals.msgList.RemoveAt(i);
                    i--;
                }
            }

            if (!lockUpdate)
            {
                if (Globals.gameState == 0)
                {
                    mainMenu.Update();
                }
                else if (Globals.gameState == 1)
                {
                    gamePlay.Update();
                }
                else if (Globals.gameState == 2)
                {
                    Globals.optionsMenu.Update();
                }
            }

            if (Globals.dragAndDropPacket != null)
            {
                Globals.dragAndDropPacket.Update();
            }

            Globals.mouse.UpdateOld();
            Globals.keyboard.UpdateOld();
            base.Update(gameTime);
        }


        public virtual void ApplyOptions(object INFO)//does not work
        {
            FormOption musicVolume = Globals.optionsMenu.GetOptionValue("Music Volume");
            float musicVolumePercent = 1.0f;

            if (musicVolume != null)
            {
                musicVolumePercent = (float)Convert.ToDecimal(musicVolume.value) / 30.0f;
            }
            Globals.soundControl.AdjustVolume(musicVolumePercent);

            SetFullScreen();
        }

        public void SetFullScreen()
        {
            FormOption fullScreenOption = Globals.optionsMenu.GetOptionValue("FullScreen");

            if (Convert.ToInt32(fullScreenOption.value, Globals.culture) == 1)
            {
                _graphics.IsFullScreen = true;
            }
            else
            {
                _graphics.IsFullScreen = false;
            }
            _graphics.ApplyChanges();
        }

        public virtual void ChangeGameState(Object INFO)
        {
            Globals.gameState = Convert.ToInt32(INFO, Globals.culture);
        }

        public virtual void ExitGame(Object INFO)
        {
            System.Environment.Exit(0);
        }


        //called when the game should draw itself
        //gameTime provides a shapshot of timing values
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //you can just close this cpritebacth and start another one with another/different SpriteSOrtMode.
            //Usually you have a spritebatch for the UI, drawing things that are not using a shader, and something that is using a shader & And one that runs of a renderObject that can do some Post-Processing
            Globals.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);//standard SpriteSOrtMode is deffered (Give me all your textures and i am going to run thge shader at the end of the frame)
            if (Globals.gameState == 0)
            {
                mainMenu.Draw();
            }
            else if (Globals.gameState == 1)
            {
                gamePlay.Draw();
            }                                                                                                        //use SpriteSOrtMode Immediate (For shaders, draws it to the backbuffer Immediately, and so you can just edit your shader)
            else if (Globals.gameState == 2)
            {
                Globals.optionsMenu.Draw();
            }
            /* gamePlay.Draw();*/ //vector2.zero is the offset we need for basic2d draw method, navigate all the way down the code
            Globals.normalEffect.Parameters["xSize"].SetValue((float)cursor.myModel.Bounds.Width);//use what you are drawing.mymodel etc to disable the redd effect?
            Globals.normalEffect.Parameters["ySize"].SetValue((float)cursor.myModel.Bounds.Height);
            Globals.normalEffect.Parameters["xDraw"].SetValue((float)((int)cursor.dims.X));
            Globals.normalEffect.Parameters["yDraw"].SetValue((float)((int)cursor.dims.Y));
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();
           
            for (int i = 0; i < Globals.msgList.Count; i++)
            {
                Globals.msgList[i].Draw();
            }


            if (Globals.dragAndDropPacket != null)
            {
                Globals.dragAndDropPacket.Draw();
            }

            cursor.Draw(new Vector2(Globals.mouse.newMousePos.X, Globals.mouse.newMouse.Y), new Vector2(0, 0), Color.White);//call the basic2d draw method. We define a new vecor 2 here.
                                                                                                                            //If we were to use the original variable already defined in the constructor of the McMouseControl class, (Globals.mouse.newMousePos which already has the x & y position set),
                                                                                                                            //we become prone for bugs, the best practice is to always make a new variable
            Globals.spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}