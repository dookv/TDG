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
    public class World
    {
        public int levelId;
        public Vector2 offset;
        public List<Projectile2d> projectiles = new List<Projectile2d>();//with lists you can initiate the new object down in your function or whatever, but when defining it here the list variable is never null.
        public List<AttackableOBject> allObjects = new List<AttackableOBject>();
        public List<SceneItem> sceneItems = new List<SceneItem>(); 
        public List<Effect2d> effects = new List<Effect2d>();
        public List<Lootbag> lootbags = new List<Lootbag>();
        public CharacterMenu charactermenu;
        public ExitMenu exitmenu;
        public User user;
        public AIPlayer aiPplayer;
        public SquareGrid grid;
        public TileBkg2d bkg;
        public LevelDrawManager levelDrawManager;//should do in the leveldrawmanager
        /*public Matrix translation;*/
        public UI ui;

        PassObject ResetWorld, changeGameState, ChangePlayState;

        public World(PassObject RESETWORLD, int LEVELID, PassObject CHANGEGAMESTATE, PassObject CHANGEPLAYSTATE)//when we create a new object of world, (see gameplay class), we want to pass in a delegate
        {

            ResetWorld = RESETWORLD;
            changeGameState = CHANGEGAMESTATE;
            ChangePlayState = CHANGEPLAYSTATE;
            levelId = LEVELID;
            levelDrawManager = new LevelDrawManager();
            GameGlobals.PassGold = AddGold;
            GameGlobals.PassLootBag = AddLootBag;
            GameGlobals.PassProjectile = AddProjectile;//(delgate)wherever a projectile can be used. Now we have set GameGlobals.PassProjectile to the AddProjectile function.
            GameGlobals.PassEffect = AddEffect;
            GameGlobals.PassMob = AddMob;                                          //when we call GameGlobals.PassProjectile we actually call AddProjectile()
            GameGlobals.PassBuilding = AddBuilding;
            GameGlobals.PassSpawnPoint = AddSpawnPoint;
            GameGlobals.CheckScroll = CheckScroll;
            GameGlobals.paused = false;//pased means you stop the updating of the game



            LoadData(levelId);//loads the level via Id
            charactermenu = new CharacterMenu(user.hero);
            exitmenu = new ExitMenu(ChangePlayState);
            offset = new Vector2(0, 0);
            ui = new UI(ResetWorld, user.hero);

            bkg = new TileBkg2d("2d\\WorldItems\\Tiles\\ForestTile", new Vector2(-100, -100), new Vector2(144, 144), new Vector2(grid.totalPhysicalDims.X + 100, grid.totalPhysicalDims.Y + 100));

            
        }


        public virtual void Update()
        {
            ui.Update(this);
            if (!DontUpdate())
            {
                levelDrawManager.Update();
                allObjects.Clear();//clear the list every frame
                allObjects.AddRange(user.GetAllObjects());
                allObjects.AddRange(aiPplayer.GetAllObjects());


                user.Update(offset, aiPplayer, grid, levelDrawManager);
                aiPplayer.Update(offset, user, grid, levelDrawManager);


                for (int i = 0; i < lootbags.Count; i++)
                {
                    lootbags[i].Update(offset, user);
                    if (lootbags[i].done)
                    {
                        lootbags.RemoveAt(i);
                        i--;
                    }
                }



                //increment for the for loop. When .count starts, i is no lingerw 0 so i++ runs. this goes on untill i is no longer smaller than the projectiles.count and so it stops. (end of loop)
                for (int i=0; i<projectiles.Count; i++) //loop trough all projectiles
                {
                    projectiles[i].Update(offset, allObjects);//so for each projectile list item (selected via array key which is [i]) we call the update method.
                                            //(every singe projectile in the list has the values we give to it in the projectile2d constructor,
                                            //and so we call the update method on every singe projectile)
                    if (projectiles[i].done)
                    {
                        projectiles.RemoveAt(i);//remove the specified projectile [i] from the list when done is true (conditions when done is true are specified in projectile2d)
                        i--;//remove 1 item from the loop, otherwise you are just looping an empty list item (a missing list item) 
                    }
                }

               
                for (int i = 0; i < effects.Count; i++)
                {
                    effects[i].Update(offset);
                    if (effects[i].done)
                    {
                        effects.RemoveAt(i);
                        i--;
                    }
                }


                for (int i = 0; i < sceneItems.Count; i++)
                {
                    sceneItems[i].Update(offset);
                    sceneItems[i].UpdateDraw(offset, levelDrawManager);
                }



            }
            else
            {
                if (Globals.keyboard.GetPress("Enter") && (user.hero.dead || (user.buildings.Count <= 0)))//implement bool lost//TODO now the spawned towers are also counting as buildings, so if the main tower gets destroyed while there are still summoned towers about, the ai of the spiderlings will freeze because they dont have a target
                {
                    GameGlobals.score = 0;
                    ResetWorld(null);
                }
            }


            charactermenu.Update();
            exitmenu.Update();

            if (grid != null)
            {
                grid.Update(offset);
            }


            if (Globals.keyboard.GetSinglePress("G"))
            {
                grid.showGrid = !grid.showGrid;
            }

            if (Globals.keyboard.GetSinglePress("C"))
            {
                charactermenu.Active = true;
                exitmenu.Active = false;
            }

            if (Globals.keyboard.GetSinglePress("Back"))
            {
                ResetWorld(null);
                changeGameState(0);
            }

            if (Globals.keyboard.GetSinglePress("U"))//implement bool lost//TODO now the spawned towers are also counting as buildings, so if the main tower gets destroyed while there are still summoned towers about, the ai of the spiderlings will freeze because they dont have a target
            {
                GameGlobals.paused = !GameGlobals.paused;//If you wnat to just flip a bool,(true -> false or false -> true) you can just put an exclemation point in front of it.
                                                         //So basically we are saying GameGlobals.paused = true; (true because the default value for this GameGlobal is false)

            }

            if (Globals.keyboard.GetSinglePress("Escape"))
            {
                exitmenu.Active = !exitmenu.Active;
                charactermenu.Active = false;
            }

            if (aiPplayer.defeated)
            {
                Globals.msgList.Add(new DismissibleMessage(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(250, 110), "You won, select next level", Color.Purple, true, WinConfirm));
               // WinConfirm(null);
            }
        }



        #region GameGlobal delgates
        public virtual void AddMob(object INFO)
        {
            Unit tempUnit = (Unit)INFO;//speed needs to be fixed

            if (user.id == tempUnit.ownerId)
            {
                user.AddUnit(tempUnit);
            }
            else if (aiPplayer.id == tempUnit.ownerId)
            {
                aiPplayer.AddUnit(tempUnit);
            }
            //commented this line out because the speed was multiplied by 2
            //aiPplayer.AddUnit((Mob)INFO);//we call the addunit class from player, because aiplayer class inherrits player class with addunit method
        }


        public virtual void AddBuilding(object INFO)//the same function name as in player, kinda dumb yo?
        {
            Building tempBuilding = (Building)INFO;

            if (user.id == tempBuilding.ownerId)
            {
                user.AddBuilding(tempBuilding);
            }
            else if (aiPplayer.id == tempBuilding.ownerId)
            {
                aiPplayer.AddBuilding(tempBuilding);
            }
           
        }


        public virtual void AddProjectile(object INFO)//(part of delgate)whatever we pass in here, we are going to cast it as a projectile,a nd it is going to be added to the projectiles list
        {
            //everything that we pass in here HAS TO BE A PROJECTILE, you can test it by putting an extra if at the top that iterates over all existing projectiles or something, idk yet.
            projectiles.Add((Projectile2d)INFO);//defining the type is brackets (Projectile2d) is called "casting"
        }


        public virtual void AddEffect(object INFO)
        {
            effects.Add((Effect2d)INFO);
        }



        public virtual void AddGold(object INFO)
        {
            
            PlayerValuePacket packet = (PlayerValuePacket)INFO;

            if (user.id == packet.playerId)
            {
                user.gold += (int)packet.value;
            }
            else if (aiPplayer.id == packet.playerId )
            {
                user.gold += (int)packet.value;
            }
        }


        public virtual void AddLootBag(object INFO)
        {
            Lootbag tempBag = (Lootbag)INFO;

            lootbags.Add(tempBag);

        }



        public virtual void AddSpawnPoint(object INFO)
        {
            SpawnPoint tempSpawnPoint = (SpawnPoint)INFO;

            if (user.id == tempSpawnPoint.ownerId)
            {
                user.AddSpawnPoint(tempSpawnPoint);//we call players addspwnpoint method, not the current AddSpawnPoint method in this world class
            }
            else if (aiPplayer.id == tempSpawnPoint.ownerId)
            {
                aiPplayer.AddSpawnPoint(tempSpawnPoint);
            }

         
        }
        /*        public virtual void CalculateTranslation(object INFO)
                {
                    Vector2 tempPos = (Vector2)INFO;
                    var dx = (Globals.screenWidth / 2) - tempPos.X;
                    var dy = (Globals.screenHeight / 2) - tempPos.Y;
                    translation = Matrix.CreateTranslation(dx, dy, 0f);

                }*/
        #endregion

        public virtual void CheckScroll(object INFO)
        {
            Vector2 tempPos = (Vector2)INFO;//tempPos is herho.pos because we call the Checkscroll gameGlobal delgate in the hero class
            float maxMovement = user.hero.speed * 4.5f;

            float diff = 0;
            if (tempPos.X < -offset.X + (Globals.screenWidth / 2))//Globals.screenWidth * .4f is 40% of the total screenwidth
            {
                diff = -offset.X + (Globals.screenWidth / 2) - tempPos.X;
                offset = new Vector2(offset.X + Math.Min(maxMovement, diff), offset.Y);//you can do  Hero.speed * 2 to make the screen go 2 tims faster so you can catch bugs
            }
            if (tempPos.X > -offset.X + (Globals.screenWidth / 2))//Globals.screenWidth * .6f
            {
                diff = tempPos.X - (-offset.X + (Globals.screenWidth / 2));
                offset = new Vector2(offset.X - Math.Min(maxMovement, diff), offset.Y);
            }


            if (tempPos.Y < -offset.Y + (Globals.screenHeight / 2))
            {
                diff = -offset.Y + (Globals.screenHeight / 2) - tempPos.Y;
                offset = new Vector2(offset.X, offset.Y + Math.Min(maxMovement, diff));
            }
            if (tempPos.Y > -offset.Y + (Globals.screenHeight / 2))
            {
                diff = tempPos.Y - (-offset.Y + (Globals.screenHeight / 2));
                offset = new Vector2(offset.X, offset.Y - Math.Min(maxMovement, diff));
            }

            #region alternateCamera
            //This camera movement is different, not centered around the character
            /*float maxMovement = user.hero.speed * 4.5f;

            float diff = 0;


            if (tempPos.X < -offset.X + (Globals.screenWidth * .4f))
            {
                diff = -offset.X + (Globals.screenWidth * .4f) - tempPos.X;
                offset = new Vector2(offset.X + Math.Min(maxMovement, diff), offset.Y);
            }
            if (tempPos.X > -offset.X + (Globals.screenWidth * .6f))
            {
                diff = tempPos.X - (-offset.X + (Globals.screenWidth * .6f));
                offset = new Vector2(offset.X - Math.Min(maxMovement, diff), offset.Y);
            }


            if (tempPos.Y < -offset.Y + (Globals.screenHeight * .4f))
            {
                diff = -offset.Y + (Globals.screenHeight * .4f) - tempPos.Y;
                offset = new Vector2(offset.X, offset.Y + Math.Min(maxMovement, diff));
            }
            if (tempPos.Y > -offset.Y + (Globals.screenHeight * .6f))
            {
                diff = tempPos.Y - (-offset.Y + (Globals.screenHeight * .6f));
                offset = new Vector2(offset.X, offset.Y - Math.Min(maxMovement, diff));
            }*/
            #endregion

        }


        public virtual bool DontUpdate()
        {
            if (user.hero.dead || user.buildings.Count == 0 || GameGlobals.paused || ui.skillMenu.active || charactermenu.Active || exitmenu.Active)
            {
                return true;
            }
            return false;                                                                                                                               
        }



        public virtual void LoadData(int LEVEL)
        {
            XDocument xml = XDocument.Load("XML\\Levels\\Level" + LEVEL + ".xml");//load in file

            XElement tempElement = null;

            if (xml.Element("Root").Element("User") != null)//inside the xml file we want the element Root, which contains the User tag with more data tags
            {
                tempElement = xml.Element("Root").Element("User");//so tempelement contains all the data tags we store in the <User> tag inside the XML
            }

            user = new User(1, tempElement);//current player is always id 1. if not multiplayer

            if (user.hero != null)
            {
                GameGlobals.AddToInventory = user.hero.AddToInventory;
            }

            tempElement = null;

            if (xml.Element("Root").Element("AIPlayer") != null)
            {
                tempElement = xml.Element("Root").Element("AIPlayer");
            }

            grid = new SquareGrid(new Vector2(25, 25), new Vector2(-100, -100), new Vector2(Globals.screenWidth + 200, Globals.screenHeight + 200), xml.Element("Root").Element("GridItems"));

            aiPplayer = new AIPlayer(2, tempElement);



            List<XElement> sceneItemList = (from t in xml.Element("Root").Element("Scene").Descendants("SceneItem")
                                           select t).ToList<XElement>();
            Type sType = null;
            for (int i = 0; i < sceneItemList.Count; i++)
            {
                sType = Type.GetType("TDG." + sceneItemList[i].Element("type").Value, true);
                sceneItems.Add((SceneItem)(Activator.CreateInstance(sType, new Vector2(Convert.ToInt32(sceneItemList[i].Element("Pos").Element("x").Value, Globals.culture), Convert.ToInt32(sceneItemList[i].Element("Pos").Element("y").Value, Globals.culture)), new Vector2((float)Convert.ToDouble(sceneItemList[i].Element("scale").Value, Globals.culture)))));
            }

        }

        public virtual void WinConfirm(Object INFO)
        {
            ResetWorld(null);
            ChangePlayState(1);
        }

        public virtual void Draw(Vector2 OFFSET)//this is still the offset needed for basic2d, we pass it in hero.draw becasue the draw function in the hero class inherrits basic2d, and in this case overrides the draw from the ionherrited basic2d class
        {
            bkg.Draw(offset);
            grid.DrawGrid(offset);

            for (int i = 0; i < lootbags.Count; i++)
            {
                lootbags[i].Draw(offset);
            }
            user.Draw(offset);//call player base class draw 
            aiPplayer.Draw(offset);


/*            for (int i = 0; i < sceneItems.Count; i++)
            {
                sceneItems[i].Draw(offset);
            }*/

            if (levelDrawManager != null)
            {
                levelDrawManager.Draw();
            }

            for (int i = 0; i < projectiles.Count; i++) //loop trough all projectiles
            {
                projectiles[i].Draw(offset);//draws last is on top
            }

            for (int i = 0; i < effects.Count; i++) //loop trough all projectiles
            {
                effects[i].Draw(offset);//draws last is on top
            }

            
            ui.Draw(this, user.hero, -offset.X + (Globals.screenWidth / 2));

            charactermenu.Draw();
            exitmenu.Draw();
        }
    }
}
