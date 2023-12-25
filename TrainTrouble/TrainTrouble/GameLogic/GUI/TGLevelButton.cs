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
    public class TGLevelButton : TGButton
    {
        TGLevelStorage _levelStorage;

        public TGLevelStorage Level 
        {
            get { return _levelStorage; }
        }

        public TGLevelButton(ContentManager content, TGLevelStorage levelStorage)
            : base(content, "", false, false)
        {
            _levelStorage = levelStorage;

            base.DefaultTexture = base.HoverTexture = base.PressedTexture = base.HoverPressedTexture = _levelStorage.Image;                        
        }


        public new bool Update(Point mousePosition, bool mouseDown)
        {
            //Update image texture of button
            base.DefaultTexture = base.HoverTexture = base.PressedTexture = base.HoverPressedTexture = _levelStorage.Image;

            if (_levelStorage.IsUnlocked == false)
                return false;

            return base.Update(mousePosition, mouseDown);
        }
    }
}
