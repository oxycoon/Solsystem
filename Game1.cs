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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Planet/World/Sun world matrix stack
        private Stack<Matrix> matrixStack = new Stack<Matrix>();

        // SOME PRIVATE VERIABLES
        private GraphicsDeviceManager graphics;
        private ContentManager content;
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;

        private BasicEffect effect;

        //Camera
        private Vector3 cameraPosition = new Vector3(1.0f, 1.0f, 1.0f);
        private Vector3 cameraTarget = Vector3.Zero;
        private Vector3 cameraUpVector = new Vector3(0.0f, 1.0f, 0.0f);

        //WVP
        private Matrix world;
        private Matrix projection;
        private Matrix view;

        private float astroUnit = 149597871.0f;                                //astronomical unit

        // Sun
        private Model sol;
        private double re_sol = 69634.2, rp_sol = 69634.2;            //radius for the sun

        // Planets
        private Planet mercury, venus, terra, mars, jupiter, saturn, uranus, neptune;
        private float re_mercury = 2439.7f, rp_mercury = 2439.7f;        //radius for mercury
        private float re_venus = 6051.8f, rp_venus = 6051.8f;            //radius for venus
        private float re_terra = 6378.1f, rp_terra = 6356.8f;            //radius for earth
        private float re_mars = 3396.2f, rp_mars = 3376.2f;              //radius for mars
        private float re_jupiter = 71492.0f, rp_jupiter = 66854.0f;      //radius for jupiter
        private float re_saturn = 60268.0f, rp_saturn = 54364.0f;        //radius for saturn
        private float re_uranus = 25559.0f, rp_uranus = 24973.0f;        //radius for uranus
        private float re_neptune = 24764.0f, rp_neptune = 24341.0f;      //radius for neptune

        private float dc_mercury = 6.98f * (float)Math.Pow(10, 7), df_mercury = 4.60f * (float)Math.Pow(10, 7);  //distance from sol at closest and furthest
        private float dc_venus = 1.075f * (float)Math.Pow(10, 8), df_venus = 1.098f * (float)Math.Pow(10, 8);
        private float dc_terra = 1.471f * (float)Math.Pow(10, 8), df_terra = 1.521f * (float)Math.Pow(10, 8);
        private float dc_mars = 2.067f * (float)Math.Pow(10, 8), df_mars = 2.491f * (float)Math.Pow(10, 8);
        private float dc_jupiter = 7.409f * (float)Math.Pow(10, 8), df_jupiter = 8.157f * (float)Math.Pow(10, 8);
        private float dc_saturn = 1.384f * (float)Math.Pow(10, 9), df_saturn = 1.503f * (float)Math.Pow(10, 9);
        private float dc_uranus = 2.739f * (float)Math.Pow(10, 9), df_uranus = 3.003f * (float)Math.Pow(10, 9);
        private float dc_neptune = 4.456f * (float)Math.Pow(10, 9), df_neptune = 4.546f * (float)Math.Pow(10, 9);

        private double rot_mercury = 1407.5;
        private double rot_venus;
        private double rot_terra;
        private double rot_mars;
        private double rot_jupiter;
        private double rot_saturn;
        private double rot_uranus;
        private double rot_neptune;

        // Moons
        private Model luna; // earth
        private Model demios, phobos; // mars
        private Model io, europa, ganymede, callisto; // jupiter
        private Model titan, iapetus, tethys, dione, rhea; // saturn
        private Model titania, oberon, ariel, umbriel, miranda; // uranus
        private Model triton, proteus, nerid; // neptune
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            initDevice();
            initCamera();
            initSun();

            effect.EnableDefaultLighting();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here

            loadModels();            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //RasterizerState rasterizerState1 = new RasterizerState();
            //rasterizerState1.CullMode = CullMode.None;
            //device.RasterizerState = rasterizerState1;

            device.Clear(Color.Black);

            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            DrawModel(sol, Matrix.Identity, effect);


            base.Draw(gameTime);
        }

        private void DrawModel(Model m, Matrix world, BasicEffect be)
        {
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (ModelMeshPart mmp in mesh.MeshParts)
                {
                    be.World = GetParentTransform(m, mesh.ParentBone) * world;
                    GraphicsDevice.SetVertexBuffer(mmp.VertexBuffer, mmp.VertexOffset);
                    GraphicsDevice.Indices = mmp.IndexBuffer;
                    be.CurrentTechnique.Passes[0].Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mmp.NumVertices, mmp.StartIndex, mmp.PrimitiveCount);
                }
                
            }
        }

        private void initDevice()
        {
            device = graphics.GraphicsDevice;

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Window.Title = "Sol-system";

            effect = new BasicEffect(graphics.GraphicsDevice);
        }

        private void initCamera()
        {
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out view);
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.01f, 1000.0f, out projection);

            effect.Projection = projection;
            effect.View = view;
        }

        private void initSun()
        {
            
            
        }

        private void initPlanets()
        {
            Matrix[] tempWVP = { world, view, projection };
            float[] tempRot = {0.0f, 0.0f, 0.0f};
            float[] tempOrbit = {0.0f, 0.0f, 0.0f};

            mercury = new Planet(tempWVP, "Mercury", sol, new Vector3(dc_mercury, 0.0f, 0.0f), 50.0f, re_mercury, dc_mercury, tempRot, tempOrbit);
            venus = new Planet(tempWVP, "Venus", sol, new Vector3(dc_venus, 0.0f, 0.0f), 50.0f, re_venus, dc_venus, tempRot, tempOrbit);
            terra = new Planet(tempWVP, "Terra", sol, new Vector3(dc_terra, 0.0f, 0.0f), 50.0f, re_terra, dc_terra, tempRot, tempOrbit);
            mars = new Planet(tempWVP, "Mars", sol, new Vector3(dc_mars, 0.0f, 0.0f), 50.0f, re_mars, dc_mercury, tempRot, tempOrbit);
            jupiter = new Planet(tempWVP, "Jupiter", sol, new Vector3(dc_jupiter, 0.0f, 0.0f), 50.0f, re_jupiter, dc_jupiter, tempRot, tempOrbit);
            saturn = new Planet(tempWVP, "Saturn", sol, new Vector3(dc_saturn, 0.0f, 0.0f), 50.0f, re_saturn, dc_saturn, tempRot, tempOrbit);
            uranus = new Planet(tempWVP, "Uranus", sol, new Vector3(dc_uranus, 0.0f, 0.0f), 50.0f, re_uranus, dc_uranus, tempRot, tempOrbit);
            neptune = new Planet(tempWVP, "Neptune", sol, new Vector3(dc_neptune, 0.0f, 0.0f), 50.0f, re_neptune, dc_neptune, tempRot, tempOrbit);
        }

        private void loadModels()
        {
            sol = Content.Load<Model>("sphere");

            /*
             * Moons
             */

            // earth
            luna = Content.Load<Model>("sphere"); 
            //mars
            demios = Content.Load<Model>("sphere");
            phobos = Content.Load<Model>("sphere");
            //jupiter
            io = Content.Load<Model>("sphere");
            europa = Content.Load<Model>("sphere");
            ganymede = Content.Load<Model>("sphere");
            callisto = Content.Load<Model>("sphere");
            //saturn
            titan = Content.Load<Model>("sphere");
            iapetus = Content.Load<Model>("sphere");
            tethys = Content.Load<Model>("sphere");
            dione = Content.Load<Model>("sphere");
            rhea = Content.Load<Model>("sphere");
            //uranus
            titania = Content.Load<Model>("sphere");
            oberon = Content.Load<Model>("sphere");
            ariel = Content.Load<Model>("sphere");
            umbriel = Content.Load<Model>("sphere");
            miranda = Content.Load<Model>("sphere");
            // neptune
            triton = Content.Load<Model>("sphere");
            proteus = Content.Load<Model>("sphere");
            nerid = Content.Load<Model>("sphere");
        }

        private Matrix GetParentTransform(Model m, ModelBone mb)
        {
            return (mb == m.Root) ? mb.Transform :
                mb.Transform * GetParentTransform(m, mb.Parent);
        }   
    }
}
