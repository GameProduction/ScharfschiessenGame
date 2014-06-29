using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
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
        private int _score;

        
        public Sheep(RenderContext rc, float3 position, float3 rotation, float3 scaleFactor,  SceneRenderer sc, Game game)
            : base(rc, position, rotation, scaleFactor, sc)
        {
            Color = new float4(0.5f, 0.8f, 0.8f, 1);
            _distance = position.Length;
            if (_distance > 50)
            {
                _score = 100;
            }
            else
            {
                _score = 50;
            }
            Speed = (10 / _distance) * game.Level;
            Radius = 4f;
            _game = game;
            Pos = position;
            _alpha = (float)Math.Tan(Pos.z/Pos.x);
            Tag = "Sheep";
        }

        public void SetSpeed(int level)
        {
            Speed = Speed *1.2f;
        }
        public override void Update()
        {
            MoveTo();
        }

        public void MoveTo()
        {
            if (_alpha == 2*Math.PI)
            {
                _alpha = 0;
            }
            else
            {
                _alpha -= (float) Time.Instance.DeltaTime*Speed;
                P.y = (float)Math.Sin(_alpha);
            }
            P.x = _distance*(float) Math.Sin(_alpha);
            P.z = _distance*(float) Math.Cos(_alpha);
            float betha = (float) ((2 * Math.PI) - (_alpha));

            ObjectMtx = float4x4.CreateTranslation(P)*float4x4.CreateRotationY(-betha) ;
        }


        public override void Collided()
        {
            _game.Points += _score;
            _game.LevelObjects.Remove(this);
        }
    }
}
