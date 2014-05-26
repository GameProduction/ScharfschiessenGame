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

        private GameHandler GameHandler;
       // internal GameState GameState;
        //internal Gui Gui;

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


            GameHandler = new GameHandler(RC);


        }


        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            GameHandler.Update();
          

            /*RC.SetShader(_spColor);

            // first mesh
            // Row order notation
            var modelViewMesh1 = mtxCam * float4x4.CreateTranslation(0, 10, 200)* float4x4.CreateRotationY(45) * float4x4.Scale(0.1f);
            RC.ModelView = modelViewMesh1;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));

            RC.Render(MeshCube);

            rot += (float)Time.Instance.DeltaTime * 5;

            var modelView = mtxCam * mtxRot * float4x4.CreateTranslation(-100, 10, 200) * float4x4.CreateRotationX(rot) * float4x4.Scale(0.25f);
            RC.SetShader(_spColor);
            RC.ModelView = modelView;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            RC.Render(Mesh);

       
            TestInput();*/

            Present();
        }

        public void TestInput()
        {
            /*TODO: Kann das besser orgneisiert werden? vllt über die GameState Klasse 
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
                Debug.WriteLine(GameState.CurrentState);
                Time.Instance.TimeFlow = 0;
            }
            else if (Input.Instance.IsKeyUp(KeyCodes.P))
            {
                GameState.CurrentState = GameState.LastState;
                Time.Instance.TimeFlow = 1;
            }

            
            //Test eingabe um Gamestate mt gui verknüpfungzu testen
            if (Input.Instance.IsKeyUp(KeyCodes.M))
            {
                GameState.CurrentState = GameState.State.MainMenu;
            }
            if (Input.Instance.IsKeyUp(KeyCodes.I))
            {
                GameState.CurrentState = GameState.State.Playing;
            }
            if (Input.Instance.IsKeyUp(KeyCodes.H))
            {
                GameState.CurrentState = GameState.State.Highscore;
            }*/

        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);
            var aspectRatio = Width / (float)Height;

            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4/5, aspectRatio, 1, 10000);

            GameHandler.Gui.Refresh();
        }

        public static void Main()
        {
            var app = new Scharfschiessen();
            app.Run();
        }
    }
}
