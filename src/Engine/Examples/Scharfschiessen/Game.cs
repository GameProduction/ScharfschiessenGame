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
        public RenderContext RC { get; private set; }
    

        public int Level { get; set; }
        public double Countdown { get; set; }
        private bool _active;
        private float4x4 _mtxCam;
        private GameHandler _gameHandler;
        public List<GameObject> LevelObjects = new List<GameObject>();
        private Mesh _meshTomtato;
        private Mesh _meshSheep;
        public DynamicWorld World { get; set; }
        public  SphereShape SphereCollider { get; private set; }
        private List<Sheep> SheepList = new List<Sheep>();
        private Weapon _weapon;

        public SceneLoader SceneLoader { get; private set; }
    
        private readonly SceneRenderer srTomato;
        private readonly SceneRenderer srSheep;
        
        // Gibt die altuelle Punktzahl an
        public int Points { get; set; }

        public Game(GameHandler gh,RenderContext rc)
        {
            _gameHandler = gh;
            RC = rc;
            SceneLoader = new SceneLoader();
            srTomato = SceneLoader.LoadTomato();
            srSheep = SceneLoader.LoadSheep();
            CreateEnvironment();
            Points = 0;
            LoadLevel(1);
            
        }

       

        public void LoadLevel(int i)
        {
            _active = true;
            DisposePhysic();
            World = new DynamicWorld();
            SphereCollider = World.AddSphereShape(1);
            Level = i;
            Countdown = 30;
            _weapon = new Weapon(World, this);
        }

        //private Mesh mesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");

        private void CreateEnvironment()
        {
            //var tomato = new Tomato(_rc, _meshTomtato, new float3(0, 0, 60), float3.Zero, 0.2f, this, srTomato);
            //LevelObjects.Add(tomato);
            //var go = new GameObject(_rc, mesh, new float3(0, 0, 250), float3.Zero, 0.2f, this);
            //LevelObjects.Add(go);
            var sheep1 = new Sheep(RC, _meshSheep, new float3(0, 0,10), float3.Zero, 0.02f, this, srSheep);
            LevelObjects.Add(sheep1);
        }

        
        //posiotion für neues Schaf
        public void FindPosition(out float3 pos, out float3 rot)
        {
            pos = new float3(0, 0, 0);
            rot = new float3(0, 0, 0);
            //Rechenen
        }
        //neues Schaf wird gespawned
        public void InstantiateSheep()
        {
            float3 pos = float3.Zero;
            float3 rot = float3.Zero;
            FindPosition(out pos, out rot);
            //var sheep = new Sheep(_rc, _meshSheep, pos, rot, 0.02f, this);
            //LevelObjects.Add(sheep);
        }


        

        public void Update()
        {
            
            if (_active)
            {
                if (World != null)
                {
                    World.StepSimulation((float)Time.Instance.DeltaTime, (Time.Instance.FramePerSecondSmooth / 60), 1 / 60);
                }
                if (Countdown > 0)
                {
                    Countdown -= Time.Instance.DeltaTime;
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

            _angleVelHorz = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
            _angleVelVert = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY);
            
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

            _mtxCam = mtxRot * float4x4.LookAt(0, 1, -1, 0, 1, 1, 0, 1, 0); 
           
            //Schiessen
            _weapon.WeaponInput(_mtxCam);
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
