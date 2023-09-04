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
    public class KeyBindMenu : Menu2d
    {
        public List<Button2d> buttons = new List<Button2d>();

        public List<KeyBindButton> keyBindButtons = new List<KeyBindButton>();
        public PassObject Exit;
        
        public KeyBindMenu(PassObject EXIT)
            : base(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(350, 500), null) 
        {
            Exit = EXIT;
            hasCloseBtn = false;
            for (int i = 0; i < GameGlobals.keyBinds.keyBinds.Count; i++)
            {
                keyBindButtons.Add(new KeyBindButton("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(30, 30), new Vector2(1, 1), "fonts\\font", GameGlobals.keyBinds.keyBinds[i].key, CheckSelected, null, GameGlobals.keyBinds.keyBinds[i].name, CheckDuplicates));

                keyBindButtons[keyBindButtons.Count - 1].info = keyBindButtons[keyBindButtons.Count - 1];
            }

            buttons.Add(new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(150, 40), new Vector2(1, 1), "fonts\\font", "Return", ExitClick, 1));
        }

        public override void Update()
        {
            base.Update();
            if (Active)
            {
                for (int i = 0; i < keyBindButtons.Count; i++)
                {
                    keyBindButtons[i].Update(topLeft + new Vector2(dims.X - keyBindButtons[i].dims.X * 1.25f, 30 + 38 * i));
                }

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Update(topLeft + new Vector2(dims.X / 2, dims.Y - keyBindButtons[i].dims.Y * 1.25f + 50 * i));
                }
            }


        }

        public virtual void ExitClick(Object INFO)
        {
            XDocument keyBindsDoc = new XDocument(new XElement("Root", ""));
            keyBindsDoc.Element("Root").Add(GameGlobals.keyBinds.ReturnXML());
            Globals.save.HandleSaveFormates(keyBindsDoc, "KeyBinds.xml");

            Exit(INFO);
        }

        public virtual void CheckDuplicates(Object INFO)
        {
            KeyBindButton tempButton = (KeyBindButton)INFO;

            for (int i = 0; i < keyBindButtons.Count; i++)
            {
                if (keyBindButtons[i] != tempButton && keyBindButtons[i].text == tempButton.text)
                {
                    keyBindButtons[i].SetNew(tempButton.previousKey);
                }
            }
        }

        public virtual void CheckSelected(Object INFO)
        {
            KeyBindButton tempButton = (KeyBindButton)INFO;

            for (int i = 0; i < keyBindButtons.Count; i++)
            {
                if (keyBindButtons[i] != tempButton)
                {
                    keyBindButtons[i].selected = false;
                }
            }
        }


        public override void Draw()
        {
            base.Draw();

            if (Active)
            {
                for (int i = 0; i < keyBindButtons.Count; i++)
                {
                    keyBindButtons[i].Draw(topLeft + new Vector2(dims.X - keyBindButtons[i].dims.X * 1.25f, 30 + 38 * i));
                }

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Draw(topLeft + new Vector2(dims.X / 2, dims.Y - keyBindButtons[i].dims.Y * 1.25f + 50 * i));
                }
            }
        }

    }
}
