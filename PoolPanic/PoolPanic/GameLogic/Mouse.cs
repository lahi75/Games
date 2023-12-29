using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PoolPanic
{
    public class PPMouse
    {
        private Vector2 _position;
        private Texture2D _cursorTextue;
        private MouseState _mouseState;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="position">initial cursor position</param>
        /// <param name="cursorTextue">texture of cursor</param>
        public PPMouse(Vector2 position, Texture2D cursorTextue)
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
    }
}
