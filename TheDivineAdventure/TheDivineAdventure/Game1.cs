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
            "SCOREBOARD", "SETTINGS", "PLAYING", "DEATH_SCREEN", "CREDITS"};
        public int currentScene;
        MouseState mouseState;

        // 2D Assets
        private SpriteFont BigFont;
        private Texture2D hudL1, hudL2, progIcon, healthBar, staminaBar, manaBar, titleScreenBack, TitleScreenFront,
            distantDemonSheet, titleLightning01, titleLightning02, titleLightning03, emberSheet01;
        private Rectangle healthBarRec, secondBarRec;
        private AnimatedSprite[] titleDemons, titleEmbers;
        private Button titleStartGame, titleLevelSelect, titleSettings, titleCredits, titleQuitGame;


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
            IsMouseVisible = true;
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
            _graphics.ApplyChanges();

            // Set screen scale to determine size of UI
            currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);

            //initialize title menu
            InitializeTitleScreen();

            //Start at either the title screen or in gameplay
            //Regular gameplay (e.g. start at title screen)
            currentScene = 0;

            //DEBUG uncomment to start in gameplay
            //InitializeLevel();
            //currentScene = 5;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            BigFont = Content.Load<SpriteFont>("BigFont");

            //set font color
            textGold = new Color(175, 127, 16);


            // Load sounds


            // Load 2D textures

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
                return;
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
            //Update based on current scene
            //game info was moved to the PlayingScene function below
            switch (currentScene)
            {
                case 0:
                    UpdateTitleScreen(gameTime);
                    break;
                case 5:
                    UpdatePlayingScene(gameTime);
                    break;
                default:
                    UpdateTitleScreen(gameTime);
                    break;                    
            }

            //DEBUG SCREEN RESOLUTION THINGS
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                _graphics.PreferredBackBufferWidth = 1920;
                _graphics.PreferredBackBufferHeight = 400;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                InitializeTitleScreen();
                currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                _graphics.PreferredBackBufferWidth = 960;
                _graphics.PreferredBackBufferHeight = 540;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
                InitializeTitleScreen();
            }

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
                case 5:
                    DrawPlayingScene(gameTime);
                    break;
                default:
                    DrawTitleScreen(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }

        protected void DrawHud()
        {
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
            if(player.IsCaster)
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


        //initialize game objects and load level
        private void InitializePlayingScene()
        {
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
            levelLength = 2250;
        }

        //function to do updates when player is playing in level.
        private void UpdatePlayingScene(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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

            LoadContent();
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

            // Render HUD
            DrawHud();
        }


        //function to do updates when player is playing in level.
        private void InitializeTitleScreen()
        {
            titleDemons = new AnimatedSprite[rand.Next(3)+2];
            for(int i=0;i<titleDemons.Length;i++)
            {
                titleDemons[i] = new AnimatedSprite(109, 108, distantDemonSheet, 7);
                titleDemons[i].Pos = new Vector2((-1 * rand.Next(400)) * currentScreenScale.X, (50 + rand.Next(500)) * currentScreenScale.Y);
                titleDemons[i].Scale = 1 - (rand.Next(50) / 100f);
                titleDemons[i].Tint = new Color(Color.White, titleDemons[i].Scale);
            }
            titleEmbers = new AnimatedSprite[10];
            for (int i = 0; i < titleEmbers.Length; i++)
            {
                titleEmbers[i] = new AnimatedSprite(174, 346, emberSheet01, 6);
                titleEmbers[i].Pos = new Vector2(rand.Next(1920) * currentScreenScale.X, rand.Next(450,750) * currentScreenScale.Y);
                titleEmbers[i].Scale = 1 - (rand.Next(-200,50) / 100f);
                titleEmbers[i].Frame = rand.Next(6);
            }
            titleStartGame = new Button(null,new Vector2(247, 686), new Vector2(366, 60), currentScreenScale);
            titleLevelSelect = new Button(null, new Vector2(247, 750), new Vector2(366, 60), currentScreenScale);
            titleSettings = new Button(null, new Vector2(247, 812), new Vector2(366, 60), currentScreenScale);
            titleCredits = new Button(null, new Vector2(247, 870), new Vector2(366, 60), currentScreenScale);
            titleQuitGame = new Button(null, new Vector2(247, 928), new Vector2(366, 60), currentScreenScale);
        }

        //function to do updates when player is in the title Screen.
        private void UpdateTitleScreen(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (titleStartGame.IsPressed())
                {
                    UnloadContent();
                    InitializePlayingScene();
                    currentScene = 5;
                    return;
                }
                if (titleLevelSelect.IsPressed())
                {
                    //currentScene = (1);
                    return;
                }
                if (titleSettings.IsPressed())
                {
                    //currentScene = (4);
                    return;

                }
                if (titleCredits.IsPressed())
                {
                    //currentScene = (7);
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
            if(rand.Next(50) > 48)
            {
                switch (rand.Next(3))
                {
                    case 1:
                        _spriteBatch.Draw(titleLightning01, Vector2.Zero, null, new Color(Color.White, 1-(rand.Next(50) / 100f)),
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
            foreach(AnimatedSprite demon in titleDemons)
            {
                if (demon.Pos.X > _graphics.PreferredBackBufferWidth)
                {
                    demon.Pos = new Vector2(-1*rand.Next(1500)*currentScreenScale.X,(50+rand.Next(500))*currentScreenScale.Y);
                    demon.Scale = 1 - (rand.Next(50) / 100f);
                    demon.Tint = new Color(Color.White, demon.Scale);
                }
                else
                {
                    demon.Pos = new Vector2(demon.Pos.X + (demon.Scale*10*currentScreenScale.X), demon.Pos.Y);
                }
                demon.Draw(_spriteBatch, currentScreenScale);
            }
            foreach (AnimatedSprite ember in titleEmbers)
            {
                if (ember.Frame == 0)
                {
                    ember.Pos = new Vector2((rand.Next(1920)) * currentScreenScale.X, rand.Next(450, 750) * currentScreenScale.Y);
                    ember.Scale = 1 - (rand.Next(-200, 50) / 100f);
                }
                ember.Draw(_spriteBatch, currentScreenScale);
            }
            _spriteBatch.Draw(TitleScreenFront, Vector2.Zero, null, Color.White, 0, Vector2.Zero, currentScreenScale, SpriteEffects.None, 0);
            _spriteBatch.End();
        }


    }
}
