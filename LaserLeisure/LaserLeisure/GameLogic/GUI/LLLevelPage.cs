using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Globalization;
using LaserLeisure.Properties;

namespace LLGameLibrary
{
    class LLLevelPage
    {
        SpriteFont _font18;
        SpriteFont _font21;
        SpriteFont _font25;

        LLDialogReset _resetDlg = null;
        
        LLButton _buttonNovice;
        LLButton _buttonAdvanced;
        LLButton _buttonExpert;
        LLButton _buttonMaster;

        LLButton _buttonReset;

        Rectangle _screenRect;
        Texture2D _background;        
        ContentManager _content;

        Boolean _back = false;

        PageState _state = PageState.standard;

        List<LLLevelButton> _levelButtons = new List<LLLevelButton>();
        
        public enum LevelResult
        {            
            game,
            exit,         
            noresult
        }

        enum PageState
        {
            reset,
            standard
        }

        public void Back()
        {
            _back = true;
        }

        public LLLevelPage(ContentManager content, Rectangle screenRect)
        {
            _content = content;

            _buttonNovice = new LLButton(content, Resources.btnNovice, true, true);
            _buttonNovice.CenterPosition(new Vector2(125, 150));

            _buttonNovice.DefaultTexture = _buttonNovice.HoverTexture =  content.Load<Texture2D>("buttons/button_default_s");
            _buttonNovice.PressedTexture = _buttonNovice.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");
            
            _buttonAdvanced = new LLButton(content, Resources.btnAdvanced, true, true);
            _buttonAdvanced.CenterPosition(new Vector2(325, 150));

            _buttonAdvanced.DefaultTexture = _buttonAdvanced.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonAdvanced.PressedTexture = _buttonAdvanced.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");

            _buttonExpert = new LLButton(content, Resources.btnExpert, true, true);
            _buttonExpert.CenterPosition(new Vector2(525, 150));

            _buttonExpert.DefaultTexture = _buttonExpert.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonExpert.PressedTexture = _buttonExpert.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");

            _buttonMaster = new LLButton(content, Resources.btnMaster, true, true);
            _buttonMaster.CenterPosition(new Vector2(725, 150));

            _buttonMaster.DefaultTexture = _buttonMaster.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonMaster.PressedTexture = _buttonMaster.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");

            _buttonReset = new LLButton(content, Resources.btnReset, false, false);
            _buttonReset.CenterPosition(new Vector2(630, 400));


            
           
            _background = content.Load<Texture2D>("backgrounds/default");
            _screenRect = screenRect;

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");
            _font25 = content.Load<SpriteFont>("fonts/tycho_25");

            // setup buttons
            int noOfButtons = 10;  // create 10 buttons for now !!!!
            int x = 5;
            for (int i = 1; i <= noOfButtons; i++)
            {
                _levelButtons.Add(new LLLevelButton(_content, new Rectangle(x, 240, 70, 70), i));
                x += 80;
            }

            switch (SettingsManager.Settings.LevelStage)
            {
                case LLSettings.Difficulty.novice:
                    _buttonNovice.Press = true;
                    break;
                case LLSettings.Difficulty.advanced:
                    _buttonAdvanced.Press = true;
                    break;
                case LLSettings.Difficulty.expert:
                    _buttonExpert.Press = true;
                    break;
                case LLSettings.Difficulty.master:
                    _buttonMaster.Press = true;
                    break;
            }

            UpdateLevelButtons();
        }

        public int SelectedLevel { get; set; }

        public void UpdateLevelButtons()
        {            
            if (SettingsManager.Settings.GameMode == LLSettings.Mode.ladder)
            {
                List<LLLevelStorage> _levels = null;

                switch (SettingsManager.Settings.LevelStage)
                {
                    case LLSettings.Difficulty.novice:
                        _levels = LevelManager.Level.Novice;
                        break;
                    case LLSettings.Difficulty.advanced:
                        _levels = LevelManager.Level.Advanced;
                        break;
                    case LLSettings.Difficulty.expert:
                        _levels = LevelManager.Level.Expert;
                        break;
                    case LLSettings.Difficulty.master:
                        _levels = LevelManager.Level.Master;
                        break;
                }

                if (_levels == null)
                    return;

                for (int i = 0; i < _levelButtons.Count; i++)
                {
                    _levelButtons[i].Enable = _levels[i].IsUnlocked;
                    _levelButtons[i].ShowPoints = true;
                    _levelButtons[i].SetPoints(_levels[i].Points);
                }
            }
            else
            {
                foreach (LLLevelButton b in _levelButtons)
                {
                    b.Enable = true;
                    b.ShowPoints = false;
                }
            }                    
        }

