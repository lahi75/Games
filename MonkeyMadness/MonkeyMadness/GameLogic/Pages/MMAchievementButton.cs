using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    public class MMAchievementButton : MMButton
    {
        MMAchievement _achievement;

        public MMAchievement Achievement 
        {
            get { return _achievement; }
        }
       
        public MMAchievementButton(Game gameMain, MMAchievement achievement)
            : base(gameMain, "", false, false)
        {
            _achievement = achievement;

            base.DefaultTexture = base.HoverTexture = base.PressedTexture = base.HoverPressedTexture = _achievement.Image;                        
        }


        public new bool Update(Point mousePosition, bool mouseDown)
        {
            //Update image texture of button
            base.DefaultTexture = base.HoverTexture = base.PressedTexture = base.HoverPressedTexture = _achievement.Image;

            return base.Update(mousePosition, mouseDown);
        }
    }
}
