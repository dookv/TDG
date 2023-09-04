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
    public class TextZone
    {
        public int maxWidth, lineHeight;
        string str;
        public Vector2 pos, dims;
        public Color color;
        public SpriteFont font;
        public List<string> lines = new List<string>();

        public TextZone(Vector2 POS, string STR, int MAXWIDTH, int LINEHEIGHT, string FONT, Color FONTCOLOR)
        {
            pos = POS;
            str = STR;
            maxWidth = MAXWIDTH;
            lineHeight = LINEHEIGHT;
            color = FONTCOLOR;
            font = Globals.content.Load<SpriteFont>(FONT);

            if (str != "")
            {
                ParseLines();
            }
        }

        #region properties
        public string Str
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
            }
        }
        #endregion

        public virtual void ParseLines()
        {
            lines.Clear();
            List<string> wordList = new List<string>();

            string tempString = "";//current focus string
            int largestWidth = 0, currentwidth = 0;

            if (Str != "" && Str != null)
            {
                wordList = Str.Split(' ').ToList<string>();//list of all words, word are the things sep[erates by a space(split(' ')), remove spaces
                for (int i = 0; i < wordList.Count; i++)
                {
                    if (tempString != "")
                    {
                        tempString += " ";//add spaces
                    }
                    currentwidth = (int)(font.MeasureString(tempString + wordList[i]).X);//take the tempstring add the next word to it and measure the total string

                    if (currentwidth > largestWidth && currentwidth <= maxWidth)
                    {
                        largestWidth = currentwidth;
                    }
                    if (currentwidth <= maxWidth)
                    {
                        tempString += wordList[i];//add word to the string
                    }
                    else
                    {
                        lines.Add(tempString);

                        tempString = wordList[i];
                    }
                }
                if (tempString != "")
                {
                    lines.Add(tempString);
                }
                SetDims(largestWidth);
/*                foreach (string word in wordList)
                {

                }*/
            }
        }


        public virtual void SetDims(int LARGESTWIDTH)
        {
            dims = new Vector2(LARGESTWIDTH, lineHeight * lines.Count);//width is the given width, and height is how many lines fit withing the line height
        }

        public virtual void Draw(Vector2 OFFSET)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Globals.spriteBatch.DrawString(font, lines[i], OFFSET + new Vector2(pos.X, pos.Y + (lineHeight * i)), color);
            }
        }
    }
}
