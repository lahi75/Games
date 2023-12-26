using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;


namespace PoolPanic
{

    public class PoolFX
    {
        public PoolFX()
        {
            Enable = true;
        }

        public Boolean Enable
        {
            get;
            set;
        }

        public void Load(Game gameMain)
        {
            _clash = gameMain.Content.Load<SoundEffect>("sounds/clash");
            _tock = gameMain.Content.Load<SoundEffect>("sounds/tock");
            _inHole = gameMain.Content.Load<SoundEffect>("sounds/inhole");
            _button = gameMain.Content.Load<SoundEffect>("sounds/button");
        }

        public void PlayClash(float volume)
        {
            if (Enable == false)
                return;

            if (volume < 0.3)
                volume = 0.3f;
            //_clash.Volume = volume;
            _clash.Play();
        }

        public void PlayTock()
        {
            if (Enable == false)
                return;

            _tock.Play();            
        }

        public void PlayInHole()
        {
            if (Enable == false)
                return;

            _inHole.Play();
        }

        public void PlayButton()
        {
            if (Enable == false)
                return;

            _button.Play();
        }

        private SoundEffect _clash;
        private SoundEffect _tock;
        private SoundEffect _inHole;
        private SoundEffect _button;
    }

    static class FxManager
    {
        public static PoolFX Fx = new PoolFX();
        
        public static void LoadSettings(Game gameMain)
        {
            Fx = new PoolFX();
            Fx.Load(gameMain);

            //Fx.Enable = SettingsManager.Settings.Fx;
        }     
    }
}
