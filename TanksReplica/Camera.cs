using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TanksReplica
{
    public class Camera
    {


        public Vector3 camTarget;
        public Vector3 camPosition;
        public Matrix projection;
        public Matrix view;
        public Matrix world;



        //bool Orbit;
        //constructor
        public Camera(Vector3 pos)
        {
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = pos - new Vector3(0, 0, 20f);
        }
        //methods
        public void initialize(GraphicsDevice g)
        {

            //camPosition = new Vector3(0f, 0f, -100f);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), g.DisplayMode.AspectRatio, 1f, 1000f);
            view = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
            world = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

        }
        public void Update()
        {


            view = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

        }
        public void Draw(GraphicsDevice g)
        {

        }



    }
}

