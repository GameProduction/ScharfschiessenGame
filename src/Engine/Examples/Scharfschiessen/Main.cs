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
        
        public GameHandler GameHandler;
      
      // is called on startup
        public override void Init()
        {
            SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height/5, true, 0, 0);
            
            RC.ClearColor = new float4(0.2f, 0.5f, 0.9f, 1);
           
            GameHandler = new GameHandler(RC, this);
            Resize();
        }


        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            GameHandler.Update();
            GameHandler.TestInput();
            GameHandler.Hide();
          
            Present();
        }


        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);
            var aspectRatio = Width / (float)Height;

            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4/5, aspectRatio, 1, 15000);

            GameHandler.Gui.Refresh();
        }


        public static void Main()
        {
            var app = new Scharfschiessen();
            app.Run();
          
        }
    }
}
