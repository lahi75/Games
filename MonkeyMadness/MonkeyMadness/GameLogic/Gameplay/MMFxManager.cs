using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonkeyMadness
{

    public class MMFx
    {
        public MMFx()
        {          
        }

        public void Load(Game gameMain)
        {
            _levelNoise = gameMain.Content.Load<SoundEffect>("sound/level");
            _levelInstance = _levelNoise.CreateInstance();
            _levelInstance.IsLooped = false;

            _screamNoise = gameMain.Content.Load<SoundEffect>("sound/monkey_scream");
            _screamInstance = _screamNoise.CreateInstance();
            _screamInstance.IsLooped = false;

            _crashScreamNoise = gameMain.Content.Load<SoundEffect>("sound/crash_scream");
            _crashScreamInstance = _crashScreamNoise.CreateInstance();
            _crashScreamInstance.IsLooped = false;

            _bonusNoise = gameMain.Content.Load<SoundEffect>("sound/bonus");
            _bonusInstance = _bonusNoise.CreateInstance();
            _bonusInstance.IsLooped = false;

            _birdNoise = gameMain.Content.Load<SoundEffect>("sound/bird");
            _birdInstance = _birdNoise.CreateInstance();
            _birdInstance.IsLooped = true;

            _pointsNoise = gameMain.Content.Load<SoundEffect>("sound/points");
            _pointsNoiseInstance = _pointsNoise.CreateInstance();
            _pointsNoiseInstance.IsLooped = true;

            _snakeCrash = gameMain.Content.Load<SoundEffect>("sound/snake_crash");
            _snakeCrashInstance = _snakeCrash.CreateInstance();
            _snakeCrashInstance.IsLooped = false;

            _ghostCrash = gameMain.Content.Load<SoundEffect>("sound/ghost_crash");
            _ghostCrashInstance = _ghostCrash.CreateInstance();
            _ghostCrashInstance.IsLooped = false;

            _vultureCrash = gameMain.Content.Load<SoundEffect>("sound/vulture");
            _vultureCrashInstance = _vultureCrash.CreateInstance();
            _vultureCrashInstance.IsLooped = false;

            _gameOver = gameMain.Content.Load<SoundEffect>("sound/fail-trombone");
            _gameOverInstance = _gameOver.CreateInstance();
            _gameOverInstance.IsLooped = false;

            _lostLive = gameMain.Content.Load<SoundEffect>("sound/lost_live");
            _lostLiveInstance = _lostLive.CreateInstance();
            _lostLiveInstance.IsLooped = false;

            _archievement = gameMain.Content.Load<SoundEffect>("sound/award");
            _archievementInstance = _archievement.CreateInstance();
            _archievementInstance.IsLooped = false;

            _teleport = gameMain.Content.Load<SoundEffect>("sound/teleport");
            _teleportInstance = _teleport.CreateInstance();
            _teleportInstance.IsLooped = false;
        }

        public void PlayMonkeyScream()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _screamInstance.Volume = 0.3f;
            _screamInstance.Play();
        }

        public void PlayLevelSuccess()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _levelInstance.Play();
        }

        public void PlayCrashScream()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _crashScreamInstance.Play();
        }

        public void PlayTeleport()
        {
           if (SettingsManager.Settings.Fx == false)
                return;
            
            _teleportInstance.Play();
        }

        public void PlayBonusCollect()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _bonusInstance.Play();
        }

        public void PlaySnakeCrash()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _snakeCrashInstance.Play();
        }

        public void PlayVultureCrash()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _vultureCrashInstance.Play();
        }

        public void PlayGhostCrash()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _ghostCrashInstance.Play();
        }

        public void PlayBirdScream(bool play)
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (play)
            {
                if (_birdInstance.State != SoundState.Playing)
                    _birdInstance.Play();
            }
            else
            {
                if( _birdInstance.State == SoundState.Playing )
                    _birdInstance.Stop();
            }
        }

        public void PlayPointsNoise(bool play)
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            if (play)
            {
                if (_pointsNoiseInstance.State != SoundState.Playing)
                {
                    _pointsNoiseInstance.Volume = 0.4f;
                    _pointsNoiseInstance.Play();
                }
            }
            else
            {
                if (_pointsNoiseInstance.State == SoundState.Playing)
                    _pointsNoiseInstance.Stop();
            }
        }

        public void PlayAwardNoise()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _archievementInstance.Play();            
        }

        public void PlayGameOver()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

            _gameOverInstance.Play();
        }

        public void PlayLostLive()
        {
            if (SettingsManager.Settings.Fx == false)
                return;

           _lostLiveInstance.Play();
        }

        public void StopAll()
        {
            PlayBirdScream(false);
            PlayPointsNoise(false);
        }

        SoundEffect _levelNoise;
        SoundEffectInstance _levelInstance;

        SoundEffect _screamNoise;
        SoundEffectInstance _screamInstance;

        SoundEffect _crashScreamNoise;
        SoundEffectInstance _crashScreamInstance;

        SoundEffect _birdNoise;
        SoundEffectInstance _birdInstance;

        SoundEffect _bonusNoise;
        SoundEffectInstance _bonusInstance;

        SoundEffect _snakeCrash;
        SoundEffectInstance _snakeCrashInstance;

        SoundEffect _vultureCrash;
        SoundEffectInstance _vultureCrashInstance;

        SoundEffect _ghostCrash;
        SoundEffectInstance _ghostCrashInstance;

        SoundEffect _pointsNoise;
        SoundEffectInstance _pointsNoiseInstance;

        SoundEffect _gameOver;
        SoundEffectInstance _gameOverInstance;

        SoundEffect _lostLive;
        SoundEffectInstance _lostLiveInstance;

        SoundEffect _archievement;
        SoundEffectInstance _archievementInstance;

        SoundEffect _teleport;
        SoundEffectInstance _teleportInstance;
    }

    static class MMFxManager
    {
        public static MMFx Fx = new MMFx();

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings(Game gameMain)
        {            
            Fx = new MMFx();
            Fx.Load(gameMain);
        }     
    }
}
