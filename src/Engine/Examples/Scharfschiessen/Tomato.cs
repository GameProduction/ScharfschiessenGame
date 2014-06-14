using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Tomato : GameObject
    {
        

        private RigidBody _tomatoRB;
        public Tomato(RenderContext rc, Mesh mesh, float3 position, float3 rotation, float scaleFactor, Game game, SceneRenderer sc)
            : base(rc, mesh, position, rotation, scaleFactor, game)
        {
            Color = new float4(0.5f, 0.1f, 0.1f, 1);
            Radius = 1;
            // load texture
            var imgData = rc.LoadImage("Assets/TomateOberflächenfarbe.jpg");
            _iTex = rc.CreateTexture(imgData);
            SceneRenderer = sc;
        }

        public void ShootTomato(DynamicWorld world, float4x4 mtxcam, SphereShape spherecollider)
        {
            _tomatoRB = world.AddRigidBody(1, new float3(0,1.5f,0), float3.Zero, spherecollider);

            float3 alt = new float3(mtxcam.Column3);
            mtxcam *= float4x4.CreateTranslation(-alt);

            float3 one = new float3(0, 0, 1);
            float3 to;
            float3.TransformVector(ref one, ref mtxcam, out to);
            float impuls = 50;

            _tomatoRB.ApplyCentralImpulse = to * impuls;
        }


        public override void Update()
        {
        
            if (_tomatoRB != null)
            {
                Position = _tomatoRB.Position;
                ObjectMtx *= float4x4.CreateTranslation(Position);
            }
        }

        public override void Collided()
        {
            Debug.WriteLine("Tomato Colloided");
        }

    }
}
