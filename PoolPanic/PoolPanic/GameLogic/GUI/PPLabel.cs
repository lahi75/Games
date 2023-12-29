using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace PoolPanic
{
    public class PPLabel
    {
        public Texture2D DefaultTexture
        {
            get { return _textureDefault; }
            set { _textureDefault = value; }
        }
       
        public PPLabel(ContentManager Content)
        {
            _textureDefault = Content.Load<Texture2D>("images/label");            
            _position = new Vector2(0, 0);

            _font = Content.Load<SpriteFont>("fonts/ButtonFont");
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
            spriteBatch.DrawString(_font, _text, new Vector2(_position.X+40, _position.Y+15), Color.WhiteSmoke);
        }         

        private Vector2 _position;
        private Texture2D _textureDefault;        
        private string _text = "";
        SpriteFont _font;
    }
}
