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
        private SceneRenderer _sceneRenderer;
        private ShaderProgram _skyBoxShaderParam;
        private ShaderProgram TextureSp;
        public Skybox(RenderContext rc, SceneRenderer sceneRenderer)
        {
            // load texture
            _sceneRenderer = sceneRenderer;
            var imgData = rc.LoadImage("Assets/skyboxOberflächenfarbe.jpg");
            _iTex = rc.CreateTexture(imgData);
            _rc = rc;

        }



        public void Render(float4x4 camMtx)
       {
           _rc.ModelView = camMtx*float4x4.CreateTranslation(0,0,0) * float4x4.Scale(100,50,100);
           _sceneRenderer.Render(_rc);
        }
    }
}
