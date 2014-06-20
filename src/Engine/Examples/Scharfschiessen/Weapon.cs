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
        public int Magazin { get; private set; }
   
        
        private readonly SceneRenderer _srTomato;
        public Weapon(DynamicWorld world, Game game)
        {
            _world = world;
            _sphereCollider = _world.AddSphereShape(1);
            _game = game;
            _srTomato = _game.SceneLoader.LoadTomato();
            Magazin = 10;
        }

        private void Shoot(float4x4 mtxcam)
        {
            var tomato = new Tomato(_game.RC, null, new float3(mtxcam.Column3.xyz), float3.Zero, 0.01f, _game, _srTomato);
            _game.LevelObjects.Add(tomato);
            tomato.ShootTomato(_world, mtxcam, _sphereCollider);
        }

        public void WeaponInput(float4x4 mtxcam)
        {
            //Schießen wenn Magazin != 0
            if (Input.Instance.IsKeyUp(KeyCodes.Space))
            {
                Shoot(mtxcam);
                Magazin--;
            }

            //Nachladen
            //_magazin = 10;
        }
    }
}
