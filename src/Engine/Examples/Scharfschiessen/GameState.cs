using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using Fusee.Engine;

namespace Examples.Scharfschiessen
{
    /// <summary>
    /// Die Klasse GameState gibt an und speichert in welchem Status sich das Spiel befindet
    /// </summary>
   

    public class GameState
    {
        private State _currentState;
        public State LastState { get; set; }
        internal Gui Gui;
        internal readonly GameHandler GameHandler;

        public State CurrentState
        {
            get { return _currentState; }
            set
            {
                LastState = _currentState;
                if (value == State.Playing)
                {
                    Input.Instance.CursorVisible = false;
                    Input.Instance.FixMouseAtCenter = true;   
                }
                else
                {
                    Input.Instance.CursorVisible = true;
                    Input.Instance.FixMouseAtCenter = false;   
                }
                if (value == State.Playing && LastState != State.HiddenPause)
                {
                    GameHandler.StartGame();
                }
                else if (value == State.Playing && LastState == State.HiddenPause)
                {
                    _currentState = value;
                }
                
                _currentState = value;
                Gui.SetGui(_currentState);
            }
        }

        public enum State
        {
            MainMenu,
            Playing,
            HiddenPause,
            Highscore

        }
        public GameState(ref Gui g, GameHandler gameHandler)
        {
            Gui = g;
            GameHandler = gameHandler;
            _currentState = State.MainMenu;
            LastState = CurrentState;
            Gui.SetGui(_currentState);
        }

        
    }
}
