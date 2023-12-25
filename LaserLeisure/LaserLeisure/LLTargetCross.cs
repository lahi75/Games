using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LLGameLibrary
{
    class LLTargetCross
    {
        Texture2D _targetCrossTexture;
        Rectangle _rect;
       
        Vector2 _tcPosition = new Vector2(400,240);

        public LLTargetCross(ContentManager content, Rectangle rect)
        {
            _targetCrossTexture = content.Load<Texture2D>("target_cross");

            _rect = rect;

            Enable = false;
            
        }

        public Boolean Enable { get; set; }

        public void Update(Point position)
        {
            if (Enable == false)
                return;

            if( _rect.Contains(position ) )
            {
                _tcPosition.X = position.X;
                _tcPosition.Y = position.Y;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if( Enable )
                spriteBatch.Draw(_targetCrossTexture, new Vector2( _tcPosition.X - _targetCrossTexture.Width/2, _tcPosition.Y - _targetCrossTexture.Height/2) , Color.White );
        }
    }
}

