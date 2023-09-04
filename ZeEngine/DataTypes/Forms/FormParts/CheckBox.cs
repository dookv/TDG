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
#endregion

namespace ZeEngine
{
    public class CheckBox : FormPart
    {

        public int boxChecked;



        public Button2d button;

        //public List<FormOption> options = new List<FormOption>();



        public CheckBox(Vector2 POS, Vector2 DIMS, string TITLE, PassObject CHANGED)
            : base(POS, DIMS, TITLE, CHANGED)
        {

            boxChecked = 0;


            button = new Button2d("2d\\UI\\CheckBox", new Vector2(-dims.X / 2 + dims.Y / 2, 0), new Vector2(dims.Y, dims.Y), new Vector2(1, 2), "Fonts\\Font", "", BoxClick, null);

        }

        public override void Update(Vector2 OFFSET)
        {
            button.Update(OFFSET + pos);

            if (boxChecked == 1)
            {
                button.currentAnimation = 1;
            }
            else
            {
                button.currentAnimation = 0;
            }

        }


        public virtual void BoxClick(object INFO)
        {
            boxChecked = 1 - boxChecked;

            if (Changed != null)
            {
                Changed(GetCurrentOption());
            }
        }


        public override FormOption GetCurrentOption()
        {
            FormOption tempItem = null;

            tempItem = new FormOption(title, boxChecked);

            return tempItem;
        }

        public override void LoadData(XElement DATA)
        {
            if (DATA != null)
            {
                if (DATA.Element("boxChecked") != null)
                {
                    boxChecked = Convert.ToInt32(DATA.Element("boxChecked").Value, Globals.culture);
                }
                else
                {

                }
            }
        }

        public override XElement ReturnXML()
        {
            XElement xml = new XElement("Option",
                                new XElement("name", title),
                                new XElement("boxChecked", boxChecked));

            return xml;
        }

        public override void Draw(Vector2 OFFSET, SpriteFont FONT)
        {
            button.Draw(OFFSET + pos);


            Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();


            if (showTitle && FONT != null)
            {
                Vector2 strDims = FONT.MeasureString(title + ": ");


                Globals.spriteBatch.DrawString(FONT, title + ": ", OFFSET + pos + new Vector2(-dims.X / 2 - strDims.X - 10, -strDims.Y / 2), Color.White);
            }


        }



    }
}
