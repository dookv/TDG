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
    //packet of data
    public class MobChoice
    {

        public int rate;//
        public string mobStr;

        public MobChoice(string MOBSTR, int RATE) 
        {
            rate = RATE;
            mobStr = MOBSTR;
        }
    }
}
