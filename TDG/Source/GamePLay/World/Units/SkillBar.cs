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
    public class SkillBar
    {
        public float spacer;
        public Vector2 firstPos;

        public List<SkillButtonSlot> slots = new List<SkillButtonSlot>();




        public SkillBar(Vector2 FIRSTPOS, float SPACER, int NUMSLOTS) 
        {
            spacer = SPACER;
            firstPos = FIRSTPOS;
            for (int i = 0; i < NUMSLOTS; i++)
            {
                slots.Add(new SkillButtonSlot(new Vector2(0, 0)));
            }
        }


        public virtual void Update(Vector2 OFFSET)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Update(firstPos + new Vector2(spacer * i, 0));
            }


            if (Globals.keyboard.GetSinglePress("D1"))//D1 is the number 1
            {
                if (slots.Count > 0)
                {
                    if (slots[0].skillButton != null)
                    {
                        slots[0].skillButton.RunBtnClick();
                    }
                }
            }

            if (Globals.keyboard.GetSinglePress("D2"))
            {
                if (slots.Count > 1)
                {
                    if (slots[1].skillButton != null)
                    {
                        slots[1].skillButton.RunBtnClick();
                    }
                }
            }

            if (Globals.keyboard.GetSinglePress("D3"))
            {
                if (slots.Count > 2)
                {
                    if (slots[2].skillButton != null)
                    {
                        slots[2].skillButton.RunBtnClick();
                    }
                }
            }
        }

        public virtual void Draw(Vector2 OFFSET)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(firstPos + new Vector2(spacer * i, 0));
            }
        }
    }
}
