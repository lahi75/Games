using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PoolPanic
{
    public class PPWinner
    {               
        public PPWinner(ContentManager Content)
        {
            _textureDefault = Content.Load<Texture2D>("images/winner");            
            _position = new Vector2(0, 0);
            _font = Content.Load<SpriteFont>("fonts/largeFont");
        }

        public void CenterPosition(Vector2 pos)
        {
            Vector2 p = new Vector2(pos.X - _textureDefault.Width / 2, pos.Y - _textureDefault.Height / 2);
            _position = p;
        }

        public void SetText(string text)
        {
            _text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {                                   
            spriteBatch.Draw(_textureDefault, _position, Color.White);

            Vector2 fontSize = _font.MeasureString(_text);
            Rectangle buttonPos = new Rectangle((int)_position.X, (int)_position.Y, _textureDefault.Width, _textureDefault.Height);
            Vector2 textPos = new Vector2(buttonPos.X + buttonPos.Width / 2 - fontSize.X / 2, buttonPos.Y + buttonPos.Height / 2 - fontSize.Y / 2);
            
            spriteBatch.DrawString(_font, _text, textPos, Color.WhiteSmoke);
        }         

        private Vector2 _position;
        private Texture2D _textureDefault;        
        private string _text = "";
        SpriteFont _font;
    }
}
