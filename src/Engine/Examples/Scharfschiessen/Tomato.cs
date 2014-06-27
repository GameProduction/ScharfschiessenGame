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
    public class Tomato : GameObject
    {


        private RigidBody TomatoRb { get; set; }
        private double timer;
       private readonly Game _game;
        private readonly DynamicWorld _world;

        public Tomato(RenderContext rc, float3 position, float3 rotation, float3 scaleFactor,SceneRenderer sc, RigidBody tomatoRigidBody, Game game, DynamicWorld world)
            : base(rc, position, rotation, scaleFactor, sc)
        {
            TomatoRb = tomatoRigidBody;
            Color = new float4(0.5f, 0.1f, 0.1f, 1);
            Radius = 2;
            Tag = "Tomato";
            timer = 2.0f;
            _game = game;
            _world = world;
        }


        public override void Update()
        {
            if (TomatoRb != null)
            {
                ObjectMtx *= float4x4.CreateTranslation(TomatoRb.Position);
            }
            
            timer -= Time.Instance.DeltaTime;
            if (timer <= 0)
            {
                DeleteMe();
                

            }
        }

      /*  public override void Render(float4x4 camMtx)
        {

            _rc.ModelView = camMtx * ObjectMtx* float4x4.Scale(0.02f);
            SceneRenderer.Render(_rc);
        }*/

        public override void Collided()
        {
            _world.RemoveRigidBody(TomatoRb);
            //TomatoRb = null;
        }


        private void DeleteMe()
        {
            //TomatoRb = null;
            _world.RemoveRigidBody(TomatoRb);
            _game.LevelObjects.Remove(this);
            
        }

    }
}
