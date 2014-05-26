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
        
        public Game(RenderContext rc)
        {
            _rc = rc;
            CreateLevel();
            _active = true;
            Debug.WriteLine("new Game");
            //init
            Level = 1;
            
            
        }


        private Mesh mesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");

        private void CreateLevel()
        {
            var go = new GameObject(_rc, mesh, new float3(0,0,250), float4x4.Identity);
            LevelObjects.Add(go);
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
            }
            else
            {
                //MenuInput();
            }

            foreach (GameObject t in LevelObjects)
            {
                t.Render(_mtxCam);
            }
        }

        public double GetTime()
        {
            return _countdown;
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
            if (_angleVert >= 0.1f)
            {
                _angleVert = 0.1f;
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

            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            _mtxCam = _rotX * _rotY * float4x4.LookAt(0, 1, -1, 0, 1, 1, 0, 1, 0);


            //Schiessen
            if (Input.Instance.IsButton(MouseButtons.Left))
            {

                Debug.WriteLine("Shoot");
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
    }
}
