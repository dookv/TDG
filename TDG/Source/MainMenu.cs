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
    public class MainMenu
    {

        public Basic2d bkg;
        public PassObject PlayClickDel, ExitClickDel;
        public List<Button2d> buttons = new List<Button2d>();  

        public MainMenu(PassObject PLAYCLICKDEL, PassObject EXITCLICKDEL) 
        {
            PlayClickDel = PLAYCLICKDEL;
            ExitClickDel = EXITCLICKDEL;
            bkg = new Basic2d("2d\\UI\\MainMenu", new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(Globals.screenWidth, Globals.screenHeight));
            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(140, 50), new Vector2(1, 1), "fonts\\font", "Play", PlayClickDel, 1));//main menu gets initialized in main, to the delgates we give into the input parameters there are the functions that close and start our game 
            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(140, 50), new Vector2(1, 1), "fonts\\font", "Options", PlayClickDel, 2));
            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(140, 50), new Vector2(1, 1), "fonts\\font", "Exit", ExitClickDel, null));

        }

        public virtual void Update()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update(new Vector2(225, 600 + 55 * i));//for every i the button is going down 45
            }
        }

        public virtual void Draw() 
        {
            bkg.Draw(Vector2.Zero);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(new Vector2(225, 600 + 55 * i));//for every i the button is going down 45
            }
        }

    }
}
