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
        private int _level;

        public Sheep(RenderContext rc, float3 position, float3 rotation, float3 scaleFactor,  SceneRenderer sc, Game game)
            : base(rc, position, rotation, scaleFactor, sc)
        {
            _distance = position.Length;
            if (_distance > 60)
            {
                _score = 120;
            }
            if (_distance > 40)
            {
                _score = 80;
            }
            else
            {
                _score = 50;
            }
            Speed = (5 / _distance) * game.Level;
            Radius = 4f;
            _game = game;
            Pos = position;
            _alpha = (float)Math.Tan(Pos.z/Pos.x);
            Tag = "Sheep";
            _level = 1;
            //zufällige Wellenbewegung

            if (position.x % 2 == 0)
            {
                TheWave = SinWave;
            }
            else
            {
                TheWave = CosWave;
            }
        }

        private delegate float Wave(float angle);

        private Wave TheWave;
        private float SinWave(float angle)
        {
            return (float)Math.Sin(angle);
        }
        private float CosWave(float angle)
        {
            return (float)Math.Cos(angle);
        }
        //Speed bei LevelUp erhöhen
        public void SetSpeed(int level)
        {
            if(level > 2 && level <4);
            {
                _level ++;
            }
            Speed = Speed *1.01f;
        }
        public override void Update()
        {
            MoveTo();
        }

        //Bewegung des scahfes auf Keisbahn um den Spieler
        public void MoveTo()
        {
            if (_alpha == 2*Math.PI)
            {
                _alpha = 0;
            }
            else
            {
                _alpha -= (float) Time.Instance.DeltaTime*Speed;
                P.y = _level*TheWave(_alpha*4); 
            }
            P.x = _distance*(float) Math.Sin(_alpha);
            P.z = _distance*(float) Math.Cos(_alpha);
            float betha = (float) ((2 * Math.PI) - (_alpha));
           
            ObjectMtx = float4x4.CreateTranslation(P)*float4x4.CreateRotationY(-betha);
        }


        //bei Kollision werden Punkte erhöt und das objekt gelöscht 
        public override void Collided()
        {
            _game.Points += _score;
            _game.LevelObjects.Remove(this);
        }
    }
}
