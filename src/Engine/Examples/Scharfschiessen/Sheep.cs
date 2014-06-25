using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Media.Animation;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Sheep : GameObject
    {
        public float Speed { get; set; }
        private float _distance;
        private float3 Pos;
        private float _alpha;
        private float3 P = float3.Zero;
        private Game _game;


        public Sheep(RenderContext rc, Mesh mesh, float3 position, float3 rotation, float3 scaleFactor,  SceneRenderer sc, Game game)
            : base(rc, mesh, position, rotation, scaleFactor, sc)
        {
            Color = new float4(0.5f, 0.8f, 0.8f, 1);
            _distance = position.Length;
            Speed = 1;
            Radius = 5;
            _game = game;
            Pos = position;
            _alpha =  (float) Math.PI;
            var imgData = rc.LoadImage("Assets/SchafOberflächenfarbe.jpg");
            _iTex = rc.CreateTexture(imgData);
            Tag = "ActionObject";
        }

        public override void Update()
        {
            base.Update();
            
            MoveTo();
        }

        public void MoveTo()
        {
            //Random rand = new Random();
            //float3 pos = new float3(rand.Next(-20, 20), 1, rand.Next(-20, 20));
            if (_alpha == 2 * Math.PI)
            {
                _alpha = 0;
            }
            else
            {
                _alpha += (float)Time.Instance.DeltaTime * Speed;
            }
            P.x = _distance* (float)Math.Sin(_alpha/6);
            P.z = _distance* (float)Math.Cos(_alpha/6);

         
            //ObjectMtx *= float4x4.CreateTranslation(P);
            //Debug.WriteLine(Position);
            ObjectMtx *= float4x4.CreateTranslation(-ObjectMtx.Column3.xyz) * float4x4.CreateTranslation(P);

            //ObjectMtx *= float4x4.CreateTranslation(new float3(ObjectMtx.Column3.x+0.1f,ObjectMtx.Column3.y,ObjectMtx.Column3.z+0.1f ));
        }

        public override void Collided()
        {
            _game.Points++;
        }
    }
}
