using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Weapon
    {
        internal Physics _physics;
        public Weapon(ref Physics physics)
        {
            _physics = physics;
            Debug.WriteLine("Weapon");
        }

        public void Shoot(float4x4 mtxcam)
        {
            
            var projectile = _physics.World.AddRigidBody(1, new float3(mtxcam.Row3), float3.Zero, _physics.sphereCollider);
  
            float3 alt = new float3(mtxcam.Column3);
            mtxcam *= float4x4.CreateTranslation(-alt);

            float3 one = new float3(0,0, 1);
            float3 to;
            float3.TransformVector(ref one, ref mtxcam, out to);
            Debug.WriteLine(to);
            float impuls = 50;
 
            projectile.ApplyCentralImpulse = to*impuls;
        }


    }
}
