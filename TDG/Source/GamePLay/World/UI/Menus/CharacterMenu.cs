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
    public class CharacterMenu : Menu2d
    {
        public Hero hero;
        public TextZone textZone;

        public CharacterMenu(Hero HERO)
            : base(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(350, 500), null) 
        {
            textZone = new TextZone(new Vector2(0, 0), "bag of earth nuts", (int)(dims.X * .79), 22, "fonts\\font", Color.Purple);
            hero = HERO;
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < hero.inventorySlots.Count; i++)
            {
                Vector2 tempVec = new Vector2(40 + 54 * (int)(i % 6), 300 + 54 * (int)(i / 6));

                hero.inventorySlots[i].Update(topLeft + tempVec);
            }
        }

        public override void Draw()
        {
            base.Draw();

            if (Active)
            {
/*                Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);
                Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
                Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
                Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
                Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                Globals.normalEffect.CurrentTechnique.Passes[0].Apply();*/

                string tempStr = "" + hero.name;
                Vector2 strDims = font.MeasureString(tempStr);
                Globals.spriteBatch.DrawString(font, tempStr, topLeft + new Vector2(bkg.dims.X / 2 - strDims.X / 2, 40), Color.White);

                textZone.Draw(topLeft + new Vector2(40, 100));
                for (int i = 0; i < hero.inventorySlots.Count; i++)
                {
                    Vector2 tempVec = new Vector2(40 + 54 * (int)(i%6), 300 + 54 * (int)(i / 6));

                    hero.inventorySlots[i].Draw(topLeft + tempVec);
                }
            }
        }

    }
}
