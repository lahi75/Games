using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LLGameLibrary
{
    /// <summary>
    /// storage class for all optical objects
    /// </summary>
    class LLTools
    {
        List<LLTool> _tools = new List<LLTool>();
        ContentManager _content;
        Rectangle _area = new Rectangle();

        /// <summary>
        /// ctor
        /// </summary>        
        public LLTools(ContentManager content, Rectangle area)
        {
            _content = content;
            _area = area; // area in which the tools are allowed
        }

        /// <summary>
        /// clears the tools 
        /// </summary>
        public void Clear()
        {
            _tools.Clear();
        }        

        /// <summary>
        /// creates a new object
        /// </summary>        
        public void AddTool(LLTool obj)
        {            
            _tools.Add(obj);
        }

        /// <summary>
        /// count how many tools are within the given rectangle
        /// </summary>        
        public int Count(Rectangle rect)
        {
            int i = 0;

            foreach (LLTool t in _tools)
            {
                if (rect.Contains(t.Position))
                    i++;
            }
            return i;
        }

        /// <summary>
        /// checks if the beams hits any object
        /// </summary>
        /// <param name="line">beam</param>
        /// <param name="intersecPoint">the intersection point</param>
        /// <param name="angle">the angle between point and object</param>
        /// <returns>the action which applies to the beam</returns>
        public LLTool.Result HitTest(LLLine line, ref Point intersecPoint, ref double angle, ref bool inside)
        {
            // miss by default
            LLTool.Result result = LLMirror.Result.Miss;

            // go through all obejcts
            foreach (LLTool m in _tools)
            {                
                result = m.Hittest(line, ref intersecPoint, ref angle, ref inside);

                // found a hit here
                if (result != LLTool.Result.Miss)
                    return result;
            }
            return result;
        }

        /// <summary>
        /// updates the objects if they are clicked and/or moved
        /// </summary>
        /// <param name="position">position of the click</param>
        /// <param name="clickDown">click or no-click</param>
        public void Update(GameTime gameTime, Point position, Boolean clickDown, LLObjects objects, LLCounters counters)
        {
            // find the one that is currently clicked
            foreach (LLTool t in _tools)
            {
                // update the tools
                t.Update(gameTime);

                if (t.IsClicked)
                {                    
                    if (clickDown)
                    {
                        t.Position = position;      // move around when being clicked                        
                    }
                    else
                    {
                        t.Click(position, false);   // release click detected

                        // now check for valid positions and revert if necessary
                        if (InArea(t) == false)
                            t.RevertPosition();

                        // check if tool inside other tool
                        if (InOtherTool(t))
                            t.RevertPosition();

                        //check if tool inside object
                        if (objects.Contains(t.Position))
                            t.RevertPosition();
                    }

                    if( counters != null )
                        counters.Update(this);
                    return;
                }
            }

            // nothing clicked, grab one if clicked inside an object
            // first try if a direct hit occured
            // then check if after inflation a hit occured
            foreach (LLTool m in _tools)
            {
                if (m.Contains(position, false))
                {
                    // grab an object here
                    m.Click(position, clickDown);
                    return;
                }
            }
            foreach (LLTool m in _tools)
            {
                if (m.Contains(position,true))
                {
                    // grab an object here
                    m.Click(position, clickDown);
                    return;
                }
            }
        }        

        /// <summary>
        /// draw all the objects
        /// </summary>         
        public void Draw(SpriteBatch spriteBatch)
        {
            // reverse draw the tools, to make sure the highlighed is drawn topmost
            for( int i = _tools.Count - 1; i >= 0; i--)
            {
               _tools[i].Draw(spriteBatch);    
            }
        }

        /// <summary>
        /// draw these tools first
        /// </summary>        
        public void DrawFirst(SpriteBatch spriteBatch)
        {
            foreach (LLTool t in _tools)
                t.DrawFirst(spriteBatch);
        }

        /// <summary>
        /// check if the given tool is inside the allowed area
        /// </summary>        
        private Boolean InArea(LLTool tool)
        {
            return _area.Contains(tool.Position);
        }        

        /// <summary>
        /// checks if this tool in inside an other tool
        /// </summary>        
        private Boolean InOtherTool(LLTool tool)
        {
            foreach (LLTool t in _tools)
            {
                // don't test myself
                if( t.Equals(tool) )
                    continue;

                if (t.Contains(tool.Position, false))
                    return true;
            }

            return false;
        }
    }
}
