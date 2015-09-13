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

        //public DbConnection DbConnection { get; private set; }
        public GameState GameState{ get; private set; }

        public Game Game { get; set; }

        public GameHandler(RenderContext rc, RenderCanvas rCanvas)
        {
            RCanvas = rCanvas;
            this.Rc = rc;
            this.gui = new Gui(Rc, RCanvas, this);
            this.GameState = new GameState(ref gui, this);
          //  DbConnection =new DbConnection(this);
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
            Gui.DrawGui();
        }

        public void StartGame()
        {
            Debug.WriteLine("StartGame");
            Game = new Game(this,Rc);
        }


        
        public void Hide()
        {

            if (Input.Instance.IsKeyUp(KeyCodes.Tab) && GameState.CurrentState != GameState.State.HiddenPause)
            {
                GameState.CurrentState = GameState.State.HiddenPause;
                RCanvas.SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0,
                    -Screen.PrimaryScreen.Bounds.Height / 5);
                Time.Instance.TimeFlow = 0;
            }
            else if (Input.Instance.IsKeyUp(KeyCodes.Tab) && GameState.CurrentState == GameState.State.HiddenPause)
            {
                GameState.CurrentState = GameState.LastState;
                RCanvas.SetWindowSize(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 5, true, 0, 0);
                Time.Instance.TimeFlow = 1;
            }
        }
    }
}
