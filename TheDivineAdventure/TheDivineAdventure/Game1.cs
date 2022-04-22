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
        private SpriteBatch _spriteBatch;

        // Render Settings
        private Vector2 currentScreenScale;


        //Menu Navigation
        public static readonly string[] SCENES = { "TITLE_SCREEN", "LEVEL_SELECT", "CHARACTER_SELECT",
            "SCOREBOARD", "SETTINGS", "PLAYING", "IS_PAUSED", "CREDITS"};
        public int currentScene, lastScene;
        public MouseState mouseState;
        public KeyboardState lastKeyboard;
        public GameWindow _gameWindow;


        // 2D Assets
        private SpriteFont BigFont, creditsFont,smallFont;
        private Texture2D hudL1, hudL2, progIcon, healthBar, staminaBar, manaBar, titleScreenBack, TitleScreenFront,
            distantDemonSheet, titleLightning01, titleLightning02, titleLightning03, emberSheet01, cursor, titleBox, titleLava,
            pauseMenu,pauseMenuSheet, whiteBox, settingsWindow, settingsButton1, settingsButton2;
        private Rectangle healthBarRec, secondBarRec;
        private AnimatedSprite[] titleDemons, titleEmbers;
        private AnimatedSprite secondaryPauseMenu;
        private TextBox resWidth, resHeight;
        private Button titleStartGame, titleScoreboard, titleSettings, titleCredits, titleQuitGame, pauseResume, pauseRestart, pauseSettings,
            pauseQuitMenu, pauseQuitGame, pauseYes, pauseNo, settingsWindowed, settingsBorderless, settingsFullscreen, settingsNoAA, settingsAA2,
            settingsAA4, settingsAA8, settingsCancel, settingsApply;
        private bool showCursor,glowState;
        private String[] credits;
        private float creditsRuntime;
        private int glowRef, pauseIsConfirming;


        // (Distance to end Boss)
        private int levelLength;
        private float travel;

        // Font Color
        Color textGold;

        // Songs
        private Song gameTheme;

        // 3D Assets
        private Model clericModel;
        private Model demonModel;
        private Model level1Model;
        private Model playerProjModel, enemyProjModel;
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

        // Camera
        private Camera camera;

        // Player
        private Player player;
        private List<SoundEffect> playerSounds = new List<SoundEffect>();
        private string playerRole;
        private int score;

        // Enemy
        private Enemy enemy;
        private List<Enemy> enemyList;
        private List<SoundEffect> enemySounds = new List<SoundEffect>();
        private string enemyRole;

        // Matrices
        private Matrix worldPlayer, worldEnemy, worldProj, worldLevel;

        //Random class
        private Random rand;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false
            };
            Content.RootDirectory = "Content";

            Window.IsBorderless = true;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            //create random object
            rand = new Random();

            // Set initial screen size
            // (Determine size of display)
            int desktop_width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int desktop_height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            // (Apply the determined size)
            _graphics.PreferredBackBufferWidth = desktop_width;
            _graphics.PreferredBackBufferHeight = desktop_height;
            //enable antialiasing (currently breaks game)
            //_graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //_graphics.PreferMultiSampling = true;
            //GraphicsDevice.PresentationParameters.MultiSampleCount = 2;
            _graphics.ApplyChanges();

            //set gamew window
            _gameWindow = Window;

            // Set screen scale to determine size of UI
            currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);

            //initialize title menu
            InitializeTitleScreen();


            //Start at either the title screen or in gameplay
            //Regular gameplay (e.g. start at title screen)
            currentScene = 0;

            //DEBUG: uncomment to start in gameplay
            //InitializeLevel();
            //currentScene = 5;
        }

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


            // Load 2D textures

            //procedural textures
            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });
            //Cursor
            cursor = Content.Load<Texture2D>("TEX_cursor");


            //Title Screen
            if (currentScene == 0)
            {
                titleScreenBack = Content.Load<Texture2D>("TEX_TitleScreen_Back");
                TitleScreenFront = Content.Load<Texture2D>("TEX_TitleScreen_Front");
                distantDemonSheet = Content.Load<Texture2D>("TEX_DemonSpriteSheet");
                titleLightning01 = Content.Load<Texture2D>("TEX_Title_Lightning_01");
                titleLightning02 = Content.Load<Texture2D>("TEX_Title_Lightning_02");
                titleLightning03 = Content.Load<Texture2D>("TEX_Title_Lightning_03");
                emberSheet01 = Content.Load<Texture2D>("TEX_EmberSheet01");
                titleLava = Content.Load<Texture2D>("TEX_Title_LavaGlow");
                return;
            }
            //Settings
            if (currentScene == 4)
            {
                settingsWindow = Content.Load<Texture2D>("TEX_Settings_Window");
                settingsButton1 = Content.Load<Texture2D>("TEX_Settings_Button1_Passive");
                settingsButton2 = Content.Load<Texture2D>("TEX_Settings_Button2");
            }

            //HUD
            if (currentScene == 5)
            {
                hudL1 = Content.Load<Texture2D>("TEX_HolyHUD_L1");
                hudL2 = Content.Load<Texture2D>("TEX_HolyHUD_L2");
                progIcon = Content.Load<Texture2D>("TEX_ProgressionIcon");
                healthBar = Content.Load<Texture2D>("TEX_HealthBar");
                manaBar = Content.Load<Texture2D>("TEX_ManaBar");
                staminaBar = Content.Load<Texture2D>("TEX_StaminaBar");
            }
            //pause screen
            if (currentScene == 6)
            {
                emberSheet01 = Content.Load<Texture2D>("TEX_EmberSheet01");
                pauseMenu = Content.Load<Texture2D>("TEX_Pause_Menu");
                pauseMenuSheet = Content.Load<Texture2D>("TEX_SideMenu_Sheet");
                return;
            }

            //Credits Scene
            if (currentScene == 7)
            {
                titleBox = Content.Load<Texture2D>("TEX_TitleBox03");
            }


            // Load 3D models

            clericModel = Content.Load<Model>("MODEL_Cleric");
            demonModel = Content.Load<Model>("MODEL_Demon");
            level1Model = Content.Load<Model>("MODEL_Level1");
            playerProjModel = Content.Load<Model>("MODEL_PlayerProjectile");
            enemyProjModel = Content.Load<Model>("MODEL_EnemyProjectile");

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
            //Update based on current scene
            //game info was moved to the PlayingScene function below
            switch (currentScene)
            {
                case 0:
                    UpdateTitleScreen(gameTime);
                    break;
                case 4:
                    UpdateSettings(gameTime);
                    break;
                case 5:
                    UpdatePlayingScene(gameTime);
                    Debug.WriteLine(player.Pos);
                    break;
                case 6:
                    UpdatePause(gameTime);
                    break;
                case 7:
                    UpdateCreditsScene(gameTime);
                    break;
                default:
                    UpdateTitleScreen(gameTime);
                    break;
            }

            //DEBUG: SCREEN RESOLUTION THINGS
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                _graphics.PreferredBackBufferWidth = 1920;
                _graphics.PreferredBackBufferHeight = 1080;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                InitializeSettings();
                currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                _graphics.PreferredBackBufferWidth = 960;
                _graphics.PreferredBackBufferHeight = 540;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                InitializeSettings();
                currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
            }
            //DEBUG: FPS COUNTER IN DEBUG LOG
            //Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);

            lastKeyboard = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Graphics device settings
            GraphicsDevice.Clear(new Color(40, 20, 20));

            //Update based on current scene
            //game info was moved to the DrawPlayingScene function below
            switch (currentScene)
            {
                case 0:
                    DrawTitleScreen(gameTime);
                    break;
                case 4:
                    DrawSettings(gameTime);
                    break;
                case 5:
                    DrawPlayingScene(gameTime);
                    break;
                case 6:
                    DrawPlayingScene(gameTime);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                    DrawPause(gameTime);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                    break;
                case 7:
                    DrawCreditsScene(gameTime);
                    break;
                default:
                    DrawTitleScreen(gameTime);
                    break;
            }

            //draw cursor
            if (showCursor == true)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                _spriteBatch.Begin();
                _spriteBatch.Draw(cursor, new Vector2(mouseState.X,mouseState.Y), null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
                _spriteBatch.End();
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }

            base.Draw(gameTime);
        }

        ///////////////
        ///SCENES/////
        /////////////

        ///////////////////////////////////////////////////////////
        /////////////////////////////////////////MAINGAME////////
        ////////////////////////////////////////////////////////

        //initialize game objects and load level
        private void InitializePlayingScene()
        {

            LoadContent();
            //hide cursor
            showCursor = false; ;

            // Role Info
            playerRole = Player.ROLES[3];
            enemyRole = Enemy.ROLES[0];

            // Initialize game objects
            player = new Player(playerSounds, playerRole);
            camera = new Camera(GraphicsDevice, Vector3.Up, player);
            enemy = new Enemy(enemySounds, enemyRole);
            enemyList = new List<Enemy>();

            // Generate resource Bars rectangles
            healthBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth * 0.099f / currentScreenScale.X),
                (int)Math.Round(_graphics.PreferredBackBufferHeight * 0.044f / currentScreenScale.Y),
                (int)Math.Round(.201f * _graphics.PreferredBackBufferWidth / currentScreenScale.X),
                (int)Math.Round(.05f * _graphics.PreferredBackBufferHeight / currentScreenScale.Y));
            secondBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth * 0.088f / currentScreenScale.X),
                (int)Math.Round(_graphics.PreferredBackBufferHeight * 0.099f / currentScreenScale.Y),
                (int)Math.Round(.201f * _graphics.PreferredBackBufferWidth / currentScreenScale.X),
                (int)Math.Round(.05f * _graphics.PreferredBackBufferHeight / currentScreenScale.Y));

            // Initialize Distance to Boss(kept as a variable in case we have multiple level length)
            levelLength = 3500;

            //set score to 0
            score = 0;
        }

        //function to do updates when player is playing in level.
        private void UpdatePlayingScene(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && lastKeyboard.IsKeyUp(Keys.Escape))
            {
                currentScene = 6;
                InitializePause();
                return;
            }

            player.Update(gameTime, camera);
            camera.Update(gameTime, player);
            enemy.Update(gameTime);

            foreach (Projectile p in player.projList)
            {
                p.Update(gameTime);
            }


            foreach (Projectile p in enemy.projList)
            {
                p.Update(gameTime);
            }

            //update distance to boss
            if (player.Pos.Z > 0 && player.Pos.Z < levelLength)
                travel = (player.Pos.Z * _graphics.PreferredBackBufferWidth * 0.276f) / Math.Abs(levelLength);

            //test score
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                score += 3;

            // Basic kill player for (will improve once some UI is built up)
            if (player.Health <= 0)
            {
                player = new Player(playerSounds, playerRole);
                score = 0;
            }
        }

        //function to do draw when player is playing in level.
        private void DrawPlayingScene(GameTime gameTime)
        {
            // Render world
            worldLevel = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(new Vector3(0, 0, -5));

            level1Model.Draw(worldLevel, camera.View, camera.Proj);

            // Render single enemy
            // THIS IS TEMPORARY
            worldEnemy = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(enemy.Pos);
            switch (enemy.role)
            {
                case "DEMON":
                    demonModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
                case "HELLHOUND":
                    //houndModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
                case "IMP":
                    //impModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
                case "GOBLIN":
                    //goblinModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
                case "OGRE":
                    //ogreModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
                case "GARGOYLE":
                    //gargoyModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
                case "SKELETON":
                    //skeleModel.Draw(worldEnemy, camera.View, camera.Proj);
                    clericModel.Draw(worldEnemy, camera.View, camera.Proj);
                    break;
            }


            // Render player
            worldPlayer = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(player.Rot.Y)) *
                        Matrix.CreateTranslation(player.Pos);
            switch (player.role)
            {
                case "WARRIOR":
                    //warriorModel.Draw(worldPlayer, camera.View, camera.Proj);
                    demonModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
                case "ROGUE":
                    //rogueModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
                case "MAGE":
                    //mageModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
                case "CLERIC":
                    clericModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
            }

            // Render player bullets
            foreach (Projectile p in player.projList)
            {
                worldProj = Matrix.CreateScale(1f) *
                        Matrix.CreateTranslation(p.Pos);
                playerProjModel.Draw(worldProj, camera.View, camera.Proj);
            }

            // Render enemy bullets
            foreach (Projectile p in enemy.projList)
            {
                worldProj = Matrix.CreateScale(1f) *
                    Matrix.CreateTranslation(p.Pos);
                enemyProjModel.Draw(worldProj, camera.View, camera.Proj);
            }

            // ** Render HUD **
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _spriteBatch.Begin();
            _spriteBatch.Draw(hudL1, Vector2.Zero, null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            //progessIcon
            _spriteBatch.Draw(progIcon,
                new Vector2(_graphics.PreferredBackBufferWidth * 0.348f + travel, _graphics.PreferredBackBufferHeight * 0.873f),
                null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            //Score
            _spriteBatch.DrawString(BigFont, score.ToString(),
                new Vector2(_graphics.PreferredBackBufferWidth * 0.498f - (BigFont.MeasureString(score.ToString()) * .5f * currentScreenScale).X, _graphics.PreferredBackBufferHeight * -0.01f),
                textGold, 0f, Vector2.Zero, currentScreenScale, SpriteEffects.None, 1);
            //resource bars
            _spriteBatch.Draw(healthBar,
                player.resourceBarUpdate(true, healthBarRec,
                new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), currentScreenScale), Color.White);
            if (player.IsCaster)
                _spriteBatch.Draw(manaBar,
                    player.resourceBarUpdate(false, secondBarRec,
                    new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), currentScreenScale), Color.White);
            else
                _spriteBatch.Draw(staminaBar,
                    player.resourceBarUpdate(false, secondBarRec,
                    new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), currentScreenScale), Color.White);
            //topHUD layer
            _spriteBatch.Draw(hudL2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 1);
            _spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }


        ///////////////////////////////////////////////////////////
        /////////////////////////////////////////TITLESCREEN//////
        /////////////////////////////////////////////////////////
        

        //Initialize Title Screen.
        private void InitializeTitleScreen()
        {

            LoadContent();
            //show custom cursor
            showCursor = true;
            //create background demons
            titleDemons = new AnimatedSprite[rand.Next(40,180)];
            for (int i = 0; i < titleDemons.Length; i++)
            {
                titleDemons[i] = new AnimatedSprite(109, 108, distantDemonSheet, 7);
                titleDemons[i].Pos = new Vector2((-1 * rand.Next(-1920,1000)) * currentScreenScale.X, rand.Next(10,600) * currentScreenScale.Y);
                titleDemons[i].Scale = 1 - (rand.Next(50) / 100f);
                titleDemons[i].Tint = new Color(Color.White, titleDemons[i].Scale);
                titleDemons[i].Frame = rand.Next(7);
            }
            //create embers
            titleEmbers = new AnimatedSprite[10];
            for (int i = 0; i < titleEmbers.Length; i++)
            {
                titleEmbers[i] = new AnimatedSprite(174, 346, emberSheet01, 6);
                titleEmbers[i].Pos = new Vector2(rand.Next(1920) * currentScreenScale.X, rand.Next(450, 750) * currentScreenScale.Y);
                titleEmbers[i].Scale = 1 - (rand.Next(-200, 50) / 100f);
                titleEmbers[i].Frame = rand.Next(6);
            }
            glowRef = 0;
            //create menu buttons
            titleStartGame = new Button(new Vector2(247, 686), new Vector2(366, 60), currentScreenScale);
            titleScoreboard = new Button(new Vector2(247, 750), new Vector2(366, 60), currentScreenScale);
            titleSettings = new Button(new Vector2(247, 812), new Vector2(366, 60), currentScreenScale);
            titleCredits = new Button(new Vector2(247, 870), new Vector2(366, 60), currentScreenScale);
            titleQuitGame = new Button(new Vector2(247, 928), new Vector2(366, 60), currentScreenScale);
        }

        //function to do updates when player is in the title Screen.
        private void UpdateTitleScreen(GameTime gameTime)
        {
            //get mouse clocks and check buttons
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (titleStartGame.IsPressed())
                {
                    UnloadContent();
                    currentScene = 5;
                    InitializePlayingScene();
                    return;
                }
                if (titleScoreboard.IsPressed())
                {
                    //currentScene = (1);
                    return;
                }
                if (titleSettings.IsPressed())
                {
                    lastScene = 0;
                    currentScene = 4;
                    InitializeSettings();
                    return;
                }
                if (titleCredits.IsPressed())
                {
                    UnloadContent();
                    currentScene = 7;
                    InitializeCreditScene();
                    return;
                }
                if (titleQuitGame.IsPressed())
                {
                    Exit();
                }
            }


        }

        //function to do draw when player is playing title Screen.
        private void DrawTitleScreen(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(titleScreenBack, Vector2.Zero, null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            //randomly draw lightning
            if (rand.Next(50) > 48)
            {
                switch (rand.Next(3))
                {
                    case 1:
                        _spriteBatch.Draw(titleLightning01, Vector2.Zero, null, new Color(Color.White, 1 - (rand.Next(50) / 100f)),
                            0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(titleLightning02, Vector2.Zero, null, new Color(Color.White, 1 - (rand.Next(50) / 100f)),
                            0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
                        break;
                    case 3:
                        _spriteBatch.Draw(titleLightning03, Vector2.Zero, null, new Color(Color.White, 1 - (rand.Next(50) / 100f)),
                            0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
                        break;
                    default:
                        break;
                }
            }
            //draw background demons
            foreach (AnimatedSprite demon in titleDemons)
            {
                if (demon.Pos.X > _graphics.PreferredBackBufferWidth)
                {
                    demon.Pos = new Vector2(-1 * rand.Next(1500) * currentScreenScale.X, (50 + rand.Next(500)) * currentScreenScale.Y);
                    demon.Scale = 1 - (rand.Next(50) / 100f);
                    demon.Tint = new Color(Color.White, demon.Scale);
                }
                else
                {
                    demon.Pos = new Vector2(demon.Pos.X + (demon.Scale * 10 * currentScreenScale.X), demon.Pos.Y);
                }
                demon.Draw(_spriteBatch, currentScreenScale);
            }
            //draw embers
            foreach (AnimatedSprite ember in titleEmbers)
            {
                if (ember.Frame == 0)
                {
                    ember.Pos = new Vector2((rand.Next(1920)) * currentScreenScale.X, rand.Next(450, 750) * currentScreenScale.Y);
                    ember.Scale = 1 - (rand.Next(-200, 50) / 100f);
                }
                ember.Draw(_spriteBatch, currentScreenScale);
            }
            //draw title foreground
            _spriteBatch.Draw(TitleScreenFront, Vector2.Zero, null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            //draw lava glow
            if (glowState)
            {
                _spriteBatch.Draw(titleLava, Vector2.Zero, null, new Color(Color.White, glowRef), 0, Vector2.Zero,
                    currentScreenScale, SpriteEffects.None, 1);
                glowRef += 5;
            }
            else
            {
                _spriteBatch.Draw(titleLava, Vector2.Zero, null, new Color(Color.White, glowRef), 0, Vector2.Zero,
                    currentScreenScale, SpriteEffects.None, 1);
                glowRef -= 5;
            }

            if (glowRef >= 255 && glowState!)
                glowState = false;
            else if (glowRef <= 0)
                glowState = true;

            if (rand.Next(0, 100) > 90)
                glowState = !glowState;

            _spriteBatch.End();
        }


        ///////////////////////////////////////////////////////////
        /////////////////////////////////////////CREDITS_SCENE////
        /////////////////////////////////////////////////////////

        //initialize Credits scene
        private void InitializeCreditScene()
        {
            LoadContent();
            //hide cursor
            showCursor = false; ;

            credits = new string[]  {"Game Programmers", "       Christopher Adkins", "       Sean Blankenship", "       Hayden Michael", "       Lucas Reed",
                " ","Game Artists", "       2D assets: Christopher Adkins", "       3D Assets: Sean Blankenship",
                " ","Sound Engineers", "       Lucas Reed",
                " ","Game Testers", "       Christopher Adkins", "       Sean Blankenship", "       Hayden Michael", "       Lucas Reed",
                " ","Created using MonoGame ",
                " ","Thank you for your time! "};
            creditsRuntime = 800;
        }
        //update credits scene
        private void UpdateCreditsScene(GameTime gameTime)
        {
            //return to ttitle screen
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                UnloadContent();
                currentScene = 0;
                InitializeTitleScreen();
            }
                

        }

        //Draw Credits Scene
        private void DrawCreditsScene(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(titleBox, 
                new Vector2 ((_graphics.PreferredBackBufferWidth/2)-(390*currentScreenScale.X), creditsRuntime - (800*currentScreenScale.Y) + _graphics.PreferredBackBufferHeight),
                null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            int cIndex = 0;
            foreach(String c in credits)
            {
                cIndex++;
                //draw each line in the credits array
                if (creditsRuntime + (120f * cIndex * currentScreenScale.Y) + _graphics.PreferredBackBufferHeight
                    < _graphics.PreferredBackBufferHeight &&
                    creditsRuntime + (120f * cIndex * currentScreenScale.Y) + _graphics.PreferredBackBufferHeight >-80)
                    _spriteBatch.DrawString(creditsFont, c,
                    new Vector2(_graphics.PreferredBackBufferWidth * 0.05f, creditsRuntime +(120f*cIndex* currentScreenScale.Y)
                    + _graphics.PreferredBackBufferHeight),
                    textGold, 0f, Vector2.Zero, currentScreenScale, SpriteEffects.None, 1);
            }
            if(creditsRuntime + (120f * credits.Length-1) + _graphics.PreferredBackBufferHeight < -120f)
            {
                UnloadContent();
                currentScene = 0;
                InitializeTitleScreen();
                return;
            }
            creditsRuntime -= 5f*currentScreenScale.Y;
            _spriteBatch.End();
        }

        ///////////////////////////////////////////////////////////
        /////////////////////////////////////////PAUSEMENU////////
        /////////////////////////////////////////////////////////

        //initialize Pause Menu
        private void InitializePause()
        {
            LoadContent();
            showCursor = true;
            pauseIsConfirming = 0;
            //create embers
            titleEmbers = new AnimatedSprite[30];
            for (int i = 0; i < titleEmbers.Length; i++)
            {
                titleEmbers[i] = new AnimatedSprite(174, 346, emberSheet01, 6);
                titleEmbers[i].Pos = new Vector2(rand.Next(1920) * currentScreenScale.X, rand.Next(450, 750) * currentScreenScale.Y);
                titleEmbers[i].Scale = 1 - (rand.Next(-200, 50) / 100f);
                titleEmbers[i].Frame = rand.Next(6);
            }

            //create buttons
            pauseResume = new Button(new Vector2(665, 304), new Vector2(284, 60), currentScreenScale);
            pauseRestart = new Button(new Vector2(656, 405), new Vector2(204, 60), currentScreenScale);
            pauseSettings = new Button(new Vector2(650, 504), new Vector2(222, 60), currentScreenScale);
            pauseQuitMenu = new Button(new Vector2(579, 596), new Vector2(366, 38), currentScreenScale);
            pauseQuitGame = new Button(new Vector2(620, 700), new Vector2(283, 38), currentScreenScale);
            pauseYes = new Button(new Vector2(1053, 527), new Vector2(134, 55), currentScreenScale);
            pauseNo = new Button(new Vector2(1055, 604), new Vector2(135, 55), currentScreenScale);

        }

        //update Pause Menu
        private void UpdatePause(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && lastKeyboard.IsKeyUp(Keys.Escape))
            {
                showCursor =false;
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                currentScene = 5;
                return;
            }

            //get mouse clocks and check buttons
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (pauseResume.IsPressed())
                {
                    showCursor = false;
                    Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                    currentScene = 5;
                    return;
                }
                if (pauseRestart.IsPressed())
                {
                    secondaryPauseMenu = new AnimatedSprite(439, 488, pauseMenuSheet, 4, false);
                    secondaryPauseMenu.Pos = new Vector2(910, 306);
                    secondaryPauseMenu.Framerate = 1.5f;
                    pauseIsConfirming = 1;
                    //currentScene = (1);
                    return;
                }
                if (pauseSettings.IsPressed())
                {
                    lastScene = 6;
                    currentScene = 4;
                    InitializeSettings();
                    return;

                }
                if (pauseQuitMenu.IsPressed())
                {
                    secondaryPauseMenu = new AnimatedSprite(439, 488, pauseMenuSheet, 4, false);
                    secondaryPauseMenu.Pos = new Vector2(910, 306);
                    secondaryPauseMenu.Framerate = 1.5f;
                    pauseIsConfirming = 2;
                    return;
                }
                if (pauseQuitGame.IsPressed())
                {
                    secondaryPauseMenu = new AnimatedSprite(439, 488, pauseMenuSheet, 4, false);
                    secondaryPauseMenu.Pos = new Vector2(910, 306);
                    secondaryPauseMenu.Framerate = 1.5f;
                    pauseIsConfirming = 3;
                }
                if(pauseIsConfirming != 0)
                {
                    if (pauseYes.IsPressed())
                    {
                        switch (pauseIsConfirming)
                        {
                            case 1:
                                UnloadContent();
                                currentScene = 5;
                                InitializePlayingScene();
                                return;
                            case 2:
                                UnloadContent();
                                currentScene = 0;
                                InitializeTitleScreen();
                                break;
                            case 3:
                                Exit();
                                break;
                            default:
                                pauseIsConfirming = 0;
                                break;
                        }
                    }
                    if (pauseNo.IsPressed())
                    {
                        pauseIsConfirming = 0;
                    }
                }
            }

        }

        //update Draw Menu
        private void DrawPause(GameTime gameTime)
        {
            //draw menu
            _spriteBatch.Begin();
            _spriteBatch.Draw(pauseMenu, Vector2.Zero,null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            //draw embers
            foreach (AnimatedSprite ember in titleEmbers)
            {
                if (ember.Frame == 0)
                {
                    ember.Pos = new Vector2((rand.Next(1920)) * currentScreenScale.X, rand.Next(450, 750) * currentScreenScale.Y);
                    ember.Scale = 1 - (rand.Next(-200, 50) / 100f);
                }
                ember.Draw(_spriteBatch, currentScreenScale);
            }
            if (pauseIsConfirming != 0)
            {
                secondaryPauseMenu.Draw(_spriteBatch, currentScreenScale);
            }
            _spriteBatch.End();

        }



        ///////////////////////////////////////////////////////////
        /////////////////////////////////////////SETTINGS/////////
        /////////////////////////////////////////////////////////

        //initialize settings Menu
        private void InitializeSettings()
        {
            LoadContent();
            showCursor = true;
            //create resolution buttons
            resWidth = new TextBox(_graphics.PreferredBackBufferWidth.ToString(), 4,
                smallFont, new Vector2 (492,325), 30 , currentScreenScale, new Color(Color.Black,60), whiteBox);
            resHeight = new TextBox(_graphics.PreferredBackBufferHeight.ToString(), 4,
                smallFont, new Vector2(682, 325), 30, currentScreenScale, new Color(Color.Black, 60), whiteBox);

            //create window buttons
            settingsWindowed = new Button(settingsButton1, settingsButton1, "Windowed",smallFont, new Vector2(392, 404), new Vector2(180, 29), currentScreenScale);
            if (Window.IsBorderless == false)
                settingsWindowed.IsActive=true;
            settingsBorderless = new Button(settingsButton1, settingsButton1, "Borderless",smallFont, new Vector2(392, 455), new Vector2(180, 29), currentScreenScale);
            if (Window.IsBorderless == true)
                settingsBorderless.IsActive = true;
            settingsFullscreen = new Button(settingsButton1, settingsButton1, "Fullscreen",smallFont, new Vector2(392, 505), new Vector2(180, 29), currentScreenScale);
            if (_graphics.IsFullScreen == true)
                settingsFullscreen.IsActive = true;


            //create antialiasing buttons
            settingsNoAA = new Button(settingsButton1, settingsButton1, "None", smallFont, new Vector2(392, 585), new Vector2(180, 29), currentScreenScale);
            if (GraphicsDevice.PresentationParameters.MultiSampleCount == 0)
                settingsNoAA.IsActive = true;
            settingsAA2 = new Button(settingsButton1, settingsButton1, "2x", smallFont, new Vector2(392, 635), new Vector2(180, 29), currentScreenScale);
            if (GraphicsDevice.PresentationParameters.MultiSampleCount == 2)
                settingsAA2.IsActive = true;
            settingsAA4 = new Button(settingsButton1, settingsButton1, "4x", smallFont, new Vector2(392, 685), new Vector2(180, 29), currentScreenScale);
            if (GraphicsDevice.PresentationParameters.MultiSampleCount == 4)
                settingsAA4.IsActive = true;
            settingsAA8 = new Button(settingsButton1, settingsButton1, "8x", smallFont, new Vector2(392, 735), new Vector2(180, 29), currentScreenScale);
            if (GraphicsDevice.PresentationParameters.MultiSampleCount == 8)
                settingsAA8.IsActive = true;

            //create back and apply buttons
            settingsCancel = new Button(settingsButton2, settingsButton2, "Cancel", smallFont, new Vector2(70, 972), new Vector2(210, 76), currentScreenScale);
            settingsApply = new Button(settingsButton2, settingsButton2, "Apply", smallFont, new Vector2(375, 972), new Vector2(210, 76), currentScreenScale);
        }

        //update settings
        private void UpdateSettings(GameTime gameTime)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //apply settings or cancel
                if (settingsCancel.IsPressed())
                {
                    settingsCancel.IsActive = true;
                    currentScene = lastScene;
                    return;
                }
                if (settingsApply.IsPressed())
                {
                    _graphics.PreferredBackBufferWidth = Int32.Parse(resWidth.Text);
                    _graphics.PreferredBackBufferHeight = Int32.Parse(resHeight.Text);
                    if (settingsWindowed.IsActive == true) {
                        Window.IsBorderless = false;
                        _graphics.IsFullScreen = false;
                    } else if (settingsBorderless.IsActive == true) {
                        Window.IsBorderless = true;
                        _graphics.IsFullScreen = false;
                    } else if (settingsFullscreen.IsActive == true)
                        _graphics.IsFullScreen = true;

                    if (settingsNoAA.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = false;
                        GraphicsDevice.PresentationParameters.MultiSampleCount = 0;
                    }else if (settingsAA2.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = true;
                        GraphicsDevice.PresentationParameters.MultiSampleCount = 2;
                    }
                    else if (settingsAA4.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = true;
                        GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
                    }
                    else if (settingsAA8.IsActive == true)
                    {
                        _graphics.PreferMultiSampling = true;
                        GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
                    }
                    GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
                    _graphics.ApplyChanges();
                    currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
                    if (lastScene == 0)
                        InitializeTitleScreen();
                    if (lastScene == 6)
                        InitializePause();
                    InitializeSettings();

                    settingsApply.IsActive = true;
                    currentScene = lastScene;
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
        private void DrawSettings(GameTime gameTime)
        {
            if (lastScene == 0)
                DrawTitleScreen(gameTime);
            else
            {
                DrawPlayingScene(gameTime);
                DrawPause(gameTime);
            }
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _spriteBatch.Begin();
            _spriteBatch.Draw(settingsWindow, Vector2.Zero, null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            //resolution settings
            _spriteBatch.DrawString(smallFont, "Width: ", new Vector2(392*currentScreenScale.X, 325*currentScreenScale.Y), Color.Black, 0f, Vector2.Zero, currentScreenScale, SpriteEffects.None, 1);
            resWidth.Draw(_spriteBatch);
            _spriteBatch.DrawString(smallFont, "Height: ", new Vector2(582*currentScreenScale.X, 325*currentScreenScale.Y), Color.Black, 0f, Vector2.Zero, currentScreenScale, SpriteEffects.None, 1);
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
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;


        }


    }
}