        public LevelResult Update(GameTime game, Point mousePosition, bool mouseDown)
        {
            if (_back)
            {
                _back = false;
                if (_state == PageState.reset)
                {
                    _resetDlg = null;
                    _state = PageState.standard;
                }
                else
                    return LevelResult.exit;
            }

            switch (_state)
            {
                case PageState.standard:
                    {
                        // select a difficulty and update the setting accordingly
                        if (_buttonNovice.Update(mousePosition, mouseDown))
                        {
                            _buttonExpert.Press = false;
                            _buttonAdvanced.Press = false;
                            _buttonMaster.Press = false;

                            SettingsManager.Settings.LevelStage = LLSettings.Difficulty.novice;
                            SettingsManager.SaveSettings();
                            UpdateLevelButtons();
                        }
                        if (_buttonAdvanced.Update(mousePosition, mouseDown))
                        {
                            _buttonExpert.Press = false;
                            _buttonNovice.Press = false;
                            _buttonMaster.Press = false;

                            SettingsManager.Settings.LevelStage = LLSettings.Difficulty.advanced;
                            SettingsManager.SaveSettings();
                            UpdateLevelButtons();
                        }
                        if (_buttonExpert.Update(mousePosition, mouseDown))
                        {
                            _buttonAdvanced.Press = false;
                            _buttonNovice.Press = false;
                            _buttonMaster.Press = false;

                            SettingsManager.Settings.LevelStage = LLSettings.Difficulty.expert;
                            SettingsManager.SaveSettings();
                            UpdateLevelButtons();
                        }
                        if (_buttonMaster.Update(mousePosition, mouseDown))
                        {
                            _buttonAdvanced.Press = false;
                            _buttonNovice.Press = false;
                            _buttonExpert.Press = false;

                            SettingsManager.Settings.LevelStage = LLSettings.Difficulty.master;
                            SettingsManager.SaveSettings();
                            UpdateLevelButtons();
                        }

                        // check if user clicked reset
                        if (SettingsManager.Settings.GameMode == LLSettings.Mode.ladder)                        
                            if (_buttonReset.Update(mousePosition, mouseDown))                        
                                _state = PageState.reset;
                        
                        // go through all buttons and and check if one was clicked
                        // store the clicked level
                        foreach (LLLevelButton button in _levelButtons)
                        {
                            if (button.Update(mousePosition, mouseDown) == true)
                            {
                                SelectedLevel = button.LevelNumber;
                                return LevelResult.game;
                            }
                        }
                    }
                    break;
                case PageState.reset:
                    {
                        if (_resetDlg == null)
                        {
                            _resetDlg = new LLDialogReset(_content, _screenRect);
                        }

                        switch (_resetDlg.Update(mousePosition, mouseDown))
                        {
                            case LLDialogReset.Result.reset:
                                _resetDlg = null;
                                _state = PageState.standard;
                                LevelManager.Level.Reset(SettingsManager.Settings.LevelStage);
                                LevelManager.SaveSettings();
                                UpdateLevelButtons();
                                break;
                            case LLDialogReset.Result.dont:
                                _resetDlg = null;
                                _state = PageState.standard;
                                break;
                            case LLDialogReset.Result.ongoing:
                                break;
                        }                      
                    }
                    break;
            }

            return LevelResult.noresult;
        }        
        
        public void Draw(SpriteBatch spriteBatch)
        {
            // fill the object with background tiles 
            for (int x = _screenRect.Left; x < _screenRect.Left + _screenRect.Width; x += _background.Width)
            {
                for (int y = (int)(_screenRect.Top); y < _screenRect.Top + _screenRect.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }


            if (_resetDlg != null)
                _resetDlg.Draw(spriteBatch);
            else
            {
                _buttonNovice.Draw(spriteBatch, _font18);
                _buttonAdvanced.Draw(spriteBatch, _font18);
                _buttonExpert.Draw(spriteBatch, _font18);
                _buttonMaster.Draw(spriteBatch, _font18);

                String text;

                if (SettingsManager.Settings.GameMode == LLSettings.Mode.ladder)
                {
                    _buttonReset.Draw(spriteBatch, _font18);

                    text = Resources.dlgPoints;
                    spriteBatch.DrawString(_font18, text, new Vector2(60, 370 - _font18.MeasureString(text).Y/2), Color.White);

                    text = LevelManager.Level.TotalPoints(SettingsManager.Settings.LevelStage).ToString();
                    spriteBatch.DrawString(_font25, text, new Vector2(180, 370 - _font25.MeasureString(text).Y / 2), Color.White);

                    text = Resources.dlgStatus;
                    spriteBatch.DrawString(_font18, text, new Vector2(60, 410 - _font18.MeasureString(text).Y / 2), Color.White);

                    text = Resources.dlgLadder + " " + (LevelManager.Level.IsActive(SettingsManager.Settings.LevelStage) ? Resources.dlgActive : Resources.dlgDone);
                    spriteBatch.DrawString(_font18, text, new Vector2(180, 410 - _font18.MeasureString(text).Y / 2), Color.White);
                }
                else
                {
                    text = Resources.dlgStatus;
                    spriteBatch.DrawString(_font18, text, new Vector2(60, 410 - _font18.MeasureString(text).Y / 2), Color.White);

                    text = Resources.dlgFreestyle;
                    spriteBatch.DrawString(_font18, text, new Vector2(180, 410 - _font18.MeasureString(text).Y / 2), Color.White);

                }

                spriteBatch.DrawString(_font21, Resources.dlgLevelSelection, new Vector2(20, 30), Color.White);
                
                foreach (LLLevelButton b in _levelButtons)
                    b.Draw(spriteBatch);
            }

            
        }
    }
}
