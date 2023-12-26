using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAGameLibrary
{
    class AAExit
    {
        AALevel.Destination _destination;
        Texture2D _texture;
        bool _vertical;
        Rectangle _position;

        public AAExit(AALevel.Destination destination, Rectangle position, bool vertical, Texture2D texture)
        {
            _texture = texture;
            _destination = destination;
            _vertical = vertical;
            _position = position;
        }

        public int Width
        {
            get
            {
                return _texture.Width;
            }
        }
        public int Height
        {
            get
            {
                return _texture.Height;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int w = _texture.Width;
            int h = _texture.Height;

            float rot = _vertical ? (float)-Math.PI / 2 : 0;

            spriteBatch.Draw(_texture, _position, null, Color.White,rot, new Vector2(0, 0), SpriteEffects.None, 0.0f);
        }

    }
}
