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
    public class UI
    {
        public SpriteFont font;
        public QuantityDisplayBar healthBar;
        public Basic2d pauseOverlay;
        public Button2d resetBtn, skillMenuBtn;
        public SkillMenu skillMenu;

        public UI(PassObject RESET, Hero HERO)
        {
            pauseOverlay = new Basic2d("2d\\UI\\PauseIcon", new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), new Vector2(175, 175));
            font = Globals.content.Load<SpriteFont>("fonts\\font");
            skillMenu = new SkillMenu(HERO);
            resetBtn = new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(140, 50), new Vector2(1, 1), "fonts\\font", "Reset", RESET, null);
            skillMenuBtn = new Button2d("2d\\UI\\Button2", new Vector2(0, 0), new Vector2(140, 50), new Vector2(1, 1), "fonts\\font", "Skills", ToggleSkillMenu, null);
            healthBar = new QuantityDisplayBar(new Vector2(130, 26), 3, Color.Green); //We an the healthbar to be 100x12 so we know the border is 2 in this case,
                                                                                    //so becase we subtract the border * 2 from the total bar, we can add the border to the total dims of the bar(if this doent make sense just keep in mind we want the healthbar to be 100x12 + the border dims)
        }

        public void Update(World WORLD)//normally we would create a packet to pass in exactly what we need, passing in the whole world is kinf of bad practice
        {
            healthBar.Update(WORLD.user.hero.health, WORLD.user.hero.healthMax);
            if (WORLD.user.hero.dead || WORLD.user.buildings.Count <= 0)
            {

                resetBtn.Update(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2 + 100));
            }
            skillMenuBtn.Update(new Vector2(Globals.screenWidth -100, Globals.screenHeight -100));
            skillMenu.Update();
        }

        public virtual void ToggleSkillMenu(Object INFO)//This style of making functions for passobjects is more versitile (because we can add more behaiviour) than just passing in (skillMenu.ToggleActive();) right away in the button
        {
            skillMenu.ToggleActive();
        }

        public void Draw(World WORLD, Hero HERO, float OFFSET)
        {


            string tempStr3 = OFFSET.ToString();
            string tempStr2 = HERO.pos.ToString();
            string tempStr5 = "z axis (Height)" + HERO.z.ToString();
            string tempStr = "Killed: " + GameGlobals.score.ToString();
            Vector2 strDims = font.MeasureString(tempStr);

            Globals.spriteBatch.DrawString(font, tempStr, new Vector2(40, 80), Color.White);//shader(effect) overrides this color.white Globals.screenWidth / 2 - strDims.X / 2, Globals.screenHeight - 400
            Globals.spriteBatch.DrawString(font, tempStr5, new Vector2(40, 130), Color.White);//shader(effect) overrides this color.white Globals.screenWidth / 2 - strDims.X / 2, Globals.screenHeight - 400
            Globals.spriteBatch.DrawString(font, tempStr2, new Vector2(10, Globals.screenHeight - 100), Color.White);
           // Globals.spriteBatch.DrawString(font, tempStr3, new Vector2(10, Globals.screenHeight - 200), Color.White);
            /*            Globals.spriteBatch.DrawString(font, tempStr4, new Vector2(10, Globals.screenHeight - 300), Color.White);*/

            tempStr = "Money: " + WORLD.user.gold;
            strDims = font.MeasureString(tempStr);
            Globals.spriteBatch.DrawString(font, tempStr, new Vector2(40, 40), Color.White);

            if (WORLD.user.hero.dead || WORLD.user.buildings.Count <= 0)
            {
                tempStr = "Press Enter or click the button to Restart!";
                strDims = font.MeasureString(tempStr);
                Globals.spriteBatch.DrawString(font, tempStr, new Vector2(Globals.screenWidth / 2 - strDims.X / 2, Globals.screenHeight / 2), Color.White);

                resetBtn.Draw(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2 + 100));
            }


            Globals.normalEffect.Parameters["xSize"].SetValue(1.0f);//we dont want it to do anything wonky, so we pass in text because 1/1 = 1. The reason for this is so we dont enter the anti aliasing shader for text.
                                                                    //In the fx file we devide 1/xsize (in this case also 1 because we use setvalue). the outcome of this is that we dont enter the if loop in the fx file,
                                                                    //and so the shader doesnt get applied. Because xSize is not smaller than .6f or .4f
            Globals.normalEffect.Parameters["ySize"].SetValue(1.0f);
            Globals.normalEffect.Parameters["xDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["yDraw"].SetValue(1.0f);
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();
            if (GameGlobals.paused)
            {
                Globals.normalEffect.Parameters["xSize"].SetValue((float)pauseOverlay.myModel.Bounds.Width);//use what you are drawing.mymodel etc to disable the redd effect?
                Globals.normalEffect.Parameters["ySize"].SetValue((float)pauseOverlay.myModel.Bounds.Height);
                Globals.normalEffect.Parameters["xDraw"].SetValue((float)((int)pauseOverlay.dims.X));
                Globals.normalEffect.Parameters["yDraw"].SetValue((float)((int)pauseOverlay.dims.Y));
                Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                Globals.normalEffect.CurrentTechnique.Passes[0].Apply();
                pauseOverlay.Draw(Vector2.Zero);
            }

            healthBar.Draw(new Vector2(10, Globals.screenHeight - 40));
            skillMenuBtn.Draw(new Vector2(Globals.screenWidth - 100, Globals.screenHeight - 100));
            skillMenu.Draw();
            /*Vector2 tempPos = (Vector2)HERO;
            tempPos.X < OFFSET + (Globals.screenWidth * .4f);*/
            //float test = (Globals.screenWidth * .4f);
            /*string tempStr4 = test.ToString();*/




        }
    }
}
