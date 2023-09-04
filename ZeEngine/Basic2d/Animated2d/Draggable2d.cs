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
    public class Draggable2d : Animated2d
    {

        public bool dragging;
        public string type;


        public Draggable2d(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, Color COLOR) 
            :base(PATH, POS, DIMS, FRAMES, COLOR)
        {

            type = "Draggable2d";


        }



        public override void Update(Vector2 OFFSET)
        {
            base.Update(OFFSET);


            if (HoverFirst(OFFSET) && Globals.mouse.LeftClickHold() && Globals.GetDistance(Globals.mouse.firstMousePos, Globals.mouse.newMousePos) > 15)
            {
                Globals.dragAndDropPacket.SetItem(this, type, modelStr);
            }
        }
    }
}
