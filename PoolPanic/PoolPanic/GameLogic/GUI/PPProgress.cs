using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace PoolPanic
{
    public class PPProgress
    {
        public Texture2D DefaultTexture
        {
            get { return _textureDefault; }
            set { _textureDefault = value; }
        }

        public Texture2D ProgressTexture
        {
            get { return _textureProgress; }
            set { _textureProgress = value; }
        }
       
        public PPProgress(ContentManager Content)
        {
            _textureDefault = Content.Load<Texture2D>("images/powerbar");
            _textureProgress = Content.Load<Texture2D>("images/power_fill");
            _textureEmpty = Content.Load<Texture2D>("images/power_empty");
            _position = new Vector2(0, 0);
        }

        public void CenterPosition(Vector2 pos)
        {
            Vector2 p = new Vector2(pos.X - _textureDefault.Width / 2, pos.Y - _textureDefault.Height / 2);
            _position = p;
        }

        public void SetProgress(float progress)
        {
            _progress = progress;
        }

        public void Draw(SpriteBatch spriteBatch)
        {            
            // calculate width of the fill bar
            float width = (float)(_textureEmpty.Width - _textureEmpty.Width * _progress );

            // draw the colored bar
            spriteBatch.Draw(_textureProgress, new Vector2(_position.X + 50, _position.Y + 30), Color.White);

            // cover the progress with the grey bar
            Rectangle r = new Rectangle((int)(_position.X + 50 + (_textureEmpty.Width - width)), (int)_position.Y + 30, (int)width, _textureEmpty.Height);
            spriteBatch.Draw(_textureEmpty, r, Color.White);

            // put the frame around it
            spriteBatch.Draw(_textureDefault, _position, Color.White);
        }         

        private Vector2 _position;
        private Texture2D _textureDefault;
        private Texture2D _textureProgress;
        private Texture2D _textureEmpty;
        private float _progress = 0.0f;
    }
}
