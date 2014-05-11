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
        internal GameState GameState;
        public WindowHandler WindowHandler;

        private float rot;
        // variables for shader
        private ShaderProgram _spColor;
        private IShaderParam _colorParam;

        // is called on startup
        public override void Init()
        {
            rot = 0;
            RC.ClearColor = new float4(0.9f, 0.9f, 0.9f, 1);

            Mesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");

            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam = _spColor.GetShaderParam("color");

            WindowHandler = new WindowHandler();
            InitGame();
            WindowHandler.SetWindowSettings(this);
        }

        internal void InitGame()
        {
            GameState = new GameState();
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            //test für rotierenes Mesh um Pause zu überprüfen nur zu Vorführungszwecken der pause Funktion
            //var mtxCam = float4x4.LookAt(0, 0, -200, 0, 0, 0, 0, 1, 0);
            var mtxCam = float4x4.LookAt(0, 200, -500, 0, 0, 0, 0, 1, 0);
            rot += (float)Time.Instance.DeltaTime * 5;
            var mtxRot = float4x4.CreateRotationX(rot);
            var modelView = mtxCam * mtxRot * float4x4.CreateTranslation(-150, 0, 0) * float4x4.CreateTranslation(0, -50, 0);
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
                Debug.WriteLine(GameState.CurrentState);
                WindowHandler.Hide(this);
                Time.Instance.TimeFlow = 0;
            }
            else if (Input.Instance.IsKeyUp(KeyCodes.B))
            {
                GameState.CurrentState = GameState.LastState;
                WindowHandler.SetWindowSettings(this);
                Time.Instance.TimeFlow = 1;
            }
        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
        }

        public static void Main()
        {
            var app = new Scharfschiessen();
            app.Run();
        }
    }
}
