using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Game
    {
        private readonly RenderContext _rc;

        public int Level { get; set; }
        private double _countdown = 30;
        private bool _active;
        private float4x4 _mtxCam;
       
        private List<GameObject> LevelObjects = new List<GameObject>();
        private Mesh _meshTomtato;
        private Mesh _meshSheep;
        public DynamicWorld World { get; set; }
        private SphereShape _sphereCollider;
        
        public Game(RenderContext rc)
        {           
            _rc = rc;
            _meshTomtato = MeshReader.LoadMesh(@"Assets/Tomato.obj.model");
            CreateEnvironment();
            
            _meshSheep = MeshReader.LoadMesh(@"Assets/Cube.obj.model"); 
            Debug.WriteLine("new Game");
            LoadLevel(1);
        }

        public void LoadLevel(int i)
        {
            
            _active = true;
            DisposePhysic();
            World = new DynamicWorld();
            _sphereCollider = World.AddSphereShape(1);
            Level = i;

        }

        private Mesh mesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");

        private void CreateEnvironment()
        {
            var tomato = new Tomato(_rc, _meshTomtato, new float3(0, 0,50), float4x4.Identity, 0.2f);
            LevelObjects.Add(tomato);
            var go = new GameObject(_rc, mesh, new float3(0,0,250), float4x4.Identity, 0.2f);
            LevelObjects.Add(go);
            var sheep = new Sheep(_rc, mesh, new float3(0, 0, 250), float4x4.Identity, 0.02f);
            LevelObjects.Add(sheep);
        }


        public void Update()
        {
            if(_active)
            {
                if (_countdown > 0)
                {
                    _countdown -= Time.Instance.DeltaTime;
                }
                else
                {
                    _active = false;
                }

                PlayerInput();
                if (World != null)
                {
                    World.StepSimulation((float)Time.Instance.DeltaTime, (Time.Instance.FramePerSecondSmooth / 60), 1 / 60);
                }
            }

            foreach (GameObject t in LevelObjects)
            {
                t.Render(_mtxCam);
                t.Update();
                foreach (GameObject u in LevelObjects)
                {
                    if(t != u && CheckForCollision(t, u))
                    {
                        //Partikeleffect an t.Position
                        //Punkte hochzählen
                    }
                }

            }
        }

        public double GetTime()
        {
            return _countdown;
        }

        private bool CheckForCollision(GameObject gameObject1, GameObject gameObject2)
        {
            var dist = (gameObject1.Position - gameObject2.Position).Length;
            if (dist <= gameObject1.Radius + gameObject2.Radius )
            {
                return true;
            }
            return false;
        }

        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;
        private float4x4 _rotY = float4x4.Identity;
        private float4x4 _rotX = float4x4.Identity;
        private const float RotationSpeed = 0.5f;
        public void PlayerInput()
        {
            // move per mouse
            _angleVelHorz = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
            _angleVelVert = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY); 

            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;

            //Bewegungsferiheit an Fadenkeruz anpassen
            if (_angleVert >= 0.2f)
            {
                _angleVert = 0.2f;
            }
            if (_angleVert <= -0.05f)
            {
                _angleVert = -0.05f;
            }

            // move per keyboard
            if (Input.Instance.IsKey(KeyCodes.Left))
                _angleHorz -= RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Right))
                _angleHorz += RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Up))
                _angleVert -= RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Down))
                _angleVert += RotationSpeed * (float)Time.Instance.DeltaTime;

            _rotY = float4x4.CreateRotationY(_angleHorz);
            _rotX = float4x4.CreateRotationX(_angleVert);

           
            _mtxCam = _rotX * _rotY * float4x4.LookAt(0, 1, -1, 0, 1, 1, 0, 1, 0);


            //Schiessen
            if (Input.Instance.IsButton(MouseButtons.Left))
            {

                Shoot();
                /* if (Weapon.Magazin > 0)
                 {
                     Debug.WriteLine("Shoot");
                 * Weapon.Shoot();
                     //magazin--;
                 }
                 else
                 {
                     Debug.WriteLine("REALOAD!!!");
                 }*/
            }
            //Nachladen
            if (Input.Instance.IsButton(MouseButtons.Right))
            {
                Debug.WriteLine("Reload");
                //Weapon.Reaload();
            }
        }

        
        public void Shoot()
        {
            Debug.WriteLine("Shoot");

            var tomato = new Tomato(_rc, _meshTomtato, new float3(0, 10, 0), float4x4.Identity, 0.01f);
            LevelObjects.Add(tomato);
            tomato.ShootTomato(World, _mtxCam, _sphereCollider);
        }

        public void DisposePhysic()
        {
            if (World != null)
            {
                World.Dispose();
            }
        }
    }
}
