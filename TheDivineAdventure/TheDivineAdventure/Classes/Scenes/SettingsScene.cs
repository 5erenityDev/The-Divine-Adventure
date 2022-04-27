﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheDivineAdventure
{
    public class SettingsScene : Scene
    {
        private TextBox resWidth, resHeight;
        private SliderSelector masterVol,musicVol,sfxVol;
        private Texture2D settingsWindow, settingsButton1, settingsButton2;
        private Button settingsWindowed, settingsBorderless, settingsFullscreen, settingsNoAA, settingsAA2,
            settingsAA4, settingsAA8, settingsCancel, settingsApply;

        public SettingsScene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 parent, ContentManager cont) : base(sb, graph, parent, cont)
        {

        }

        //initialize settings Menu
        public override void Initialize()
        {
            base.Initialize();
            LoadContent();
            parent.showCursor = true;
            //create resolution buttons
            resWidth = new TextBox(_graphics.PreferredBackBufferWidth.ToString(), 4,
                parent.smallFont, new Vector2(492, 325), 30, new Color(Color.Black, 60), parent);
            resHeight = new TextBox(_graphics.PreferredBackBufferHeight.ToString(), 4,
                parent.smallFont, new Vector2(682, 325), 30, new Color(Color.Black, 60), parent);

            //creat volume sliders
            masterVol = new SliderSelector(new Vector2(1126, 334), new Vector2(300, 9), parent, Content);
            masterVol.Value = 1f;
            musicVol = new SliderSelector(new Vector2(1126, 415), new Vector2(300, 9), parent, Content);
            musicVol.Value = 1f;
            sfxVol = new SliderSelector(new Vector2(1126, 496), new Vector2(300, 9), parent, Content);
            sfxVol.Value = 1f;

            //create window buttons
            settingsWindowed = new Button(settingsButton1, settingsButton1, "Windowed", parent.smallFont,
                new Vector2(392, 404), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.Window.IsBorderless == false && _graphics.IsFullScreen == false)
                settingsWindowed.IsActive = true;
            settingsBorderless = new Button(settingsButton1, settingsButton1, "Borderless",
                parent.smallFont, new Vector2(392, 455), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.Window.IsBorderless == true && _graphics.IsFullScreen == false)
                settingsBorderless.IsActive = true;
            settingsFullscreen = new Button(settingsButton1, settingsButton1, "Fullscreen",
                parent.smallFont, new Vector2(392, 505), new Vector2(180, 29), parent.currentScreenScale);
            if (_graphics.IsFullScreen == true)
                settingsFullscreen.IsActive = true;


            //create antialiasing buttons
            settingsNoAA = new Button(settingsButton1, settingsButton1, "None",
                parent.smallFont, new Vector2(392, 585), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 0)
                settingsNoAA.IsActive = true;
            settingsAA2 = new Button(settingsButton1, settingsButton1, "2x",
                parent.smallFont, new Vector2(392, 635), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 2)
                settingsAA2.IsActive = true;
            settingsAA4 = new Button(settingsButton1, settingsButton1, "4x", 
                parent.smallFont, new Vector2(392, 685), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 4)
                settingsAA4.IsActive = true;
            settingsAA8 = new Button(settingsButton1, settingsButton1, "8x", 
                parent.smallFont, new Vector2(392, 735), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 8)
                settingsAA8.IsActive = true;

            //create back and apply buttons
            settingsCancel = new Button(settingsButton2, settingsButton2, "Cancel",
                parent.smallFont, new Vector2(70, 972), new Vector2(210, 76), parent.currentScreenScale);
            settingsApply = new Button(settingsButton2, settingsButton2, "Apply",
                parent.smallFont, new Vector2(375, 972), new Vector2(210, 76), parent.currentScreenScale);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            settingsWindow = Content.Load<Texture2D>("TEX_Settings_Window");
            settingsButton1 = Content.Load<Texture2D>("TEX_Settings_Button1_Passive");
            settingsButton2 = Content.Load<Texture2D>("TEX_Settings_Button2");
        }
        //update settings
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //set volume slider click states
            if (!musicVol.IsActive && !sfxVol.IsActive)
                masterVol.IsPressed();
            if (!sfxVol.IsActive && !masterVol.IsActive)
                musicVol.IsPressed();
            if (!musicVol.IsActive && !masterVol.IsActive)
                sfxVol.IsPressed();


            //check what mouse is clicking
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //apply settings or cancel
                if (settingsCancel.IsPressed())
                {
                    settingsCancel.IsActive = true;
                    parent.currentScene = parent.lastScene;
                    return;
                }
                if (settingsApply.IsPressed())
                {
                    _graphics.PreferredBackBufferWidth = Int32.Parse(resWidth.Text);
                    _graphics.PreferredBackBufferHeight = Int32.Parse(resHeight.Text);
                    //switch to windowed
                    if (settingsWindowed.IsActive == true  && parent.Window.IsBorderless)
                    {
                        if (_graphics.IsFullScreen)
                        {
                            _graphics.ToggleFullScreen();
                            _graphics.ApplyChanges();
                        }

                        if (parent.Window.IsBorderless)
                        {
                            parent.Window.IsBorderless = false;
                            parent.Window.Position = new Point(parent.Window.Position.X+10, parent.Window.Position.Y+10);
                        }
                    }//switch to borderless window
                    else if (settingsBorderless.IsActive == true)
                    {
                        _graphics.PreferredBackBufferWidth = Int32.Parse(resWidth.Text);
                        _graphics.PreferredBackBufferHeight = Int32.Parse(resHeight.Text);
                        if (_graphics.IsFullScreen)
                        {
                            _graphics.ToggleFullScreen();
                            _graphics.ApplyChanges();
                        }
                        if (!parent.Window.IsBorderless)
                        {
                            parent.Window.IsBorderless = true;
                            parent.Window.Position = new Point(0, 0);
                        }
                    }//switch to fullscreen
                    else if (settingsFullscreen.IsActive == true && !_graphics.IsFullScreen) {
                        parent.Window.IsBorderless = true;
                        _graphics.ToggleFullScreen();
                        _graphics.ApplyChanges();
                    }

                    if (settingsNoAA.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = false;
                        parent.GraphicsDevice.PresentationParameters.MultiSampleCount = 0;
                    }
                    else if (settingsAA2.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = true;
                        parent.GraphicsDevice.PresentationParameters.MultiSampleCount = 2;
                    }
                    else if (settingsAA4.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = true;
                        parent.GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
                    }
                    else if (settingsAA8.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = true;
                        parent.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
                    }
                    parent.currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
                    if (parent.lastScene == 0)
                        parent.titleScene.Initialize();
                    if (parent.lastScene == 6)
                        parent.pauseScene.Initialize();
                    this.Initialize();

                    settingsApply.IsActive = true;
                    parent.currentScene = parent.lastScene;
                    return;
                }
                //update volumebars
                masterVol.Update();
                musicVol.Update();
                sfxVol.Update();

                //update texboxes;
                resWidth.IsPressed();
                resHeight.IsPressed();

                //update window buttons
                if (settingsWindowed.IsPressed())
                {
                    settingsWindowed.IsActive = true;
                    settingsFullscreen.IsActive = false;
                    settingsBorderless.IsActive = false;
                    return;
                }
                if (settingsBorderless.IsPressed())
                {
                    settingsBorderless.IsActive = true;
                    settingsFullscreen.IsActive = false;
                    settingsWindowed.IsActive = false;
                    return;
                }
                if (settingsFullscreen.IsPressed())
                {
                    settingsFullscreen.IsActive = true;
                    settingsBorderless.IsActive = false;
                    settingsWindowed.IsActive = false;
                    return;
                }
                //update antialiasing buttons
                if (settingsNoAA.IsPressed())
                {
                    settingsNoAA.IsActive = true;
                    settingsAA2.IsActive = false;
                    settingsAA4.IsActive = false;
                    settingsAA8.IsActive = false;
                    return;
                }
                if (settingsAA2.IsPressed())
                {
                    settingsNoAA.IsActive = false;
                    settingsAA2.IsActive = true;
                    settingsAA4.IsActive = false;
                    settingsAA8.IsActive = false;
                    return;
                }
                if (settingsAA4.IsPressed())
                {
                    settingsNoAA.IsActive = false;
                    settingsAA2.IsActive = false;
                    settingsAA4.IsActive = true;
                    settingsAA8.IsActive = false;
                    return;
                }
                if (settingsAA8.IsPressed())
                {
                    settingsNoAA.IsActive = false;
                    settingsAA2.IsActive = false;
                    settingsAA4.IsActive = false;
                    settingsAA8.IsActive = true;
                    return;
                }


            }
            resWidth.Update(gameTime);
            resHeight.Update(gameTime);

        }

        //Draw settings
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (parent.lastScene == 0)
                parent.titleScene.Draw(gameTime);
            else
            {
                parent.playScene.Draw(gameTime);
                parent.pauseScene.Draw(gameTime);
            }
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _spriteBatch.Begin();
            _spriteBatch.Draw(settingsWindow, Vector2.Zero, null,
                Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);
            //resolution settings
            _spriteBatch.DrawString(parent.smallFont, "Width: ",
                new Vector2(392 * parent.currentScreenScale.X, 325 * parent.currentScreenScale.Y),
                Color.Black, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
            resWidth.Draw(_spriteBatch);
            _spriteBatch.DrawString(parent.smallFont, "Height: ", new Vector2(582 * parent.currentScreenScale.X, 325 * parent.currentScreenScale.Y),
                Color.Black, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
            resHeight.Draw(_spriteBatch);
            //draw window Options
            settingsWindowed.DrawButton(_spriteBatch);
            settingsFullscreen.DrawButton(_spriteBatch);
            settingsBorderless.DrawButton(_spriteBatch);
            //draw antialiasing buttons
            settingsNoAA.DrawButton(_spriteBatch);
            settingsAA2.DrawButton(_spriteBatch);
            settingsAA4.DrawButton(_spriteBatch);
            settingsAA8.DrawButton(_spriteBatch);
            //draw close and apply buttons
            settingsCancel.DrawButton(_spriteBatch);
            settingsApply.DrawButton(_spriteBatch);
            //draw volume options
            masterVol.Draw(_spriteBatch, gameTime);
            _spriteBatch.DrawString(parent.smallFont, "%"+Math.Round(masterVol.Value*100,0).ToString(),
                new Vector2(1440 * parent.currentScreenScale.X, 322 * parent.currentScreenScale.Y),
                parent.textGold, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);

            musicVol.Draw(_spriteBatch, gameTime);
            _spriteBatch.DrawString(parent.smallFont, "%" + Math.Round(musicVol.Value * 100, 0).ToString(),
                new Vector2(1440 * parent.currentScreenScale.X, 403 * parent.currentScreenScale.Y),
                parent.textGold, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);

            sfxVol.Draw(_spriteBatch, gameTime);
            _spriteBatch.DrawString(parent.smallFont, "%" + Math.Round(sfxVol.Value * 100, 0).ToString(),
                new Vector2(1440 * parent.currentScreenScale.X, 484 * parent.currentScreenScale.Y),
                parent.textGold, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);

            _spriteBatch.End();
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

    }
}
