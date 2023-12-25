using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGGameLibrary
{
    struct TGTile
    {
        /// <summary>
        /// all types of possible tracks
        /// </summary>
        public enum TrackType
        {   
            vertical,
            horizontal,
            turn_1,
            turn_2,
            turn_3,
            turn_4,
            switch_A,
            switch_B,
            switch_C,
            switch_D,
            switch_E,
            switch_F,
            switch_G,
            switch_H,
            cross,
            empty
        }

        /// <summary>
        /// the train/track direction
        /// </summary>
        public enum Rotation
        {
            UP,
            UP_RIGHT,
            RIGHT,
            DOWN_RIGHT,
            DOWN,
            DOWN_LEFT,
            LEFT,
            UP_LEFT
        }

        /// <summary>
        /// states of switches, a = straight forward, b = turn
        /// </summary>
        public enum SwitchState
        {
            a,
            b            
        }
        
        private Texture2D _texture_A;
        private Texture2D _texture_B;        

        private const int _width = 25;
        private const int _height = 25;

        public static readonly Vector2 Size = new Vector2(_width, _height);
                 
        private TrackType _type;
        private SwitchState _switchState;

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public TGTile(Texture2D texture_a, Texture2D texture_b, TrackType type)
        {
            _texture_A = texture_a;
            _texture_B = texture_b;
            _type = type;
            _switchState = SwitchState.a;
        }

        /// <summary>
        /// draw the tile
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch, Vector2 p)
        {
            Vector2 position = p * TGTile.Size;

            // if a switch draw either the a or b tile
            if (_switchState == SwitchState.a)
            {                
                if (_texture_A != null)
                {
                    // Draw it in screen space.                    
                    spriteBatch.Draw(_texture_A, position, Color.White);
                }
            }
            else
            {
                if (_texture_B != null)
                {
                    // Draw it in screen space.                    
                    spriteBatch.Draw(_texture_B, position, Color.White);
                }
            }
        }

        /// <summary>
        /// check if the train need a position change, depended on the track tile
        /// </summary>        
        public Rotation UpdateRotation( Vector2 position, Rotation rot, bool tileChange)
        {
            switch (_type)
            {
                // these tracks don't change the rotation
                case TrackType.cross:
                case TrackType.horizontal:                                    
                case TrackType.vertical:
                    return rot;
                case TrackType.turn_1:
                    return CheckTurn1(position, rot);
                case TrackType.turn_2:
                    return CheckTurn2(position, rot);                                        
                case TrackType.turn_3:
                    return CheckTurn3(position, rot);
                case TrackType.turn_4:
                    return CheckTurn4(position, rot);
                case TrackType.switch_H:                    
                case TrackType.switch_A:
                    // entering a switch from the non-switchable side can change the direction of the switch
                    if (tileChange)
                        ForceDirection2(rot);
                    if (_switchState == SwitchState.b)
                        return CheckTurn2(position, rot);
                    break;
                case TrackType.switch_F:
                case TrackType.switch_B:
                    // entering a switch from the non-switchable side can change the direction of the switch
                    if (tileChange)
                        ForceDirection4(rot);
                    if (_switchState == SwitchState.b)
                        return CheckTurn4(position, rot);
                    break;
                case TrackType.switch_C:
                case TrackType.switch_E:
                    // entering a switch from the non-switchable side can change the direction of the switch
                    if (tileChange)
                        ForceDirection3(rot);
                    if (_switchState == SwitchState.b)
                        return CheckTurn3(position, rot);
                    break;
                case TrackType.switch_D:
                case TrackType.switch_G:
                    // entering a switch from the non-switchable side can change the direction of the switch
                    if (tileChange)
                        ForceDirection1(rot);
                    if (_switchState == SwitchState.b)
                        return CheckTurn1(position, rot);
                    break;                                       
            }
            return rot;
        }

        /// <summary>
        /// switch a switchable track tile
        /// </summary>        
        public bool Switch()
        {
            if (IsSwitchable())
            {
                if (_switchState == SwitchState.a)
                    _switchState = SwitchState.b;
                else
                    _switchState = SwitchState.a;

                return true;
            }
            return false;
        }

        /// <summary>
        /// check if this track tile is switchable
        /// </summary>        
        public bool IsSwitchable()
        {
            return _type == TrackType.switch_A ||
                _type == TrackType.switch_B ||
                _type == TrackType.switch_C ||
                _type == TrackType.switch_D ||
                _type == TrackType.switch_E ||
                _type == TrackType.switch_F ||
                _type == TrackType.switch_G ||
                _type == TrackType.switch_H;            
        }        

        /// <summary>
        /// automatically switch a switch tile if required
        /// </summary>        
        private void ForceDirection2(Rotation rot)
        {
            if (_type == TrackType.switch_A)
            {
                if (rot == Rotation.UP)
                {
                    if (_switchState == SwitchState.a)
                        _switchState = SwitchState.b;
                }
                if (rot == Rotation.LEFT)
                {
                    if (_switchState == SwitchState.b)
                        _switchState = SwitchState.a;
                }

            }
                if (_type == TrackType.switch_H)
                {
                    if (rot == Rotation.DOWN)
                    {
                        if (_switchState == SwitchState.b)
                            _switchState = SwitchState.a;
                    }
                    if (rot == Rotation.RIGHT)
                    {
                        if (_switchState == SwitchState.a)
                            _switchState = SwitchState.b;
                    }
                }            
        }
        private void ForceDirection1(Rotation rot)
        {
            if (_type == TrackType.switch_D)
            {
                if (rot == Rotation.UP)
                {
                    if (_switchState == SwitchState.a)
                        _switchState = SwitchState.b;
                }
                if (rot == Rotation.RIGHT)
                {
                    if (_switchState == SwitchState.b)
                        _switchState = SwitchState.a;
                }
            }

                if (_type == TrackType.switch_G)
                {
                    if (rot == Rotation.DOWN)
                    {
                        if (_switchState == SwitchState.b)
                            _switchState = SwitchState.a;
                    }
                    if (rot == Rotation.LEFT)
                    {
                        if (_switchState == SwitchState.a)
                            _switchState = SwitchState.b;
                    }
                }            
        }
        private void ForceDirection3(Rotation rot)
        {
            if (_type == TrackType.switch_C)
            {
                if (rot == Rotation.DOWN)
                {
                    if (_switchState == SwitchState.a)
                        _switchState = SwitchState.b;
                }
                if (rot == Rotation.RIGHT)
                {
                    if (_switchState == SwitchState.b)
                        _switchState = SwitchState.a;
                }
            }

            if (_type == TrackType.switch_E)
            {
                if (rot == Rotation.UP)
                {
                    if (_switchState == SwitchState.b)
                        _switchState = SwitchState.a;
                }
                if (rot == Rotation.LEFT)
                {
                    if (_switchState == SwitchState.a)
                        _switchState = SwitchState.b;
                }
            }            
        }
        private void ForceDirection4(Rotation rot)
        {
            if (_type == TrackType.switch_B)
            {
                if (rot == Rotation.DOWN)
                {
                    if (_switchState == SwitchState.a)
                        _switchState = SwitchState.b;
                }
                if (rot == Rotation.LEFT)
                {
                    if (_switchState == SwitchState.b)
                        _switchState = SwitchState.a;
                }

            }
                if (_type == TrackType.switch_F)
                {
                    if (rot == Rotation.UP)
                    {
                        if (_switchState == SwitchState.b)
                            _switchState = SwitchState.a;
                    }
                    if (rot == Rotation.RIGHT)
                    {
                        if (_switchState == SwitchState.a)
                            _switchState = SwitchState.b;
                    }
                }            
        }

        /// <summary>
        /// check if the current direction on a tile requires a direction change
        /// </summary>        
        private Rotation CheckTurn1( Vector2 position, Rotation rot)
        {
            if (rot == Rotation.UP)
                if (position.Y < Size.Y * 3/4)
                    return Rotation.UP_RIGHT;
                        
            if (rot == Rotation.UP_RIGHT)
                if (position.Y <= Size.Y / 2 )
                    return Rotation.RIGHT;

            if (rot == Rotation.LEFT)
                if (position.X < Size.X * 3 / 4)
                    return Rotation.DOWN_LEFT;

            if( rot == Rotation.DOWN_LEFT )
                if ( position.X < Size.X/2 )
                    return Rotation.DOWN;
            
            return rot;
        }
        private Rotation CheckTurn2(Vector2 position, Rotation rot)
        {
            if (rot == Rotation.RIGHT)            
                if (position.X > Size.X / 4)
                    return Rotation.DOWN_RIGHT;
            
            if (rot == Rotation.DOWN_RIGHT)
                if (position.X > Size.X / 2)
                    return Rotation.DOWN;

            if (rot == Rotation.UP)
                if (position.Y <= Size.Y * 3 / 4)
                    return Rotation.UP_LEFT;

            if (rot == Rotation.UP_LEFT)
                if (position.Y <= Size.Y / 2)
                    return Rotation.LEFT;

            return rot;
        }
        private Rotation CheckTurn3(Vector2 position, Rotation rot)
        {
            if (rot == Rotation.LEFT)
                if (position.X < Size.X * 3 / 4)
                    return Rotation.UP_LEFT;

            if (rot == Rotation.UP_LEFT)
                if (position.Y < Size.Y / 4)
                    return Rotation.UP;

            if (rot == Rotation.DOWN)
                if (position.Y >= Size.Y / 4)
                    return Rotation.DOWN_RIGHT;

            if (rot == Rotation.DOWN_RIGHT)
                if (position.Y >= Size.Y / 2)
                    return Rotation.RIGHT;

            return rot;
        }
        private Rotation CheckTurn4(Vector2 position, Rotation rot)
        {
            if (rot == Rotation.RIGHT)
                if (position.X >= Size.X / 4)
                    return Rotation.UP_RIGHT;

            if (rot == Rotation.UP_RIGHT)
                if (position.X >= Size.X / 2)
                    return Rotation.UP;

            if (rot == Rotation.DOWN)
                if (position.Y >= Size.Y / 4)
                    return Rotation.DOWN_LEFT;

            if (rot == Rotation.DOWN_LEFT)
                if (position.Y > Size.Y / 2)
                    return Rotation.LEFT;

            return rot; 
        }
    }
}
