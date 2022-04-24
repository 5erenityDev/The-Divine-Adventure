using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Windows;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    public class PlayScene : Scene
    {
        //capture travel distance
        float travel;
        //Camera
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

        public PlayScene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 game) : base(sb, graph, game)
        {

        }

        //initialize game objects and load level
        public override void Initialize()
        {
            base.Initialize();
            //hide cursor
            parent.showCursor = false; ;

            // Role Info
            playerRole = Player.ROLES[3];
            enemyRole = Enemy.ROLES[0];

            // Initialize game objects
            player = new Player(playerSounds, playerRole);
            camera = new Camera(parent.GraphicsDevice, Vector3.Up, player);
            enemy = new Enemy(enemySounds, enemyRole);
            enemyList = new List<Enemy>();

            // Generate resource Bars rectangles
            parent.healthBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth * 0.099f /parent.currentScreenScale.X),
                (int)Math.Round(_graphics.PreferredBackBufferHeight * 0.044f /parent.currentScreenScale.Y),
                (int)Math.Round(.201f * _graphics.PreferredBackBufferWidth /parent.currentScreenScale.X),
                (int)Math.Round(.05f * _graphics.PreferredBackBufferHeight /parent.currentScreenScale.Y));
            parent.secondBarRec = new Rectangle(
                (int)Math.Round(_graphics.PreferredBackBufferWidth * 0.088f /parent.currentScreenScale.X),
                (int)Math.Round(_graphics.PreferredBackBufferHeight * 0.099f /parent.currentScreenScale.Y),
                (int)Math.Round(.201f * _graphics.PreferredBackBufferWidth /parent.currentScreenScale.X),
                (int)Math.Round(.05f * _graphics.PreferredBackBufferHeight /parent.currentScreenScale.Y));

            // Initialize Distance to Boss(kept as a variable in case we have multiple level length)
            parent.levelLength = 3500;
            travel = 0;

            //set score to 0
            score = 0;
        }

        //function to do updates when player is playing in level.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //pause game if window is tabbed out of
            if (!parent.IsActive)
            {
                parent.currentScene = 6;
                parent.pauseScene.Initialize();
                return;
            }
            //pause game on pressing esc
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && parent.lastKeyboard.IsKeyUp(Keys.Escape))
            {
                parent.currentScene = 6;
                parent.pauseScene.Initialize();
                return;
            }

            player.Update(gameTime, camera);
            camera.Update(gameTime, player);
            enemy.Update(gameTime, player);

            foreach (Attack p in player.projList)
            {
                p.Update(gameTime);
            }


            foreach (Attack p in enemy.projList)
            {
                p.Update(gameTime);
            }

            //update distance to boss
            if (player.Pos.Z > 0 && player.Pos.Z < parent.levelLength)
                travel = (player.Pos.Z * _graphics.PreferredBackBufferWidth * 0.276f) / Math.Abs(parent.levelLength);

            //test score
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                score += 3;

            // Basic kill player for (will improve once some UI is built up)
            if (player.Health <= 0)
            {
                player = new Player(playerSounds, playerRole);
                score = 0;
                travel = 0;
            }
        }

        //function to do draw when player is playing in level.
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //draw Skybox
            parent.sky.Draw(camera.View, camera.Proj, camera.Pos, gameTime);
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // Render world
            worldLevel = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(new Vector3(0, 0, -5));

            parent.level1Model.Draw(worldLevel, camera.View, camera.Proj);

            // Render single enemy
            // THIS IS TEMPORARY
            worldEnemy = Matrix.CreateScale(1f) *
                        Matrix.CreateRotationY(MathHelper.ToRadians(180f)) *
                        Matrix.CreateTranslation(enemy.Pos);
            switch (enemy.role)
            {
                case "DEMON":
                    parent.demonModel.Draw(worldEnemy, camera.View, camera.Proj);
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
                    parent.clericModel.Draw(worldEnemy, camera.View, camera.Proj);
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
                    parent.demonModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
                case "ROGUE":
                    //rogueModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
                case "MAGE":
                    //mageModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
                case "CLERIC":
                    parent.clericModel.Draw(worldPlayer, camera.View, camera.Proj);
                    break;
            }

            // Render player bullets
            foreach (Attack p in player.projList)
            {
                worldProj = Matrix.CreateScale(1f) *
                        Matrix.CreateTranslation(p.Pos);
                if (p.IsMelee)
                {
                    parent.playerMelModel.Draw(worldProj, camera.View, camera.Proj);
                }
                else
                {
                    parent.playerProjModel.Draw(worldProj, camera.View, camera.Proj);
                }
                
            }

            // Render enemy bullets
            foreach (Attack p in enemy.projList)
            {
                worldProj = Matrix.CreateScale(1f) *
                    Matrix.CreateTranslation(p.Pos);
                if (p.IsMelee)
                {
                    parent.enemyMelModel.Draw(worldProj, camera.View, camera.Proj);
                }
                else
                {
                    parent.enemyProjModel.Draw(worldProj, camera.View, camera.Proj);
                }
            }

            // ** Render HUD **
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _spriteBatch.Begin();
            _spriteBatch.Draw(parent.hudL1, Vector2.Zero, null, Color.White, 0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
            //progessIcon
            _spriteBatch.Draw(parent.progIcon,
                new Vector2(_graphics.PreferredBackBufferWidth * 0.348f + travel, _graphics.PreferredBackBufferHeight * 0.873f),
                null, Color.White, 0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
            //Score
            _spriteBatch.DrawString(parent.BigFont, score.ToString(),
                new Vector2(_graphics.PreferredBackBufferWidth * 0.498f - (parent.BigFont.MeasureString(score.ToString()) * .5f *parent.currentScreenScale).X, _graphics.PreferredBackBufferHeight * -0.01f),
                parent.textGold, 0f, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 1);
            //resource bars
            _spriteBatch.Draw(parent.healthBar,
                player.resourceBarUpdate(true, parent.healthBarRec,
                new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),parent.currentScreenScale), Color.White);
            if (player.IsCaster)
                _spriteBatch.Draw(parent.manaBar,
                    player.resourceBarUpdate(false, parent.secondBarRec,
                    new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),parent.currentScreenScale), Color.White);
            else
                _spriteBatch.Draw(parent.staminaBar,
                    player.resourceBarUpdate(false, parent.secondBarRec,
                    new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),parent.currentScreenScale), Color.White);
            //topHUD layer
            _spriteBatch.Draw(parent.hudL2, Vector2.Zero, null, Color.White, 0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 1);
            _spriteBatch.End();
            parent.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            parent.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
}
