using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Button settingsWindowed, settingsBorderless, settingsFullscreen, settingsNoAA, settingsAA2,
            settingsAA4, settingsAA8, settingsCancel, settingsApply;

        public SettingsScene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 parent) : base(sb, graph, parent)
        {

        }

        //initialize settings Menu
        public override void Initialize()
        {
            base.Initialize();
            parent.showCursor = true;
            //create resolution buttons
            resWidth = new TextBox(_graphics.PreferredBackBufferWidth.ToString(), 4,
                parent.smallFont, new Vector2(492, 325), 30, parent.currentScreenScale, new Color(Color.Black, 60), parent.whiteBox);
            resHeight = new TextBox(_graphics.PreferredBackBufferHeight.ToString(), 4,
                parent.smallFont, new Vector2(682, 325), 30, parent.currentScreenScale, new Color(Color.Black, 60), parent.whiteBox);

            //create window buttons
            settingsWindowed = new Button(parent.settingsButton1, parent.settingsButton1, "Windowed", parent.smallFont,
                new Vector2(392, 404), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.Window.IsBorderless == false)
                settingsWindowed.IsActive = true;
            settingsBorderless = new Button(parent.settingsButton1, parent.settingsButton1, "Borderless",
                parent.smallFont, new Vector2(392, 455), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.Window.IsBorderless == true)
                settingsBorderless.IsActive = true;
            settingsFullscreen = new Button(parent.settingsButton1, parent.settingsButton1, "Fullscreen",
                parent.smallFont, new Vector2(392, 505), new Vector2(180, 29), parent.currentScreenScale);
            if (_graphics.IsFullScreen == true)
                settingsFullscreen.IsActive = true;


            //create antialiasing buttons
            settingsNoAA = new Button(parent.settingsButton1, parent.settingsButton1, "None",
                parent.smallFont, new Vector2(392, 585), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 0)
                settingsNoAA.IsActive = true;
            settingsAA2 = new Button(parent.settingsButton1, parent.settingsButton1, "2x",
                parent.smallFont, new Vector2(392, 635), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 2)
                settingsAA2.IsActive = true;
            settingsAA4 = new Button(parent.settingsButton1, parent.settingsButton1, "4x", 
                parent.smallFont, new Vector2(392, 685), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 4)
                settingsAA4.IsActive = true;
            settingsAA8 = new Button(parent.settingsButton1, parent.settingsButton1, "8x", 
                parent.smallFont, new Vector2(392, 735), new Vector2(180, 29), parent.currentScreenScale);
            if (parent.GraphicsDevice.PresentationParameters.MultiSampleCount == 8)
                settingsAA8.IsActive = true;

            //create back and apply buttons
            settingsCancel = new Button(parent.settingsButton2, parent.settingsButton2, "Cancel",
                parent.smallFont, new Vector2(70, 972), new Vector2(210, 76), parent.currentScreenScale);
            settingsApply = new Button(parent.settingsButton2, parent.settingsButton2, "Apply",
                parent.smallFont, new Vector2(375, 972), new Vector2(210, 76), parent.currentScreenScale);
        }

        //update settings
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
                    if (settingsWindowed.IsActive == true)
                    {
                        parent.Window.IsBorderless = false;
                        _graphics.IsFullScreen = false;
                    }
                    else if (settingsBorderless.IsActive == true)
                    {
                        parent.Window.IsBorderless = true;
                        _graphics.IsFullScreen = false;
                    }
                    else if (settingsFullscreen.IsActive == true)
                        _graphics.IsFullScreen = true;

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
                    parent.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
                    _graphics.ApplyChanges();
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
            _spriteBatch.Draw(parent.settingsWindow, Vector2.Zero, null,
                Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);
            //resolution settings
            _spriteBatch.DrawString(parent.smallFont, "Width: ",
                new Vector2(392 * parent.currentScreenScale.X, 325 * parent.currentScreenScale.Y), Color.Black, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
            resWidth.Draw(_spriteBatch);
            _spriteBatch.DrawString(parent.smallFont, "Height: ", new Vector2(582 * parent.currentScreenScale.X, 325 * parent.currentScreenScale.Y), Color.Black, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
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

            _spriteBatch.End();
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

    }
}
