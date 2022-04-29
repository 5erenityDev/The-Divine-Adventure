using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System.Diagnostics;

namespace TheDivineAdventure
{
    class WorldSprite
    {
        //essential
        private ContentManager Content;
        private Game1 parent;
        //animation info
        private bool animated;
        private Texture2D stillTex, animTex;
        private int currFrame, frames;
        private Vector2 spriteRes;
        private Rectangle currentBox;

        //render variables
        private Model frame;

        public WorldSprite(Texture2D tex, Game1 game, ContentManager cont)
        {
            //Assign content manager
            Content = cont;

            //Assign texture info
            animated = false;
            stillTex = tex;
            //Assign parent game 
            parent = game;

            //Assign model
            frame = Content.Load<Model>("MODEL_SpriteFrame");
        }

        public WorldSprite(Texture2D tex,int frameWidth, Game1 game, ContentManager cont)
        {
            //Assign content manager
            Content = cont;

            //Assign texture info
            stillTex = tex;
            animated = true;
            //Assign parent game 
            parent = game;

            //Assign model
            frame = Content.Load<Model>("MODEL_SpriteFrame");

            //set frame to 0
            currFrame = 0;


            spriteRes.X = frameWidth;
            spriteRes.Y = tex.Height;
            currentBox = new Rectangle(0, 0, (int)spriteRes.X, (int)spriteRes.Y);
            frames = tex.Width/frameWidth;
        }

        public void Draw(Matrix world, Matrix camera, Matrix projection)
        {
            parent.GraphicsDevice.BlendState = BlendState.Additive;
            if (animated)
            {
                //animate and assign animated texture
                DrawAnimTex();
                foreach (ModelMesh mesh in frame.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.Texture = animTex;
                        effect.TextureEnabled = true;
                    }
                }
            }
            else
            {
                //assign stilltexture
                foreach (BasicEffect effect in frame.Meshes[0].Effects)
                {
                    effect.Texture = stillTex;
                    effect.TextureEnabled = true;
                }
            }

            //draw a 2d sprite for the projectile
            frame.Draw(world, camera, projection);
            parent.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        private void DrawAnimTex()
        {

            animTex = new Texture2D(parent.GraphicsDevice, (int)spriteRes.X, (int)spriteRes.Y);
            int count = (int)(spriteRes.X * spriteRes.Y);
            Color[] data = new Color[count];
            stillTex.GetData<Color>(0,currentBox,data,0, count);
            animTex.SetData(data);

            //progress animation
            if (currFrame < frames)
            {
                currentBox = new Rectangle((int)spriteRes.X * currFrame, 0, (int)spriteRes.X, (int)spriteRes.Y);
                currFrame++;
            }
            else
            {
                currentBox = new Rectangle(0, 0, (int)spriteRes.X, (int)spriteRes.Y);
                currFrame = 0;
            }
        }
    }
}
