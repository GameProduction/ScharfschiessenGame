using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fusee.Engine;

namespace Examples.Scharfschiessen
{
    public class GameHandler
    {
        internal readonly RenderContext Rc;

        internal Gui gui;
        public Gui Gui {
            get { return gui; }
        }
    

       
        private GameState GameState;
        private Game Game;

        public GameHandler(RenderContext rc)
        {
            this.Rc = rc;
            this.gui = new Gui(Rc);
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
            TestInput();
            Gui.DrawGui();
        }

        public void StartGame()
        {
            Debug.WriteLine("StartGame");
            Game = new Game(Rc);
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
