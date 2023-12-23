using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MonkeyMadness
{

    public class Score : IComparable
    {
        public string Rank { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public int Points { get; set; }
        public string Country { get; set; }

        public Score()
        {
            Rank = "";
            Name = "";
            Info = "";
            Points = 0;
            Country = "";
        }

        public Score(string rank, string name, string info, int points, string country)
        {
            Rank = rank;
            Name = name;
            Info = info;
            Points = points;
            Country = info;
        }

        public int CompareTo(Object o)
        {
            if (o is Score)
                return ((Score)o).Points - this.Points;
            return 0;
        }
    }



    public class HighscoreTable 
    {
        private Score[] _scores = new Score[10];

        public Score[] Scores { get { return _scores; } }

        public HighscoreTable(Score[] scores)
        {
            _scores = scores;
        }        
    }


    public class MMHighscore
    {
        private bool _waiting = false;

        private string _responseString = null;

        private HighscoreTable _highScore = null;

        public HighscoreTable HighScore 
        { 
            get 
            {
                if (_highScore == null && _responseString != null)                
                    _highScore = ParseToHighscoreTable(_responseString);
                
                return _highScore; 
            } 
        }

        public bool IsUpdating
        {
            get
            {
                return _responseString == null && _waiting;
            }
        }

        public void GetScores(string modeid, string format)
        {
            string postString = "ModeID=" + modeid + "&Format=" + format;

            _waiting = true;
            
            _highScore = null; // reset highscores
            _responseString = null;
            //WebPost(Resource1.strHighScoreRequestURL, postString);            
        }

        public string SendScore(Score score)
        {
            return SendScore(score.Rank, score.Name, score.Info, score.Points, score.Country);
        }

        public string SendScore(string modeid, string name, string info, int score, string country)
        {
            string highscoreString = name + info + score + country + "if(x=10){_last++;}";
            string postString = "ModeID=" + modeid + "&Name=" + name + "&Info=" + info + "&Score=" + score + "&Country=" + country +"&Hash=" + HashString(highscoreString);
            //string response = null;

            //response = WebPost(Resource1.strHighScoreSendURL, postString);

            return "";// response.Trim();
        }

        private string WebPost(string _URI, string postString)
        {            
#if WINDOWS || WINDOWS_PHONE 
            //try
            //{
            //    const string REQUEST_METHOD_POST = "POST";
            //    const string CONTENT_TYPE = "application/x-www-form-urlencoded";

            //    // Create a request using a URL that can receive a post.
            //    WebRequest request = WebRequest.Create(_URI);

            //    // Set the Method property of the request to POST.
            //    request.Method = REQUEST_METHOD_POST;

            //    // Set the ContentType property of the WebRequest.
            //    request.ContentType = CONTENT_TYPE;

            //    // Set the ContentLength property of the WebRequest.
            //    //    request.ContentLength = byteArray.Length;

            //    // Get the request stream.
            //    request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), new Object[] { request, postString });
            //}
            //catch
            //{
            //}
#endif

            return null;           
        }

        private void GetRequestStreamCallback(IAsyncResult asyncResult)
        {

#if WINDOWS || WINDOWS_PHONE 
            try
            {
                //HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState[0];
                HttpWebRequest request = (HttpWebRequest)((Object[])asyncResult.AsyncState)[0];
                string postData = (String)((Object[])asyncResult.AsyncState)[1];

                // End the operation
                Stream postStream = request.EndGetRequestStream(asyncResult);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Write the data to the request stream.
                postStream.Write(byteArray, 0, byteArray.Length);

                // Close the Stream object.
                postStream.Close();

                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
            }
            catch
            {
            }
#endif
        }

        private void GetResponseCallback(IAsyncResult asyncResult)
        {

#if WINDOWS || WINDOWS_PHONE 
            try
            {

                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;

                WebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);

                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                Stream dataStream = null;
                StreamReader reader = null;
                string responseString = null;

                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);

                // Read the content.
                responseString = reader.ReadToEnd();

                // Display the content.
                Console.WriteLine(responseString);

                _responseString = responseString;

                // Clean up the streams.
                if (reader != null)
                    reader.Close();

                if (dataStream != null)
                    dataStream.Close();

                if (response != null)
                    response.Close();
            }
            catch
            {                
            }
            // we received an answer
            _waiting = false;    
#endif
        }

        private HighscoreTable ParseToHighscoreTable(string tableString)
        {
            if (tableString == null)
                return null;

            const string SERVER_VALID_DATA_HEADER = "SERVER_";
            if (tableString.Trim().Length < SERVER_VALID_DATA_HEADER.Length ||
            !tableString.Trim().Substring(0, SERVER_VALID_DATA_HEADER.Length).Equals(SERVER_VALID_DATA_HEADER)) return null;
            string toParse = tableString.Trim().Substring(SERVER_VALID_DATA_HEADER.Length);

            Score[] scores = new Score[10];
            
            string[] rows = Regex.Split(toParse, "_ROW_");
            for (int i = 0; i < 10; i++)
            {
                string rank = "";
                string name = "";
                string info = "";
                string country = "";
                int points = 0;

                if (rows.Length > i && rows[i].Trim() != "")
                {
                    string[] cols = Regex.Split(rows[i], "_COL_");
                    if (cols.Length == 6)
                    {
                        name = cols[0].Trim();
                        info = cols[1].Trim();
                        points = int.Parse(cols[2]);
                        country = cols[3].Trim();
                        rank = cols[4];
                    }
                }

                scores[i] = new Score(rank, name, info, points, country);
            }
            return new HighscoreTable(scores);
        }

        private string HashString(string value)
        {
            string ret = "";

//#if WINDOWS || WINDOWS_PHONE 
//            System.Security.Cryptography.SHA1Managed x = new System.Security.Cryptography.SHA1Managed();
//            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            
//            data = x.ComputeHash(data);
            
      

//            for (int i = 0; i < data.Length; i++) 
//                ret += data[i].ToString("x2").ToLower();
//#endif
            return ret;
        }
    }    
}
