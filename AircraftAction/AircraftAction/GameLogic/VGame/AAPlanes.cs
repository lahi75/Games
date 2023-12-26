using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AAGameLibrary
{
    class AAPlanes
    {
        List<AAPlane> _planes = new List<AAPlane>();
        ContentManager _content;
        AALine _line;

        public AAPlanes(IServiceProvider serviceProvider)
        {            
            _content = new ContentManager(serviceProvider, "Content");
            _line = new AALine(_content.Load<Texture2D>("line"));
        }

        public void CreatePlane()
        {
            _planes.Add( new AAPlane( _content, AALevel.Destination.red));
            _planes.Add(new AAPlane(_content, AALevel.Destination.yellow));
            _planes.Add(new AAPlane(_content, AALevel.Destination.orange));
        }

        public void Update(GameTime gameTime, Point position, Boolean clickDown)
        {
            if (_line.IsClicked == false && clickDown)
            {
                foreach (AAPlane p in _planes)
                {
                    if (p.Hittest(position))
                        p.Selected = true;
                }
            }

            AAPlane selectedPlane = GetSelected();

            if( selectedPlane != null)
            {

                // releasing touch event
                if (_line.IsClicked && clickDown == false)
                {
                    selectedPlane.Selected = false;
                    // update plane rotation only when vector is long enough
                    if (_line.GetLength() > 20)
                        selectedPlane.SetRotation(_line.GetRotation());
                }

                _line.Update(position, clickDown);
                _line.SetOrigin(selectedPlane.Position());
            }                                

            foreach (AAPlane p in _planes)
                p.Update();
        }

        AAPlane GetSelected()
        {
            foreach (AAPlane p in _planes)
                if (p.Selected)
                    return p;

            return null;
        }

        /// <summary>
        /// draw the planes
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AAPlane p in _planes)
            {
                p.Draw(spriteBatch);
            }

            _line.Draw(spriteBatch);            
        }
    }
}
