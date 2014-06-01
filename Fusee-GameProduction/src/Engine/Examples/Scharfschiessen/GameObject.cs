using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class GameObject
    {
        private readonly RenderContext _rc;
        private readonly Mesh _mesh;
        public float3 Position { get; set; }

        internal Game Game;
        private float4x4 _orientation;

        public int Radius { get; set; }
        private float _scale = 1;

        // variables for shader
        private ShaderEffect _shaderEffect;
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;
        internal float4 Color;

        public GameObject(RenderContext rc, Mesh mesh, float3 position, float4x4 orientation, float scaleFactor, Game game)
        {
            this.Game = game;
            _rc = rc;
            _spColor = MoreShaders.GetDiffuseColorShader(_rc);
            _colorParam = _spColor.GetShaderParam("color");
            _mesh = mesh;
            _scale = scaleFactor;
            _orientation = orientation;
            Position = position;
            Color = new float4(0.5f, 0.2f, 0.4f, 1);
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

        public void Render(float4x4 camMtx)
        {
            _rc.ModelView = camMtx * _orientation * float4x4.CreateTranslation(Position.x, Position.y, Position.z) * float4x4.Scale(_scale);
            _rc.SetShader(_spColor);
            _rc.SetShaderParam(_colorParam, Color);
            _rc.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
            _rc.Render(_mesh);
        }
    }
}
