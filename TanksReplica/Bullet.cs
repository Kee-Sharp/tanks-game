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
    public class Bullet : Model3D
    {
        public Vector3 speed;
        public int ricochet;
        public BoundingSphere boundingsphere;

        public Bullet(Vector3 s, Vector3 pos, int i, float w, float l, float d) : base(pos, i, w, l, d)
        {
            speed = s;
            ricochet = 1;
        }
        public void load(ContentManager c)
        {
            model = c.Load<Model>("sphere");
        }
        public new void load(ContentManager c, String s)
        {
            model = c.Load<Model>(s);
        }

        public override void update()
        {
            position += speed;
            boundingsphere = new BoundingSphere(position, 0.5f);
            base.update();


        }

    }
}
