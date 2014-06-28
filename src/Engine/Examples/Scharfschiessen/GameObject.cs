using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class GameObject
    {
        internal readonly RenderContext _rc;

        public float3 Position
        {
            get { return ObjectMtx.Column3.xyz; }
        }

        internal float3 Rotation { get; set; }
        internal float4x4 ObjectMtx;

        public float Radius { get; set; }
        private float3 _scale = new float3(1,1,1);

        public String Tag { get; internal set; }
        internal float4 Color;
        internal SceneRenderer SceneRenderer;

        public GameObject(RenderContext rc, float3 position, float3 rotation, float3 scaleFactor, SceneRenderer sc)
        {
            SceneRenderer = sc;
            _rc = rc;

            _scale = scaleFactor;
            ObjectMtx = float4x4.CreateRotationX(rotation.x)
                     *float4x4.CreateRotationY(rotation.y)
                     *float4x4.CreateRotationZ(rotation.z)
                     *float4x4.CreateTranslation(position);

            Rotation = rotation;
            Tag = "GameObject";
        }

        public virtual void Update()
        {

        }

        public virtual void Collided()
        {
            Debug.WriteLine("GameObject Collided");
        }

        public virtual void Render(float4x4 camMtx)
        {
            _rc.ModelView = camMtx * ObjectMtx * float4x4.Scale(_scale);
            SceneRenderer.Render(_rc);
        }
    }
}
