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
    public class SoundControl
    {

        public SoundItem bkgMusic;
        public List<SoundItem> sounds = new List<SoundItem>();   //this shoulkd be private 

        public SoundControl(string MUSICPATH) 
        {

            if (MUSICPATH != "")
            {
                ChangeMusic(MUSICPATH);
            }
        }

        public virtual void AddSound(string NAME, string SOUNDPATH, float VOLUME)//this is a seperate function instead of just the soundiTem calss,
                                                                                 //because if you still want to do something wwith the sound(pitch shift or whatever, ) you can do that here
        {
            sounds.Add(new SoundItem(NAME, SOUNDPATH, VOLUME));
        }


        public virtual void AdjustVolume(float PERCENT)
        {
            if (bkgMusic.instance != null)
            {
                bkgMusic.instance.Volume = PERCENT * bkgMusic.volume;
            }
        }



        public virtual void ChangeMusic(string MUSICPATH)
        {
            bkgMusic = new SoundItem("Bkg Music", MUSICPATH, 0.5f);
            bkgMusic.CreateInstance();

            FormOption musicVolume = Globals.optionsMenu.GetOptionValue("Music Volume");
            float musicVolumePercent = 1.0f;

            if (musicVolume != null)
            {
                musicVolumePercent = (float)Convert.ToDecimal(musicVolume.value, Globals.culture) / 30.0f;
            }
            AdjustVolume(musicVolumePercent);
            bkgMusic.instance.IsLooped = true;
            bkgMusic.instance.Play();

        }


        public virtual void PlaySound(string NAME)
        {
            for (int i = 0; i < sounds.Count; i++)//with bigger games (100s of effects) you ar going to need to sort this in an update or whatever, and then you are going to need to search it
            {
                if (sounds[i].name == NAME)
                {
                    sounds[i].CreateInstance(); //create an instance of a sound everytime it gets fired (projectile for example), leave this out to only fire a new instance of a sound everytime the sound has finished playing 
                    RunSound(sounds[i].sound, sounds[i].instance, sounds[i].volume);
                }
            }
        }

        public virtual void RunSound(SoundEffect SOUND, SoundEffectInstance INSTANCE, float VOLUME)//controll weather the sound offscren is more quiet than onscreen, left adn right speaker, whatever you can ddo here
        {
            FormOption soundVolume = Globals.optionsMenu.GetOptionValue("Sound Volume");
            float soundVolumePercent = 1.0f;

            if (soundVolume != null)
            {
                soundVolumePercent = (float)Convert.ToDecimal(soundVolume.value, Globals.culture) / 30.0f;
            }

            INSTANCE.Volume = soundVolumePercent * VOLUME;
            INSTANCE.Play();
        }
    }
}
