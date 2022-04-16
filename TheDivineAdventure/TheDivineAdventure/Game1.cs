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


        // 2D Assets
        private SpriteFont BigFont;
        private Texture2D hudL1, hudL2, progIcon, healthBar, staminaBar, manaBar;
        private Rectangle healthBarRec, secondBarRec;
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
        private Matrix worldPlayer, worldEnemy, worldLevel;


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

            // Role Info
            playerRole = Player.ROLES[3];
            enemyRole = Enemy.ROLES[0];

            // Initialize game objects
            player = new Player(playerSounds, playerRole);
            camera = new Camera(GraphicsDevice, Vector3.Up, player);
            enemy = new Enemy(enemySounds, enemyRole);
            enemyList = new List<Enemy>();

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

            // Generate resource Bars rectangles
            healthBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth*0.099f / currentScreenScale.X),
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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            BigFont = Content.Load<SpriteFont>("BigFont");

            //set font color
            textGold = new Color(175, 127, 16);


            // Load sounds


            // Load 2D textures
            hudL1 = Content.Load<Texture2D>("TEX_HolyHUD_L1");
            hudL2 = Content.Load<Texture2D>("TEX_HolyHUD_L2");
            progIcon = Content.Load<Texture2D>("TEX_ProgressionIcon");
            healthBar = Content.Load<Texture2D>("TEX_HealthBar");
            manaBar = Content.Load<Texture2D>("TEX_ManaBar");
            staminaBar = Content.Load<Texture2D>("TEX_StaminaBar");


            // Load 3D models
            
            clericModel = Content.Load<Model>("MODEL_Cleric");
            demonModel = Content.Load<Model>("MODEL_Demon");
            level1Model = Content.Load<Model>("MODEL_Level1");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            camera.Update(gameTime, player);
            enemy.Update(gameTime);

            //update distance to boss
            if(player.Pos.Z>0 && player.Pos.Z<levelLength)
                travel = (player.Pos.Z * _graphics.PreferredBackBufferWidth*0.276f) / Math.Abs(levelLength);

            //test score
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                score += 3;

            // Basic kill player for (will improve once some UI is built up)
            if (player.Health <= 0)
            {
                player = new Player(playerSounds, playerRole);
                score = 0;
            }

            //DEBUG SCREEN RESOLUTION THINGS
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                _graphics.PreferredBackBufferWidth = 1920;
                _graphics.PreferredBackBufferHeight = 400;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                _graphics.PreferredBackBufferWidth = 960;
                _graphics.PreferredBackBufferHeight = 540;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                currentScreenScale = new Vector2(_graphics.PreferredBackBufferWidth / 1920f, _graphics.PreferredBackBufferHeight / 1080f);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Graphics device settings
            GraphicsDevice.Clear(new Color(40, 20, 20));


            // Render world
            worldLevel = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(new Vector3(0,0,-5));

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
            switch(player.role)
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


            // Render HUD
            drawHud();
            
            base.Draw(gameTime);
        }

        protected void drawHud()
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
    }
}
