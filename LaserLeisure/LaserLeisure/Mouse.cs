using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LLGameLibrary
{
    public class LLMouse
    {
        private Vector2 _position;
        private Texture2D _cursorTextue;
        private MouseState _mouseState;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="position">initial cursor position</param>
        /// <param name="cursorTextue">texture of cursor</param>
        public LLMouse(Vector2 position, Texture2D cursorTextue)
        {
            _position = position;           //Inital pos (0,0)
            _cursorTextue = cursorTextue;   //Cursor texture
        }

        /// <summary>
        /// Call OnUpdate
        /// </summary>
        public void Update()
        {
            _mouseState = Mouse.GetState(); //Needed to find the most current mouse states.

            _position.X = _mouseState.X; //Change x pos to mouseX
            _position.Y = _mouseState.Y; //Change y pos to mouseY            
        }

        public void Draw(SpriteBatch batch) //SpriteBatch to use.
        {
            batch.Draw(_cursorTextue, _position, Color.White); //Draw it using the batch.
        }

        /*
        public bool ButtonClick(Button b)
        {
            if (this.pos.X >= b.position.X // To the right of the left side
            && this.pos.X < b.position.X + b.tex.Width //To the left of the right side
            && this.pos.Y > b.position.Y //Below the top side
            && this.pos.Y < b.position.Y + b.tex.Height) //Above the bottom side
                return true; //We are; return true.
            else
                return false; //We're not; return false.
        } */
    }
}
