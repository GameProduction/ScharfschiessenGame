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
        private readonly Mesh _mesh;
        internal float3 Position { get; set; }
        internal float3 Rotation { get; set; }
        internal float4x4 ObjectMtx;

        public int Radius { get; set; }
        private float3 _scale = new float3(1,1,1);

        public String Tag { get; internal set; }
        internal ITexture _iTex;
        internal float4 Color;
        internal SceneRenderer SceneRenderer;

        public GameObject(RenderContext rc, Mesh mesh, float3 position, float3 rotation, float3 scaleFactor, SceneRenderer sc)
        {
            SceneRenderer = sc;
            _rc = rc;
            _mesh = mesh;
            _scale = scaleFactor;
            ObjectMtx = float4x4.CreateRotationX(rotation.x)
                     *float4x4.CreateRotationY(rotation.y)
                     *float4x4.CreateRotationZ(rotation.z)
                     *float4x4.CreateTranslation(position);

            Position = position;
            Rotation = rotation;
            Tag = "GameObject";
        }

        public void SetTexture(string name)
        {

            var imgData = _rc.LoadImage("Assets/"+name+".jpg");
            _iTex = _rc.CreateTexture(imgData);
        }

        public virtual void Update()
        {
           
            //setPosition
            //setOrientaion
        }

        public virtual void Collided()
        {
            Debug.WriteLine("GameObject Collided");
        }

        public virtual void Render(float4x4 camMtx)
        {
            _rc.ModelView = camMtx * ObjectMtx/* float4x4.CreateTranslation(Position.x, Position.y, Position.z) */* float4x4.Scale(_scale);
         
           // _rc.SetShader(_spTexture);
           // _rc.SetShaderParamTexture(_textureParam, _iTex);
            //_rc.SetShader(_spColor);
            //_rc.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            //_rc.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
            //_rc.Render(_mesh);
            
            SceneRenderer.Render(_rc);

        }
    }
}
