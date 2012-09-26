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
    public class Planet
    {
        private BasicEffect effect;

        // Planet matrixes
        private Matrix matScale, matRotate, matTranslate, matOrbitRotation, matOrbitTranslate;
        Matrix planetWorld, planetView, planetProjection;

        // Planet attributes
        private float planetOrbitY;


        private String planetName;
        public String PlanetName
        {
            get { return planetName; }
            set { planetName = value; }
        }
        private Model planetModel;
        public Model PlanetModel
        {
            get { return planetModel; }
            set { planetModel = value; }
        }

        private Vector3 planetPosition;
        public Vector3 PlanetPosition
        {
            get { return planetPosition; }
            set { planetPosition = value; }
        }

        private float planetScale;
        public float PlanetScale
        {
            get { return planetScale; }
            set { planetScale = value; }
        }

        private float planetDistanceFromSun;
        public float PlanetDistanceFromSun
        {
            get { return planetDistanceFromSun; }
            set { planetDistanceFromSun = value; }
        }

        private float planetSpeed;
        public float PlanetSpeed
        {
            get { return planetSpeed; }
            set { planetSpeed = value; }
        }

        // Rotation own axis
        private float planetRotationY;
        public float PlanetRotationY
        {
            get { return planetRotationY; }
            set { planetRotationY = value; }
        }

        private float planetRotationX;
        public float PlanetRotationX
        {
            get { return planetRotationX; }
            set { planetRotationX = value; }
        }

        private float planetRotationZ;
        public float PlanetRotationZ
        {
            get { return planetRotationZ; }
            set { planetRotationZ = value; }
        }


        public Planet()
        {

        }

        public Planet(Matrix[] WVP, String name, Model model, Vector3 position, float speed, float scale, float distanceSun, float[] rotation)
        {
            this.planetWorld = WVP[0];
            this.planetView = WVP[1];
            this.planetProjection = WVP[3];

            this.planetName = name;
            this.planetModel = model;
            this.planetPosition = position;
            this.planetSpeed = speed;
            this.planetScale = scale;
            this.planetDistanceFromSun = distanceSun;

            this.planetRotationX = rotation[0];
            this.planetRotationY = rotation[1];
            this.planetRotationZ = rotation[2];
        }

        public void DrawPlanet(GameTime gameTime, Stack<Matrix> matrixStack)
        {
            Matrix _world = matrixStack.Peek();

            // Scaling matrix
            matScale = Matrix.CreateScale(planetScale);

            // Translation matrix
            matTranslate = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);

            // Rotation matrix
            matRotate = Matrix.CreateRotationY(planetRotationY);
            planetRotationY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
            planetRotationY = planetRotationY % (float)(2 * Math.PI);

            // Orbit/Rotation matrix
            matOrbitTranslate = Matrix.CreateTranslation(planetDistanceFromSun, 0.0f, 0.0f);
            planetOrbitY += (planetSpeed / 60) * (float)gameTime.ElapsedGameTime.Milliseconds / 50.0f;
            planetOrbitY = planetOrbitY % (float)(2 * Math.PI);
            matOrbitRotation = Matrix.CreateRotationY(planetOrbitY);
            
            // Creating the new world
            planetWorld = matScale * matRotate * matOrbitTranslate * matOrbitRotation * _world;

            effect.World = planetWorld;

            planetModel.Draw(planetWorld, planetView, planetProjection);
        }
    }
}
