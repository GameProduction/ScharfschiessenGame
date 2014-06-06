using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Math;
using System.IO;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using Fusee.Serialization;

namespace Examples.Scharfschiessen
{
    public class Game
    {
        private readonly RenderContext _rc;

        public int Level { get; set; }
        private static double _countdown = 30;
        private bool _active;
        private float4x4 _mtxCam;
        private GameHandler _gameHandler;
        private List<GameObject> LevelObjects = new List<GameObject>();
        private Mesh _meshTomtato;
        private Mesh _meshSheep;
        public DynamicWorld World { get; set; }
        private SphereShape _sphereCollider;
        
        // Gibt die altuelle Punktzahl an
        public int Points { get; set; }
        // Zeigt verbleibende Zeit an
        public static double GetTime()
        {
            return _countdown;
        }

        public Game(GameHandler gh,RenderContext rc)
        {
            _gameHandler = gh;
            _rc = rc;
            _meshTomtato = MeshReader.LoadMesh(@"Assets/Tomato.obj.model");
            _meshSheep = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            CreateEnvironment();
            Points = 0;
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
            /*var tomato = new Tomato(_rc, _meshTomtato, new float3(0, 0, 50), float4x4.Identity, 0.2f, this);
            LevelObjects.Add(tomato);*/
            var go = new GameObject(_rc, mesh, new float3(0, 0, 250), float4x4.Identity, 0.2f, this);
            LevelObjects.Add(go);
            var sheep1 = new Sheep(_rc, _meshSheep, new float3(50, 0, 100), float4x4.Identity, 0.02f, this);
            LevelObjects.Add(sheep1);

            var sheep2 = new Sheep(_rc, _meshSheep, new float3(-50, 0, 50), float4x4.Identity, 0.02f, this);
            LevelObjects.Add(sheep2);
        }


        public void Update()
        {
            if (_active)
            {
                if (World != null)
                {
                    World.StepSimulation((float)Time.Instance.DeltaTime, (Time.Instance.FramePerSecondSmooth / 60), 1 / 60);
                }
                if (_countdown > 0)
                {
                    _countdown -= Time.Instance.DeltaTime;
                }
                else
                {
                    _active = false;
                    _gameHandler.GameState.CurrentState = GameState.State.Highscore;
                }

                PlayerInput();
                
            }

            for (int t = 0; t < LevelObjects.Count; t++)
            {
                if (LevelObjects[t] != null)
                {
                    LevelObjects[t].Render(_mtxCam);
                    LevelObjects[t].Update();
                
                
                    for (int i = 0; i < LevelObjects.Count; i++)
                    {
                        if (LevelObjects[i] != null && LevelObjects[t] != LevelObjects[i] && CheckForCollision(LevelObjects[t], LevelObjects[i]))
                        {
                            LevelObjects[t].Collided();
                            LevelObjects[i].Collided();
                            var p1 = LevelObjects.IndexOf(LevelObjects[t]);
                            var p2 = LevelObjects.IndexOf(LevelObjects[i]);
                            LevelObjects[p1] = null;
                            LevelObjects[p2] = null;
                            
                        }
                    }
                }
            }
            UpdateLevelObjectList();
        }

        private void UpdateLevelObjectList()
        {
            List<GameObject> helper = new List<GameObject>();
            foreach (var levelObject in LevelObjects)
            {
                if (levelObject != null)
                {
                    helper.Add(levelObject);
                }
            }
            LevelObjects.Clear();
            LevelObjects = helper;
            helper = null;
        }
    
        private bool CheckForCollision(GameObject gameObject1, GameObject gameObject2)
        {
            if (gameObject1 != null && gameObject2 != null)
            {
                var dist = (gameObject1.Position - gameObject2.Position).Length;
                if (dist <= gameObject1.Radius + gameObject2.Radius)
                {
                    return true;
                }
            }
                
            return false;
        }

        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;
        private float4x4 _rotY = float4x4.Identity;
        private float4x4 _rotX = float4x4.Identity;
        private const float RotationSpeed = 0.71f;

        public void PlayerInput()
        {
            //if (Input.Instance.IsButton(MouseButtons.Left))
           // {
                _angleVelHorz = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
                _angleVelVert = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY);
            //}
            // move per mouse
            //_angleVelHorz = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
            //_angleVelVert = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY); 
            
            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;
            
            //Bewegungsferiheit an Fadenkeruz anpassen
            if (_angleVert >= 0.5f)
            {
                _angleVert = 0.5f;
            }
            if (_angleVert <= -0.1f)
            {
                _angleVert = -0.1f;
            }

            _rotY = float4x4.CreateRotationY(_angleHorz);
            _rotX = float4x4.CreateRotationX(_angleVert);
            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);

            var mtxCam = mtxRot * float4x4.LookAt(0, 1, -1, 0, 1, 1, 0, 1, 0); _mtxCam = mtxCam;
            
            //Schiessen
            if (Input.Instance.IsKeyUp(KeyCodes.Space))
            {
                Shoot(mtxCam);
            }
        }
       
        public void Shoot(float4x4 camMtx)
        {
            var tomato = new Tomato(_rc, _meshTomtato, new float3(camMtx.Row3), float4x4.Identity, 0.01f, this);
            LevelObjects.Add(tomato);
            tomato.ShootTomato(World, camMtx, _sphereCollider);
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
