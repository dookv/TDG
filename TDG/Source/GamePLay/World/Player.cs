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
    public class Player
    {
        public bool defeated;
        public int id, gold;
        public Hero hero;
        public List<Unit> units = new List<Unit>();
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        public List<Building> buildings = new List<Building>();

        public Player(int ID, XElement DATA) 
        {
            id = ID;
            gold = 10;
            defeated = false;
            LoadData(DATA);
        } 

        public virtual void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            if (hero != null)
            {
                hero.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
            }


            for (int i = 0; i < spawnPoints.Count; i++)
            {
                spawnPoints[i].Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
                if (spawnPoints[i].dead)
                {
                    LEVELDRAWMANAGER.Remove(spawnPoints[i]);
                    spawnPoints.RemoveAt(i);
                    i--;
                }
            }


            for (int i = 0; i < units.Count; i++)//we wnat o be ab;le to have enemy's on both user and aiPlayer side, so we as the player can spawn units (the smae goes for spawnpoints)
            {
                units[i].Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
                if (units[i].dead)
                {
                    LEVELDRAWMANAGER.Remove(units[i]);
                    ChangeScore(1);
                    units.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < buildings.Count; i++)
            {
                buildings[i].Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
                if (buildings[i].dead)
                {
                    LEVELDRAWMANAGER.Remove(buildings[i]);
                    buildings.RemoveAt(i);
                    i--;
                }
            }

            CheckIfDefeated();//all players check if defeated every frame
        }

        public virtual void AddBuilding(Object INFO)//DELGAAAAATE
        {
            Building tempBuilding = (Building)INFO;//CASTING TO UNIT
            tempBuilding.ownerId = id;
            buildings.Add(tempBuilding);
        }

        public virtual void AddUnit(Object INFO)//DELGAAAAATE
        {
            Unit tempUnit = (Unit)INFO;//CASTING TO UNIT
            tempUnit.ownerId = id;
            units.Add(tempUnit);
        } 
        
        public virtual void AddSpawnPoint(Object INFO)
        {
            SpawnPoint tempSpawnPoint = (SpawnPoint)INFO;
            tempSpawnPoint.ownerId = id;
            spawnPoints.Add((SpawnPoint)INFO);
        }

        public virtual void ChangeScore(int SCORE)
        {

        }


        public virtual void CheckIfDefeated()
        {

        }

        public virtual List<AttackableOBject> GetAllObjects()//this is still a method, only its a list of total attackable objects
        {
            List<AttackableOBject> tempObjects = new List<AttackableOBject>();
            tempObjects.AddRange(units.ToList<AttackableOBject>());
            tempObjects.AddRange(spawnPoints.ToList<AttackableOBject>());
            tempObjects.AddRange(buildings.ToList<AttackableOBject>());
            if (hero != null)
            {
                tempObjects.Add(hero);
            }

            return tempObjects;
        }

        public virtual void LoadData(XElement DATA)//Data here is <User> and <AIPlayer>, defined in World LoadData
        {
            List<XElement> spawnList = (from t in DATA.Descendants("SpawnPoint") 
                                        select t).ToList<XElement>();

            Type sType = null;

            for (int i = 0; i < spawnList.Count; i++)
            {

                sType = Type.GetType("TDG."+ spawnList[i].Element("type").Value, true);//sType, we dynamically creating a new class from a string,
                                                                                       //Instead of the new POrtal constructor, we usse sType. The value from sType we get from the XML <type> tag.
                                                                                       //IN this case the XML contains <type>Portal</type> so sType is class Portal

                //in this case we use an activator, (spawnpoint) is the type, followed by activator initiation, and then instead of "new Portal()",
                //we use the variable sType (which is the portal class created from a string).
                //Then you just fill in your constructor variables the same way 
                //sType is way more expensive qua resources, so if you are cathing lag you might hardcode this, (so use new Portal instead of activator)
                spawnPoints.Add((SpawnPoint)(Activator.CreateInstance(sType, new Vector2(Convert.ToInt32(spawnList[i].Element("Pos").Element("x").Value, Globals.culture), Convert.ToInt32(spawnList[i].Element("Pos").Element("y").Value, Globals.culture)), id, spawnList[i], new Vector2(1, 1))));//we can add a spawnpoint via here or via the delgate AddSpawnPoint
                
            }


            List<XElement> buildingList = (from t in DATA.Descendants("Building")
                                        select t).ToList<XElement>();

            for (int i = 0; i < buildingList.Count; i++)
            {
                //create the instance via an activator(dynamic class via xml, or just create an anstance of a class yoursels, just like tower here)
               /* sType = Type.GetType("TDG." + spawnList[i].Element("type").Value, true);
                buildings.Add((Building)(Activator.CreateInstance(sType, new Vector2(Convert.ToInt32(buildingList[i].Element("Pos").Element("x").Value, Globals.culture), Convert.ToInt32(buildingList[i].Element("Pos").Element("y").Value, Globals.culture)), new Vector2(1, 1), id)));*/
                buildings.Add(new Tower(new Vector2(Convert.ToInt32(buildingList[i].Element("Pos").Element("x").Value, Globals.culture), Convert.ToInt32(buildingList[i].Element("Pos").Element("y").Value, Globals.culture)), new Vector2(1, 1), id));
            }

            if (DATA.Element("Hero") != null)//create Globals xmlLoad helper class
            {
                hero = new Hero("2d\\AnimatedSprites\\TDGAnimation", new Vector2(Convert.ToInt32(DATA.Element("Hero").Element("Pos").Element("x").Value, Globals.culture), Convert.ToInt32(DATA.Element("Hero").Element("Pos").Element("y").Value, Globals.culture)), new Vector2(100, 100), new Vector2(4, 2), id);
            }
        }


        public virtual void Draw(Vector2 OFFSET)
        {
            if (hero != null)
            {
                hero.Draw(OFFSET);
            }

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                spawnPoints[i].Draw(OFFSET);

            }

            for (int i = 0; i < buildings.Count; i++)//we wnat o be ab;le to have enemy's on both user and aiPlayer side, so we as the player can spawn units (the smae goes for spawnpoints)
            {
                buildings[i].Draw(OFFSET);

            }


            for (int i = 0; i < units.Count; i++)//we wnat o be ab;le to have enemy's on both user and aiPlayer side, so we as the player can spawn units (the smae goes for spawnpoints)
            {
                units[i].Draw(OFFSET);

            }



        }


    }
}
