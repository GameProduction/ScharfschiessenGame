using System.Linq.Expressions;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Physics
    {
        private DynamicWorld _world;
        public DynamicWorld World
        {
            get { return _world; }
            set { _world = value; }
        }

        public SphereShape sphereCollider;
        public BoxShape floorCollider;
        public Physics()
        {
            World = new DynamicWorld();

            sphereCollider = World.AddSphereShape(1);
            floorCollider = World.AddBoxShape(300, 0.1f, 300);
            World.AddRigidBody(0, new float3(0, 0, 0), float3.Zero, floorCollider);
        }
    }
}
