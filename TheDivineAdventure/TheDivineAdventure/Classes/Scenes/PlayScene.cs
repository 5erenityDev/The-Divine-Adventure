using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Windows;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    public class PlayScene : Scene
    {
        //2d Textures
        private Texture2D hudL1, hudL2, progIcon, healthBar, staminaBar, manaBar, ClericIcon;
        private Skybox sky;

        // 3D Assets
        public Model clericModel;
        public Model demonModel;
        public Model level1Model;
        public Model playerProjModel, enemyProjModel;
        public Model playerMelModel, enemyMelModel;
        /*
        //20 Planned Character Models in total
        //8 Planned World Models in total
        private Model   warriorModel, rogueModel, mageModel;
        private Model   houndModel, impModel, goblinModel, 
                        ogreModel, gargoyModel, skeleModel;
        private Model   luciModel, leviModel, satanModel, belphModel, 
                        mammonModel, beelzModel, asmoModel, angelModel;
        private model   level2Model, level3Model, level4Model,
                        level5Model, level6Model, level7Model, level8Model;
        */

        //Capture travel distance
        float travel;
        //Camera
        private Camera camera;

        // Player
        private Player player;
        private List<SoundEffect> playerSounds = new List<SoundEffect>();
        private string playerRole;
        public static int score;
        private Texture2D playerIcon;

        // Enemy
        private List<Enemy> enemyList;
        private List<SoundEffect> enemySounds = new List<SoundEffect>();
        private string enemyRole;
        private float enemyTimer, enemyTimerMax;

        // Matrices
        private Matrix worldPlayer, worldEnemy, worldProj, worldLevel;

        public PlayScene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 game, ContentManager cont) : base(sb, graph, game, cont)
        {

        }

        //initialize game objects and load level
        public override void Initialize()
        {
            base.Initialize();
            LoadContent();
            //hide cursor
            parent.showCursor = false; ;

            // Role Info
            playerRole = Player.ROLES[3];

            // Timer Info
            enemyTimerMax = 3f;
            enemyTimer = enemyTimerMax;

            // Initialize game objects
            player = new Player(playerSounds, playerRole);
            camera = new Camera(parent.GraphicsDevice, Vector3.Up, player);
            enemyList = new List<Enemy>();

            // Generate resource Bars rectangles
            parent.healthBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth * 0.099f / parent.currentScreenScale.X),
                (int)Math.Round(_graphics.PreferredBackBufferHeight * 0.044f / parent.currentScreenScale.Y),
                (int)Math.Round(.201f * _graphics.PreferredBackBufferWidth / parent.currentScreenScale.X),
                (int)Math.Round(.05f * _graphics.PreferredBackBufferHeight / parent.currentScreenScale.Y));
            parent.secondBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth * 0.088f / parent.currentScreenScale.X),
                (int)Math.Round(_graphics.PreferredBackBufferHeight * 0.099f / parent.currentScreenScale.Y),
                (int)Math.Round(.201f * _graphics.PreferredBackBufferWidth / parent.currentScreenScale.X),
                (int)Math.Round(.05f * _graphics.PreferredBackBufferHeight / parent.currentScreenScale.Y));

            // Initialize Distance to Boss(kept as a variable in case we have multiple level length)
            parent.levelLength = 3500;
            travel = 0;

            //set score to 0
            score = 0;

            //Select Role Icon
            switch (player.role)
            {
                case "WARRIOR":
                    playerIcon = ClericIcon;
                    break;
                case "ROGUE":
                    playerIcon = ClericIcon;
                    break;
                case "MAGE":
                    playerIcon = ClericIcon;
                    break;
                default:
                    playerIcon = ClericIcon;
                    break;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //load 2d textures
            hudL1 = Content.Load<Texture2D>("TEX_HolyHUD_L1");
            hudL2 = Content.Load<Texture2D>("TEX_HolyHUD_L2");
            progIcon = Content.Load<Texture2D>("TEX_ProgressionIcon");
            healthBar = Content.Load<Texture2D>("TEX_HealthBar");
            manaBar = Content.Load<Texture2D>("TEX_ManaBar");
            staminaBar = Content.Load<Texture2D>("TEX_StaminaBar");
            ClericIcon = Content.Load<Texture2D>("TEX_Cleric_Icon");

            //load skybox
            sky = new Skybox("TEX_SkyboxLevel1", Content);

            // Load 3D models
            clericModel = Content.Load<Model>("MODEL_Cleric");
            demonModel = Content.Load<Model>("MODEL_Demon");
            level1Model = Content.Load<Model>("MODEL_Level1");
            playerProjModel = Content.Load<Model>("MODEL_PlayerProjectile");
            enemyProjModel = Content.Load<Model>("MODEL_EnemyProjectile");
            playerMelModel = Content.Load<Model>("MODEL_PlayerMelee");
            enemyMelModel = Content.Load<Model>("MODEL_EnemyMelee");
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

        //function to do updates when player is playing in level.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //pause game if window is tabbed out of
            if (!parent.IsActive)
            {
                parent.currentScene = "PAUSE";
                parent.pauseScene.Initialize();
                return;
            }
            //pause game on pressing esc
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && parent.lastKeyboard.IsKeyUp(Keys.Escape))
            {
                parent.currentScene = "PAUSE";
                parent.pauseScene.Initialize();
                return;
            }

            player.Update(deltaTime, camera);
            camera.Update(deltaTime, player);

            // Spawn enemies
            enemyTimer -= deltaTime;
            if (enemyTimer < 0f && enemyList.Count < 20)
            {
                enemyTimer = enemyTimerMax;
                enemyRole = Enemy.ROLES[0];
                enemyList.Add(new Enemy(enemySounds, enemyRole, player.Pos));
                if (enemyTimerMax > 2f)
                    enemyTimerMax -= 0.05f;
            }

            foreach (Enemy e in enemyList)
            {
                if (player.Pos.Z < parent.levelLength)
                    e.Update(deltaTime, player);
                foreach (Attack p in e.projList)
                {
                    p.Update(deltaTime, player);
                }
                if (e.TimeToDestroy)
                {
                    enemyList.Remove(e);
                    break;
                }
            }

            foreach (Attack p in player.projList)
            {
                p.Update(deltaTime, enemyList);
            }

            //update distance to boss
            if (player.Pos.Z > 0 && player.Pos.Z < parent.levelLength)
                travel = (player.Pos.Z * 434 * parent.currentScreenScale.X) / Math.Abs(parent.levelLength);

            //DEBUG: test score
            //if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
            //    score += 3;

            // Basic kill player for (will improve once some UI is built up)
            if (player.Health <= 0)
            {
                player = new Player(playerSounds, playerRole);
                score = 0;
                travel = 0;
                enemyList.Clear();
                enemyTimerMax = 3f;
                enemyTimer = enemyTimerMax;
            }
        }

        //function to do draw when player is playing in level.
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //draw Skybox
            sky.Draw(camera.View, camera.Proj, player.Pos, gameTime);
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // Render world
            worldLevel = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(new Vector3(0, 0, -5));

            level1Model.Draw(worldLevel, camera.View, camera.Proj);

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
            foreach (Attack p in player.projList)
            {
                worldProj = Matrix.CreateScale(1f) *
                        Matrix.CreateTranslation(p.Pos);
                if (p.IsMelee)
                {
                    playerMelModel.Draw(worldProj, camera.View, camera.Proj);
                }
                else
                {
                    playerProjModel.Draw(worldProj, camera.View, camera.Proj);
                }

            }

            // Render enemies
            foreach (Enemy e in enemyList)
            {
                // THIS IS TEMPORARY
                worldEnemy = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(e.Pos);
                switch (e.Role)
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

                // Render enemy bullets
                foreach (Attack p in e.projList)
                {
                    worldProj = Matrix.CreateScale(1f) *
                        Matrix.CreateTranslation(p.Pos);
                    if (p.IsMelee)
                    {
                        enemyMelModel.Draw(worldProj, camera.View, camera.Proj);
                    }
                    else
                    {
                        enemyProjModel.Draw(worldProj, camera.View, camera.Proj);
                    }
                }
            }

            // ** Render HUD **
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _spriteBatch.Begin();
            _spriteBatch.Draw(hudL1, Vector2.Zero, null, Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);
            //progessIcon
            _spriteBatch.Draw(progIcon,
                new Vector2(714 * parent.currentScreenScale.X + travel, 958 * parent.currentScreenScale.Y),
                null, Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);
            //Score
            _spriteBatch.DrawString(parent.BigFont, score.ToString(),
                new Vector2(_graphics.PreferredBackBufferWidth * 0.498f - (parent.BigFont.MeasureString(score.ToString()) * .5f * parent.currentScreenScale).X, _graphics.PreferredBackBufferHeight * -0.01f),
                parent.textGold, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
            //Resource bars
            _spriteBatch.Draw(healthBar,
                player.resourceBarUpdate(true, parent.healthBarRec,
                new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                parent.currentScreenScale), Color.White);
            if (player.IsCaster)
                _spriteBatch.Draw(manaBar,
                    player.resourceBarUpdate(false, parent.secondBarRec,
                    new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                    parent.currentScreenScale), Color.White);
            else
                _spriteBatch.Draw(staminaBar,
                    player.resourceBarUpdate(false, parent.secondBarRec,
                    new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)
                    , parent.currentScreenScale), Color.White);
            //topHUD layer
            _spriteBatch.Draw(hudL2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
            //draw player Icon
            _spriteBatch.Draw(playerIcon, new Vector2(49, 19) * parent.currentScreenScale, null,
                Color.White, 0, Vector2.Zero, 0.071f * parent.currentScreenScale, SpriteEffects.None, 1);
            FadeIn(0.01f);
            _spriteBatch.End();
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
}
