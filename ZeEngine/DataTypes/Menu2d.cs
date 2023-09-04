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
    public class Menu2d
    {
        
        
        protected bool active, hasCloseBtn;

        public Vector2 pos, dims, topLeft;
        public Animated2d bkg;
        public Button2d closeBtn;
        public SpriteFont font;
        public PassObject CloseAction;

        public Menu2d(Vector2 POS, Vector2 DIMS, PassObject CLOSEACTION) 
        {
            pos = POS;
            dims = DIMS;
            CloseAction = CLOSEACTION;
            hasCloseBtn = true;
            bkg = new Animated2d("2d\\UI\\ButtonSmall", new Vector2(0, 0), new Vector2(dims.X, dims.Y), new Vector2(1, 1), Color.White);
            closeBtn = new Button2d("2d\\UI\\ButtonSmall", new Vector2(bkg.dims.X /2, -bkg.dims.X / 2 - 80 ), new Vector2(35, 35), new Vector2(1, 1), "", "", Close, null);
            font = Globals.content.Load<SpriteFont>("fonts\\font");
        }


        #region properties

        public virtual bool Active//properties work like a function but are used like a variable
        { 
            get 
            { 
                return active; 
            }
            set 
            {
                active = value;//value is whatever you set it equal to
            } 
        }
        #endregion
        public virtual void Update()
        {
            if (Active)
            {
                topLeft = pos - dims / 2;
                if (hasCloseBtn)
                {
                    closeBtn.Update(pos);
                }

            }
        }



        public virtual void Close(Object INFO)
        {
            Active = false;
        }

        public virtual void Draw()
        {
            if (Active)
            {
                bkg.Draw(pos);
                if (hasCloseBtn)
                {
                    closeBtn.Draw(pos);
                }
                
            }
        }

    }
}
