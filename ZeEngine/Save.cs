#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ZeEngine
{
    public class Save
    {
        public int gameId, userId;//steam? webp api
        public string gameName, baseFolder, backupFolder, backupPath;

        public bool loadingID = true;

        public XDocument saveFile;//when save file gets large edit it instead of reamking it



        public Save(int GAMEID, string GAMENAME)
        {
            gameId = GAMEID;
            gameName = GAMENAME;
            //heroLevel = 1;

            //LoadGame();
            backupFolder = "bzaxcyk";//backup folder has to have a weird name?
            backupPath = "bath";

            baseFolder = Globals.appDataFilePath +"\\"+gameName+"";//base folder is the appdata dir + gameName

            CreateBaseFolders();

        }


        public void CreateBaseFolders()
        {
            CreateFolder(Globals.appDataFilePath +"\\"+gameName+"");
            CreateFolder(Globals.appDataFilePath +"\\"+gameName+"\\XML");
            CreateFolder(Globals.appDataFilePath +"\\"+gameName+"\\XML\\SavedGames");
        }

        public void CreateFolder(string s)
        {
            DirectoryInfo CreateSiteDirectory = new DirectoryInfo(s);
            if(!CreateSiteDirectory.Exists)
            {
                CreateSiteDirectory.Create();
            }
        }

        public virtual bool CheckIfFileExists(string PATH)
        {
            bool fileExists;

            fileExists = File.Exists(Globals.appDataFilePath + "\\"+gameName+"\\"+PATH);
            //check if file exists in the game saves folder

            return fileExists;
            //return true;
        }





        public virtual void DeleteFile(string PATH)
        {
            File.Delete(PATH);
        }


        #region Getting XML Files

        public XDocument GetFile(string FILE)//only works with xml files
        {
            if(CheckIfFileExists(FILE))
            {
                return XDocument.Load(Globals.appDataFilePath + "\\"+gameName+"\\"+FILE);
            }

            return null;
        }


        #endregion

        public virtual XDocument LoadFile(string FILEPATH, bool CHECKKEY = true)
        {
            XDocument xml = GetFile(FILEPATH);

            return xml;
        }




        public virtual void HandleSaveFormates(XDocument xml)//file written in bytes
        {

            byte[] compress = Encoding.ASCII.GetBytes(StringToBinary(xml.ToString()));
            File.WriteAllBytes(Globals.appDataFilePath + "\\"+gameName+"\\XML\\SavedGames\\"+Convert.ToString(gameId, Globals.culture), compress);
        }

        public virtual void HandleSaveFormates(XDocument xml, string PATH)//thjis writes xml
        {

            xml.Save(Globals.appDataFilePath + "\\"+gameName+"\\XML\\"+PATH);

        }


        #region Converting a string to Binary and back

        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach(char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for(int i = 0;i < data.Length;i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }

            return Encoding.ASCII.GetString(byteList.ToArray());
        }
        #endregion



    }
}
