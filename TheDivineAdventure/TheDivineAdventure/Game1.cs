///Course:      CSC 316
///Project:     Final Project
///Creation:    4/14/22
///Authors:     Adkins, Christopher 
///             Blankenship, Sean A.
///             Michael, Hayden T.
///             Reed, Lucas
///Description: This program is our submission for the final project in CSC 316.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheDivineAdventure
{
    public class Game1 : Game
    {
        ///////////////
        ///VARIABLES///
        ///////////////

        // Essential
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        // Render Settings
        public Vector2 currentScreenScale;

        //Scenes
        public TitleScene titleScene;
        public CharacterSelectScene characterSelectScene;
        public PlayScene playScene;
        public CreditsScene creditsScene;
        public PauseScene pauseScene;
        public SettingsScene settingsScene;


        //Menu Navigation
        public static readonly string[] SCENES = { "TITLE_SCREEN", "LEVEL_SELECT", "CHARACTER_SELECT",
            "SCOREBOARD", "SETTINGS", "PLAYING", "IS_PAUSED", "CREDITS", "IS_DEAD"};
        public string currentScene, lastScene;
        public MouseState mouseState,lastMouseState;
        public KeyboardState keyboardState, lastKeyboard;
        public GameWindow _gameWindow;

        //player information to traverse scenes
        public string playerRole;


        // 2D Assets
        public SpriteFont BigFont, creditsFont, smallFont;
        public Texture2D cursor, whiteBox;
        public Rectangle healthBarRec, secondBarRec;
        public bool showCursor;

        // (Distance to end Boss)
        public int levelLength;

        // Font Color
        public Color textGold;

        // Songs
        private Song gameTheme;

        /*
        //28 Planned Character Models in total
        //8 Planned World Models in total
        private Model   warriorModel, rogueModel, mageModel;
        private Model   houndModel, impModel, goblinModel, 
                        ogreModel, gargoyModel, skeleModel;
        private Model   baelModel, agaresModel, vassaModel, samiModel,
                        marbasModel, valeModel, amonModel, barbaModel;
        private Model   luciModel, leviModel, satanModel, belphModel, 
                        mammonModel, beelzModel, asmoModel, angelModel;
        private model   level2Model, level3Model, level4Model,
                        level5Model, level6Model, level7Model, level8Model;
        */

        //Random class
        public Random rand;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false
            };
            Content.RootDirectory = "Content";

            Window.IsBorderless = true;
            IsMouseVisible = false;
            Window.Title = "The Divine Adventure";
        }

        protected override void Initialize()
        {
            base.Initialize();

            //create random object
            rand = new Random();

            //switch commenting to use saved settings
            //bool useSettings = true;
            bool useSettings = false;
            if (GameSettings.HasSettings() && useSettings)
            {
                //get settings
                string[,] settings = GameSettings.ReadSettings();

                //set resolution to preferrence
                _graphics.PreferredBackBufferWidth = Int32.Parse(settings[0,1]);
                _graphics.PreferredBackBufferHeight = Int32.Parse(settings[1, 1]);

                //set preferred window mode
                switch (Int32.Parse(settings[2, 1]))
                {
                    case 1:
                        Window.IsBorderless = true;
                        _graphics.IsFullScreen = false;
                        break;
                    case 2:
                        Window.IsBorderless = true;
                        _graphics.IsFullScreen = true;
                        break;
                    default:
                        Window.IsBorderless = false;
                        _graphics.IsFullScreen = false;
                        break;
                }

            }
            
            //// Set initial screen size
            //// (Determine size of display)
            int desktop_width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int desktop_height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //// (Apply the determined size)
            _graphics.PreferredBackBufferWidth = desktop_width;
            _graphics.PreferredBackBufferHeight = desktop_height;

            ////enable antialiasing (currently breaks game)
            //_graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //_graphics.PreferMultiSampling = true;
            //GraphicsDevice.PresentationParameters.MultiSampleCount = 2;

            _graphics.ApplyChanges();

            //set gamew window
            _gameWindow = Window;

            // Set screen scale to determine size of UI
            currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);

            //create Scenes
            titleScene = new TitleScene(_spriteBatch, _graphics, this, Content);
            characterSelectScene = new CharacterSelectScene(_spriteBatch, _graphics, this, Content);
            playScene = new PlayScene(_spriteBatch, _graphics, this, Content);
            creditsScene = new CreditsScene(_spriteBatch, _graphics, this, Content);
            pauseScene = new PauseScene(_spriteBatch, _graphics, this, Content);
            settingsScene = new SettingsScene(_spriteBatch, _graphics, this, Content);

            
            //initialize title menu
            titleScene.Initialize();


            //Start at either the title screen or in gameplay
            //Regular gameplay (e.g. start at title screen)
            currentScene = "TITLE";

            //DEBUG: uncomment to start in gameplay
            //InitializeLevel();
            //currentScene = 5;
        }

        //most content loading has been offloaded to neaten things up a bit. I'm keeping some things here that I haven't
        //moved or are needed in a bunch of locations (e.g.the cursor)
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            BigFont = Content.Load<SpriteFont>("BigFont");
            creditsFont = Content.Load<SpriteFont>("CreditsFont");
            smallFont = Content.Load<SpriteFont>("SmallFont");

            //set font color
            textGold = new Color(175, 127, 16);


            // Load sounds


            //Cursor texture
            cursor = Content.Load<Texture2D>("TEX_cursor");

            /*
            // Heroes
            warriorModel = Content.Load<Model>("MODEL_Warrior");
            rogueModel = Content.Load<Model>("MODEL_Rogue");
            mageModel = Content.Load<Model>("MODEL_Mage");

            // Enemies
            houndModel = Content.Load<Model>("MODEL_HellHound");
            impModel  = Content.Load<Model>("MODEL_Imp");
            goblinModel = Content.Load<Model>("MODEL_Goblin");
            ogreModel = Content.Load<Model>("MODEL_Ogre");
            gargoyModel = Content.Load<Model>("MODEL_Gargoyle");
            skeleModel = Content.Load<Model>("MODEL_Skeleton");

            // Minibosses
            baelModel = Content.Load<Model>("MODEL_Bael");
            agaresModel = Content.Load<Model>("MODEL_Agares");
            vassaModel = Content.Load<Model>("MODEL_Vassago");
            samiModel = Content.Load<Model>("MODEL_Samigina");
            marbasModel = Content.Load<Model>("MODEL_Marbas");
            valeModel = Content.Load<Model>("MODEL_Valefor");
            amonModel = Content.Load<Model>("MODEL_Amon");
            barbaModel = Content.Load<Model>("MODEL_Barbatos");

            // Bosses
            luciModel = Content.Load<Model>("MODEL_Lucifer");
            leviModel = Content.Load<Model>("MODEL_Leviathan");
            satanModel = Content.Load<Model>("MODEL_Satan");
            belphModel = Content.Load<Model>("MODEL_Belphegor");
            mammonModel = Content.Load<Model>("MODEL_Mammon");
            beelzModel = Content.Load<Model>("MODEL_Beelzebub");
            asmoModel = Content.Load<Model>("MODEL_Asmodeus");
            angelModel = Content.Load<Model>("MODEL_Angel");

            // Levels
            level2Model = Content.Load<Model>("MODEL_Level2");
            level3Model = Content.Load<Model>("MODEL_Level3");
            level4Model = Content.Load<Model>("MODEL_Level4");
            level5Model = Content.Load<Model>("MODEL_Level5");
            level6Model = Content.Load<Model>("MODEL_Level6");
            level7Model = Content.Load<Model>("MODEL_Level7");
            level8Model = Content.Load<Model>("MODEL_Level8");
            */
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            //Update based on current scene
            //game info was moved to the PlayingScene function below
            switch (currentScene)
            {
                case "TITLE":
                    titleScene.Update(gameTime);
                    break;
                case "CHARACTER_SELECT":
                    characterSelectScene.Update(gameTime);
                    break;
                case "SETTINGS":
                    settingsScene.Update(gameTime);
                    break;
                case "PLAY":
                    playScene.Update(gameTime);
                    break;
                case "PAUSE":
                    pauseScene.Update(gameTime);
                    break;
                case "CREDITS":
                    creditsScene.Update(gameTime);
                    break;
                default:
                    titleScene.Update(gameTime);
                    break;
            }

            //DEBUG: FPS COUNTER IN DEBUG LOG
            //Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);

            lastKeyboard = keyboardState;
            lastMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Graphics device settings
            GraphicsDevice.Clear(new Color(40, 20, 20));

            //Update based on current scene
            switch (currentScene)
            {
                case "TITLE":
                    titleScene.Draw(gameTime);
                    break;
                case "CHARACTER_SELECT":
                    characterSelectScene.Draw(gameTime);
                    break;
                case "SETTINGS":
                    settingsScene.Draw(gameTime);
                    break;
                case "PLAY":
                    playScene.Draw(gameTime);
                    break;
                case "PAUSE":
                    playScene.Draw(gameTime);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                    pauseScene.Draw(gameTime);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                    break;
                case "CREDITS":
                    creditsScene.Draw(gameTime);
                    break;
                default:
                    titleScene.Draw(gameTime);
                    break;
            }

            //draw cursor
            if (showCursor == true)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                _spriteBatch.Begin();
                _spriteBatch.Draw(cursor, new Vector2(mouseState.X, mouseState.Y), null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
                _spriteBatch.End();
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }

            base.Draw(gameTime);
        }

        //Scene Functions
        public void ReloadContent()
        {
            UnloadContent();
            LoadContent();
            currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);

        }

    }
}