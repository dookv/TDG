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
    public class InventoryItem : Draggable2d
    {

        public InventorySlot slot;
        public InventoryItem(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, Color COLOR)
        : base(PATH, POS, DIMS, FRAMES, COLOR)
        {
            type = "InventoryItem";
            slot = null;
        }
    }
}
