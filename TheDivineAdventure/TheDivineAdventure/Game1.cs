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

        // 3D Assets
        private Model   clericModel;
        private Model   demonModel;
        private Model   level1Model;
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

        // 2D Assets
        private SpriteFont gameFont;
        private Texture2D cursor, titleLogo;

        // Songs
        private Song gameTheme;

        // Player
        private Player player;
        private List<SoundEffect> playerSounds = new List<SoundEffect>();

        // Enemy
        private Enemy enemy;
        private List<Enemy> enemyList;
        private List<SoundEffect> enemySounds = new List<SoundEffect>();

        // Camera
        private Vector3 camTarget;
        private Vector3 camPosition, camDistFromPlayer;
        private float fov;
        private float renderDistance;

        // Matrices
        private Matrix worldPlayer, worldEnemy, worldLevel;
        private Matrix view, proj;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Initialize game objects
            player = new Player(playerSounds);
            enemy = new Enemy(enemySounds);
            enemyList = new List<Enemy>();

            // Camera
            camDistFromPlayer = new Vector3(0f, 30f, -70f);
            camPosition = player.Pos + camDistFromPlayer;
            camTarget = player.Pos;
            fov = 45f;
            renderDistance = 2000f;

            view = Matrix.CreateLookAt(camPosition,
                                        camTarget,
                                        Vector3.Up);
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov),
                                                        _graphics.GraphicsDevice.Viewport.AspectRatio,
                                                        1.0f,
                                                        renderDistance);


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts

            // Load sounds

            // Load 2D textures

            // Load 3D models
            clericModel = Content.Load<Model>("MODEL_Cleric");
            demonModel = Content.Load<Model>("MODEL_Demon");
            level1Model = Content.Load<Model>("MODEL_Level1");
            /*
            warriorModel = Content.Load<Model>("MODEL_Warrior");
            rogueModel = Content.Load<Model>("MODEL_Rogue");
            mageModel = Content.Load<Model>("MODEL_Mage");
            impModel  = Content.Load<Model>("MODEL_Imp");
            goblinModel = Content.Load<Model>("MODEL_Goblin");
            ogreModel = Content.Load<Model>("MODEL_Ogre");
            gargoyModel = Content.Load<Model>("MODEL_Gargoyle");
            skeleModel = Content.Load<Model>("MODEL_Skeleton");
            baelModel = Content.Load<Model>("MODEL_Bael");
            agaresModel = Content.Load<Model>("MODEL_Agares");
            vassaModel = Content.Load<Model>("MODEL_Vassago");
            samiModel = Content.Load<Model>("MODEL_Samigina");
            marbasModel = Content.Load<Model>("MODEL_Marbas");
            valeModel = Content.Load<Model>("MODEL_Valefor");
            amonModel = Content.Load<Model>("MODEL_Amon");
            barbaModel = Content.Load<Model>("MODEL_Barbatos");
            luciModel = Content.Load<Model>("MODEL_Lucifer");
            leviModel = Content.Load<Model>("MODEL_Leviathan");
            satanModel = Content.Load<Model>("MODEL_Satan");
            belphModel = Content.Load<Model>("MODEL_Belphegor");
            mammonModel = Content.Load<Model>("MODEL_Mammon");
            beelzModel = Content.Load<Model>("MODEL_Beelzebub");
            asmoModel = Content.Load<Model>("MODEL_Asmodeus");
            angelModel = Content.Load<Model>("MODEL_Angel");
            */
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            enemy.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Graphics device settings
            GraphicsDevice.Clear(new Color(40, 20, 20));

            // Camera
            camPosition = player.Pos + camDistFromPlayer;
            camTarget = player.Pos;
            view = Matrix.CreateLookAt(camPosition,
                                        camTarget,
                                        Vector3.Up);

            // Render world
            worldLevel = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(new Vector3(0,0,-5));

            level1Model.Draw(worldLevel, view, proj);

            // Render single enemy
            // THIS IS TEMPORARY
            worldEnemy = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(enemy.Pos);
            demonModel.Draw(worldEnemy, view, proj);

            // Render player
            worldPlayer = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(0f)) *
                        Matrix.CreateTranslation(player.Pos);
            clericModel.Draw(worldPlayer, view, proj);

            base.Draw(gameTime);
        }
    }
}
