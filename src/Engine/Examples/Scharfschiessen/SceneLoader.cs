using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using Fusee.Serialization;

namespace Examples.Scharfschiessen
{
    public class SceneLoader
    {
        public SceneRenderer _sr;
        private float4x4 _modelScaleOffset;

        private Mesh _meshTomtato;

        public void LoadEnvironment()
        {
            //return Load("environment");
        }

        public SceneRenderer LoadTomato()
        {
            return Loader("tomato");
        }

        public SceneRenderer LoadSheep()
        {
            return Loader("schaf");
        }


        private SceneRenderer Loader(string name)
        {
            // Scene loading
            SceneContainer scene;
            var ser = new Serializer();
            string filename = "Assets/" + name + ".fus";
            using (var file = File.OpenRead(@filename))
            {
                scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
            }
            _sr = new SceneRenderer(scene, "Assets");
            AdjustModelScaleOffset();
            return _sr;
        }

        public void AdjustModelScaleOffset()
        {
            AABBf? box = null;
            if (_sr == null || (box = _sr.GetAABB()) == null)
            {
                _modelScaleOffset = float4x4.Identity;
            }
            var bbox = ((AABBf)box);
            float scale = Math.Max(Math.Max(bbox.Size.x, bbox.Size.y), bbox.Size.z);
            _modelScaleOffset = float4x4.CreateScale(200.0f / scale) * float4x4.CreateTranslation(-bbox.Center);
        }

        

    }
}
