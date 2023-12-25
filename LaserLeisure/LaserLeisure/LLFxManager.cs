using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LLGameLibrary
{

    public class LLFx
    {
        public LLFx()
        {          
        }

        public void Load(ContentManager content)
        {
            _laserNoise = content.Load<SoundEffect>("fx/laser");
            _laserNoiseInstance = _laserNoise.CreateInstance();
            _laserNoiseInstance.IsLooped = true;

            _reflectNoise = content.Load<SoundEffect>("fx/bing01");
            _reflectNoiseInstance = _reflectNoise.CreateInstance();
            _reflectNoiseInstance.IsLooped = false;

            _successBell = content.Load<SoundEffect>("fx/bell01");
            _successBellInstance = _successBell.CreateInstance();
            _successBellInstance.IsLooped = false;

            _failBell = content.Load<SoundEffect>("fx/buzz01");
            _failBellInstance = _failBell.CreateInstance();
            _failBellInstance.IsLooped = false;

            _coinsNoise = content.Load<SoundEffect>("fx/coins");
            _coinsNoiseInstance = _coinsNoise.CreateInstance();
            _coinsNoiseInstance.IsLooped = false;

            _absorbNoise = content.Load<SoundEffect>("fx/blong01");
            _absorbNoiseInstance = _absorbNoise.CreateInstance();
            _absorbNoiseInstance.IsLooped = false;

            _burnNoise = content.Load<SoundEffect>("fx/burn");
            _burnNoiseInstance = _burnNoise.CreateInstance();
            _burnNoiseInstance.IsLooped = true;

            _bombNoise = content.Load<SoundEffect>("fx/bomb");
            _bombNoiseInstance = _bombNoise.CreateInstance();
            _bombNoiseInstance.IsLooped = false;            
        }
        
        public void PlayLaserNoise(bool enable)
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (enable)
            {
                if (_laserNoiseInstance.State != SoundState.Playing)
                    _laserNoiseInstance.Play();
            }
            else
            {
                if (_laserNoiseInstance.State != SoundState.Stopped)
                    _laserNoiseInstance.Stop();
            }
        }

        public void PlayFireNoise(bool enable)
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (enable)
            {
                if (_burnNoiseInstance.State != SoundState.Playing)
                    _burnNoiseInstance.Play();
            }
            else
            {
                if (_burnNoiseInstance.State != SoundState.Stopped)
                    _burnNoiseInstance.Stop();
            }
        }        

        public void PlayCoinsNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (_coinsNoiseInstance.State != SoundState.Playing)
                _coinsNoiseInstance.Play();
        }

        public void PlayBombNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (_bombNoiseInstance.State != SoundState.Playing)
                _bombNoiseInstance.Play();
        }

        public void PlayReflectNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (_reflectNoiseInstance.State != SoundState.Playing)
                _reflectNoiseInstance.Play();
        }

        public void PlayAbsorbNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (_absorbNoiseInstance.State != SoundState.Playing)
                _absorbNoiseInstance.Play();
        }

        public void PlayFailBell()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (_failBellInstance.State != SoundState.Playing)
                _failBellInstance.Play();
        }

        public void PlaySuccessBell()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (_successBellInstance.State != SoundState.Playing)
                _successBellInstance.Play();
        }
               
        public void StopAll()
        {
        
        }
        
        SoundEffect _laserNoise;
        SoundEffectInstance _laserNoiseInstance;

        SoundEffect _reflectNoise;
        SoundEffectInstance _reflectNoiseInstance;

        SoundEffect _successBell;
        SoundEffectInstance _successBellInstance;

        SoundEffect _failBell;
        SoundEffectInstance _failBellInstance;

        SoundEffect _coinsNoise;
        SoundEffectInstance _coinsNoiseInstance;

        SoundEffect _absorbNoise;
        SoundEffectInstance _absorbNoiseInstance;

        SoundEffect _burnNoise;
        SoundEffectInstance _burnNoiseInstance;

        SoundEffect _bombNoise;
        SoundEffectInstance _bombNoiseInstance;
    }

    static class LLFxManager
    {
        public static LLFx Fx = new LLFx();
        
        public static void LoadSettings(ContentManager content)
        {            
            Fx = new LLFx();
            Fx.Load(content);
        }     
    }
}
