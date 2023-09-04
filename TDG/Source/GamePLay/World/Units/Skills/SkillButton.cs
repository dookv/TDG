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
    public class SkillButton : Button2d
    {
        public Vector2 lastOffset;
        public Skill skill;
        public SkillButtonSlot slot;

        public SkillButton(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, PassObject BUTTONCLICKED, object INFO)
            : base(PATH, POS, DIMS, FRAMES, "", "", BUTTONCLICKED, INFO)
        {
            skill = (Skill)INFO;
            slot = null;
        }

        public override void Update(Vector2 OFFSET)
        {
            lastOffset = OFFSET;
            if (skill != null)
            {
                base.Update(OFFSET);
            }


        }


        public override void RunBtnClick()//override button2d
        {
            if (ButtonClicked != null)
            {
                SkillSelectionTypePacket tempPacket = new SkillSelectionTypePacket(1, (Skill)info);//1 is pressing the button, 0 is clicking the button
                                                                                                   //Prevents The blink skill from firering directly when the button in the skillbar is pressed, 

                if (Hover(lastOffset))//checks if the mous hoves over the drawn button
                {
                    tempPacket.selectionType = 0;
                }
                
                ButtonClicked(tempPacket);
            }

            Reset();
        }




        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);

            if (skill != null)
            {
                skill.icon.Draw(OFFSET);//wherever we are drawing the button, we are drawing the icon on top of it
            }
        }
    }
}
