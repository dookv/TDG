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
    public class McTimer
    {
        public bool goodToGo;
        protected int mSec;//milliseconds
        protected TimeSpan timer = new TimeSpan();

        public McTimer(int m)//m is milliseconds
        {
            goodToGo = false;
            mSec = m;
        }



        public McTimer(int m, bool STARTLOADED)//test the timer, provide bool with true to fire timer right away.
                                               //andy for when you want to fire some updating string or thread or whatever right when you start the game.
        {
            goodToGo = STARTLOADED;
            mSec = m;
        }



        public int MSec
        {
            get { return mSec; }//get only returns sthe assigned value, so only using get, gets you a readonly field
            set { mSec = value; }//set sets the variable, so only using set gets you a writeonly field
                                 // Both accessors may also perform some conversion or computation on the data before it's stored or returned.
                                 // Example below
        }



        /*        private double _seconds;

                public double Hours
                {
                    get { return _seconds / 3600; }
                    set
                    {
                        if (value < 0 || value > 24)
                            throw new ArgumentOutOfRangeException(nameof(value),
                                  "The valid range is between 0 and 24.");

                        _seconds = value * 3600;
                    }
                }*/


        public int Timer
        {
            get { return (int)timer.TotalMilliseconds; }
        }



        public void UpdateTimer()//get the time and add the elapsed time to it,
        {                        //+= operator is just (x += y) is equivalent to (x = x + y) so it sets the calculated value in the original value while using the riginal value to calculate
            timer += Globals.gameTime.ElapsedGameTime;
        }
        public void UpdateTimer(float SPEED)//overloaded, allows you to slow down and speed up the time by multiplying the lapsed time with given speed modifier, if you add a 2 its going to go twice as fast
        {
            timer += TimeSpan.FromTicks((long)(Globals.gameTime.ElapsedGameTime.Ticks * SPEED));
        }


        public virtual void AddToTimer(int MSEC)//when you load the game, ex: you have a 30 cooldown, use the cooldown, wait 10 seconds and logout, so you have a 20 sec cooldown left. 
        {                                       //so when you load the game you add those 10 seconds to that timer. Also if you have a ability, you get 3 crits in a row,
                                                //it reduces the cooldown of your other ability by 1 second. yu can just add that second to that other timer
            timer += TimeSpan.FromMilliseconds((long)(MSEC));
        }

        public bool Test()//if your milliseconds is greater that the milliseconds that we want or goodtogo is true, then we run true.
        {
            if (timer.TotalMilliseconds >= mSec || goodToGo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()//if you have a timer that runs for 80 sec, but the event is only 60 sec, we still have 20 seconds left on the timer.
        {
            timer = timer.Subtract(new TimeSpan(0, 0, mSec / 60000, mSec / 1000, mSec % 1000));
            if (timer.TotalMilliseconds < 0)
            {
                timer = TimeSpan.Zero;
            }
            goodToGo = false;
        }
        public void Reset(int NEWTIMER)//reset but with a given value, leftover??
        {

            timer = TimeSpan.Zero;
            MSec = NEWTIMER;
            goodToGo = false;
        }
        public void ResetToZero()//this is what we use 95% of the time
        {
            timer = TimeSpan.Zero;
            goodToGo = false;
        }
        

        public virtual XElement ReturnXml()//returns the xml for it
        {
            XElement xml = new XElement("Timer",
                new XElement("mSec", mSec),
                new XElement("timer", timer));

            return xml;
        }
        public void SetTimer(TimeSpan TIME)//set timer to begin with timespan value
        {
            timer = TIME;
        }
        public virtual void SetTimer(int MSEC)//set timer to begin with int value
        {
            timer = TimeSpan.FromMilliseconds((long)(MSEC));
        }

    }
}
