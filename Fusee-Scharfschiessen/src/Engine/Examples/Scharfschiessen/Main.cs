using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Forms;
using Fusee.Engine;
using Fusee.SceneManagement;
using Fusee.Math;
using MouseButtons = Fusee.Engine.MouseButtons;

namespace Examples.Scharfschiessen
{
    public class Scharfschiessen : RenderCanvas
    {
        // angle variables
        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;
        private float4x4 rotY = float4x4.Identity;
        private float4x4 rotX = float4x4.Identity;
        private const float RotationSpeed = 0.71f;
        private const float Damping = 0.75f;

        internal static Physics _physics;
        public Physics Physics
        {
            get { return _physics; }
            set { _physics = value; }
        }

        private Weapon _weapon;
        public Weapon Weapon
        {
            get { return _weapon; }
            set { _weapon = value; }
        }
        // model variables
        private Mesh _meshCube, _meshSphere;


        // variables for shader
        private ShaderProgram _spColor;

        private IShaderParam _colorParam;


        
        // is called on startup
        public override void Init()
        {
            Debug.WriteLine(System.Windows.SystemParameters.PrimaryScreenWidth);
            Width = (int) System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;
            Height = (int) (System.Windows.SystemParameters.PrimaryScreenHeight*2/9);
            //var dpi =  Graphics.Display.DisplayInformation.GetForCurrentView().ResolutionScale;
            var graph = Graphics
            //RC.ClearColor = new float4(0.8f, 0.8f, 1, 1);
            
            // initialize the variables
            _meshCube = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshSphere = MeshReader.LoadMesh(@"Assets/Sphere.obj.model");
            _spColor = MoreShaders.GetDiffuseColorShader(RC);    

            _colorParam = _spColor.GetShaderParam("color");

            //init physics
            Physics = new Physics();
            Weapon = new Weapon(ref _physics);
        }

        
        // is called once a frame
        public override void RenderAFrame()
        {
            

            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            Physics.World.StepSimulation((float)Time.Instance.DeltaTime, (Time.Instance.FramePerSecondSmooth / 60), 1 / 60);

            // move per mouse
            if (Input.Instance.IsButton(MouseButtons.Left))
            {
                _angleVelHorz = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
                _angleVelVert = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY);
                
            }
            else
            {
                var curDamp = (float)Math.Exp(-Damping * Time.Instance.DeltaTime);

                _angleVelHorz *= curDamp;
                _angleVelVert *= curDamp;
            }

            //_angleVelHorz = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
            //_angleVelVert = RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY);

            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;
   
            /*if (_angleVert >= 0.9f)
            {
                _angleVert = 0.9f;
            }
            if (_angleVert <= -0.9f)
            {
                _angleVert = -0.9f;
            }*/

            
            // move per keyboard
            if (Input.Instance.IsKey(KeyCodes.Left))
                _angleHorz -= RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Right))
                _angleHorz += RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Up))
                _angleVert -= RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Down))
                _angleVert += RotationSpeed * (float)Time.Instance.DeltaTime;

            // Row order notation
            var mtxRot_ROW = float4x4.CreateRotationY_ROW(_angleHorz) * float4x4.CreateRotationX_ROW(_angleVert);
            // Column order notation
            rotY = float4x4.CreateRotationY(_angleHorz);
            rotX = float4x4.CreateRotationX(_angleVert);
            
            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            //Debug.Assert(mtxRot_ROW == float4x4.Transpose(mtxRot));
            //Debug.WriteLine(mtxRot);
            // Row order notation
           // var mtxCam_ROW = float4x4.LookAt_ROW(0, 500, -900, 0, 0, 0, 0, 1, 0);

           // mtxCam_ROW = float4x4.Transpose(mtxCam_ROW);
            var mtxCam = rotX * rotY * float4x4.LookAt(0, 1, -1, 0, 1, 1, 0, 1, 0);
            if (Input.Instance.IsKeyUp(KeyCodes.Space))
            {
                //Debug.WriteLine("leftMouse");
                Weapon.Shoot(mtxCam);
            }
            // Column order notation
            //RC.ModelView = float4x4.CreateTranslation(0, -50, 0)*mtxRot*float4x4.CreateTranslation(-150, 0, 0)*mtxCam;
            // Debug.Assert(mtxCam_ROW == float4x4.Transpose(mtxCam));

            RC.SetShader(_spColor);

            // first mesh
            // Row order notation
            var modelViewMesh1 = mtxCam * float4x4.CreateTranslation(0, 20, 200)*float4x4.CreateScale(0.2f, 0.2f, 0.2f);
            RC.ModelView = modelViewMesh1;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));

            RC.Render(_meshCube);
            RenderRb(mtxCam);
            // swap buffers
            Present();
        }

        public void RenderRb(float4x4 mtxcam)
        {    
            for (int i = 0; i < Physics.World.NumberRigidBodies(); i++)
            {
                var rb = Physics.World.GetRigidBody(i);
                var matrix = float4x4.Transpose(rb.WorldTransform);
               
                if (rb.CollisionShape.GetType().ToString() == "Fusee.Engine.BoxShape")
                {
                    var shape = (BoxShape) rb.CollisionShape;
                    RC.ModelView = mtxcam*matrix*
                                   float4x4.Scale(shape.HalfExtents.x/100, shape.HalfExtents.y/100,
                                       shape.HalfExtents.z/100);


                    RC.SetShader(_spColor);
                    RC.SetShaderParam(_colorParam, new float4(0.9f, 0.9f, 0.0f, 1));
                    RC.SetRenderState(new RenderStateSet {AlphaBlendEnable = false, ZEnable = true});
                    RC.Render(_meshCube);
                }
                else if (rb.CollisionShape.GetType().ToString() == "Fusee.Engine.SphereShape")
                {
                    var shape = (SphereShape) rb.CollisionShape;
                    RC.ModelView = mtxcam*matrix*float4x4.Scale(shape.Radius);
                    RC.SetShader(_spColor);
                    RC.SetShaderParam(_colorParam, new float4(0.9f, 0.0f, 0.0f, 1));
                    RC.SetRenderState(new RenderStateSet {AlphaBlendEnable = false, ZEnable = true});
                    RC.Render(_meshSphere);
                }
            }
        }



        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0,0, Width, Height);
            
            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
        }

        public static void Main()
        {
            var app = new Scharfschiessen();
            app.Run();
            _physics.World.Dispose();
        }
    }
}
