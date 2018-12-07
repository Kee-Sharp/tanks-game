using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksReplica
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Player cube;
        int level;
        int leveltimer;
        int levelseconds;
        List<List<Enemy>> levelenemies;
        float enemyrange;
        Model3D floor;
        Vector3 originalcubepos;

        Texture2D screen;
        Texture2D start;
        Texture2D keyboard;
        Texture2D controller;
        Texture2D win;
        Texture2D lose;
        Texture2D nextlevel;
        Texture2D heart;
        Rectangle frame;
        Rectangle[] h;
        float state;
        bool paused;
        int pausetimer;

        SpriteFont data;
        SpriteBatch spritebatch;

        public BasicEffect basic;
        BasicEffect bill;

        public VertexPositionColor[] triangleVertices;
        public VertexPositionColor[] wall1;
        public VertexBuffer vertexBuffer;
        GamePadCapabilities capabilities;
        bool cap;

        List<Model3D> walls;

        List<SoundEffect> soundeffects;
        SoundEffectInstance startmusic;
        SoundEffectInstance world;
        SoundEffectInstance select;
        SoundEffectInstance fire;
        int musicdelay;




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            enemyrange = 35;
            paused = false;
            pausetimer = 0;
            leveltimer = 0;
            musicdelay = 0;


            //BoundingBox

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
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            void Window_ClientSizeChanged(object sender, EventArgs e)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }

            originalcubepos = new Vector3(0, 0, -5);
            floor = new Model3D(new Vector3(0, -1, 0), 0, 256, 0, 192);
            cube = new Player(originalcubepos, 1, 2, 2, 2);
            cube.initialize(GraphicsDevice);
            cube.health = 4;
            level = 0;
            levelenemies = new List<List<Enemy>>();
            List<Enemy> e0 = new List<Enemy>();
            e0.Add(new Enemy(new Vector3(15, 0, 30), 2, 2, 2, 2));
            e0.Add(new Enemy(new Vector3(-15, 0, 30), 2, 2, 2, 2));
            levelenemies.Add(e0);
            List<Enemy> e1 = new List<Enemy>();
            e1.Add(new Enemy(new Vector3(40, 0, 40), 2, 2, 2, 2));
            e1.Add(new Enemy(new Vector3(-40, 0, 40), 2, 2, 2, 2));
            e1.Add(new Enemy(new Vector3(-40, 0, -40), 2, 2, 2, 2));
            e1.Add(new Enemy(new Vector3(40, 0, -40), 2, 2, 2, 2));
            levelenemies.Add(e1);
            List<Enemy> e2 = new List<Enemy>(); ;
            float x1 = (float)((enemyrange + 5) * Math.Sin(2 * Math.PI / 5));
            float x2 = (float)((enemyrange + 5) * Math.Sin(4 * Math.PI / 5));
            float z1 = (float)((enemyrange + 5) * Math.Cos(2 * Math.PI / 5));
            float z2 = (float)((enemyrange + 5) * Math.Cos(Math.PI / 5));
            e2.Add(new Enemy(new Vector3(0, 0, enemyrange + 5), 2, 2, 2, 2));
            e2.Add(new Enemy(new Vector3(x1, 0, z1), 2, 2, 2, 2));
            e2.Add(new Enemy(new Vector3(-x1, 0, z1), 2, 2, 2, 2));
            e2.Add(new Enemy(new Vector3(x2, 0, -z2), 2, 2, 2, 2));
            e2.Add(new Enemy(new Vector3(-x2, 0, -z2), 2, 2, 2, 2));
            levelenemies.Add(e2);

            //m.initialize(GraphicsDevice);
            for (int i = 0; i < levelenemies.Count; i++)
            {
                List<Enemy> e = levelenemies.ElementAt<List<Enemy>>(i);
                for (int j = 0; j < e.Count; j++)
                {
                    Enemy enemy = e.ElementAt<Enemy>(j);
                    enemy.initialize(GraphicsDevice);
                }
            }
            floor.initialize(GraphicsDevice);
            state = 0;
            basic = new BasicEffect(GraphicsDevice);
            basic.Alpha = 1.0f;
            //basic.TextureEnabled = true;
            basic.VertexColorEnabled = true;
            basic.LightingEnabled = false;
            bill = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
            walls = new List<Model3D>();
            for (int i = 0; i < 50; i++)
            {
                walls.Add(new Model3D(new Vector3((2 * i) - 50, 0, 50), 0, 2, 2, 2));
                walls.Add(new Model3D(new Vector3((2 * i) - 50, 0, -50), 0, 2, 2, 2));
                walls.Add(new Model3D(new Vector3(-50, 0, (2 * i) - 50), 0, 2, 2, 2));
                walls.Add(new Model3D(new Vector3(50, 0, (2 * i) - 50), 0, 2, 2, 2));


            }
            h = new Rectangle[4];
            h[0] = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 8, GraphicsDevice.Viewport.Height / 8);
            h[1] = new Rectangle(GraphicsDevice.Viewport.Width / 8, 0, GraphicsDevice.Viewport.Width / 8, GraphicsDevice.Viewport.Height / 8);
            h[2] = new Rectangle(GraphicsDevice.Viewport.Width / 4, 0, GraphicsDevice.Viewport.Width / 8, GraphicsDevice.Viewport.Height / 8);
            h[3] = new Rectangle(3 * GraphicsDevice.Viewport.Width / 8, 0, GraphicsDevice.Viewport.Width / 8, GraphicsDevice.Viewport.Height / 8);
            /*for (int i = 0; i < walls.Count; i++)
            {
                Model3D w = walls.ElementAt<Model3D>(i);
                walls.Add(new Model3D(w.position+new Vector3(0,2,0),0, 2,2,2));
                walls.Add(new Model3D(w.position + new Vector3(0, 4, 0), 0, 2, 2, 2));
            }*/
            for (int i = 0; i < walls.Count; i++)
            {
                Model3D w = walls.ElementAt<Model3D>(i);
                w.initialize(GraphicsDevice);
            }
            soundeffects = new List<SoundEffect>();
            base.Initialize();

            //Create the triangle
            /*triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(-1, 0, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-1, 10, 10), Color.Blue);
            triangleVertices[2] = new VertexPositionColor(new Vector3(-1, 0, 20), Color.Green);
           // triangleVertices[3] = new VertexPositionColor(new Vector3(100, 0, 0), Color.Goldenrod);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
            
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
            */



        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            cube.load(Content, "Cube");
            for (int i = 0; i < levelenemies.Count; i++)
            {
                List<Enemy> e = levelenemies.ElementAt<List<Enemy>>(i);
                for (int j = 0; j < e.Count; j++)
                {
                    Enemy enemy = e.ElementAt<Enemy>(j);
                    enemy.load(Content, "Cube");
                }
            }
            //Block.sharedmodel = Content.Load<Model>("Cube");
            for (int i = 0; i < walls.Count; i++)
            {
                Model3D w = walls.ElementAt<Model3D>(i);
                w.load(Content, "wall");
            }
            floor.load(Content, "floor5");
            data = Content.Load<SpriteFont>("Data");
            spritebatch = new SpriteBatch(GraphicsDevice);
            start = Content.Load<Texture2D>("start");
            keyboard = Content.Load<Texture2D>("keyboard");
            controller = Content.Load<Texture2D>("controller");
            win = Content.Load<Texture2D>("win");
            lose = Content.Load<Texture2D>("lose");
            nextlevel = Content.Load<Texture2D>("nextlevel");
            heart = Content.Load<Texture2D>("heart");
            frame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            screen = start; 

            soundeffects.Add(Content.Load<SoundEffect>("startmusic"));
            soundeffects.Add(Content.Load<SoundEffect>("world"));
            soundeffects.Add(Content.Load<SoundEffect>("select"));
            startmusic = soundeffects[0].CreateInstance();
            startmusic.IsLooped = true;
            world = soundeffects[1].CreateInstance();
            world.IsLooped = true;
            select = soundeffects[2].CreateInstance();
            startmusic.Play();




            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            // TODO: Add your update logic here
            capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            cap = capabilities.IsConnected;




            GamePadState s = GamePad.GetState(PlayerIndex.One);
            if ((Keyboard.GetState().IsKeyDown(Keys.P) || s.Buttons.Start == ButtonState.Pressed) && pausetimer == 0)
            {
                paused = !paused;
                pausetimer = 20;
            }
            if (pausetimer > 0)
            {
                pausetimer--;
            }
            if (paused)
            {
                if (cap)
                {
                    screen = controller;
                }
                else
                {
                    screen = keyboard;
                }
            }
            else
            {
                if (state == 0)
                {

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || s.Buttons.A == ButtonState.Pressed)
                    {
                        state = 1;
                        cube.bullettimer = 15;
                        startmusic.Stop();
                        select.Play();
                    }

                }

                else if (state == 1)
                {
                    if (select.State == SoundState.Stopped && world.State == SoundState.Stopped)
                    {
                        world.Play();
                    }

                    List<Enemy> enemies = levelenemies.ElementAt<List<Enemy>>(level);
                    cube.update(Content, cap);

                    if (cube.health == 0)
                    {
                        state = 3;
                        screen = lose;
                    }
                    if (cube.position.X < -47)
                    {
                        cube.position.X = -47;
                        cube.speed = Vector3.Zero;
                    }
                    if (cube.position.X > 47)
                    {
                        cube.position.X = 47;
                        cube.speed = Vector3.Zero;
                    }
                    if (cube.position.Z < -47)
                    {
                        cube.position.Z = -47;
                        cube.speed = Vector3.Zero;
                    }
                    if (cube.position.Z > 47)
                    {
                        cube.position.Z = 47;
                        cube.speed = Vector3.Zero;
                    }

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Enemy e = enemies.ElementAt<Enemy>(i);
                        e.update();
                        if (e.health == 0)
                        {
                            enemies.Remove(e);
                        }
                        if (Math.Sqrt(Math.Pow(e.position.X - cube.position.X, 2) + Math.Pow(e.position.Z - cube.position.Z, 2)) <= enemyrange)
                        {
                            e.Shoot(cube.position, Content);
                        }
                    }
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Enemy e = enemies.ElementAt<Enemy>(i);
                        for (int j = 0; j < e.bullets.Count; j++)
                        {
                            Bullet b = e.bullets.ElementAt<Bullet>(j);
                            if (b.boundingsphere.Intersects(cube.boundingbox))
                            {
                                cube.health--;
                                e.bullets.Remove(b);
                            }
                        }
                    }
                    floor.update();
                    if (cube.bullets.Count > 0)
                    {
                        for (int i = 0; i < cube.bullets.Count; i++)
                        {
                            Bullet b = cube.bullets.ElementAt<Bullet>(i);
                            for (int j = 0; j < enemies.Count; j++)
                            {
                                Enemy e = enemies.ElementAt<Enemy>(j);
                                if (b.boundingsphere.Intersects(e.boundingbox))
                                {

                                    e.health -= 1;
                                    cube.bullets.Remove(b);

                                }
                            }

                        }
                        for (int i = 0; i < cube.bullets.Count; i++)
                        {
                            Bullet b = cube.bullets.ElementAt<Bullet>(i);

                            if ((b.position.X < -48 || b.position.X > 48) && b.ricochet > 0)
                            {
                                b.speed.X = -b.speed.X;
                                b.ricochet--;
                            }
                            else if (b.position.X < -48 || b.position.X > 48)
                            {
                                cube.bullets.Remove(b);
                            }
                            if ((b.position.Z < -48 || b.position.Z > 48) && b.ricochet > 0)
                            {
                                b.speed.Z = -b.speed.Z;
                                b.ricochet--;
                            }
                            else if (b.position.Z < -48 || b.position.Z > 48)
                            {
                                cube.bullets.Remove(b);
                            }

                        }

                    }
                    if (enemies.Count == 0)
                    {
                        if (level < 2)
                        {
                            level++;
                            cube.position = originalcubepos;
                            cube.direction = Vector3.Forward;
                            cube.initialize(GraphicsDevice);
                            cube.update();
                            /*state = 1.1f;
                            screen = nextlevel;
                            levelseconds = 3;
                            world.Pause();*/
                        }
                        else
                        {
                            state = 2;
                            screen = win;
                            world.Stop();
                        }
                    }

                }



                else if (state == 1.1)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || s.Buttons.A == ButtonState.Pressed)
                    {
                        state = 1;
                        world.Resume();
                    }
                }
                else if (state == 2 || state == 3)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.E) || s.Buttons.A == ButtonState.Pressed)
                    {
                        this.Exit();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        screen = start;
                        //state = 0;
                        this.Initialize();
                        startmusic.Play();
                        //cube.position = originalcubepos;
                        //cube.update();
                        //m.position = Vector3.Zero;
                        //m.update();
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E) || s.Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (state == 1.1)
            {
                spritebatch.Begin();
                spritebatch.DrawString(data, "" + levelseconds, new Vector2(GraphicsDevice.Viewport.Width / 2, 3 * GraphicsDevice.Viewport.Height / 4), Color.Black);
                spritebatch.End();
            }
            if (state != 1 || paused == true)
            {
                //
                //GraphicsDevice.Clear(Color.Aqua);
                spritebatch.Begin();
                spritebatch.Draw(screen, frame, Color.White);
                spritebatch.End();


            }

            else
            {

                //GraphicsDevice.BlendState = BlendState.Opaque;
                /*spritebatch.Begin();
                for (int i = 0; i < cube.health; i++)
                {
                    spritebatch.Draw(heart, h[i], Color.White);
                }
                spritebatch.End();*/

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                //GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                GraphicsDevice.Clear(Color.SlateBlue);


                basic.Projection = cube.camera.projection;
                basic.View = cube.camera.view;
                basic.World = cube.camera.world;
                GraphicsDevice.SetVertexBuffer(vertexBuffer);

                //turn off back face culling
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                GraphicsDevice.RasterizerState = rasterizerState;

                foreach (EffectPass pass in basic.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    //GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
                }

                cube.draw(GraphicsDevice);
                List<Enemy> e = levelenemies.ElementAt<List<Enemy>>(level);
                for (int j = 0; j < e.Count; j++)
                {
                    Enemy enemy = e.ElementAt<Enemy>(j);
                    enemy.draw(GraphicsDevice, cube.camera);
                }



                floor.draw(GraphicsDevice, cube.camera);
                for (int i = 0; i < walls.Count; i++)
                {
                    Model3D w = walls.ElementAt<Model3D>(i);
                    w.draw(GraphicsDevice, cube.camera);
                }

                spritebatch.Begin(0, null, null, null, RasterizerState.CullNone, bill);
                spritebatch.DrawString(data, "Score", Vector2.Zero, Color.Black, 0, Vector2.Zero, 0.5f, 0, 0);
                spritebatch.End();

                //drawing code

                base.Draw(gameTime);
            }

        }
    }
}


