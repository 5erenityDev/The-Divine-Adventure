using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Camera
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constants
        private const float CAM_HEIGHT = 20f;
        private const float RENDER_DIST = 2000f;
        private const float FOV = 45f;

        // Movement
        private Vector3 pos, target, distFromPlayer;
        private Matrix view, proj, view_proj;
        private Vector3 up;



        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Camera(GraphicsDevice gpu, Vector3 UpDir)
        {
            distFromPlayer = new Vector3(0f, 30f, -70f);

            up = UpDir;
            pos = new Vector3(0f, 0f, 0f);
            target = Vector3.Zero;
            
            view = Matrix.CreateLookAt(pos, target, up);
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), 
                            gpu.Viewport.AspectRatio, 
                            1.0f, 
                            RENDER_DIST);
            view_proj = view * proj;
        }

        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime, Player player)
        {
            pos = player.Pos + distFromPlayer;
            target = player.Pos;
            view = Matrix.CreateLookAt(pos, target, up);
        }

        ////////////////////
        ///GETTER/SETTERS///
        ////////////////////
        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        public Matrix Proj
        {
            get { return proj; }
            set { proj = value; }
        }
    }
}
