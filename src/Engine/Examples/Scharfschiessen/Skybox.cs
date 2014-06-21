using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Skybox
    {
        private RenderContext _rc;
        private ITexture _iTex;
        private ShaderProgram _skyBoxShaderParam;
        private ShaderProgram TextureSp;
        private Mesh _syboxMesh;

        private ImageData imgData;
        public Skybox(RenderContext rc)
        {
            // load texture
           
            imgData = rc.LoadImage("Assets/skyboxOberflächenfarbe.jpg");
            _iTex = rc.CreateTexture(imgData);
            _rc = rc;
            _syboxMesh = MeshReader.LoadMesh(@"Assets/skybox.obj.model");
        }



        public void Render(float4x4 camMtx)
        {
            _rc.ModelView = camMtx*float4x4.CreateTranslation(0,0,0) * float4x4.Scale(100,50,100);
            _rc.SetShader(MoreShaders.GetSkyboxShader(_rc));
            _iTex = _rc.CreateTexture(imgData);
            _rc.Render(_syboxMesh);
        }
    }
}
