#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif
using Microsoft.Xna.Framework.Media;
#endregion

namespace ZeEngine
{
    public class DrawSlot//essentially this is a package that encapsulates the scene item with a couple other variables to use in drawmanager
    {
        public bool drawing;//allowed to draw this item yes no, 1 example, when it is off the screen, when its dead whatever reason 
        public int id;//id matches with drawLocId from sceneItem
        public Vector2 offset;//worlds offset
        public SceneItem item;//the sceneitem we are working with 

        public DrawSlot(SceneItem ITEM, bool DRAWING)
        {
            drawing = DRAWING;
            id = ITEM.drawLocId;
            item = ITEM;
            item.DrawManagerDel = new PassObject(UpdateDetails);
        }

        public virtual void UpdateDetails(Object INFO)
        {
            DrawSlotUpdatePackage temp = (DrawSlotUpdatePackage)INFO;
            offset = new Vector2(temp.offset.X, temp.offset.Y);
            drawing = temp.drawing;
        }

        public virtual void Draw()
        {
            if(drawing)
            {

                item.Draw(offset);
                
            }
            else
            {

            }
        }
    }
}
