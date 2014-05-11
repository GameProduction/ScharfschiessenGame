using System.Diagnostics;
using System.Windows.Forms;
using Fusee.Engine;
using Fusee.SceneManagement;
using Fusee.Math;
using System.Windows;


namespace Examples.Scharfschiessen
{
    public class Scharfschiessen : RenderCanvas
    {
        internal GameState GameState;
        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(0.1f, 0.1f, 0.5f, 1);
            SetWindowSettings();
        }

        internal void InitGame()
        {
            GameState = new GameState();
        }


        internal void SetWindowSettings()
        {
            //Höhe und Breite an Primären Bildschirm anpassen
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height/6;
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            Present();
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
