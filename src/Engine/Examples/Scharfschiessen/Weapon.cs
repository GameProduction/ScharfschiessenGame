using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using System.IO;

namespace Examples.Scharfschiessen
{
    public class Weapon
    {
        private DynamicWorld _world;
        private SphereShape _sphereCollider;
        private Game _game;
        private int _magazin = 10;
        
        private readonly SceneRenderer _srTomato;
        public Weapon(DynamicWorld world, Game game)
        {
            _world = world;
            _sphereCollider = _world.AddSphereShape(1);
            _game = game;
            _srTomato = _game.SceneLoader.LoadTomato();
        }

        public void Shoot(float4x4 mtxcam)
        {
            var tomatoRb = _world.AddRigidBody(1, new float3(0, 0, 0), float3.Zero, _game.SphereCollider);
            var tomato = new Tomato(_game.RC, null, tomatoRb.Position, float3.Zero, 0.01f, _game, _srTomato, tomatoRb);
            _game.LevelObjects.Add(tomato);
            float3 alt = new float3(mtxcam.Column3.xyz);
            mtxcam *= float4x4.CreateTranslation(-alt);
            float3 one = new float3(0, 0, 1);
            float3 to;
            float3.TransformVector(ref one, ref mtxcam, out to);
            float impuls = 50;
            tomatoRb.ApplyCentralImpulse = to * impuls;  
           
        }

        public void WeaponInput(float4x4 mtxcam)
        {
            //Schießen wenn Magazin != 0
            if (Input.Instance.IsKeyUp(KeyCodes.Space))
            {
                Shoot(mtxcam);
                _magazin--;
            }

            //Nachladen
            //_magazin = 10;
        }
    }
}
