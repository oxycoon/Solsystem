using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Solsystem
{
    public class Star
    {
        private BasicEffect effect;

        // star matrixes
        private Matrix matScale, matRotate, matTranslate, matOrbitRotation, matOrbitTranslate;
        Matrix starWorld, starView, starProjection;

        // star attributes
        private float starOrbitY;


        private String starName;
        public String StarName
        {
            get { return starName; }
            set { starName = value; }
        }
        private Model starModel;
        public Model StarModel
        {
            get { return starModel; }
            set { starModel = value; }
        }

        private Vector3 starPosition;
        public Vector3 StarPosition
        {
            get { return starPosition; }
            set { starPosition = value; }
        }

        private float starScale;
        public float StarScale
        {
            get { return starScale; }
            set { starScale = value; }
        }

        private float starDistanceFromSun;
        public float StarDistanceFromSun
        {
            get { return starDistanceFromSun; }
            set { starDistanceFromSun = value; }
        }

        private float starSpeed;
        public float StarSpeed
        {
            get { return starSpeed; }
            set { starSpeed = value; }
        }

        // Rotation own axis
        private float starRotationY;
        public float StarRotationY
        {
            get { return starRotationY; }
            set { starRotationY = value; }
        }

        private float starRotationX;
        public float StarRotationX
        {
            get { return starRotationX; }
            set { starRotationX = value; }
        }

        private float starRotationZ;
        public float StarRotationZ
        {
            get { return starRotationZ; }
            set { starRotationZ = value; }
        }


        public Star()
        {

        }

        public Star(Matrix[] WVP, String name, Model model, Vector3 position, float speed, float scale, float distanceSun, float[] rotation)
        {
            this.starWorld = WVP[0];
            this.starView = WVP[1];
            this.starProjection = WVP[3];

            this.starName = name;
            this.starModel = model;
            this.starPosition = position;
            this.starSpeed = speed;
            this.starScale = scale;
            this.starDistanceFromSun = distanceSun;

            this.starRotationX = rotation[0];
            this.starRotationY = rotation[1];
            this.starRotationZ = rotation[2];
        }

        public void Drawstar(GameTime gameTime, Stack<Matrix> matrixStack)
        {
            Matrix _world = matrixStack.Peek();

            // Scaling matrix
            matScale = Matrix.CreateScale(starScale);

            // Translation matrix
            matTranslate = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);

            // Rotation matrix
            matRotate = Matrix.CreateRotationY(starRotationY);
            starRotationY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
            starRotationY = starRotationY % (float)(2 * Math.PI);

            // Creating the new world
            starWorld = matScale * matRotate * matTranslate;

            effect.World = starWorld;

            starModel.Draw(starWorld, starView, starProjection);
        }
    }
}
