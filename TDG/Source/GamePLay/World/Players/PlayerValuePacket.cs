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
    //a packet in this case is just a set of data that ca be passed trough as an individual variable(ofter object)
    public class PlayerValuePacket//packet in networking is the data that you are sending (client to server)
    {

        public int playerId;
        public Object value;


        public PlayerValuePacket(int PLAYERID, Object VALUE) 
        {
            playerId = PLAYERID;
            value = VALUE;
        }


    }
}
