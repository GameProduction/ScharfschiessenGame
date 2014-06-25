﻿using System;
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


        public RigidBody TomatoRB { get; set; }
        private double timer;
        private bool active;
        private Game _game;
        public Tomato(RenderContext rc, Mesh mesh, float3 position, float3 rotation, float3 scaleFactor,SceneRenderer sc, RigidBody tomatoRigidBody, ImageData imgData, Game game)
            : base(rc, mesh, position, rotation, scaleFactor, sc)
        {
            TomatoRB = tomatoRigidBody;
            Color = new float4(0.5f, 0.1f, 0.1f, 1);
            Radius = 2;
            _iTex = rc.CreateTexture(imgData);
            Tag = "ActionObject";
            timer = 1.0f;
            _game = game;
        }


        public override void Update()
        {
            if (TomatoRB != null)
            {
                ObjectMtx *= float4x4.CreateTranslation(TomatoRB.Position);
            }
            
            timer -= Time.Instance.DeltaTime;
            if (timer <= 0)
            {
                DeleteMe();
                

            }
        }

        public override void Render(float4x4 camMtx)
        {

            _rc.ModelView = camMtx * ObjectMtx* float4x4.Scale(0.02f);
            SceneRenderer.Render(_rc);
 
            
        }
        public override void Collided()
        {
            TomatoRB = null;
        }


        private void DeleteMe()
        {
            TomatoRB = null;
            _game.LevelObjects.Remove(this);
        }

    }
}
