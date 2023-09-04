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
    public class TestItem : InventoryItem
    {


        public TestItem() 
            : base("2d\\Misc\\OffhandIcon1", new Vector2(0, 0), new Vector2(32, 32), new Vector2(1, 1), Color.White)
        {
    
        }
    }
}
