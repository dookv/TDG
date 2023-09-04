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
    public class AIPlayer : Player
    {

        public AIPlayer(int ID, XElement DATA) :base(ID, DATA)
        {
/*            spawnPoints.Add(new Portal(new Vector2(50, 50), id));

            spawnPoints.Add(new Portal(new Vector2(Globals.screenWidth / 2, 50), id));
            spawnPoints[spawnPoints.Count - 1].spawnTimer.AddToTimer(500);//listitem.count -1 always gets you the last item

            spawnPoints.Add(new Portal(new Vector2(Globals.screenWidth - 50, 50), id));
            spawnPoints[spawnPoints.Count - 1].spawnTimer.AddToTimer(1000);*/
        } 

        public override void Update(Vector2 OFFSET, Player ENEMY, SquareGrid GRID, LevelDrawManager LEVELDRAWMANAGER)
        {
            base.Update(OFFSET, ENEMY, GRID, LEVELDRAWMANAGER);
        }

        public override void ChangeScore(int SCORE)
        {
            GameGlobals.score += SCORE;//when enemy dies gain score
        }

        public override void CheckIfDefeated()
        {
            if (spawnPoints.Count <= 0 && units.Count <= 0)
            {
                defeated = true;
            }
        }
    }
}
