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
    public class ExitMenu : Menu2d
    {
        public int state;
        public KeyBindMenu keyBindMenu;
        
        public List<Button2d> buttons = new List<Button2d>();
        public PassObject Exit;
        
        public ExitMenu(PassObject EXIT)
            : base(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(350, 500), null) 
        {

            state = 0;
            Exit = EXIT;
            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(150, 40), new Vector2(1, 1), "fonts\\font", "Return", ReturnClick, 0));
            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(150, 40), new Vector2(1, 1), "fonts\\font", "Key Binds", KeyBindClick, 0));
            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(150, 40), new Vector2(1, 1), "fonts\\font", "Exit Level", ExitClick, 1));
            keyBindMenu = new KeyBindMenu(ReturnFromKeybinds);
        }

        public override void Update()
        {
            if (state == 0)
            {
                base.Update();
                if (Active)
                {
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].Update(topLeft + new Vector2(dims.X / 2, 30 + 50 * i));
                    }
                }
            }
            else if (state == 1)
            {
                keyBindMenu.Update();
            }



        }

        public virtual void ExitClick(Object INFO)
        {
            Exit(INFO);
        }

        public virtual void ReturnClick(Object INFO)
        {
            Active = false;
        }

        public virtual void KeyBindClick(Object INFO)
        {
            state = 1;
            keyBindMenu.Active = true;
        }

        public virtual void ReturnFromKeybinds(Object INFO)
        {
            state = 0;
            keyBindMenu.Active = false;
            /*Active = false;*/
        }

        public override void Draw()
        {
            if (state == 0)
            {
                base.Draw();

                if (Active)
                {
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].Draw(topLeft + new Vector2(dims.X / 2, 30 + 50 * i));
                    }
                }
            }
            else if (state == 1)
            {
                keyBindMenu.Draw();
            }

        }

    }
}
