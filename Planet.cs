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
        // Planet matrixes
        private Matrix matScale, matRotate, matTranslate;
        Matrix planetWorld, planetView, planetProjection;

        // Planet attributes
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

        private double planetDistanceFromSun;
        public double PlanetDistanceFromSun
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

        // Rotation/Orbit parent axis
        private float planetSunOrbitRotationX;
        public float PlanetSunOrbitRotationX
        {
            get { return planetSunOrbitRotationX; }
            set { planetSunOrbitRotationX = value; }
        }

        private float planetSunOrbitRotationY;
        public float PlanetSunOrbitRotationY
        {
            get { return planetSunOrbitRotationY; }
            set { planetSunOrbitRotationY = value; }
        }

        private float planetSunOrbitRotationZ;
        public float PlanetSunOrbitRotationZ
        {
            get { return planetSunOrbitRotationZ; }
            set { planetSunOrbitRotationZ = value; }
        }


        public Planet()
        {

        }

        public Planet(Matrix[] WVP, String name, Model model, Vector3 position, float speed, float scale, double distanceSun, float[] rotation, float[] orbitRotation)
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

            this.planetSunOrbitRotationX = orbitRotation[0];
            this.planetSunOrbitRotationY = orbitRotation[1];
            this.planetSunOrbitRotationZ = orbitRotation[2];
        }

        public void DrawPlanet()
        {

        }
    }
}
