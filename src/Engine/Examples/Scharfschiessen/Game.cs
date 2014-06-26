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
        public Weapon Weapon;

        public SceneLoader SceneLoader { get; private set; }
    
        private readonly SceneRenderer srTomato;
        private readonly SceneRenderer srSheep;
        private readonly SceneRenderer srSkybox;
        private readonly SceneRenderer srLandschaft;
        private readonly SceneRenderer srTrees;
        private readonly SceneRenderer srChicken;
        private readonly SceneRenderer srCows ;
        private readonly SceneRenderer srBuildings;
        
        // Gibt die altuelle Punktzahl an
        public int Points { get; set; }
        private Skybox _skybox;

        
        public Game(GameHandler gh,RenderContext rc)
        {
            _gameHandler = gh;
            RC = rc;
            SceneLoader = new SceneLoader();
            srTomato = SceneLoader.LoadTomato();
            srSheep = SceneLoader.LoadSheep();
            srTrees = SceneLoader.LoadTrees();
            //srChicken = SceneLoader.LoadChicken();
            srCows = SceneLoader.LoadCows();
            srLandschaft = SceneLoader.LoadEnvironment();
            srBuildings = SceneLoader.LoadBuildings();
            CreateEnvironment();
            Points = 0;
            LoadLevel(1);
            _skybox = new Skybox(RC);
            
        }

       

        public void LoadLevel(int i)
        {
            _active = true;
            DisposePhysic();
            World = new DynamicWorld();
            SphereCollider = World.AddSphereShape(1);
            Level = i;
            Countdown = 30;
            Weapon = new Weapon(World, this);
        }

        //private Mesh mesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");

        private void CreateEnvironment()
        {
            var houses = new GameObject(RC, null, new float3(0, 0, 0), float3.Zero, new float3(1f, 1f, 1f), srBuildings);
            houses.SetTexture("GebäudeOberflächenfarbe");
            LevelObjects.Add(houses);
            var cows = new GameObject(RC, null, new float3(0, 0, 0), new float3(0,-20,0), new float3(1f, 1f, 1f), srCows);
            cows.SetTexture("KuhOberflächenfarbe");
            LevelObjects.Add(cows);
            var cows1 = new GameObject(RC, null, new float3(-20, 0, 0), new float3(0,-180,0), new float3(1f, 1f, 1f), srCows);
            cows1.SetTexture("KuhOberflächenfarbe");
            LevelObjects.Add(cows1);
            var trees = new GameObject(RC, null, new float3(0, -50, 0), new float3(0, 0, 0), new float3(1.5f, 1.5f, 1.5f), srTrees);
            trees.SetTexture("treesOberflächenfarbe");
            LevelObjects.Add(trees);
            var ebene = new GameObject(RC, null, new float3(0, -100, 0), float3.Zero, new float3(20,1,20), srLandschaft);
            ebene.SetTexture("EbeneOberflächenfarbe");
            LevelObjects.Add(ebene);
            var sheep1 = new Sheep(RC, _meshSheep, new float3(50, 0,50), float3.Zero, new float3(0.02f, 0.02f, 0.02f), srSheep, this);
            LevelObjects.Add(sheep1);
            var sheep2 = new Sheep(RC, _meshSheep, new float3(-50, 0, 10), float3.Zero, new float3(0.02f, 0.02f, 0.02f), srSheep, this);
            LevelObjects.Add(sheep2);
            var sheep3 = new Sheep(RC, _meshSheep, new float3(-150, 0, 80), float3.Zero, new float3(0.02f, 0.02f, 0.02f), srSheep, this);
            LevelObjects.Add(sheep3);
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

            RenderObjects();
            CollisionUpdate();
            UpdateLevelObjectList();
           // Debug.WriteLine(Time.Instance.FramePerSecond);
        }

        
        private void RenderObjects()
        {
            for (int i = 0; i < LevelObjects.Count; i++)
            {
                LevelObjects[i].Render(_mtxCam);
                //muss nach LevelObjects[i].Render() aufgerufen werden, da sonst LevelObjects[i] out of range passieren kann
                LevelObjects[i].Update();
            }
            _skybox.Render(_mtxCam);
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

        private void CollisionUpdate()
        {
            for (int t = 0; t < LevelObjects.Count; t++)
            {
                if (LevelObjects[t] != null && LevelObjects[t].Tag == "ActionObject")
                {
                    for (int i = 0; i < LevelObjects.Count; i++)
                    {
                        if (LevelObjects[i] != null && LevelObjects[i].Tag == "ActionObject" && LevelObjects[t] != LevelObjects[i])
                        {
                            if (CheckForCollision(LevelObjects[t], LevelObjects[i]))
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
            }
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
            if (_angleVert >= 0.9f)
            {
                _angleVert = 0.9f;
            }
            if (_angleVert <= -0.8f)
            {
                _angleVert = -0.8f;
            }

            _rotY = float4x4.CreateRotationY(_angleHorz);
            _rotX = float4x4.CreateRotationX(_angleVert);
            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);

            _mtxCam = mtxRot * float4x4.LookAt(0, 40, -1, 0, 40, 1, 0, 1, 0);
            _mtxCam *= float4x4.CreateTranslation(0, 40, -1);
            //Schiessen
            Weapon.WeaponInput(_mtxCam);
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
