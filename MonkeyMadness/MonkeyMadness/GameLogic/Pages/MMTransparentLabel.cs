using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    public class MMTransparentLabel : MMButton
    {
        public MMTransparentLabel(Game gameMain, string text)
            : base(gameMain, text, false, false)
        {
            base.DefaultTexture = gameMain.Content.Load<Texture2D>("misc/transparentLabel");
        }

        public new bool Update(Point mousePosition, bool mouseDown)
        {            
            return base.Update(mousePosition, mouseDown);
        }
    }
}
