using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Tomato : GameObject
    {
        

        private RigidBody _tomatoRB;
        public Tomato(RenderContext rc, Mesh mesh, float3 position, float4x4 orientation, float scaleFactor)
            : base(rc, mesh, position, orientation, scaleFactor)
        {
            Radius = 1;
        }

        public void ShootTomato(DynamicWorld world, float4x4 mtxCam, SphereShape spherecollider)
        {
            _tomatoRB = world.AddRigidBody(1, new float3(mtxCam.Row3), float3.Zero, spherecollider);

            float3 alt = new float3(mtxCam.Column3);
            mtxCam *= float4x4.CreateTranslation(-alt);
            float3 one = new float3(0, 0, 1);
            float3 to;
            float3.TransformVector(ref one, ref mtxCam, out to);
            float3 impuls = new float3(0,30,50);
            _tomatoRB.ApplyCentralImpulse = to * impuls;
        }


        public override void Update()
        {
            base.Update();
        
            if (_tomatoRB != null)
            {
                Position = _tomatoRB.Position;
            }
        }

    }
}
