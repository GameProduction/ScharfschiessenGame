using System;
using System.Diagnostics;
using System.Windows.Forms;
using Fusee.Engine;
using Fusee.SceneManagement;
using Fusee.Math;
using System.Windows;
using MouseButtons = Fusee.Engine.MouseButtons;


namespace Examples.Scharfschiessen
{
    public class Scharfschiessen : RenderCanvas
    {
        internal Mesh Mesh;
        internal Mesh MeshCube;

        internal GameState GameState;

        private float rot;
        // variables for shader
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;


        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;
        private float4x4 rotY = float4x4.Identity;
        private float4x4 rotX = float4x4.Identity;
        private const float RotationSpeed = 0.71f;
        private const float Damping = 0.75f;


        // is called on startup
        public override void Init()
        {


            SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0, 0);

            rot = 0;
            RC.ClearColor = new float4(0.9f, 0.9f, 0.9f, 1);

            Mesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");
            MeshCube = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam = _spColor.GetShaderParam("color");

           
            InitGame();
            
        }

        internal void InitGame()
        {
            GameState = new GameState();
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);



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

            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;


            // move per keyboard
            if (Input.Instance.IsKey(KeyCodes.Left))
                _angleHorz -= RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Right))
                _angleHorz += RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Up))
                _angleVert -= RotationSpeed * (float)Time.Instance.DeltaTime;

            if (Input.Instance.IsKey(KeyCodes.Down))
                _angleVert += RotationSpeed * (float)Time.Instance.DeltaTime;

            rotY = float4x4.CreateRotationY(_angleHorz);
            rotX = float4x4.CreateRotationX(_angleVert);

            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            var mtxCam = rotX * rotY * float4x4.LookAt(0, 1, -1, 0, 1, 1, 0, 1, 0);
            
            // Column order notation
            //RC.ModelView = float4x4.CreateTranslation(0, -50, 0)*mtxRot*float4x4.CreateTranslation(-150, 0, 0)*mtxCam;
            // Debug.Assert(mtxCam_ROW == float4x4.Transpose(mtxCam));

            RC.SetShader(_spColor);

            // first mesh
            // Row order notation
            var modelViewMesh1 = mtxCam * float4x4.CreateTranslation(0, 10, 200)* float4x4.CreateRotationY(45) * float4x4.Scale(0.1f);
            RC.ModelView = modelViewMesh1;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));

            RC.Render(MeshCube);


            //test für rotierenes Mesh um Pause zu überprüfen nur zu Vorführungszwecken der pause Funktion
            /*var mtxCam = float4x4.LookAt(0, 0, -200, 0, 0, 0, 0, 1, 0);
            var mtxCam = float4x4.LookAt(0, 200, -500, 0, 0, 0, 0, 1, 0);*/
            rot += (float)Time.Instance.DeltaTime * 5;

            var modelView = mtxCam * mtxRot * float4x4.CreateTranslation(-100, 10, 200) * float4x4.CreateRotationX(rot) * float4x4.Scale(0.25f);
            RC.SetShader(_spColor);
            RC.ModelView = modelView;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            RC.Render(Mesh);

       
            TestInput();

            Present();
        }

        public void TestInput()
        {
            //TODO: Kann das besser orgneisiert werden? vllt über die GameState Klasse 
            if (Input.Instance.IsKeyUp(KeyCodes.B) && GameState.CurrentState != GameState.State.HiddenPause)
            {
                GameState.CurrentState = GameState.State.HiddenPause;
                SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height/5, true, 0,
                    -Screen.PrimaryScreen.Bounds.Height/5);
                Debug.WriteLine(GameState.CurrentState);
                Time.Instance.TimeFlow = 0;
            }
            else if (Input.Instance.IsKeyUp(KeyCodes.B))
            {
                GameState.CurrentState = GameState.LastState;
                SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0, 0);
                Time.Instance.TimeFlow = 1;
            }
            if (Input.Instance.IsKeyUp(KeyCodes.P) && GameState.CurrentState != GameState.State.HiddenPause)
            {
                GameState.CurrentState = GameState.State.HiddenPause;
                //SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0,
                  //  -Screen.PrimaryScreen.Bounds.Height / 5);
                Debug.WriteLine(GameState.CurrentState);
                Time.Instance.TimeFlow = 0;
            }
            else if (Input.Instance.IsKeyUp(KeyCodes.P))
            {
                GameState.CurrentState = GameState.LastState;
                //SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0, 0);
                Time.Instance.TimeFlow = 1;
            }
        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);
            var aspectRatio = Width / (float)Height;

            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4/5, aspectRatio, 1, 10000);
        }

        public static void Main()
        {
            var app = new Scharfschiessen();
            app.Run();
        }
    }
}
