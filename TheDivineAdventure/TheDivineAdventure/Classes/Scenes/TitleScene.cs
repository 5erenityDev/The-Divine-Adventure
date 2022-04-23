using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheDivineAdventure
{
    public class TitleScene : Scene
    {
        private Button titleStartGame, titleScoreboard, titleSettings, titleCredits, titleQuitGame;
        private AnimatedSprite[] titleDemons, titleEmbers;
        private bool glowState;
        private int glowRef;

        public TitleScene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 game) : base(sb, graph, game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            //show custom cursor
            parent.showCursor = true;
            //create background demons
            titleDemons = new AnimatedSprite[rand.Next(40, 180)];
            for (int i = 0; i < titleDemons.Length; i++)
            {
                titleDemons[i] = new AnimatedSprite(109, 108, parent.distantDemonSheet, 7);
                titleDemons[i].Pos = new Vector2((-1 * rand.Next(-1920, 1000)) *parent.currentScreenScale.X, rand.Next(10, 600) *parent.currentScreenScale.Y);
                titleDemons[i].Scale = 1 - (rand.Next(50) / 100f);
                titleDemons[i].Tint = new Color(Color.White, titleDemons[i].Scale);
                titleDemons[i].Frame = rand.Next(7);
            }
            //create embers
            titleEmbers = new AnimatedSprite[10];
            for (int i = 0; i < titleEmbers.Length; i++)
            {
                titleEmbers[i] = new AnimatedSprite(174, 346, parent.emberSheet01, 6);
                titleEmbers[i].Pos = new Vector2(rand.Next(1920) *parent.currentScreenScale.X, rand.Next(450, 750) *parent.currentScreenScale.Y);
                titleEmbers[i].Scale = 1 - (rand.Next(-200, 50) / 100f);
                titleEmbers[i].Frame = rand.Next(6);
            }
            glowRef = 0;
            //create menu buttons
            titleStartGame = new Button(new Vector2(247, 686), new Vector2(366, 60),parent.currentScreenScale);
            titleScoreboard = new Button(new Vector2(247, 750), new Vector2(366, 60),parent.currentScreenScale);
            titleSettings = new Button(new Vector2(247, 812), new Vector2(366, 60),parent.currentScreenScale);
            titleCredits = new Button(new Vector2(247, 870), new Vector2(366, 60),parent.currentScreenScale);
            titleQuitGame = new Button(new Vector2(247, 928), new Vector2(366, 60),parent.currentScreenScale);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //get mouse clocks and check buttons
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (titleStartGame.IsPressed())
                {
                    parent.currentScene = 5;
                    parent.ReloadContent();
                    parent.playScene.Initialize();
                    return;
                }
                if (titleScoreboard.IsPressed())
                {
                    //currentScene = (1);
                    return;
                }
                if (titleSettings.IsPressed())
                {
                    parent.lastScene = 0;
                    parent.currentScene = 4;
                    parent.settingsScene.Initialize();
                    return;
                }
                if (titleCredits.IsPressed())
                {
                    parent.currentScene = 7;
                    parent.creditsScene.Initialize();
                    return;
                }
                if (titleQuitGame.IsPressed())
                {
                    parent.Exit();
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.Draw(parent.titleScreenBack, Vector2.Zero, null, Color.White, 0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
            //randomly draw lightning
            if (rand.Next(50) > 48)
            {
                switch (rand.Next(3))
                {
                    case 1:
                        _spriteBatch.Draw(parent.titleLightning01, Vector2.Zero, null, new Color(Color.White, 1 - (rand.Next(50) / 100f)),
                            0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
                        break;
                    case 2:
                        _spriteBatch.Draw(parent.titleLightning02, Vector2.Zero, null, new Color(Color.White, 1 - (rand.Next(50) / 100f)),
                            0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
                        break;
                    case 3:
                        _spriteBatch.Draw(parent.titleLightning03, Vector2.Zero, null, new Color(Color.White, 1 - (rand.Next(50) / 100f)),
                            0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
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
                    demon.Pos = new Vector2(-1 * rand.Next(1500) *parent.currentScreenScale.X, (50 + rand.Next(500)) *parent.currentScreenScale.Y);
                    demon.Scale = 1 - (rand.Next(50) / 100f);
                    demon.Tint = new Color(Color.White, demon.Scale);
                }
                else
                {
                    demon.Pos = new Vector2(demon.Pos.X + (demon.Scale * 10 *parent.currentScreenScale.X), demon.Pos.Y);
                }
                demon.Draw(_spriteBatch,parent.currentScreenScale);
            }
            //draw embers
            foreach (AnimatedSprite ember in titleEmbers)
            {
                if (ember.Frame == 0)
                {
                    ember.Pos = new Vector2((rand.Next(1920)) *parent.currentScreenScale.X, rand.Next(450, 750) *parent.currentScreenScale.Y);
                    ember.Scale = 1 - (rand.Next(-200, 50) / 100f);
                }
                ember.Draw(_spriteBatch,parent.currentScreenScale);
            }
            //draw title foreground
            _spriteBatch.Draw(parent.TitleScreenFront, Vector2.Zero, null, Color.White, 0, Vector2.Zero,parent.currentScreenScale, SpriteEffects.None, 0);
            //draw lava glow
            if (glowState)
            {
                _spriteBatch.Draw(parent.titleLava, Vector2.Zero, null, new Color(Color.White, glowRef), 0, Vector2.Zero,
                   parent.currentScreenScale, SpriteEffects.None, 1);
                glowRef += 5;
            }
            else
            {
                _spriteBatch.Draw(parent.titleLava, Vector2.Zero, null, new Color(Color.White, glowRef), 0, Vector2.Zero,
                   parent.currentScreenScale, SpriteEffects.None, 1);
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
    }
}
