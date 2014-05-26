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
        internal readonly RenderContext Rc;
        internal readonly Mesh Mesh;
        private float3 position;
        private float4x4 orientation;

        // variables for shader
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;

        public GameObject(RenderContext rc, Mesh mesh, float3 position, float4x4 orientation)
        {
            Rc = rc;
            _spColor = MoreShaders.GetDiffuseColorShader(Rc);
            _colorParam = _spColor.GetShaderParam("color");
            Mesh = mesh;
            this.orientation = orientation;
            this.position = position;
        }

        public void Update()
        {
            //setPosition
            //setOrientaion
        }

        public void Render(float4x4 camMtx)
        {
            Rc.ModelView = camMtx * orientation * float4x4.CreateTranslation(position.x, position.y, position.z) * float4x4.Scale(0.2f);
            Rc.SetShader(_spColor);
            Rc.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            Rc.Render(Mesh);
        }
    }
}
