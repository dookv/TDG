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
using System.Text.RegularExpressions;
#endregion

namespace ZeEngine
{
    public class OptionsMenu
    {
        Button2d exitBtn;
        SpriteFont font;
        PassObject ApplyOptions;
        public List<FormPart> formParts = new List<FormPart>();

        public OptionsMenu(PassObject APPLYOPTIONS) 
        {
            ApplyOptions = APPLYOPTIONS;
            exitBtn = new Button2d("2d\\UI\\Button2", new Vector2(Globals.screenWidth /2, Globals.screenHeight - 100), new Vector2(140, 50), new Vector2(1, 1), "fonts\\font", "Exit", ExitClick, null);

            font = Globals.content.Load<SpriteFont>("fonts\\font");


            ArrowSelector tempSelector = null;

/*            tempSelector = new ArrowSelector(new Vector2(Globals.screenWidth / 2, 300), new Vector2(128, 40), "FullScreen", null);
            tempSelector.AddOption(new FormOption("No", 0));
            tempSelector.AddOption(new FormOption("Yes", 1));*/
            formParts.Add(new CheckBox(new Vector2(Globals.screenWidth / 2, 300), new Vector2(128, 40), "FullScreen", null));

            tempSelector = new ArrowSelector(new Vector2(Globals.screenWidth / 2, 400), new Vector2(128, 40), "Music Volume", null);
            for (int i = 0; i < 31; i++)
            {
                tempSelector.AddOption(new FormOption("" + i, i));
                //tempSelector.AddOption(new FormOption("" + i, i));
                
            }
            tempSelector.selected = (int)tempSelector.options.Count / 2;
            formParts.Add(tempSelector);


            tempSelector = new ArrowSelector(new Vector2(Globals.screenWidth / 2, 500), new Vector2(128, 40), "Sound Volume", null);
            for (int i = 0; i < 31; i++)
            {
                tempSelector.AddOption(new FormOption("" + i, i));
                //tempSelector.AddOption(new FormOption("" + i, i));

            }
            tempSelector.selected = (int)tempSelector.options.Count / 2;
            formParts.Add(tempSelector);


            XDocument xml = Globals.save.GetFile("XML\\options.xml");
            LoadData(xml);
        }



        public virtual void Update()
        {
            for (int i = 0; i < formParts.Count; i++)
            {
                formParts[i].Update(Vector2.Zero);
            }

            exitBtn.Update(Vector2.Zero);
        }

        public virtual void ExitClick(Object INFO)
        {
            SaveOptions();//save on exit of the menu
            Globals.gameState = 0;
        }

                                                                                    
        public virtual void LoadData(XDocument DATA)//loop that loads in all form options
                                                    //for the future, if you have more than arrow selectors as form input, create a base class that arrowselector inherrits (and other sliders or selectors),
                                                    //and then loop that base class here (instead of the arrowselctor) so you can get all the form data loaded in
        {
            if (DATA != null)
            {

                List<string> allOptions = new List<string>();
                for (int i = 0; i < formParts.Count; i++)
                {
                    allOptions.Add(formParts[i].title);
                }
                for (int i = 0; i < allOptions.Count; i++)
                {
                    List<XElement> optionList = (from t in DATA.Element("Root").Element("Options").Descendants("Option")
                                                 where t.Element("name").Value == allOptions[i]
                                                 select t).ToList<XElement>();
                    if (optionList.Count > 0)
                    {
                        for (int j = 0; j < formParts.Count; j++)
                        {
                            if (formParts[j].title == allOptions[i])
                            {
                                formParts[j].LoadData(optionList[0]);
                            }
                        }
                    }
                }                                                                                                                   



                #region hardcoded selectQuery

                /* List<XElement> optionList = (from t in DATA.Element("Root").Element("Options").Descendants("Option")
                                              where t.Element("name").Value == "FullScreen"
                                              select t).ToList<XElement>();
                 if (optionList.Count > 0)
                 {
                     for (int j = 0; j < arrowSelectors.Count; j++)
                     {
                         if (arrowSelectors[j].title == "FullScreen")
                         {
                             arrowSelectors[j].selected = Convert.ToInt32(optionList[0].Element("selected").Value, Globals.culture);//set the selected xml value to the selected value of the given arrowSelector
                         }
                     }
                 }*/
                #endregion
            }
        }


        public virtual void SaveOptions()//save the actual dtaa to a file
        {//XDocument can save and load, XElements are just items you create in an XDocument(I think)
            XDocument xml = new XDocument(new XElement("Root",
                                                        new XElement("Options", "")));

            for (int i = 0; i < formParts.Count; i++)
            {
                xml.Element("Root").Element("Options").Add(formParts[i].ReturnXML());//loop and add options from the ReturnXML function in ArrowSelector
            }

            Globals.save.HandleSaveFormates(xml, "options.xml");
            ApplyOptions(null);
        }




        public virtual FormOption GetOptionValue(string NAME)//use this to get something out of your options file
        {
            for (int i = 0; i < formParts.Count; i++)
            {
                if (formParts[i].title == NAME)
                {
                    return formParts[i].GetCurrentOption();
                }
            }
            return null;
        }

        public virtual void Draw()
        {
            Vector2 strDims = font.MeasureString("Options");

            Globals.spriteBatch.DrawString(font, "Options", new Vector2(Globals.screenWidth / 2 - strDims.X/2, 100), Color.White);
            exitBtn.Draw(Vector2.Zero);
            for (int i = 0; i < formParts.Count; i++)
            {
                formParts[i].Draw(Vector2.Zero, font);
            }


        }
    }
}
