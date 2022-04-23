using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheDivineAdventure
{
    class Camera
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Essential
        GraphicsDevice gpu;

        // Constants
        private const float CAM_HEIGHT = 15f;
        private const float RENDER_DIST = 6000f;
        private const float FOV = 45f;

        // Vectors
        private Vector3 pos;
        private Vector3 cameraRotation;
        private Vector3 cameraLookAt;

        private Vector3 distFromPlayer;

        // Rotation Speed
        private float rotSpeed;

        // Mouse
        private Vector3 mouseRotationBuffer;
        private MouseState curMouseState;
        private MouseState prevMouseState;



        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Camera(GraphicsDevice graphicsDevice, Vector3 rotation, Player player)
        {
            // Set gpu
            gpu = graphicsDevice;

            // Set camera data
            rotSpeed = 3f;
            distFromPlayer = new Vector3(0f, CAM_HEIGHT, -60f);
            Proj = Matrix.CreatePerspectiveFieldOfView(
                                MathHelper.ToRadians(FOV),
                                gpu.Viewport.AspectRatio,
                                0.05f,
                                RENDER_DIST);

            // Set mouseState
            prevMouseState = Mouse.GetState();
        }



        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime, Player player)
        {
            // Variables
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            curMouseState = Mouse.GetState();

            //Move camera with player
            Follow(player);

            // Handle mouse movement
            float deltaX;
            float deltaY;

            // Only handle mouse movement code if the mouse has moved (the state has changed)
            if (curMouseState != prevMouseState)
            {
                // Cache mouse location
                deltaX = curMouseState.X - (gpu.Viewport.Width / 2);
                deltaY = curMouseState.Y - (gpu.Viewport.Height / 2);

                // Creates rotation
                mouseRotationBuffer.X -= (rotSpeed / 100) * deltaX * dt;
                mouseRotationBuffer.Y -= (rotSpeed / 100) * deltaY * dt;

                // Limits rotation
                if (mouseRotationBuffer.X > .50f)
                    mouseRotationBuffer.X = .50f;
                else if (mouseRotationBuffer.X < -.50f)
                    mouseRotationBuffer.X = -.50f;
                if (mouseRotationBuffer.Y > .30f)
                    mouseRotationBuffer.Y = .30f;
                else if (mouseRotationBuffer.Y < -.30f)
                    mouseRotationBuffer.Y = -.30f;

                // Applies the rotation (we are only rotating on the X and Y axes)
                Rot = new Vector3(-mouseRotationBuffer.Y, mouseRotationBuffer.X, 0);

                // Reset delta's after calculation
                deltaX = 0;
                deltaY = 0;

            }

            // Set mouse position to the center of the screen
            Mouse.SetPosition(gpu.Viewport.Width / 2, gpu.Viewport.Height / 2);

            prevMouseState = curMouseState;
        }

        // Set camera's position to follow the player
        private void Follow(Player player)
        {
            Pos = player.Pos + distFromPlayer;
        }

        // Update the lookAt vector
        private void UpdateLookAt()
        {
            // Build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X)
                                    * Matrix.CreateRotationY(cameraRotation.Y);

            // Build a lookAtOffset vector
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);

            // Update the camera's lookAt vector
            cameraLookAt = pos + lookAtOffset;
        }



        ////////////////////
        ///GETTER/SETTERS///
        ////////////////////
        public Matrix Proj
        {
            get;
            protected set;
        }

        public Matrix View
        {
            get { return Matrix.CreateLookAt(pos, cameraLookAt, Vector3.Up); }
        }

        public Vector3 LookAt
        {
            get { return new Vector3(View.M31, -View.M32, -View.M33) * new Vector3(1000, 1000, 1000); }
        }

        public Vector3 Pos
        {
            get { return pos; }
            set
            {
                pos = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rot
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
                UpdateLookAt();
            }
        }
    }
}
