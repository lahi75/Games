using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace LLGameLibrary
{
    public class LLLevelIntro
    {                                
        Texture2D _background;        
        Rectangle _screenRect;                        
        ContentManager _content;

        Boolean _fxOn = false;

        LLTools _tools;
        LLObjects _objects;                                        

        IntroState _state = IntroState.fire;
        IntroState _afterWait;        

        public enum IntroState
        {
            wait,
            fire,
            done
        }

        public enum IntroResult
        {            
            exit,
            noresult
        }        

        public LLLevelIntro(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {                               
            _content = new ContentManager(serviceProvider, "Content");                        
            _tools = new LLTools(_content, new Rectangle(0,80,800,400));
            _objects = new LLObjects(_content);

            _background = _content.Load<Texture2D>("backgrounds/metal_texture");
                                  
            _screenRect = screenRect;            
        }

        public void Back()
        {
            SettingsManager.Settings.Fx = _fxOn;
        }

        public void Init(LLSettings.Difficulty stage, int levelNumber)
        {
            _objects.Clear();
            _tools.Clear();

            _fxOn = SettingsManager.Settings.Fx;
            SettingsManager.Settings.Fx = false;

            _state = IntroState.fire;

            // create animation

            // L
            _objects.AddObject(new LLLaser(_content, new Vector2(20, 160), 6 * Math.PI/4, LLLaser.ObjectColor.green));
            _tools.AddTool(new LLMirror(_content, new Point(20, 400), 5 * Math.PI / 4, true));

            // E
            _tools.AddTool(new LLSplitter(_content, new Point(120, 400), 3 * Math.PI / 4, true));
            _tools.AddTool(new LLSplitter(_content, new Point(120, 270), 3 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(120, 160), 7 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(200, 160), 6 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(200, 270), 6 * Math.PI / 4, true));

            // V 
            _tools.AddTool(new LLSplitter(_content, new Point(280, 400), 3 * Math.PI / 4, true));
            _tools.AddTool(new LLPrism(_content, new Point(280, 390), 0 * Math.PI / 4, true));
            _tools.AddTool(new LLPrism(_content, new Point(240, 345), 7 * Math.PI / 4, true));
            _tools.AddTool(new LLPrism(_content, new Point(320, 345), 1 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(230, 345), 2 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(330, 345), 6 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(250, 160), 4 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(320, 160), 4 * Math.PI / 4, true));
   
            // E
            _tools.AddTool(new LLSplitter(_content, new Point(380, 400), 3 * Math.PI / 4, true));
            _tools.AddTool(new LLSplitter(_content, new Point(380, 270), 3 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(380, 160), 7 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(460, 160), 6 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(460, 270), 6 * Math.PI / 4, true));

            // L
            _tools.AddTool(new LLSplitter(_content, new Point(510, 400), 3 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(510, 160), 4 * Math.PI / 4, true));
            _tools.AddTool(new LLMirror(_content, new Point(590, 400), 6 * Math.PI / 4, true));

            if (levelNumber == 1)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(700, 400), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(700, 160), 4 * Math.PI / 4, true));
            }
            else if (levelNumber == 2)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(750, 400), 0 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(670, 400), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 270), 7 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 270), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 2 * Math.PI / 4, true));
            }
            else if (levelNumber == 3)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(670, 400), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(750, 400), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(750, 270), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 270), 2 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 2 * Math.PI / 4, true));
            }
            else if (levelNumber == 4)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(750, 400), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLSplitter(_content, new Point(750, 270), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 4 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 270), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 4 * Math.PI / 4, true));
            }
            else if (levelNumber == 5)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(670, 400), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(750, 400), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 270), 1 * Math.PI / 4, true));                
                _tools.AddTool(new LLMirror(_content, new Point(670, 270), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 7 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 6 * Math.PI / 4, true));
            }
            else if (levelNumber == 6)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(750, 160), 0 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 7 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 400), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 400), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 270), 1 * Math.PI / 4, true));                                
                _tools.AddTool(new LLMirror(_content, new Point(670, 270), 2 * Math.PI / 4, true));
            }
            else if (levelNumber == 7)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(670, 160), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 400), 0 * Math.PI / 4, true));
            }
            else if (levelNumber == 8)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(750, 400), 0 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(670, 400), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(670, 270), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 7 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 270), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 390), 0 * Math.PI / 4, true));                
            }
            else if (levelNumber == 9)
            {
                _objects.AddObject(new LLLaser(_content, new Vector2(670, 400), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(750, 400), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(750, 270), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(750, 160), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 270), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 4 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(670, 160), 2 * Math.PI / 4, true));                
            }
            else if (levelNumber == 10)
            {
                // 0
                _objects.AddObject(new LLLaser(_content, new Vector2(690, 400), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(770, 400), 3 * Math.PI / 4, true));                
                _tools.AddTool(new LLMirror(_content, new Point(770, 160), 1 * Math.PI / 4, true));                
                _tools.AddTool(new LLMirror(_content, new Point(690, 160), 7 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(690, 390), 0 * Math.PI / 4, true));

                // 1
                _objects.AddObject(new LLLaser(_content, new Vector2(640, 400), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                _tools.AddTool(new LLMirror(_content, new Point(640, 160), 4 * Math.PI / 4, true));
            }
        }              

        public IntroResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {        
            switch (_state)
            {
                case IntroState.fire:
                    {
                        _tools.Update(gameTime, position, false, _objects, null);
                        _objects.Update(gameTime, _tools);

                        if (_objects.LaserDead())
                        {
                            _state = IntroState.wait;
                            _afterWait = IntroState.done;
                             
                            SettingsManager.Settings.Fx = _fxOn;
                        }
                    }
                    break;
                case IntroState.wait:

                    _objects.Update(gameTime, _tools);

                    if (clickDown)
                        _state = _afterWait;

                    break;
                case IntroState.done:
                    return IntroResult.exit;
            }

            return IntroResult.noresult;
        }
     
        public void Draw(SpriteBatch spriteBatch)
        {
            // fill the object with tiles
            for (int x = _screenRect.Left; x < _screenRect.Left + _screenRect.Width; x += _background.Width)
            {
                for (int y = (int)(_screenRect.Top); y < _screenRect.Top + _screenRect.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }
            
            // draws fixed objects, including the beams of the laser
            _objects.Draw(spriteBatch);           
        }             
    }
}
