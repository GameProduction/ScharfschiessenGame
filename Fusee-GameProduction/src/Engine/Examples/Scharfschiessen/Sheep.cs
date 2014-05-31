using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Media.Animation;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Sheep : GameObject
    {
        public float Speed { get; set; }
        private float _distance;


        public Sheep(RenderContext rc, Mesh mesh, float3 position, float4x4 orientation, float scaleFactor)
            : base(rc, mesh, position, orientation, scaleFactor)
        {
            _distance = position.Length;
            Speed = 1;
            Radius = 2;
        }

        public override void Update()
        {
            base.Update();
            Move();
        }

        public void Move()
        {
            Position = Position*float4x4.CreateRotationY(0.001f)*Speed;
        }

        public void Collided()
        {
            Debug.WriteLine("Collided");
        }
    }
}
