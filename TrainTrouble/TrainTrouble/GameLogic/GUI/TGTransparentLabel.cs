using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGGameLibrary
{
    public class TGTransparentLabel : TGButton
    {
        public TGTransparentLabel(ContentManager Content, string text)
            : base(Content, text, false, false)
        {
            base.DefaultTexture = Content.Load<Texture2D>("buttons/transparentLabel");
        }


        public new bool Update(Point mousePosition, bool mouseDown)
        {            
            return base.Update(mousePosition, mouseDown);
        }
    }
}
