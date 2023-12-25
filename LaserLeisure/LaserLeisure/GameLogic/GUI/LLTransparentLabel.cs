using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LLGameLibrary
{
    public class LLTransparentLabel : LLButton
    {
        public LLTransparentLabel(ContentManager Content, string text)
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
