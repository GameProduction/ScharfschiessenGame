using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Examples.Scharfschiessen
{
    /// <summary>
    /// Die Klasse GameState gibt an und speichert in welchem Status sich das Spiel befindet
    /// </summary>
   

    public class GameState
    {
        private State _currentState;
        public State LastState { get; set; }
    
        public State CurrentState
        {
            get { return _currentState; }
            set
            {
                if (value == State.HiddenPause)
                {
                    LastState = _currentState;
                }
                _currentState = value; 
                
            }
        }

        public enum State
        {
            MainMenu,
            Playing,
            HiddenPause,
            Highscore

        }
        public GameState()
        {
            _currentState = State.MainMenu;
            Debug.WriteLine(CurrentState);
        }

        
    }
}
