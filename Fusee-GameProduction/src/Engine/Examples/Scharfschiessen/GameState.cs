using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Examples.Scharfschiessen
{
    /// <summary>
    /// The GameState class stores the state the Game is in.
    /// 
    /// </summary>
    public class GameState
    {
        public State CurrentState { get; set; }

        public enum State
        {
            MainMenu,
            Playing,
            HiddenPause,
            Highscore

        }
        public GameState()
        {
            CurrentState = State.MainMenu;
            Debug.WriteLine(CurrentState);
        }

        
    }
}
