using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    public class MMAnimatedTexture
    {        
        /// <summary>
        /// constructor
        /// </summary>        
        public MMAnimatedTexture(Vector2 origin, float rotation, float scale, float depth, bool loop)
        {
            this._origin = origin;
            this._rotation = rotation;
            this._scale = scale;
            this._depth = depth;
            _loop = loop;
        }

        /// <summary>
        /// return the size of a single image of the animation
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return new Vector2(_texture.Width / _frameCount, _texture.Height);
            }
        }

        /// <summary>
        /// load the animation from a asset
        /// </summary>        
        public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
        {
            _frameCount = frameCount;
            _texture = content.Load<Texture2D>(asset);
            _timePerFrame = (float)1 / framesPerSec;
            _frame = 0;
            _totalElapsed = 0;
            _paused = false;
        }

        /// <summary>
        /// calculate the state of the animation
        /// </summary>        
        public void UpdateFrame(float elapsed)
        {
            if (_paused)
                return;

            _totalElapsed += elapsed;

            if (_totalElapsed > _timePerFrame)
            {
                _frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                _frame = _frame % _frameCount;
                _totalElapsed -= _timePerFrame;

                if (_frame == 0 && !_loop)
                    Pause();
            }
        }

        /// <summary>
        /// draw the current animation on the sprite batch at the given position
        /// </summary>        
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos,SpriteEffects effect)
        {
            DrawFrame(batch, _frame, screenPos,effect);
        }

        /// <summary>
        /// draw a specific frame of the animation on the sprite batch
        /// </summary>        
        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos, SpriteEffects effect)
        {
            int frameWidth = _texture.Width / _frameCount;
            Rectangle sourcerect = new Rectangle(frameWidth * frame, 0, frameWidth, _texture.Height);
            batch.Draw(_texture, screenPos, sourcerect, Color.White, _rotation, _origin, _scale, effect, _depth);
        }

        /// <summary>
        /// check if the animation is paused
        /// </summary>
        public bool Is_paused
        {
            get { return _paused; }
        }

        /// <summary>
        ///  reset the animation (doesn't stop it)
        /// </summary>
        public void Reset()
        {
            _frame = 0;
            _totalElapsed = 0f;
        }

        /// <summary>
        /// halts and resets the animation
        /// </summary>
        public void Stop()
        {
            Pause();
            Reset();
        }

        /// <summary>
        /// play the animation
        /// </summary>
        public void Play()
        {
            _paused = false;
        }

        /// <summary>
        /// pause the animation
        /// </summary>
        public void Pause()
        {
            _paused = true;
        }

        private int _frameCount;
        private Texture2D _texture;
        private float _timePerFrame;
        private int _frame;
        private float _totalElapsed;
        private bool _paused;
        private float _rotation, _scale, _depth;
        private Vector2 _origin;
        private bool _loop;
    }
}
