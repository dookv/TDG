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
    public class KeyBindButton : Button2d
    {
        public string keyBindString, previousKey;
        public bool selected;
        public PassObject Updated;

        public KeyBindButton(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, string FONTPATH, string TEXT, PassObject BUTTONCLICKED, object INFO, string KEYBINDSTRING, PassObject UDATED)//PassObject BUTTONCLICKED is out delgate and object INFO is what we pass trough that delgate
            : base(PATH, POS, DIMS, FRAMES, FONTPATH, TEXT, BUTTONCLICKED, INFO)
        {
            selected = false;
            Updated = UDATED;
            keyBindString = KEYBINDSTRING;
            previousKey = "";
        }




        public override void Update(Vector2 OFFSET)
        {


            base.Update(OFFSET);

            if (selected)
            {
                if (Globals.keyboard.pressedKeys.Count > 0)//some key is pressed
                {
                   
                    SetNew(Globals.keyboard.pressedKeys[0].key);//this will take the first key currently pressed
                }
            }
        }

        public virtual void SetNew(string TXT)
        {
            KeyBind tempKeyBind = GameGlobals.keyBinds.GetKeyBindByName(keyBindString);
            text = TXT;
            if (tempKeyBind != null)
            {
                previousKey = tempKeyBind.key;
                tempKeyBind.key = text;


                if (Updated != null)
                {
                    Updated(this);
                }
                selected = false;
            }
        }

        public override void RunBtnClick()
        {
            selected = true;
            base.RunBtnClick();
        }


        public override void Draw(Vector2 OFFSET)
        {
            Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

            Vector2 strDims = font.MeasureString(keyBindString);
            Globals.spriteBatch.DrawString(font, keyBindString, pos + OFFSET + new Vector2(-200, -strDims.Y / 2), Color.White);
            base.Draw(OFFSET);
        }
    }
}
