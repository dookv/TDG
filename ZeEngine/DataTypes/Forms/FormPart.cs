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
using System.Security.Cryptography.X509Certificates;
#endregion

namespace ZeEngine
{
    public class FormPart
    {
        public bool showTitle;
        public string title;
        public Vector2 pos, dims;
        public PassObject Changed;

        public FormPart(Vector2 POS, Vector2 DIMS, string TITLE, PassObject CHANGED) 
        {
            title = TITLE;
            pos = POS;
            dims = DIMS;
            Changed = CHANGED;
            showTitle = true;
        }

        public virtual void Update(Vector2 OFFSET)
        {

        }


        public virtual FormOption GetCurrentOption()
        {
            return null;
        }



        public virtual void LoadData(XElement DATA)
        {
            if (DATA != null)
            {

            }
        }



        public virtual XElement ReturnXML()//we chose the XElement class here as functrion datatype, because know we are going to return a XElement
                                           //this is the function you are going to need when saving data to xml, use this all over the place
        {
            XElement xml = new XElement("Option",
                                        new XElement("name", title));
            return xml;
        }



        public virtual void Draw(Vector2 OFFSET, SpriteFont FONT)
        {

        }
    }
}
