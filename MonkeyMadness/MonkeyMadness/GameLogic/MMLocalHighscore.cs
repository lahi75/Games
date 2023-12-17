using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonkeyMadness
{
    public class MMLocalHighscore
    {       
        public List<Score> _highscoreEasy = new List<Score>(); //must be public because of serialization .. hmmmm
        public List<Score> _highscoreMedium = new List<Score>(); //must be public because of serialization .. hmmmm
        public List<Score> _highscoreHard = new List<Score>(); //must be public because of serialization .. hmmmm

        public Score[] GetScores(MMSettings.DifficultyType difficulty)
        {
            List<Score> score = null;

            switch (difficulty)
            {
                case MMSettings.DifficultyType.easy:
                    score = _highscoreEasy;
                    break;

                case MMSettings.DifficultyType.medium:
                    score = _highscoreMedium;
                    break;

                case MMSettings.DifficultyType.hard:
                    score = _highscoreHard;
                    break;
            }

            score.Sort();
            TrimHighscore(score);

            return score.ToArray();
        }


        public void AddScore(MMSettings.DifficultyType difficulty, Score score)
        {
            List<Score> scoreList = null;

            switch (difficulty)
            {
                case MMSettings.DifficultyType.easy:
                    scoreList = _highscoreEasy;
                    break;

                case MMSettings.DifficultyType.medium:
                    scoreList = _highscoreMedium;
                    break;

                case MMSettings.DifficultyType.hard:
                    scoreList = _highscoreHard;
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


        public MMLocalHighscore()
        {
        }

        public MMLocalHighscore(MMLocalHighscore serializedObject)
        {
            _highscoreEasy = serializedObject._highscoreEasy;
            _highscoreMedium = serializedObject._highscoreMedium;
            _highscoreHard = serializedObject._highscoreHard;
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
        public static MMLocalHighscore Highscore = new MMLocalHighscore();

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings()
        {

            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Highscore = new MMLocalHighscore();

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
                    Highscore = new MMLocalHighscore((MMLocalHighscore)serializer.Deserialize(stream));
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Highscore = new MMLocalHighscore();

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
