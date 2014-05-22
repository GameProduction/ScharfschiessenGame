using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Gui
    {
       
        private GUIHandler _guiHandler;
        private GUIHandler _mainmenuHandler;
        private GUIHandler _inGameHandler;
        private GUIHandler _highScoreHandler;
        private IFont _guiFontCabin12;
        private IFont _guiFontCabin18;
        private IFont _guiFontCabin24;
        private GUIText _guiText;
        private GUIText _guiText1;
        private GUIText _guiText2;
        private GUIButton[] _guiDiffs;

        public Gui(RenderContext RC) //da initialisieren wir alles für den GuiHandler
        {
            _guiHandler = new GUIHandler(RC);
            _mainmenuHandler = new GUIHandler(RC);
            _inGameHandler = new GUIHandler(RC);
            _highScoreHandler = new GUIHandler(RC);

            _guiFontCabin12 = RC.LoadFont("Assets/Cabin.ttf", 12);
            _guiFontCabin18 = RC.LoadFont("Assets/Cabin.ttf", 18);
            _guiFontCabin24 = RC.LoadFont("Assets/Cabin.ttf", 24);

            _guiText = new GUIText("Ende", _guiFontCabin12, 45, 15);
            _guiText.TextColor = new float4(1, 0, 0, 1);
            _guiHandler.Add(_guiText);

            _guiDiffs = new GUIButton[2];
            _guiDiffs[0] = new GUIButton(0, 0, 40, 20);
            _guiDiffs[1] = new GUIButton(40, 0, 40, 20);

        }

        public void DrawGui()
        {
            //Gui Rendern
            _guiHandler.RenderGUI();
        }

        public void Refresh()
        {
            _guiHandler.Refresh();
        }

        public void SetGui(GameState.State _state)
        {
            switch (_state)
            {
                    case GameState.State.MainMenu:
                    MainMenuGui();
                    break;
                case GameState.State.Playing:
                    InGameGui();
                    break;
                case GameState.State.HiddenPause:
                    InGameGui();
                    break;
                case GameState.State.Highscore:
                    HighScoreGui();
                    break;
                default:
                    MainMenuGui();
                    break;
            }
        }

        internal void MainMenuGui()
        {
            Console.WriteLine("MainMenuGui");
            //set guiHander GUI für Hauptmenu

           
        }

        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _guiHandler.Refresh();
            _guiText1 = new GUIText("Start", _guiFontCabin12, 10, 15);
            _guiText1.TextColor = new float4(0, 1, 0, 1);
            _guiHandler.Add(_guiText1);
            _guiHandler.Add(_guiDiffs[0]);
            _guiHandler.Add(_guiDiffs[1]);
        }

        internal void HighScoreGui()
        {
            //set guiHander für die Highscore Anzeige
            Console.WriteLine("HighScoreGui");
        }
    }
}
