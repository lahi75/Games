using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LLGameLibrary
{
    /// <summary>
    /// class holding all beams of the game
    /// </summary>
    class LLBeams
    {
        /// <summary>
        /// list of beams
        /// </summary>
        List<LLBeam> _beams = new List<LLBeam>();
        ContentManager _content;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="content">game content to access resources</param>
        /// <param name="origin">origin of the beam</param>
        /// <param name="rotation">initial rotation of the beam</param>
        /// <param name="color">color of the beam</param>
        public LLBeams(ContentManager content, Vector2 origin, double rotation, LLLaser.ObjectColor color)
        {
            _content = content;            
            _beams.Add( new LLBeam(content, origin, rotation,color,18)); // start with one beam
        }

        /// <summary>
        /// create empty beam object, fill it real beams later with AddBeam
        /// </summary>
        /// <param name="content"></param>
        public LLBeams(ContentManager content)
        {
            _content = content;            
        }

        /// <summary>
        /// reset all the beams, deletes all splits, first beam will be at original position
        /// </summary>
        public void Reset()
        {
            LLBeam newBeam = _beams[0].Reset();
            _beams.Clear();
            _beams.Add(newBeam);
        }

        /// <summary>
        /// check if all beams are dead here
        /// </summary>        
        public Boolean AllDead()
        {
            foreach (LLBeam b in _beams)
            {
                if (b.IsActive)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// adds a beam to the list
        /// </summary>
        /// <param name="beam">new beam</param>
        public void AddBeam(LLBeam beam)
        {
            _beams.Add(beam);
        }

        /// <summary>
        /// update all beams, check interaction with object and create new beams
        /// </summary>
        /// <param name="objects">objects to test with</param>
        public void Update(LLTools tools,LLObjects objects)
        {
            // some tools will create new beams
            LLBeams newBeams = new LLBeams(_content);
            
            // update all the beams and test if they hit tools
            foreach (LLBeam b in _beams)
            {
                // update the beam, check interaction with tools and create new beams
                b.Update(tools,newBeams);

                // check interaction with other objects
                b.Update(objects);
            }

            // add new beams to the list if the tools created them
            foreach (LLBeam b in newBeams._beams)
                AddBeam(b);              
        }
        
        /// <summary>
        /// draw the beams
        /// </summary>
        /// <param name="spriteBatch">spirtebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {            
            foreach (LLBeam b in _beams)
            {         
                b.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// kills all beams
        /// </summary>
        public void ShutDown()
        {
            // kill all beams
            foreach (LLBeam b in _beams)
            {
                b.Kill();
            }
        }
    }
}
