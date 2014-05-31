using System;
using System.Collections.Generic;
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
    
        private float4x4 _orientation;

        public int Radius { get; set; }
        private float _scale = 1;

        // variables for shader
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;

        public GameObject(RenderContext rc, Mesh mesh, float3 position, float4x4 orientation, float scaleFactor)
        {
            _rc = rc;
            _spColor = MoreShaders.GetDiffuseColorShader(_rc);
            _colorParam = _spColor.GetShaderParam("color");
            _mesh = mesh;
            _scale = scaleFactor;
            _orientation = orientation;
            Position = position;
        }

        public virtual void Update()
        {
            //setPosition
            //setOrientaion
        }

        public void Render(float4x4 camMtx)
        {
            _rc.ModelView = camMtx * _orientation * float4x4.CreateTranslation(Position.x, Position.y, Position.z) * float4x4.Scale(_scale);
            _rc.SetShader(_spColor);
            _rc.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            _rc.Render(_mesh);
        }
    }
}
