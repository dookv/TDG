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
    public class SkillSelectionTypePacket
    {


        public int selectionType;
        public Skill skill;
        public SkillSelectionTypePacket(int SELECTIONTYPE, Skill SKILL)
        {
            selectionType = SELECTIONTYPE;
            skill = SKILL;
        }





    }
}
