using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LLGameLibrary
{
    public class LLLocalHighscore
    {       
        public List<Score> _level0 = new List<Score>(); //must be public because of serialization .. hmmmm
        public List<Score> _level1 = new List<Score>(); //must be public because of serialization .. hmmmm
        public List<Score> _level2 = new List<Score>(); //must be public because of serialization .. hmmmm
        public List<Score> _level3 = new List<Score>(); //must be public because of serialization .. hmmmm
        public List<Score> _level4 = new List<Score>(); //must be public because of serialization .. hmmmm

        public Score[] GetScores(Int32 level)
        {
            List<Score> score = null;

            switch (level)
            {
                case 0:
                    score = _level0;
                    break;
                case 1:
                    score = _level1;
                    break;
                case 2:
                    score = _level2;
                    break;
                case 3:
                    score = _level3;
                    break;
                case 4:
                    score = _level4;
                    break;
            }

            score.Sort();
            TrimHighscore(score);

            return score.ToArray();
        }


        public void AddScore(Int32 level, Score score)
        {
            List<Score> scoreList = null;

            switch (level)
            {
                case 0:
                    scoreList = _level0;
                    break;
                case 1:
                    scoreList = _level1;
                    break;
                case 2:
                    scoreList = _level2;
                    break;
                case 3:
                    scoreList = _level3;
                    break;
                case 4:
                    scoreList = _level4;
                    break;
            }

            scoreList.Add(score);
            scoreList.Sort();
            TrimHighscore(scoreList);
        }

        private void TrimHighscore(List<Score> score)
        {
            if (score.Count < 10)
            {
                do
                {
                    score.Add(new Score("", "", "", -1, ""));
                }
                while (score.Count < 10);
            }

            if (score.Count > 10)
            {
                while (score.Count > 10)
                {
                    score.RemoveAt(score.Count - 1);
                }
            }
        }


        public LLLocalHighscore()
        {
        }

        public LLLocalHighscore(LLLocalHighscore serializedObject)
        {
            _level0 = serializedObject._level0;
            _level1 = serializedObject._level1;
            _level2 = serializedObject._level2;
            _level3 = serializedObject._level3;
            _level4 = serializedObject._level4;
        }
    }


    #region settings manager

    /// <summary>
    /// static class to acccess it from everywhere
    /// </summary>
    static class LocalHighscoreManager
    {
        private static string fileName = "highscore.xml";

        /// <summary>
        /// create the one and only instance
        /// </summary>
        public static LLLocalHighscore Highscore = new LLLocalHighscore();

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings()
        {

            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Highscore = new LLLocalHighscore();

            //Obtain a virtual store for application

#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

            // Check if file is there
            if (fileStorage.FileExists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(Highscore.GetType());
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, fileStorage));

                try
                {
                    Highscore = new LLLocalHighscore((LLLocalHighscore)serializer.Deserialize(stream));
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Highscore = new LLLocalHighscore();

                    // Saving is optional - here we assume it works and the error is due to the file not being there.
                    SaveSettings();
                }
            }

            else
            {
                SaveSettings();
            }
        }

        public static void SaveSettings()
        {
            //Obtain a virtual store for application
#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif
            XmlSerializer serializer = new XmlSerializer(Highscore.GetType());

            StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, fileStorage));

            try
            {
                serializer.Serialize(stream, Highscore);
            }
            catch
            {

            }

            stream.Close();
        }
    }

    #endregion
}
