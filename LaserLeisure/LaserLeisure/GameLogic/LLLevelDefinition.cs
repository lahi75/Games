using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{

    /// <summary>
    /// class defining the levels
    /// </summary>
    class LLLevelDefinition
    {
        ContentManager _content;
        LLObjects _objects;
        LLTools _tools;
        LLCounters _counter;
        Rectangle _area = new Rectangle(50, 80, 750, 400);

        /// <summary>
        /// different skill level
        /// </summary>
        public enum LevelStage
        {
            novice,
            advanced,
            expert,
            master
        }

        /// <summary>
        /// ctor
        /// </summary>        
        public LLLevelDefinition(ContentManager content, LLObjects objects, LLTools tools, LLCounters counter)
        {
            _objects = objects;
            _tools = tools;
            _content = content;
            _counter = counter;
        }

        /// <summary>
        /// return the area in which the tool can be placed
        /// </summary>        
        public Rectangle Area()
        {
            return _area;
        }

        /// <summary>
        /// create the given level
        /// </summary>
        /// <param name="stage">stage to create</param>
        /// <param name="number">levelnumber create</param>
        public Texture2D CreateLevel(LevelStage stage, int number, out int time)
        {
            Texture2D background;
            _objects.Clear();
            _tools.Clear();
            _counter.Clear();            
            switch (stage)
            {
                default:
                case LevelStage.novice:
                    background = CreateNovice(number, out time);
                    break;
                case LevelStage.advanced:
                    background = CreateAdvanced(number, out time);
                    break;
                case LevelStage.expert:
                    background = CreateExpert(number, out time);
                    break;
                case LevelStage.master:
                    background = CreateMaster(number, out time);
                    break;
            }

            CreateCoin(_area);
            return background;
        }

        /// <summary>
        /// create all novice levels
        /// </summary>        
        private Texture2D CreateNovice(int number, out int time)
        {
            // create the 4 outer walls 
            CreateOuterWall();

            Point p;
            #region Level 1
            if (number == 1)
            {           
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(100, 400), Math.PI, LLLaser.ObjectColor.blue));                

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(600, 200), LLLaser.ObjectColor.blue));                

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 2
            else if (number == 2)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(100, 400), 3*Math.PI/4, LLLaser.ObjectColor.blue));              

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(600, 400), LLLaser.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                // create wall
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 5 - 60), new Vector2(10, 120), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion            
            #region Level 3
            else if (number == 3)
            {
                // time to edit the level
                time = 3 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(150, 150), 6 * Math.PI / 4, LLLaser.ObjectColor.green));                

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(700, 150), LLLaser.ObjectColor.green));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                // create wall
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 130), new Vector2(10, 260), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_a2");
            }
            #endregion
            #region Level 4
            else if (number == 4)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(100, 350), 4 * Math.PI / 4, LLLaser.ObjectColor.red));               

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(700, 300), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(500, 150), LLLaser.ObjectColor.red));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));                

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));                

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));                

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 5
            else if (number == 5)
            {
                // time to edit the level
                time = 3 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(700, 150), 0 * Math.PI / 4, LLLaser.ObjectColor.green));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(700, 300), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(150, 440), LLLaser.ObjectColor.green));               

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                // create wall
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 200, 220), new Vector2(400, 10), LLObject.ObjectColor.blue));
                
                return _content.Load<Texture2D>("backgrounds/b_a3");
            }
            #endregion
            #region Level 6
            else if (number == 6)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(140, 130), 5 * Math.PI / 4, LLLaser.ObjectColor.blue));                

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(350, 450), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(620, 130), LLLaser.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 8 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 8 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 80), new Vector2(10, 160), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 110, _area.Center.Y), new Vector2(220, 10), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion
            #region Level 7
            else if (number == 7)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(750, 400), 0 * Math.PI / 4, LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(200, 400), LLLaser.ObjectColor.green));              

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 80, 300), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 80, 300), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 140), new Vector2(10, 280), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 8
            else if (number == 8)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(700, 150), 7 * Math.PI / 4, LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(150, 150), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(100, 400), LLLaser.ObjectColor.green));                

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));


                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 80), new Vector2(10, 160), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 110, 300), new Vector2(220, 10), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion
            #region Level 9
            else if (number == 9)
            {
                // time to edit the level
                time = (int)(3.5 * 60);

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(700, 150), 0 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(150, 350), 4 * Math.PI / 4, LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(150, 150), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(700, 350), LLLaser.ObjectColor.blue));                

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a2");
            }
            #endregion
            #region Level 10
            else
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(650, 150), 6 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(150, 400), 3 * Math.PI / 4, LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(150, 150), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(650, 400), LLLaser.ObjectColor.green));               

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4,false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4,false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4,false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4,false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4,false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4,false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4,false));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(450, _area.Top + 70), new Vector2(10, 140), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(450, _area.Bottom - 70), new Vector2(10, 140), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 130, _area.Center.Y), new Vector2(260, 10), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion                        
        }

        /// <summary>
        /// create all advanced levels
        /// </summary>        
        private Texture2D CreateAdvanced(int number, out int time)
        {
            // create the 4 outer walls 
            CreateOuterWall();

            Point p;
            #region Level 1
            if (number == 1)
            {
                // time to edit the level
                time = 3 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(180, 280),Math.PI, LLLaser.ObjectColor.green));               

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(600, 150), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(600, 410), LLLaser.ObjectColor.green));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));                

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));                

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));                

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a4");
            }
            #endregion
            #region Level 2
            else if (number == 2)
            {
                // time to edit the level
                time = 3 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(400, 440), 2 * Math.PI/4, LLLaser.ObjectColor.red));                

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(600, 440), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(200, 440), LLLaser.ObjectColor.red));

                // create the 4 outer walls 
                CreateOuterWall();

                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(500, _area.Bottom - 40), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(300, _area.Bottom - 40), new Vector2(10, 80), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_a3");
            }
            #endregion
            #region Level 3
            else if (number == 3)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(430, _area.Center.Y), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(370, _area.Center.Y), 0 * Math.PI / 4, LLLaser.ObjectColor.green));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(400, _area.Top + 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(400, _area.Bottom - 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(400, _area.Top + 100 ), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(400, _area.Bottom - 100), LLLaser.ObjectColor.green));                

                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));                

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                
                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a4");
            }
            #endregion
            #region Level 4
            else if (number == 4)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(150, _area.Top + 60), 6 * Math.PI / 4, LLLaser.ObjectColor.blue));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y + 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y - 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y - 120), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y + 120), LLLaser.ObjectColor.blue));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 50, _area.Center.Y), new Vector2(100, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 50, _area.Center.Y + 80), new Vector2(100, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 50, _area.Center.Y - 80), new Vector2(100, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(220, _area.Top + 130), new Vector2(10, 260), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(620, _area.Top + 130), new Vector2(10, 260), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(410, _area.Bottom - 130), new Vector2(10, 260), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                
                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                
                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                
                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b4");
            }
            #endregion
            #region Level 5
            else if (number == 5)
            {
                // time to edit the level
                time = 3 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X, _area.Top + 60), 6 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X, _area.Bottom - 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 200, _area.Bottom - 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 200, _area.Bottom - 40), LLLaser.ObjectColor.red));

                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a2");

            }
            #endregion
            #region Level 6
            else if (number == 6)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(700, _area.Center.Y), 7 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(750, 150), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Bottom - 50), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 180,_area.Bottom - 100), LLLaser.ObjectColor.red));


                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2( _area.Right - 100, 200), new Vector2(200, 10), LLObject.ObjectColor.blue));                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Bottom - 150), new Vector2(10, 300), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 200, _area.Bottom - 140), new Vector2(200, 10), LLObject.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));                

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                // create tools and the counter for the tool
                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));                

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));                

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));                

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));


                return _content.Load<Texture2D>("backgrounds/b_a4");
            }
            #endregion
            #region Level 7
            else if (number == 7)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X, _area.Bottom - 40), Math.PI/2, LLLaser.ObjectColor.red));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 80, _area.Center.Y ), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 80, _area.Center.Y), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Top + 80), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 250, _area.Top + 80), LLLaser.ObjectColor.red));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 110), new Vector2(10, 220), LLObject.ObjectColor.blue));

                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                
                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));


                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));


                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));


                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a3");
            }
            #endregion
            #region Level 8
            else if (number == 8)
            {
                // time to edit the level
                time = 3 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(700, _area.Center.Y), 0 * Math.PI / 4, LLLaser.ObjectColor.green));                

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 80, _area.Center.Y), LLLaser.ObjectColor.green));


                // create fixed tools
                _tools.AddTool(new LLPrism(_content, new Point(300, _area.Center.Y), 6 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(550, _area.Center.Y), 7 * Math.PI / 4, true));

                _tools.AddTool(new LLMirror(_content, new Point( 550, _area.Center.Y + 100), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(700, _area.Center.Y + 100), 3 * Math.PI / 4, true));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, Math.PI, false));
                _tools.AddTool(new LLMirror(_content, p, Math.PI, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLWood(_content, p, Math.PI, false));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 150, _area.Center.Y), new Vector2(10, 60), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_b2");
            }
            #endregion
            #region Level 9
            else if (number == 9)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 80, _area.Top + 80 ), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 80, _area.Bottom - 80), 4 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 80, _area.Center.Y), 0 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 80, _area.Bottom - 80), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 80, _area.Center.Y), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 80, _area.Top + 80), LLLaser.ObjectColor.red));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 110, _area.Center.Y - 50), new Vector2(220, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 110, _area.Center.Y + 50), new Vector2(220, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 110, _area.Center.Y - 50), new Vector2(220, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 110, _area.Center.Y + 50), new Vector2(220, 10), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));


                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));


                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 10
            else
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 100, _area.Top + 80), 6 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 100, _area.Top + 80), 6 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create targets

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 50, _area.Top + 80), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 180, _area.Top + 80), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 50, _area.Top + 80), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 180, _area.Top + 80), LLLaser.ObjectColor.green));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 150, _area.Top + 140), new Vector2(10, 280), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 150, _area.Top + 140), new Vector2(10, 280), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 150 - 80, _area.Top + 280), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 150 + 80, _area.Top + 280), new Vector2(160, 10), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));                

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));


                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));


                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b4");
            }
            #endregion          
        }

        /// <summary>
        /// create all expert levels
        /// </summary>        
        private Texture2D CreateExpert(int number, out int time)
        {
            // create the 4 outer walls 
            CreateOuterWall();
            Point p;

            #region Level 1
            if (number == 1)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 180, _area.Top + 30), 5* Math.PI/4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 180, _area.Top + 30), 7 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X, _area.Bottom - 30), 2 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 160, _area.Bottom - 30), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 160, _area.Bottom - 30), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 60, _area.Top + 140), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 60, _area.Top + 140), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 60, _area.Top + 30), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 60, _area.Top + 30), LLLaser.ObjectColor.green));

                // create bomb
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Bottom - 170), LLObject.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));


                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 120, _area.Top + 30), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 120, _area.Top + 30), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Top + 100), new Vector2(200, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 100, _area.Top + 100), new Vector2(200, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 100, _area.Bottom - 50), new Vector2(10, 100), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 100, _area.Bottom - 50), new Vector2(10, 100), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 100 - 20, _area.Bottom - 100), new Vector2(40, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 100 + 20, _area.Bottom - 100), new Vector2(40, 10), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 2
            else if (number == 2)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 40, _area.Top + 40), 4 * Math.PI / 4, LLLaser.ObjectColor.green));

                // add targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 245, _area.Top + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 40, _area.Bottom - 100), LLLaser.ObjectColor.green));

                // add bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 130, _area.Bottom - 170), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 70, _area.Bottom - 190), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 210, _area.Bottom - 190), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 180, _area.Top + 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 260, _area.Bottom - 170), LLObject.ObjectColor.blue));


                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 220, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 220, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 270, _area.Top + 90), new Vector2(10, 180), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 210, _area.Top + 180), new Vector2(420, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 420, _area.Top + 120), new Vector2(10, 130), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 280 + 70, _area.Top + 60), new Vector2(140, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 220 + 70, _area.Top + 120), new Vector2(140, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 60), new Vector2(620, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 310, _area.Bottom - 60 - 40), new Vector2(10, 80), LLObject.ObjectColor.blue));
                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 150, _area.Bottom - 60 - 40), new Vector2(10, 80), LLObject.ObjectColor.blue));


                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 60 - 40), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 240, _area.Top + 180 + 40), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 95, _area.Top + 180 + 40), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 505, _area.Top + 180), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 220, _area.Top + 180 + 40), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 300, _area.Bottom - 140 ), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60, _area.Center.Y), new Vector2(10, 290), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60 - 50, _area.Top + 180), new Vector2(100, 10), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));


                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));


                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));


                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));


                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b3");
            }
            #endregion
            #region Level 3
            else if (number == 3)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 60, _area.Top + 60), 6 * Math.PI / 4, LLLaser.ObjectColor.blue));                
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 60, _area.Bottom - 60), 1 * Math.PI / 4, LLLaser.ObjectColor.green));

                // add bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 60, _area.Center.Y + 140), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 180, _area.Bottom - 180), LLObject.ObjectColor.blue));

                // add targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Top + 80), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Top + 160), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Top + 240), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 180, _area.Bottom - 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 280, _area.Bottom - 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 380, _area.Bottom - 50), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 600, _area.Bottom - 50), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2((_area.Right - 500), _area.Bottom - 50), new Vector2(10, 100), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                
                _tools.AddTool(new LLSplitter(_content, new Point( _area.Left + 60, _area.Bottom - 180), 5 * Math.PI/4, true));


                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));                

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));                

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                
                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_c1");
            }
            #endregion
            #region Level 4
            else if (number == 4)
            {
                // time to edit the level
                time = 4 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 80, _area.Top + 60), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 80, _area.Bottom - 60), 0 * Math.PI / 4, LLLaser.ObjectColor.red));


                // add bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 120, _area.Top + 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 120, _area.Bottom - 60), LLObject.ObjectColor.blue));

                // fixed tools
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X + 120, _area.Bottom - 60), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X - 120, _area.Top + 60), 5 * Math.PI / 4, true));

                // add targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 80, _area.Center.Y - 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 80, _area.Center.Y + 40), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 80, _area.Center.Y - 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 80, _area.Center.Y + 40), LLLaser.ObjectColor.blue));


                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Center.Y + 90), new Vector2(200, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Center.Y - 90), new Vector2(200, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 100, _area.Center.Y + 90), new Vector2(200, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 100, _area.Center.Y - 90), new Vector2(200, 10), LLObject.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));
                _tools.AddTool(new LLWood(_content, p, 0, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));


                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a4");
            }
            #endregion
            #region Level 5
            else if (number == 5)
            {
                // real level 6 !!!
                // time to edit the level
                time = 6 * 60;
                
                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 50, _area.Top + 50), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 50, _area.Bottom - 50), 4 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create walls                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Center.Y - 30), new Vector2(200, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Center.Y + 30), new Vector2(200, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 300, _area.Center.Y - 30), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 300, _area.Center.Y + 30), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 270, _area.Center.Y - 30), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 270, _area.Center.Y + 30), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 150, _area.Center.Y - 30), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 150, _area.Center.Y + 30), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30, _area.Center.Y - 90), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30, _area.Center.Y + 90), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30 - 60, _area.Center.Y - 140), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30 - 60, _area.Center.Y + 140), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 210, _area.Center.Y - 90), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 210, _area.Center.Y + 90), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 200 + 80, _area.Center.Y - 90), new Vector2(180, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 200 + 80, _area.Center.Y + 90), new Vector2(180, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60, _area.Center.Y - 90 - 25), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60, _area.Center.Y + 90 + 25), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 120, _area.Center.Y), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 180, _area.Center.Y - 65), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 180, _area.Center.Y + 65), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 240, _area.Center.Y - 65), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 240, _area.Center.Y + 65), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 270, _area.Center.Y), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y - 120), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y + 120), new Vector2(10, 60), LLObject.ObjectColor.blue));
                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 310, _area.Center.Y - 120), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 310, _area.Center.Y + 120), new Vector2(10, 60), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 190, _area.Center.Y - 145), new Vector2(10, 110), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 190, _area.Center.Y + 145), new Vector2(10, 110), LLObject.ObjectColor.blue));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 190 + 28, _area.Bottom - 80), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 190 + 88, _area.Bottom - 80), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 190 + 148, _area.Bottom - 80), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 35, _area.Bottom - 80), LLLaser.ObjectColor.blue));                
                _objects.AddObject(new LLTarget(_content, new Vector2( _area.Center.X + 220, _area.Center.Y), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 35, _area.Center.Y), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 35, _area.Top + 80), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 190 + 28, _area.Top + 80), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 190 + 88, _area.Top + 80), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 190 + 148, _area.Top + 80), LLLaser.ObjectColor.red));

                // add bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 35, _area.Center.Y), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 35, _area.Center.Y - 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 35, _area.Center.Y + 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 300, _area.Center.Y), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 210, _area.Center.Y - 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 210, _area.Center.Y + 60), LLObject.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_c1");
            }
            #endregion
            #region Level 6
            else if (number == 6)
            {
                // time to edit the level
                time = 5 * 60;

                // real 7

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 150, _area.Top + 30), 5 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 150, _area.Top + 30), 7 * Math.PI / 4, LLLaser.ObjectColor.red));

                // add bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Bottom - 160), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 100), new Vector2(10, 200), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 120), new Vector2(200, 10), LLObject.ObjectColor.blue));

                // add targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 80, _area.Bottom - 120), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 80, _area.Bottom - 120), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 40, _area.Bottom - 80), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 40, _area.Bottom - 80), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 180, _area.Bottom - 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 180, _area.Bottom - 30), LLLaser.ObjectColor.red));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a3");
            }
            #endregion
            #region Level 7
            else if (number == 7)
            {
                // time to edit the level
                time = 6 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 50, _area.Top + 30), 6 * Math.PI / 4, LLLaser.ObjectColor.green));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 160), new Vector2(10, 320), LLObject.ObjectColor.blue));                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 240, _area.Top + 160), new Vector2(10, 320), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 240, _area.Top + 160), new Vector2(10, 320), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 150, _area.Bottom - 160), new Vector2(10, 320), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 150, _area.Bottom - 160), new Vector2(10, 320), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 30, _area.Center.Y), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 30, _area.Center.Y + 60), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 30, _area.Center.Y - 60), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 30, _area.Center.Y + 120), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 30, _area.Center.Y - 120), new Vector2(60, 10), LLObject.ObjectColor.blue));


                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y + 60), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y - 60), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y + 120), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Center.Y - 120), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 250, _area.Center.Y), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 250, _area.Center.Y + 60), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 250, _area.Center.Y - 60), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 250, _area.Center.Y + 120), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 250, _area.Center.Y - 120), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30, _area.Center.Y), new Vector2(60, 10), LLObject.ObjectColor.blue));                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30, _area.Center.Y + 60), new Vector2(60, 10), LLObject.ObjectColor.blue));                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30, _area.Center.Y + 120), new Vector2(60, 10), LLObject.ObjectColor.blue));

                // add targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 30, _area.Center.Y - 30), LLLaser.ObjectColor.green));                
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 30, _area.Center.Y + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 30, _area.Center.Y - 90), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 30, _area.Center.Y + 90), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Center.Y - 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Center.Y + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Center.Y - 90), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Center.Y + 90), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 250, _area.Center.Y - 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 250, _area.Center.Y + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 250, _area.Center.Y - 90), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 250, _area.Center.Y + 90), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 30, _area.Center.Y + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 30, _area.Center.Y + 90), LLLaser.ObjectColor.green));

                // add bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 30, _area.Top + 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 30, _area.Bottom - 40), LLObject.ObjectColor.blue));                
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 30, _area.Bottom - 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 120, _area.Bottom - 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 120, _area.Bottom - 40), LLObject.ObjectColor.blue));

                // add tools
                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));                
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a2");
            }
            #endregion
            #region Level 8
            else if (number == 8)
            {
                // time to edit the level
                time = 6 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 50, _area.Top + 30), 5 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 50, _area.Top + 30), 7 * Math.PI / 4, LLLaser.ObjectColor.green));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 140), new Vector2(10, 280), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 200, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 200, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 250, _area.Top + 120), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 250, _area.Top + 120), new Vector2(10, 120), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 300, _area.Top + 210), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 300, _area.Top + 210), new Vector2(10, 60), LLObject.ObjectColor.blue));


                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 225, _area.Top + 120), new Vector2(50, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 225, _area.Top + 120), new Vector2(50, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 275, _area.Top + 180), new Vector2(50, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 275, _area.Top + 180), new Vector2(50, 10), LLObject.ObjectColor.blue));


                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60, _area.Center.Y - 20), new Vector2(120, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 60, _area.Center.Y - 20), new Vector2(120, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 150, _area.Center.Y + 30), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 150, _area.Center.Y + 30), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 210, _area.Center.Y + 80), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 210, _area.Center.Y + 80), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 30, _area.Center.Y + 80), new Vector2(60, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 30, _area.Center.Y + 80), new Vector2(60, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 60, _area.Center.Y + 130), new Vector2(120, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60, _area.Center.Y + 130), new Vector2(120, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 60, _area.Center.Y + 5), new Vector2(10, 50), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 60, _area.Center.Y + 5), new Vector2(10, 50), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 120, _area.Center.Y + 25), new Vector2(10, 100), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 120, _area.Center.Y + 25), new Vector2(10, 100), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 180, _area.Center.Y + 75), new Vector2(10, 100), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 180, _area.Center.Y + 75), new Vector2(10, 100), LLObject.ObjectColor.blue));


                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 275, _area.Top + 155), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 275, _area.Top + 155), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 225, _area.Top + 95), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 225, _area.Top + 95), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 35, _area.Center.Y + 5), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 35, _area.Center.Y + 5), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 90, _area.Center.Y + 5), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 90, _area.Center.Y + 5), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 35, _area.Center.Y + 102), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 35, _area.Center.Y + 102), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 205, _area.Center.Y + 102), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 205, _area.Center.Y + 102), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 150, _area.Center.Y + 55), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 150, _area.Center.Y + 55), LLLaser.ObjectColor.red));    
            

                // create bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 30, _area.Top + 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 30, _area.Top + 40), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 30, _area.Bottom - 35), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 30, _area.Bottom - 35), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 225, _area.Top + 155), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 225, _area.Top + 155), LLObject.ObjectColor.blue));

                // create tools                
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));                

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));                

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));                

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));


                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b4");

            }
            #endregion
            #region Level 9
            else if(number == 9)
            {
                // time to edit the level
                time = 4 * 60;

                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 50, _area.Bottom - 50), 1 * Math.PI / 4, LLLaser.ObjectColor.green));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 100, _area.Center.Y - 50), new Vector2(200, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 100, _area.Center.Y + 50), new Vector2(200, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 200, _area.Bottom - 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 200, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));

                // targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 70, _area.Top + 70), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 140, _area.Top + 70), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 90, _area.Bottom - 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 90, _area.Bottom - 110), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 90, _area.Top + 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 90, _area.Top + 110), LLLaser.ObjectColor.green));


                // create tools                
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));                

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a1");

            }
            #endregion
            #region Level 10
            else
            {
                // time to edit the level
                time = 6 * 60;

                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 50, _area.Top + 30), 6 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 175, _area.Top + 30), 6 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 300, _area.Top + 30), 6 * Math.PI / 4, LLLaser.ObjectColor.green));


                // create bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 50, _area.Center.Y), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 50, _area.Center.Y), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Top + 35), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Bottom - 35), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Center.Y), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 100, _area.Center.Y), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 100, _area.Center.Y), new Vector2(160, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Center.Y - 50), new Vector2(10, 60), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Center.Y + 50), new Vector2(10, 60), LLObject.ObjectColor.blue));

                // add targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Bottom - 30), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 150, _area.Bottom - 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 250, _area.Bottom - 30), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Bottom - 30), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 150, _area.Bottom - 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Bottom - 30), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Top + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 150, _area.Top + 30), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 250, _area.Top + 30), LLLaser.ObjectColor.blue));

                // create tools                
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_a3");
            }
            #endregion          
        }

        /// <summary>
        /// create all master levels
        /// </summary>        
        private Texture2D CreateMaster(int number, out int time)
        {
            // create the 4 outer walls 
            CreateOuterWall();
            Point p;

            #region Level 1
            if (number == 1)
            {
                // time to edit the level
                time = 6 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 40, _area.Top + 40), 7 * Math.PI/4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 40, _area.Bottom - 40), 3 * Math.PI / 4, LLLaser.ObjectColor.green));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 160, _area.Center.Y  - 60), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 160, _area.Center.Y), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 160, _area.Center.Y + 60), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 160, _area.Center.Y - 60), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 160, _area.Center.Y), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 160, _area.Center.Y + 60), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 120, _area.Center.Y - 100), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X, _area.Center.Y - 100), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 120, _area.Center.Y - 100), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 120, _area.Center.Y + 100), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X, _area.Center.Y + 100), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 120, _area.Center.Y + 100), LLLaser.ObjectColor.green));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));


                // create bomb
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Center.Y), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 100, _area.Center.Y - 35), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 100, _area.Center.Y + 35), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 100, _area.Center.Y + 35), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 100, _area.Center.Y - 35), LLObject.ObjectColor.blue));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion
            #region Level 2
            else if (number == 2)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 60, _area.Top + 50), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 60, _area.Bottom - 50), 2 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 60, _area.Bottom - 50), 0 * Math.PI / 4, LLLaser.ObjectColor.red));

                // create bomb
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 60, _area.Top + 50), LLObject.ObjectColor.blue));

                // fixed tools
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X, _area.Top + 50), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X, _area.Bottom - 50), 1 * Math.PI / 4, true));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X, _area.Center.Y - 60), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X, _area.Center.Y + 60), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 60, _area.Center.Y), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 60, _area.Center.Y), LLLaser.ObjectColor.blue));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 180, _area.Center.Y - 100), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 180, _area.Center.Y - 100), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 180, _area.Center.Y + 100), new Vector2(160, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 180, _area.Center.Y + 100), new Vector2(160, 10), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 260, _area.Center.Y - 65), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 260, _area.Center.Y + 65), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 260, _area.Center.Y - 65), new Vector2(10, 80), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 260, _area.Center.Y + 65), new Vector2(10, 80), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion
            #region Level 3
            else if (number == 3)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 40, _area.Top + 40), 6 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 40, _area.Bottom - 40), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                

                // create bomb
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Left + 40, _area.Bottom - 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Right - 40, _area.Top + 40), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 30, _area.Center.Y), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 30, _area.Center.Y), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 270, _area.Center.Y + 45), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 270, _area.Center.Y - 45), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 270, _area.Center.Y + 45), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 270, _area.Center.Y - 45), LLObject.ObjectColor.blue));


                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 30, _area.Center.Y - 100), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 30, _area.Center.Y - 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 30, _area.Center.Y + 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 30, _area.Center.Y + 100), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 30, _area.Center.Y - 100), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 30, _area.Center.Y - 50), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 30, _area.Center.Y + 50), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 30, _area.Center.Y + 100), LLLaser.ObjectColor.red));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Center.Y), new Vector2(10, 240), LLObject.ObjectColor.blue));
                
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 270, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 270, _area.Bottom - 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 270, _area.Top + 60), new Vector2(10, 120), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 270, _area.Bottom - 60), new Vector2(10, 120), LLObject.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 7 * Math.PI / 4, false));                

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p,  4 * Math.PI / 4, false));
                

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                

                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 4
            else if (number == 4)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 40, _area.Bottom - 80), 2 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 80, _area.Bottom - 40), 0 * Math.PI / 4, LLLaser.ObjectColor.green));


                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Center.Y), new Vector2(10, 70), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 45, _area.Center.Y - 35), new Vector2(100, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 45, _area.Center.Y + 35), new Vector2(100, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 90, _area.Center.Y - 70), new Vector2(10, 70), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 90, _area.Center.Y + 70), new Vector2(10, 70), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 135, _area.Center.Y - 105), new Vector2(100, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 135, _area.Center.Y + 105), new Vector2(100, 10), LLObject.ObjectColor.blue));

                // create bomb
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 35, _area.Center.Y + 5), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 135, _area.Center.Y - 65), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 45, _area.Center.Y + 75), LLObject.ObjectColor.blue));
                
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 45, _area.Center.Y - 65), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 45, _area.Center.Y  + 5), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 135, _area.Center.Y + 75), LLObject.ObjectColor.blue));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 20, _area.Center.Y + 55), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 115, _area.Center.Y - 15), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 60, _area.Center.Y + 125), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 60, _area.Center.Y - 125), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 30, _area.Center.Y - 50), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 120, _area.Center.Y + 25), LLLaser.ObjectColor.green));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));


                return _content.Load<Texture2D>("backgrounds/b_a1");
            }
            #endregion
            #region Level 5
            else if (number == 5)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X, _area.Bottom - 40), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X - 40, _area.Bottom - 40), 3 * Math.PI / 4, LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X + 40, _area.Bottom - 40), 1 * Math.PI / 4, LLLaser.ObjectColor.green));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 100, _area.Top + 30 ), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 100, _area.Top + 30), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 240, _area.Top + 30), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 240, _area.Top + 30), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y + 80), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y + 80), LLLaser.ObjectColor.blue));

                // fixed tools
                _tools.AddTool(new LLPrism(_content, new Point(_area.Center.X, _area.Center.Y + 60), 0 * Math.PI / 4, true));                
                _tools.AddTool(new LLPrism(_content, new Point(_area.Center.X + 55, _area.Center.Y ), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLPrism(_content, new Point(_area.Center.X - 55, _area.Center.Y), 7 * Math.PI / 4, true));
                _tools.AddTool(new LLPrism(_content, new Point(_area.Center.X + 55, _area.Center.Y - 80), 0 * Math.PI / 4, true));
                _tools.AddTool(new LLPrism(_content, new Point(_area.Center.X - 55, _area.Center.Y - 80), 0 * Math.PI / 4, true));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));                

                return _content.Load<Texture2D>("backgrounds/b_a2");
            }
            #endregion
            #region Level 6
            else if (number == 6)
            {
                // time to edit the level
                time = 6 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 50, _area.Center.Y), 0 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 50, _area.Center.Y), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y - 70), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y - 140), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y + 70), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y + 140), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y - 70), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y - 140), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y + 70), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y + 140), LLLaser.ObjectColor.blue));

                // create bombs
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X + 30, _area.Center.Y), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X - 30, _area.Center.Y), LLObject.ObjectColor.blue));

                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 7 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 2 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b4");
            }
            #endregion
            #region Level 7
            else if(number == 7)
            {
                // time to edit the level
                time = 6 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X + 150, _area.Bottom - 35), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X - 150, _area.Top + 35), 6 * Math.PI / 4, LLLaser.ObjectColor.green));


                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y - 20), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y - 70), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Right - 50, _area.Center.Y + 70), LLLaser.ObjectColor.red));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y + 20), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y + 70), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Left + 50, _area.Center.Y - 70), LLLaser.ObjectColor.green));

                // fixed tools
                _tools.AddTool(new LLMirror(_content, new Point(_area.Center.X + 150, _area.Top + 35), 1 * Math.PI / 4, true));
                _tools.AddTool(new LLMirror(_content, new Point(_area.Center.X - 150, _area.Bottom - 35), 5 * Math.PI / 4, true));

                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X + 150, _area.Center.Y - 20), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X + 150, _area.Center.Y + 20), 5 * Math.PI / 4, true));

                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X - 150, _area.Center.Y + 70), 3 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X - 150, _area.Center.Y - 70), 5 * Math.PI / 4, true));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));


                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));


                return _content.Load<Texture2D>("backgrounds/b_a4");
            }
            #endregion
            #region Level 8
            else if (number == 8)
            {

                // time to edit the level
                time = 6 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X, _area.Center.Y - 40), 2 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X - 50, _area.Center.Y + 50), 0 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Center.X + 50, _area.Center.Y + 50), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));

                // create wall
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 65), new Vector2(10, 130), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 45), new Vector2(10, 90), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Center.Y - 110), new Vector2(500, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Center.Y + 70), new Vector2(250, 10), LLObject.ObjectColor.blue));


                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 60, _area.Bottom - 100), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 60, _area.Bottom - 30), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 60, _area.Bottom - 100), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 60, _area.Bottom - 30), LLLaser.ObjectColor.red));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 150, _area.Bottom - 30), LLLaser.ObjectColor.red));


                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 50, _area.Top + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 200, _area.Top + 30), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 330, _area.Top + 30), LLLaser.ObjectColor.green));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 50, _area.Top + 30), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 200, _area.Top + 30), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 330, _area.Top + 30), LLLaser.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 5 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 0 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 3 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b2");
            }
            #endregion
            #region Level 9
            else if (number == 9)
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 30, _area.Center.Y), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));

                // create walls
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 40, _area.Center.Y), new Vector2(10, 200), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 140, _area.Center.Y), new Vector2(10, 200), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 220, _area.Center.Y), new Vector2(10, 200), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 320, _area.Center.Y), new Vector2(10, 200), LLObject.ObjectColor.blue));

                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X - 130, _area.Center.Y - 30), new Vector2(180, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 50, _area.Center.Y + 30), new Vector2(180, 10), LLObject.ObjectColor.blue));
                _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X + 230, _area.Center.Y + 10), new Vector2(180, 10), LLObject.ObjectColor.blue));

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 190, _area.Center.Y - 60 ), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 70, _area.Center.Y - 60), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 130 , _area.Center.Y), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 190, _area.Center.Y + 60), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 70, _area.Center.Y + 60), LLLaser.ObjectColor.blue));


                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 50 , _area.Center.Y + 60), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 10, _area.Center.Y - 80), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 110, _area.Center.Y - 80), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 10, _area.Center.Y), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 110, _area.Center.Y), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 230, _area.Center.Y- 80 ), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 230, _area.Center.Y - 20), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 230, _area.Center.Y + 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 230, _area.Center.Y + 100), LLLaser.ObjectColor.blue));


                // create tools and the counter for the tool
                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLMirror(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 210);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 1 * Math.PI / 4, false));


                p = new Point(30, 260);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 310);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 2 * Math.PI / 4, false));


                p = new Point(30, 360);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 1 * Math.PI / 4, false));

                p = new Point(30, 410);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 4 * Math.PI / 4, false));

                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));
                _tools.AddTool(new LLPrism(_content, p, 6 * Math.PI / 4, false));

                return _content.Load<Texture2D>("backgrounds/b_b1");
            }
            #endregion
            #region Level 10
            else
            {
                // time to edit the level
                time = 5 * 60;

                // create laser
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 40, _area.Center.Y - 80), 4 * Math.PI / 4, LLLaser.ObjectColor.red));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Right - 40, _area.Center.Y), 0 * Math.PI / 4, LLLaser.ObjectColor.green));
                _objects.AddObject(new LLLaser(_content, new Vector2(_area.Left + 40, _area.Center.Y + 80), 4 * Math.PI / 4, LLLaser.ObjectColor.blue));

                // create bomb
                _objects.AddObject(new LLBomb(_content, new Vector2(_area.Center.X, _area.Center.Y), LLObject.ObjectColor.blue));                

                // create targets
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 80, _area.Bottom - 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 80, _area.Bottom - 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 160, _area.Bottom - 40), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 160, _area.Bottom - 40), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 0, _area.Bottom - 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 240, _area.Bottom - 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 240, _area.Bottom - 40), LLLaser.ObjectColor.blue));

                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 80, _area.Top + 40), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 80, _area.Top + 40), LLLaser.ObjectColor.green));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 160, _area.Top + 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 160, _area.Top + 40), LLLaser.ObjectColor.red));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 0, _area.Top + 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X + 240, _area.Top + 40), LLLaser.ObjectColor.blue));
                _objects.AddObject(new LLTarget(_content, new Vector2(_area.Center.X - 240, _area.Top + 40), LLLaser.ObjectColor.blue));

                // create fixed tools
                _tools.AddTool(new LLSplitter(_content, new Point( _area.Center.X, _area.Center.Y - 80), 5 * Math.PI / 4, true));
                _tools.AddTool(new LLSplitter(_content, new Point(_area.Center.X, _area.Center.Y + 80), 7 * Math.PI / 4, true));


                p = new Point(30, 110);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 3 * Math.PI / 4, false));

                p = new Point(30, 160);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));
                _tools.AddTool(new LLSplitter(_content, p, 5 * Math.PI / 4, false));


                p = new Point(30, 460);
                _counter.Add(new LLCounter(_content, p));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));
                _tools.AddTool(new LLWood(_content, p, 1 * Math.PI / 4, false));


                return _content.Load<Texture2D>("backgrounds/b_a1");
            }            
            #endregion

        }

        /// <summary>
        /// create the wall enclosing the area where tools can be placed
        /// </summary>
        private void CreateOuterWall()
        {
            _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Top + 5), new Vector2(_area.Width, 10), LLObject.ObjectColor.blue));

            _objects.AddObject(new LLWall(_content, new Vector2(_area.Left + 5, _area.Center.Y), new Vector2(10, _area.Height), LLObject.ObjectColor.blue));

            _objects.AddObject(new LLWall(_content, new Vector2(_area.Center.X, _area.Bottom - 5), new Vector2(_area.Width, 10), LLObject.ObjectColor.blue));

            _objects.AddObject(new LLWall(_content, new Vector2(_area.Right - 5, _area.Center.Y), new Vector2(10, _area.Height), LLObject.ObjectColor.blue));
            
        }

        /// <summary>
        /// create the bonus coins for the level
        /// </summary>        
        private void CreateCoin(Rectangle rect)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            Rectangle o;

            int v = r.Next(2);

            if (v < 1)
            {
                int x;
                int y;

                do
                {
                    x = r.Next(rect.Left, rect.Right);
                    y = r.Next(rect.Top, rect.Bottom);
                    o = new Rectangle(x - 25, y - 25, 50, 50);
                }
                while( _objects.Intersects(o));

                _objects.AddObject(new LLCoin(_content, new Vector2(x, y), LLObject.ObjectColor.blue));
            }
        }
    }
}
