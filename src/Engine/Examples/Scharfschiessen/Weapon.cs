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
    public class Weapon
    {
        private DynamicWorld _world;
        private SphereShape _sphereCollider;
        private Game _game;
        private readonly SceneRenderer _srTomato;
        public Weapon(DynamicWorld world, Game game)
        {
            _world = world;
            _sphereCollider = _world.AddSphereShape(1);
            _game = game;
            _srTomato = _game.SceneLoader.LoadTomato();
        }

        public void Shoot(float4x4 mtxcam, DynamicWorld world)
        {
            var tomato = new Tomato(_game.RC, null, new float3(mtxcam.Column3.xyz), float3.Zero, 0.01f, _game, _srTomato);
            _game.LevelObjects.Add(tomato);
            tomato.ShootTomato(world, mtxcam, _sphereCollider);
        }
    }
}
