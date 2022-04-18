using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TheDivineAdventure
{
    class Button
    {
        private Vector2 pos, size, scale, center;
        private MouseState mouseState;
        private Vector2 mousePos;
        private Texture2D? texture;
        public Button(Texture2D? tex, Vector2 position, Vector2 size, Vector2 screenScale)
        {
            pos.X = position.X * screenScale.X;
            pos.Y = position.Y * screenScale.Y;
            this.size.X = size.X * screenScale.X;
            this.size.Y = size.Y * screenScale.Y;
            scale = screenScale;
            center = new Vector2(pos.X + (this.size.X / 2), pos.Y + (this.size.Y / 2));
            texture = tex;
        }

        public bool IsPressed()
        {
            mouseState = Mouse.GetState();
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            if (Math.Abs(center.X-mousePos.X ) < (size.X / 2f) && Math.Abs(center.Y - mousePos.Y) < (size.Y / 2f))
                return true;
            else
                return false;
        }

        public void DrawButton(SpriteBatch sb)
        {
            sb.Draw(texture, pos, new Rectangle(0,0, (int)size.X, (int)size.Y), Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);
        }
    }
}
