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
    public class SceneItem : Animated2d
    {


        public int drawLocId, drawLayer;//id = find out if the drawmanager already has an instance of this object, layer = 0 to 10 drawing from bottom to top,
                                        //layer example
                                        //Clouds
                                        //birds
                                        //baseLayer
                                        //footprints
                                        //floor(not bkg),

        public Vector2 sortOffset;//draw an item with and offset to comensate for the center origin drawing
        public PassObject DrawManagerDel;//delgate gets set in DrawSlot constructor

        public SceneItem(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, Vector2 SCALE) 
            : base(PATH, POS, DIMS * SCALE, FRAMES, Color.White)
        {
            drawLocId = 0;
            drawLayer = 5;//default 5 plent of layers below it and infinite layers above it
            sortOffset = new Vector2(0, 0);
        }
        #region properties
        public virtual Vector2 SortDrawPos
        {
            get
            { 
                return pos + sortOffset; //return the actual pos of the item + the offset
            }
            set
            {
                pos = value - (sortOffset);
            }
        }
        #endregion


        public virtual void Update(Vector2 OFFSET, LevelDrawManager LEVELDRAWMANAGER)
        {
            if (LEVELDRAWMANAGER != null)
            {
                UpdateDraw(OFFSET, LEVELDRAWMANAGER);
            }
        }

        public virtual void UpdateDraw(Vector2 OFFSET, LevelDrawManager LEVELDRAWMANAGER)//Thisupdates all the info that leveldrawmanager needs in order to update properly(mainly the offset and if it is drawable)
                                                                                         //It will also register the item in the levelDrawManager,
        {
            if (drawLocId == 0 && LEVELDRAWMANAGER != null)
            {
                LEVELDRAWMANAGER.AddOrUpdateDraws(this, true);
            }

            if (DrawManagerDel != null)
            {
                DrawManagerDel(new DrawSlotUpdatePackage(OFFSET, true));//this updates the offsets
            }
        }







    }

}
