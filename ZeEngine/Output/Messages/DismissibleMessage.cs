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
    public class DismissibleMessage : Message
    {
        public Basic2d bkg;
        public Button2d button;
        public PassObject ConfirmFunc;

        public DismissibleMessage(Vector2 POS, Vector2 DIMS, string MSG, Color COLOR, bool LOCKSCREEN, PassObject CONFIRMFUNC) 
            :base(POS, DIMS, MSG, 1000, COLOR, LOCKSCREEN)
        {


            bkg = new Basic2d("2d\\UI\\ButtonSmall", new Vector2(pos.X, pos.Y), new Vector2(dims.X, dims.Y));//we take the pos and dims from Message class, these get set when we can a new DismissibleMessage and provide all the input parameters,
                                                                                                             //these input parameters will be passed intop the base constructor (Message class) an will get set there so pos = POS and dims = DIMS

            button = new Button2d("2d\\UI\\Button2", new Vector2(pos.X + 20, pos.Y + 20), new Vector2(70, 35), new Vector2(1, 2), "fonts\\font", "Ok", new PassObject(CompleteClick), null);
            ConfirmFunc = CONFIRMFUNC;
        }


        public override void Update()
        {

            button.Update(Vector2.Zero);
            //base.Update();
        }


        public virtual void CompleteClick(Object INFO)
        {
            ConfirmFunc(INFO);
            done = true;
        }


        public override void Draw()
        {
            bkg.Draw(Vector2.Zero);
            textZone.Draw(new Vector2(pos.X - textZone.dims.X / 2, pos.Y - 20));

            if (button != null)
            {
                button.Draw(Vector2.Zero);
            }
            //base.Draw();
        }
    }
}
