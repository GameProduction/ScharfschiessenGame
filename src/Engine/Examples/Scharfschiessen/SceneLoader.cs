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
        private SceneRenderer _sr;

        public SceneRenderer LoadEnvironment()
        {
            return Loader("Landschaft1");
        }

        public SceneRenderer LoadTomato()
        {
            return Loader("tomato");
        }

        public SceneRenderer LoadSheep()
        {
            return Loader("schaf");
        }
        public SceneRenderer LoadCows()
        {
            return Loader("kühe");
        }

        public SceneRenderer LoadTrees()
        {
            return Loader("trees");
        }

        public SceneRenderer LoadBuildings()
        {
            return Loader("Gebäude");
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
            return _sr;
        }
    }
}
