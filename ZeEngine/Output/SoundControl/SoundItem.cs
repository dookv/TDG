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
    public class SoundItem
    {

        public SoundEffect sound;
        public string name;
        public SoundEffectInstance instance;
        public float volume;




        public SoundItem(string NAME, string SOUNDPATH, float VOLUME) 
        {
        
            name = NAME;
            volume = VOLUME;
            sound = Globals.content.Load<SoundEffect>(SOUNDPATH);
            CreateInstance();
        }


        public virtual void CreateInstance()
        {
            instance = sound.CreateInstance();
        }

    }
}
