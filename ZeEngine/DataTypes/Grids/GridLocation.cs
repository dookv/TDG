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
#endregion

namespace ZeEngine
{
    public class GridLocation//a sigular grid unit
    {
        public bool filled, impassable, unPathable, hasBeenUsed, isViewable;//impassable = filled and cannot walk trough it, unPathable = cant walk trough it, but isnt necesary filled, elevated platform
        public float fScore, cost, currentDist, distLef;
        public Vector2 parent, pos;

        public GridLocation()
        {
            filled = false;
            impassable = false;
            unPathable = false;
            hasBeenUsed = false;
            isViewable = false;
            cost = 1.0f;

        }

        public GridLocation(float COST, bool FILLED) 
        {
            cost = COST;//the cost of moving trough here, lets say a river had cost 8 and desert has cost 1.5 because a desert is much faster to move trough than a river
            filled = FILLED;//grid filled or not
            hasBeenUsed = false;
            isViewable = false;
            unPathable = false;
            impassable = false;
        }

        public GridLocation(Vector2 POS, float COST, bool FILLED, float FSCORE)
        {
            cost = COST;
            filled = FILLED;
            impassable = FILLED;
            unPathable = false;
            hasBeenUsed = false;
            isViewable = false;

            pos = POS;

            fScore = FSCORE;
        }

        public void SetNode(Vector2 PARENT, float FSCORE, float CURRENTDIST)
        {
            parent = PARENT;
            fScore = FSCORE;
            currentDist = CURRENTDIST;
        }

        public virtual void SetToFilled(bool IMPASSABLE)
        {
            filled = true;
            impassable = IMPASSABLE;
        }


    }
}
