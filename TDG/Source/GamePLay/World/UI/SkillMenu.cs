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
    public class SkillMenu
    {

        public bool active;//menu active or not
        Animated2d bkg;
        Skill selectedSkill;//drag n drop skill menu doesnt need this
        Hero hero;



        public SkillMenu(Hero HERO)
        {
            hero = HERO;
            bkg = new Animated2d("2d\\UI\\ButtonSmall", new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(300, 400), new Vector2(1, 1), Color.Gray);
        }

        public virtual void Update()
        {
            if (active)
            {
                for (int i = 0; i < hero.skills.Count; i++)
                {
                    if (hero.skills[i].icon.Hover(bkg.pos - bkg.dims / 2 + new Vector2(60 + (i % 4) * 50, 100 + (i / 4) * 50)))
                    {

                        if (Globals.mouse.LeftClick())//we check the click for this specific button here, this if is general mouseclick,
                                                      //but because we are inside another if loop that targets the hover of our skill icons, we basically target that buttonClick
                        {
                            selectedSkill = hero.skills[i];
                            selectedSkill.icon.color = Color.Blue;
                        }
                    }
                    else
                    {
                        hero.skills[i].icon.color = Color.White;
                    }
                }
                if (Globals.keyboard.GetSinglePress("D1"))//D1 is the number 1
                {
                    hero.skillBar.slots[0].skillButton = new SkillButton("2d\\UI\\HealtBar2", new Vector2(0, 0), new Vector2(40, 40), new Vector2(1, 1), hero.SetSkill, selectedSkill);
                }

                if (Globals.keyboard.GetSinglePress("D2"))
                {
                    hero.skillBar.slots[1].skillButton = new SkillButton("2d\\UI\\HealtBar2", new Vector2(0, 0), new Vector2(40, 40), new Vector2(1, 1), hero.SetSkill, selectedSkill);
                    
                }

                if (Globals.keyboard.GetSinglePress("D3"))
                {
                    hero.skillBar.slots[2].skillButton = new SkillButton("2d\\UI\\HealtBar2", new Vector2(0, 0), new Vector2(40, 40), new Vector2(1, 1), hero.SetSkill, selectedSkill);
                }





            }
        }

        public virtual void ToggleActive()
        {
            active = !active;
            selectedSkill = null;
        }

        public virtual void Draw()
        {
            if (active)
            {

                bkg.Draw(Vector2.Zero);

                for (int i = 0; i < hero.skills.Count; i++)
                {
                    hero.skills[i].icon.Draw(bkg.pos - bkg.dims / 2 + new Vector2(60 + (i%4) * 50, 100 + (i/4) * 50));//(i%4) means that when the for loop reaches 4 and i = 4, i is 0 again
                }
            }
        }
    }
}
