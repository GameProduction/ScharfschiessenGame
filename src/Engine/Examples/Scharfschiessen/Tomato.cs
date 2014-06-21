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
        

        private RigidBody _tomatoRB;
        public Tomato(RenderContext rc, Mesh mesh, float3 position, float3 rotation, float3 scaleFactor, Game game, SceneRenderer sc, RigidBody tomatoRigidBody)
            : base(rc, mesh, position, rotation, scaleFactor, game, sc)
        {
            _tomatoRB = tomatoRigidBody;
            Color = new float4(0.5f, 0.1f, 0.1f, 1);
            Radius = 2;
            // load texture
            var imgData = rc.LoadImage("Assets/TomateOberflächenfarbe.jpg");
            _iTex = rc.CreateTexture(imgData);
        }


        public override void Update()
        {
        
            if (_tomatoRB != null)
            {
                Position = _tomatoRB.Position;
                ObjectMtx *= float4x4.CreateTranslation(Position);
            }
        }

        public override void Render(float4x4 camMtx)
        {
            _rc.ModelView = camMtx * ObjectMtx* float4x4.Scale(0.02f);
            SceneRenderer.Render(_rc);
        }
        public override void Collided()
        {
            Debug.WriteLine("Tomato Colloided");
        }

    }
}
