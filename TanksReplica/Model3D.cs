using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace TanksReplica
{
    public class Model3D
    {
        public Model model;
        public Vector3 position;
        public Vector3 direction;
        //public Camera camera;
        public Matrix world;
        int keyset;

        public float width;
        public float length;
        public float depth;
        public BoundingBox boundingbox;




        public Model3D(int i, float w, float l, float d)
        {
            position = new Vector3(0, 0, 0);
            keyset = i;
            width = w;
            length = l;
            depth = d;

        }
        public Model3D(Vector3 initial, int i, float w, float l, float d)
        {
            position = initial;
            keyset = i;
            width = w;
            length = l;
            depth = d;



        }
        public virtual void initialize(GraphicsDevice g)
        {

            direction = new Vector3(0, 0, 0.5f);
            world = Matrix.CreateTranslation(position);
            Vector3 diagonal = new Vector3(width / 2, length / 2, depth / 2);
            boundingbox = new BoundingBox(position - diagonal, position + diagonal);


        }
        public void load(ContentManager c, String s)
        {
            model = c.Load<Model>(s);
        }
        public virtual void update()
        {
            if (keyset == 1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    position += new Vector3(0, 0, 1f);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    position += new Vector3(0, 0, -1f);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    position += new Vector3(1f, 0, 0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    position += new Vector3(-1f, 0, 0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    //bullets.Add(new Bullet(direction, position, ))
                }

            }
            else if (keyset == 2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    position += new Vector3(0, 0, 1f);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    position += new Vector3(0, 0, -1f);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    position += new Vector3(1f, 0, 0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    position += new Vector3(-1f, 0, 0);
                }
            }
            world = Matrix.CreateTranslation(position);
            Vector3 diagonal = new Vector3(width / 2, length / 2, depth / 2);
            boundingbox = new BoundingBox(position - diagonal, position + diagonal);

        }
        public virtual void draw(GraphicsDevice g, Camera c)
        {

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), g.DisplayMode.AspectRatio, 1f, 1000f);
                    effect.View = Matrix.CreateLookAt(c.camPosition, c.camTarget, Vector3.Up);
                    effect.World = world;
                }
                mesh.Draw();
            }


        }

    }
}

