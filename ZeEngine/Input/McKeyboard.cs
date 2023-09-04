using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeEngine
{
    public class McKeyboard
    {

        public KeyboardState newKeyboard, oldKeyboard;

        public List<McKey> pressedKeys = new List<McKey>(), previousPressedKeys = new List<McKey>();

        public McKeyboard()
        {

        }

        public virtual void Update()
        {
            newKeyboard = Keyboard.GetState();//get the keyboard state (so everything that is happening with your keyboard at once) and puts it into newKeyBoard 

            GetPressedKeys();

        }

        public void UpdateOld()
        {
            oldKeyboard = newKeyboard;

            previousPressedKeys = new List<McKey>();
            for (int i = 0; i < pressedKeys.Count; i++)
            {
                previousPressedKeys.Add(pressedKeys[i]);//previouspressedkeys gets set to whatever was their last frame
            }
        }


        public bool GetPress(string KEY) //searches trough the pressedkeys list, in which we just added the given key (getpressedkeys() method)
        {

            for (int i = 0; i < pressedKeys.Count; i++)//loop trough all keys that are pressed
            {

                if (pressedKeys[i].key == KEY) //check if a specific key gets pressed
                {
                    return true;
                }

            }


            return false;
        }


        public virtual void GetPressedKeys()
        {
            bool found = false;

            pressedKeys.Clear();//remove all pressedKeys ?
            for (int i = 0; i < newKeyboard.GetPressedKeys().Length; i++) //check for every pressedkey in newkeyboard
            {

                pressedKeys.Add(new McKey(newKeyboard.GetPressedKeys()[i].ToString(), 1));//add the key to the pressedkeys , 1 is passed for the state

            }
        }


        public bool GetSinglePress(string KEY)
        {
            for (int i = 0; i < pressedKeys.Count; i++)//loop trough all pressed keys
            {
                bool isIn = false;

                for (int j = 0; j < previousPressedKeys.Count; j++)//also check if it is in the previous pressed keys, previous pressed keys gets set to whatever was their last frame
                {
                    if (pressedKeys[i].key == previousPressedKeys[j].key)//is the pressedkey is in the previous pressed keys (aka held down)
                    {
                        isIn = true;
                        break;
                    }
                }

                if (!isIn && (pressedKeys[i].key == KEY || pressedKeys[i].print == KEY))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
