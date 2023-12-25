using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace TGGameLibrary
{

    public class TGFx
    {
        public TGFx()
        {          
        }

        public void Load(ContentManager content)
        {
            _coinNoise = content.Load<SoundEffect>("sound/coins");
            _coinNoiseInstance = _coinNoise.CreateInstance();
            _coinNoiseInstance.IsLooped = false;

            _whistleNoise = content.Load<SoundEffect>("sound/whistle");
            _whistleNoiseInstance = _whistleNoise.CreateInstance();
            _whistleNoiseInstance.IsLooped = false;

            _levelNoise = content.Load<SoundEffect>("sound/level");
            _levelNoiseInstance = _levelNoise.CreateInstance();
            _levelNoiseInstance.IsLooped = false;

            _breakNoise = content.Load<SoundEffect>("sound/break");
            _breakNoiseInstance = _breakNoise.CreateInstance();
            _breakNoiseInstance.IsLooped = false;

            _crashNoise = content.Load<SoundEffect>("sound/crash");
            _crashNoiseInstance = _crashNoise.CreateInstance();
            _crashNoiseInstance.IsLooped = false;

            _stationNoise = content.Load<SoundEffect>("sound/station_full");
            _stationNoiseInstance = _stationNoise.CreateInstance();
            _stationNoiseInstance.IsLooped = false;

            _switchNoise = content.Load<SoundEffect>("sound/switch");
            _switchNoiseInstance = _switchNoise.CreateInstance();
            _switchNoiseInstance.IsLooped = false;
           
        }
        
        public void PlayCoinNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;
            
            _coinNoiseInstance.Play();
        }

        public void PlayWhistleNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _whistleNoiseInstance.Play();
        }

        public void PlayLevelClearedNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _levelNoiseInstance.Play();
        }

        public void PlayBreakNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _breakNoiseInstance.Play();
        }

        public void PlayCrashNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _crashNoiseInstance.Play();
        }

        public void PlayStationNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _stationNoiseInstance.Play();
        }

        public void PlaySwitchNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _switchNoiseInstance.Play();
        }
               
        public void StopAll()
        {
        
        }
        
        SoundEffect _coinNoise;
        SoundEffectInstance _coinNoiseInstance;

        SoundEffect _whistleNoise;
        SoundEffectInstance _whistleNoiseInstance;

        SoundEffect _levelNoise;
        SoundEffectInstance _levelNoiseInstance;

        SoundEffect _breakNoise;
        SoundEffectInstance _breakNoiseInstance;

        SoundEffect _crashNoise;
        SoundEffectInstance _crashNoiseInstance;

        SoundEffect _stationNoise;
        SoundEffectInstance _stationNoiseInstance;

        SoundEffect _switchNoise;
        SoundEffectInstance _switchNoiseInstance;                
    }

    static class TGFxManager
    {
        public static TGFx Fx = new TGFx();
        
        public static void LoadSettings(ContentManager content)
        {            
            Fx = new TGFx();
            Fx.Load(content);
        }     
    }
}
