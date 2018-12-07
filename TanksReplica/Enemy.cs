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
    class Enemy : Model3D
    {
        public float health;
        Vector3 target;
        public List<Bullet> bullets;
        int bullettimer;
        VertexPositionColor[] healthbar;
        BasicEffect effect;
        public Enemy(int i, float w, float l, float d) : base(i, w, l, d)
        {

        }
        public Enemy(Vector3 initial, int i, float w, float l, float d) : base(initial, 0, 2, 2, 2)
        {

        }
        public override void initialize(GraphicsDevice g)
        {
            base.initialize(g);
            health = 3;
            bullets = new List<Bullet>();
            bullettimer = 0;
            healthbar = new VertexPositionColor[6];
            healthbar[0].Position = position + new Vector3(1, 3f, 0);
            healthbar[1].Position = position + new Vector3(1, 3.2f, 0);
            healthbar[2].Position = position + new Vector3(1 - (health * 2 / 3), 3f, 0);
            healthbar[3].Position = healthbar[1].Position;
            healthbar[4].Position = position + new Vector3(1 - (health * 2 / 3), 3.2f, 0);
            healthbar[5].Position = healthbar[2].Position;
            for (int i = 0; i < 6; i++)
            {
                healthbar[i].Color = Color.Red;
            }

            effect = new BasicEffect(g);
        }
        public override void update()
        {
            base.update();
            healthbar[2].Position = position + new Vector3(1 - (health * 2 / 3), 3f, 0);
            healthbar[4].Position = position + new Vector3(1 - (health * 2 / 3), 3.2f, 0);
            healthbar[5].Position = healthbar[2].Position;
            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    Bullet b = bullets.ElementAt<Bullet>(i);
                    b.update();
                }
            }
            if (bullettimer > 0)
            {
                bullettimer--;
            }
            world = Matrix.CreateWorld(position, direction, Vector3.Up);

        }
        public override void draw(GraphicsDevice g, Camera c)
        {
            effect.View = c.view;
            effect.Projection = c.projection;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            g.RasterizerState = rasterizerState;
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                g.DrawUserPrimitives(PrimitiveType.TriangleList, healthbar, 0, 2);
            }
            if (bullets.Count > 0)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets.ElementAt<Bullet>(i).draw(g, c);
                }
            }
            base.draw(g, c);
        }
        public void Shoot(Vector3 tpos, ContentManager c)
        {
            target = tpos - position;
            float difference = (float)Math.Atan2(target.Z - direction.Z, target.X - direction.X);
            /*if (target.X < direction.X) {

            }
            if (difference < Math.PI / 8) {*/
            direction = target;
            direction.Normalize();
            direction = direction * 0.5f;
            if (bullettimer == 0)
            {
                bullets.Add(new Bullet(direction * 3 / 2, position, 0, 1, 1, 1));
                bullets.ElementAt<Bullet>(bullets.Count - 1).load(c, "ebullet");
                bullettimer = 70;
            }
            //}
            //else if()
        }

    }
}

