using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fusee.Engine;

namespace Examples.Scharfschiessen
{
    public class GameHandler
    {
        internal readonly RenderContext Rc;
        internal RenderCanvas RCanvas;
        internal Gui gui;
        public Gui Gui {
            get { return gui; }
        }


        public GameState GameState{ get; private set; }

        public Game Game { get; set; }

        public GameHandler(RenderContext rc, RenderCanvas rCanvas)
        {
            RCanvas = rCanvas;
            this.Rc = rc;
            this.gui = new Gui(Rc, RCanvas, this);
            this.GameState = new GameState(ref gui, this);
        }

       

        public void Update()
        {
            switch (GameState.CurrentState)
            {
                case GameState.State.MainMenu:
                    break;
                case GameState.State.Playing:
                    Game.Update();
                    break;
                case GameState.State.HiddenPause:
                    break;
                case GameState.State.Highscore:
                    break;
            }
            //Hide();
            //TestInput();
            Gui.DrawGui();
        }

        public void StartGame()
        {
            Debug.WriteLine("StartGame");
            Game = new Game(this,Rc);
        }


        
        public void Hide()
        {
            if (Input.Instance.IsKeyUp(KeyCodes.B) && GameState.CurrentState != GameState.State.HiddenPause)
            {
                GameState.CurrentState = GameState.State.HiddenPause;
                RCanvas.SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0,
                    -Screen.PrimaryScreen.Bounds.Height / 5);
                Time.Instance.TimeFlow = 0;
            }
            else if (Input.Instance.IsKeyUp(KeyCodes.B))
            {
                GameState.CurrentState = GameState.LastState;
                RCanvas.SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0, 0);
                Time.Instance.TimeFlow = 1;
            }
        }

        public void TestInput()
        {
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
            }
        }
    }
}
