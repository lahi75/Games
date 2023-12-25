using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using LaserLeisure.Properties;

#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace LLGameLibrary
{
    /// <summary>
    /// class receiving news from the phoebit web server and displaying them in a text box
    /// </summary>
    class LLNewsTicker
    {
        ContentManager _content;
        Point _position;
        Rectangle _rectangle;
        bool _mouseDown = false;

        Texture2D _background;
        Texture2D _frame;
        Texture2D _globe;

        SpriteFont _font;
        float _bounce; // bounces along the y axis
        double _bounceTime = 0;

        double _nextUpdate = 0;
        float _lastPosition = 0;

        float _scrollSpeed = 3.0f;

        string _scope = "LL"; // scope is "train trouble" 

        LLNewsReceiver _newsReceiver = new LLNewsReceiver();

        string[] _headlines;
        int _currentHeadline = -1;
        string _currentText = "";
        float _currentPosition = 0;
        
        /// <summary>
        /// ctor
        /// </summary>        
        public LLNewsTicker(ContentManager content, Point position, string language)
        {
            _content = content;
            _position = position;            

            // always full screen with at 60 height
            _rectangle = new Rectangle(position.X, position.Y, 800, 60);

            try
            {
                _newsReceiver.ReceiveNews(language, Resources.newsURL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            _background = _content.Load<Texture2D>("helper/news_10x10");
            _frame = _content.Load<Texture2D>("helper/white_2x2");
            _globe = _content.Load<Texture2D>("globe");
            _font = content.Load<SpriteFont>("fonts/tycho_21");
        }

        /// <summary>
        /// update the newsticker
        /// </summary>        
        public void Update(GameTime gameTime, Point mousePosition, bool mouseDown)
        {
            // click on the new ticker -> open webpage
            if (mouseDown && !_mouseDown)
            {
                if (_rectangle.Contains(mousePosition))
                {
                    #if WINDOWS_PHONE
                    try
                    {
                        var wbt = new Microsoft.Phone.Tasks.WebBrowserTask();
                        wbt.Uri = new Uri(Resource1.phoebitURL);
                        wbt.Show();                    
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    #endif
                    #if WINDOWS
                        System.Diagnostics.Process.Start(Resources.moreGamesURL);                                
                    #endif
                }
                _mouseDown = mouseDown;
                return;
            }

            _mouseDown = mouseDown;

            try
            {
                // get the text but filter those we don't wanna see            
                if (_newsReceiver.GetNews != null && _headlines == null)
                {
                    News[] news = _newsReceiver.GetNews.AllNews;

                    int count = 0;
                    for (int i = 0; i < news.Length; i++)
                    {
                        if (news[i].Scope != _scope)
                            count++;
                    }

                    if (count > 0)
                    {
                        _headlines = new string[count];

                        int j = 0;
                        for (int i = 0; i < news.Length; i++)
                        {
                            // filter out all from the given scope
                            if (news[i].Scope != _scope)
                                _headlines[j++] = news[i].Text;
                        }

                        // start with the first headline, bounce the globes for 2 secs
                        _currentHeadline = 0;
                        _currentText = "+ + + " + _headlines[_currentHeadline] + " + + +";
                        _currentPosition = _rectangle.Right;
                        _bounceTime = gameTime.TotalGameTime.TotalMilliseconds + 2000;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _headlines = null;
            }

            // scroll the text and flip to a new headline if the current is out of the screen
            if (gameTime.TotalGameTime.TotalMilliseconds > _nextUpdate && _currentHeadline > -1)
            {
                // never update faster than 20ms
                _nextUpdate = gameTime.TotalGameTime.TotalMilliseconds + 20;

                _currentPosition -= _scrollSpeed;               

                float size = _font.MeasureString(_currentText).X;

                if (_currentPosition + (int)_font.MeasureString(_currentText).X < _rectangle.Left)
                {
                    // wrap around
                    _currentHeadline = ++_currentHeadline%(_headlines.Length);

                    _currentText = "+ + + " + _headlines[_currentHeadline] + " + + +";
                    _currentPosition = _rectangle.Right;

                    _bounceTime = gameTime.TotalGameTime.TotalMilliseconds + 1500;
                }
            }

            // Bounce control constants
            const float BounceHeight = 0.10f;
            const float BounceRate = 8.0f;
            const float BounceSync = -0.75f;

            if (gameTime.TotalGameTime.TotalMilliseconds < _bounceTime)
            {
                // Bounce along a sine curve over time.
                // Include the X coordinate so that neighboring items bounce in a nice wave pattern.            
                double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + _position.X * BounceSync;
                _bounce = (float)Math.Sin(t) * BounceHeight * _globe.Height;
            }
            else
                _bounce = 0.0f;

        }

        /// <summary>
        /// draw the newsticker
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {         
            // fill the object with background tiles 
            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _background.Width)
            {
                for (int y = (int)(_rectangle.Top); y < _rectangle.Top + _rectangle.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }

            // draw the news text
            spriteBatch.DrawString(_font, _currentText, new Vector2(_currentPosition, _rectangle.Center.Y - _font.MeasureString(_currentText).Y / 2), Color.White);
            
            // overwrite the text on the outermost positions with background again
            for (int x = _rectangle.Left; x < _rectangle.Left + 30; x += _background.Width)
            {
                for (int y = (int)(_rectangle.Top); y < _rectangle.Top + _rectangle.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }
            for (int x = _rectangle.Right-30; x < _rectangle.Right; x += _background.Width)
            {
                for (int y = (int)(_rectangle.Top); y < _rectangle.Top + _rectangle.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }

            // draw the globes
            Vector2 p = new Vector2(_rectangle.Left + 7, _rectangle.Center.Y - _globe.Height / 2);
            p += new Vector2(0.0f, _bounce);
            spriteBatch.Draw(_globe, p, Color.White);

            p = new Vector2(_rectangle.Right - 7 - _globe.Width, _rectangle.Center.Y - _globe.Height / 2);
            p += new Vector2(0.0f, _bounce);
            spriteBatch.Draw(_globe, p, Color.White);

            // frame the whole stuff
            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _frame.Width)
                spriteBatch.Draw(_frame, new Vector2(x, _rectangle.Top), Color.White);

            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _frame.Width)
                spriteBatch.Draw(_frame, new Vector2(x, _rectangle.Bottom - _frame.Height), Color.White);

            for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _frame.Height)
                spriteBatch.Draw(_frame, new Vector2(_rectangle.Left, y), Color.White);

            for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _frame.Height)
                spriteBatch.Draw(_frame, new Vector2(_rectangle.Right - _frame.Width, y), Color.White);

            _lastPosition = _currentPosition;
        }
    }
}
