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
    public class ArrowSelector : FormPart
    {

        public int selected;
        public List<Button2d> buttons = new List<Button2d>();   
        public List<FormOption> options = new List<FormOption>();  //protected, so adding options  

        public ArrowSelector(Vector2 POS, Vector2 DIMS, string TITLE, PassObject CHANGED) 
            :base(POS, DIMS, TITLE, CHANGED)
        {

            selected = 0;
            buttons.Add(new Button2d("2d\\UI\\ArrowLeft", new Vector2(-dims.X/2 + dims.Y/2, 0), new Vector2(dims.Y, dims.Y), new Vector2(1, 1), "fonts\\font", "", ArrowLeftClick, null));
            buttons.Add(new Button2d("2d\\UI\\ArrowRight", new Vector2(dims.X/2 - dims.Y/2, 0), new Vector2(dims.Y, dims.Y), new Vector2(1, 1), "fonts\\font", "", ArrowRightClick, null));
        }

        public override void Update(Vector2 OFFSET)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update(OFFSET + pos);
            }
        }

        public virtual void AddOption(FormOption FORMOPTION)
        {
            options.Add(FORMOPTION);
        }

        public virtual void ArrowLeftClick(Object INFO)
        {
            selected--;
            if (selected < 0 )
            {
                selected = 0;//if you wan to wrap your cont around, you woeuld say options.count - 1 
            }
        }

        public virtual void ArrowRightClick(Object INFO)
        {
            selected++;
            if (selected >= options.Count)
            {
                selected = options.Count - 1;
            }
        }

        public override FormOption GetCurrentOption()
        {
            return options[selected];
        }

        public override XElement ReturnXML()//we chose the XElement class here as functrion datatype, because know we are going to return a XElement
                                           //this is the function you are going to need when saving data to xml, use this all over the place
        {
            XElement xml = new XElement("Option",
                                        new XElement("name", title),//nesting xml tags here, title is the titl;e of the option when creating a new arrowSelector
                                        new XElement("selected", selected),
                                        new XElement("SelectedName", options[selected].name));
            return xml;
        }

        public override void LoadData(XElement DATA)
        {
            if (DATA != null)
            {
                if (DATA.Element("selected") != null)
                {
                    selected = Convert.ToInt32(DATA.Element("selected").Value, Globals.culture);//set the selected xml value to the selected value of the given arrowSelector
                }
                
            }
        }

        public override void Draw(Vector2 OFFSET, SpriteFont FONT)
        {

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(OFFSET + pos);
            }
            Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();

            if (options.Count > selected && selected >= 0)
            {
                Vector2 strDims = FONT.MeasureString(options[selected].name);
                Globals.spriteBatch.DrawString(FONT, options[selected].name, OFFSET + pos + new Vector2(-strDims.X / 2, -strDims.Y/2), Color.White);

                strDims = FONT.MeasureString(title + ": ");
                Globals.spriteBatch.DrawString(FONT, title + ": ", OFFSET + pos + new Vector2(-dims.X /2 - strDims.X - 10, -strDims.Y / 2), Color.White);

            }


        }


    }
}
