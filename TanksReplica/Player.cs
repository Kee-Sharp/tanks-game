using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TanksReplica
{
    class Player : Model3D
    {
        public Camera camera;
        public Vector3 camOffset;
        public Vector3 speed;
        Vector3 left;
        public List<Bullet> bullets;
        public int health;
        float rotateangle;
        public int bullettimer;

        public Player(int i, float w, float l, float d) : base(i, w, l, d)
        {
            camera = new Camera(new Vector3(0, 0, 0));
            camOffset = new Vector3(0, 2f, -10f);
            camera.camPosition = position + camOffset;
        }
        public Player(Vector3 pos, int i, float w, float l, float d) : base(pos, i, w, l, d)
        {
            camera = new Camera(pos);


        }
        public override void initialize(GraphicsDevice g)
        {
            base.initialize(g);
            camera.initialize(g);
            camOffset = new Vector3(0, 2f, -8f);
            camera.camPosition = position + camOffset;
            rotateangle = 0;
            bullets = new List<Bullet>();
            bullettimer = 0;
            health = 4;
        }
        public void update(ContentManager c, bool bo)
        {
            Vector3 diagonal = new Vector3(width / 2, length / 2, depth / 2);
            boundingbox = new BoundingBox(position - diagonal, position + diagonal);
            speed = direction / 4;
            left = Vector3.Transform(speed, Matrix.CreateRotationY(90));
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position += speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position += left;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position -= left;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && bullettimer == 0)
            {
                bullets.Add(new Bullet(direction, position, 0, 1, 1, 1));
                bullets.ElementAt<Bullet>(bullets.Count - 1).load(c);
                bullettimer = 60;
            }
            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    Bullet b = bullets.ElementAt<Bullet>(i);
                    b.update();
                    if (Math.Abs(b.position.Z - position.Z) >= 250)
                    {
                        bullets.Remove(b);
                    }
                }
            }
            if (bullettimer > 0)
            {
                bullettimer--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                rotateangle = .02f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                rotateangle = -.02f;
            }
            else
            {
                rotateangle = 0;
            }
            if (bo)
            {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                float thumb = (float)Math.Atan2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
                direction = Vector3.Transform(direction, Matrix.CreateRotationY(thumb));
                camOffset = Vector3.Transform(camOffset, Matrix.CreateRotationY(thumb));
                if (state.ThumbSticks.Left.X < -0.8)
                {
                    position += left;
                }
                if (state.ThumbSticks.Left.X > 0.8)
                {
                    position -= left;
                }
                if (state.ThumbSticks.Left.Y < -0.8)
                {
                    position -= direction;
                }
                if (state.ThumbSticks.Left.Y > -0.8)
                {
                    position += direction;
                }
                if (state.Triggers.Right > 0.5 && bullettimer == 0)
                {
                    bullets.Add(new Bullet(direction, position, 0, 1, 1, 1));
                    bullets.ElementAt<Bullet>(bullets.Count - 1).load(c);
                    bullettimer = 60;
                }
            }
            direction = Vector3.Transform(direction, Matrix.CreateRotationY(rotateangle));
            camOffset = Vector3.Transform(camOffset, Matrix.CreateRotationY(rotateangle));
            camera.camPosition = position + camOffset;
            camera.camTarget = position;
            camera.view = Matrix.CreateLookAt(camera.camPosition, camera.camTarget, Vector3.Up);
            world = Matrix.CreateWorld(position, direction, Vector3.Up);
            //world = Matrix.CreateRotationY(rotateangle) * Matrix.CreateTranslation(position);
        }
        public void draw(GraphicsDevice g)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Projection = camera.projection;
                    effect.View = camera.view;
                    effect.World = world;
                }
                mesh.Draw();
            }
            camera.Draw(g);
            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets.ElementAt<Bullet>(i).draw(g, camera);
                }
            }
        }
    }
}


