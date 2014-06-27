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

        private ImageData imgData;
        private readonly SceneRenderer _srTomato;
        private RenderContext RC;
 
        public Weapon(DynamicWorld world, Game game)
        {
            _world = world;
            _sphereCollider = _world.AddSphereShape(1);
            _game = game;
            _srTomato = _game.SceneLoader.LoadTomato();
            Magazin = 10;
            imgData = game.RC.LoadImage("Assets/TomateOberflächenfarbe.jpg");
            RC = game.RC;
        }

        private void Shoot(float4x4 mtxcam)
        {
            var start = mtxcam.Column3.xyz;
            start.y += 0.1f;
            RigidBody tomatoRb = _world.AddRigidBody(1, start, float3.Zero, _sphereCollider);
            Tomato tomato = new Tomato(RC, tomatoRb.Position, float3.Zero, new float3(0.02f, 0.02f, 0.02f), _srTomato, tomatoRb, _game, _world);
            _game.LevelObjects.Add(tomato);
            float3 one = new float3(0, 0, 1);
            float3 to;
            float3.TransformVector(ref one, ref mtxcam, out to);
            float impuls = 70;
            tomatoRb.ApplyCentralImpulse = to * impuls;  
        }

        public void WeaponInput(float4x4 mtxcam)
        {
            //Schießen wenn Magazin != 0
         // if (Input.Instance.IsButton(MouseButtons.Left))
          if (Input.Instance.IsKeyUp(KeyCodes.Space))
               {
                   if (Magazin > 0)
                   {
                       Shoot(mtxcam);
                       Magazin--;
                   }
               }

            //Nachladen
            if (Input.Instance.IsButton(MouseButtons.Right))
            {
                Magazin = 10;
            }
        }
    }
}
