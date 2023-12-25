using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace LLGameLibrary
{

    public class News
    {
        public string Scope { get; set; }
        public string Text { get; set; }        

        public News()
        {
            Scope = "";
            Text = "";            
        }

        public News(string scope, string text)
        {
            Scope = scope;
            Text = text;            
        }        
    }


    public class NewsTable 
    {
        private News[] _news;

        public News[] AllNews{ get { return _news; } }

        public NewsTable(News[] news)
        {
            _news = news;
        }        
    }


    public class LLNewsReceiver
    {
        private bool _waiting = false;

        private string _responseString = null;

        private NewsTable _news = null;

        public NewsTable GetNews 
        { 
            get 
            {
                if (_news == null && _responseString != null)
                    _news = ParseToNewsTable(_responseString);

                return _news; 
            } 
        }

        public bool IsUpdating
        {
            get
            {
                return _responseString == null && _waiting;
            }
        }

        public void ReceiveNews(string language, string url)
        {
            string postString = "LANGUAGE=" + language;

            _waiting = true;

            _news = null; // reset highscores
            _responseString = null;
            WebPost( url, postString);            
        }        

        private string WebPost(string _URI, string postString)
        {            
#if WINDOWS || WINDOWS_PHONE 
            try
            {
                const string REQUEST_METHOD_POST = "POST";
                const string CONTENT_TYPE = "application/x-www-form-urlencoded";

                // Create a request using a URL that can receive a post.
                WebRequest request = WebRequest.Create(_URI);

                // Set the Method property of the request to POST.
                request.Method = REQUEST_METHOD_POST;

                // Set the ContentType property of the WebRequest.
                request.ContentType = CONTENT_TYPE;

                // Set the ContentLength property of the WebRequest.
                //    request.ContentLength = byteArray.Length;

                // Get the request stream.
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), new Object[] { request, postString });
            }
            catch
            {
            }
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

        private NewsTable ParseToNewsTable(string tableString)
        {
            if (tableString == null)
                return null;

            // remove line feeds and cr from text file input
            tableString = tableString.Replace("\r\n", string.Empty);
            
            tableString = Regex.Replace(tableString, @"[^\u0000-\u007F]", "");
            
            const string SERVER_VALID_DATA_HEADER = "NEWS_";
            if (tableString.Trim().Length < SERVER_VALID_DATA_HEADER.Length ||
            !tableString.Trim().Substring(0, SERVER_VALID_DATA_HEADER.Length).Equals(SERVER_VALID_DATA_HEADER)) return null;
            string toParse = tableString.Trim().Substring(SERVER_VALID_DATA_HEADER.Length);

            string[] rows = Regex.Split(toParse, "_ROW_");

            News[] news = new News[rows.Length];            
            
            for (int i = 0; i < rows.Length; i++)
            {
                string scope = "";
                string text = "";                

                if (rows.Length > i && rows[i].Trim() != "")
                {
                    string[] cols = Regex.Split(rows[i], "_COL_");
                    if (cols.Length == 2)
                    {
                        scope = cols[0].Trim();
                        text = cols[1].Trim();                        
                    }
                }

                news[i] = new News(scope,text);
            }             

            return new NewsTable(news);
        }       
    }    
}
