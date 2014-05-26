using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

            var height = Screen.PrimaryScreen.Bounds.Width;
            var width = Screen.PrimaryScreen.Bounds.Height;

            _guiDiffs = new GUIButton[3];
            _guiDiffs[0] = new GUIButton(width, height/15, 40, 20);
            _guiDiffs[1] = new GUIButton(40, 0, 40, 20);
            _guiDiffs[2] = new GUIButton(0, 0, 40, 20);

            //Console.WriteLine("Höhe = " + height);
            //Console.WriteLine("Breite = " + width);

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
            _guiHandler = _mainmenuHandler;
            _guiText1 = new GUIText("Scha(r)fschießen", _guiFontCabin24, 580, 50);
            _guiText1.TextColor = new float4(1, 0, 0, 1);
            _guiText2 = new GUIText("Starten", _guiFontCabin12, 640, 85);
            _guiText2.TextColor = new float4(0, 0, 0, 1);
            _mainmenuHandler.Add(_guiText1);
            _mainmenuHandler.Add(_guiText2);
            _guiHandler.Add(_guiDiffs[0]);
            _guiDiffs[0].OnGUIButtonDown += OnDiffButtonDown;

        }

        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _guiDiffs[0].OnGUIButtonDown -= OnDiffButtonDown;
            _guiHandler = _inGameHandler;
            _guiText1 = new GUIText("Zeit:", _guiFontCabin12, 10, 15);
            _guiText1.TextColor = new float4(0, 0, 0, 1);
            _inGameHandler.Add(_guiText1);
           // _guiHandler.Add(_guiDiffs[0]);
           // _inGameHandler.Add(_guiDiffs[1]);
        }

        internal void HighScoreGui()
        {
            //set guiHander für die Highscore Anzeige
            Console.WriteLine("HighScoreGui");
            _guiHandler = _highScoreHandler;
            _guiText1 = new GUIText("Game Over!", _guiFontCabin24, 600, 50);
            _guiText1.TextColor = new float4(1, 0, 0, 1);
            _guiText2 = new GUIText("Nochmal spielen", _guiFontCabin12, 615, 85);
            _guiText2.TextColor = new float4(0, 0, 0, 1);
            _highScoreHandler.Add(_guiText1);
            _highScoreHandler.Add(_guiText2);
            _guiHandler.Add(_guiDiffs[0]);
            _guiDiffs[0].OnGUIButtonDown += OnDiffButtonDown;

        }

        private void OnDiffButtonDown(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
        InGameGui();
        }

    }
}
