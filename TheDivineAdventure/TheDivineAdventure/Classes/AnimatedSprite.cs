using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheDivineAdventure
{
    class AnimatedSprite
    {
        private Vector2 pos, spriteRes;
        private Rectangle currentBox;
        private Texture2D sprite;
        private int curFrame, frames;
        private float extendFrame, scale;
        private Color tint;

        public AnimatedSprite(int width, int height, Texture2D spriteTex, int frames)
        {
            spriteRes.X = width;
            spriteRes.Y = height;
            sprite = spriteTex;
            currentBox = new Rectangle(218, 0, (int)spriteRes.X, (int)spriteRes.Y);
            curFrame = 0;
            extendFrame = 0;
            scale = 1;
            tint = Color.White;
            this.frames = frames;
        }

        public void Draw(SpriteBatch sb, Vector2 screenScale)
        {
            sb.Draw(sprite, pos, currentBox, tint, 0, Vector2.Zero, screenScale*scale, SpriteEffects.None, 0);
            //progress animation
            if (curFrame < frames)
            {
                currentBox = new Rectangle((int)spriteRes.X * curFrame, 0, (int)spriteRes.X, (int)spriteRes.Y);
                //slow sprite framerate
                if (extendFrame < 1)
                {
                    extendFrame += 0.4f;
                }
                else
                {
                    curFrame++;
                    extendFrame = 0f;
                }
            }
            else
            {
                currentBox = new Rectangle(0, 0, (int)spriteRes.X, (int)spriteRes.Y);
                curFrame = 0;
            }
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Color Tint
        {
            get { return tint; }
            set { tint = value; }
        }

        public int Frame
        {
            get { return curFrame; }
            set { curFrame = value; }
        }

    }
}
