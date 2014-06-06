using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.Scharfschiessen
{
    public class Gui
    {
        private RenderCanvas _rCanvas;
        private readonly GameHandler _gameHandler ;
        private GUIHandler _guiHandler;
        private GUIHandler _mainmenuHandler;
        private GUIHandler _inGameHandler;
        private GUIHandler _highScoreHandler;
        private IFont _guiFontCabin12;
        private IFont _guiFontAlphaWood18;
        private IFont _guiFontWESTERN30;
        private IFont _guiFontCabin24;
        private GUIText _guiText1;
        private GUIText _guiText2;
        private GUIText _guiText3;
        private GUIText _guiText4;
        private GUIText _guiText5;
       // private GUIButton[] _guiText;
        private GUIButton[] _guiDiffs;
        private GUIImage _guiImage1;
        private GUIImage _guiImage2;
        private double _countdown;


        public Gui(RenderContext RC, RenderCanvas rCanvas, GameHandler gameHandler) //da initialisieren wir alles für den GuiHandler
        {
            _rCanvas = rCanvas;
            _gameHandler = gameHandler;
            _guiHandler = new GUIHandler(RC);
            _mainmenuHandler = new GUIHandler(RC);
            _inGameHandler = new GUIHandler(RC);
            _highScoreHandler = new GUIHandler(RC);
            _countdown = Game.GetTime();
           
            _guiFontCabin12 = RC.LoadFont("Assets/Cabin.ttf", 12);
            _guiFontCabin24 = RC.LoadFont("Assets/Cabin.ttf", 24);
            _guiFontAlphaWood18 = RC.LoadFont("Assets/AlphaWood.ttf", 18);
            _guiFontWESTERN30 = RC.LoadFont("Assets/WESTERN.ttf", 30);

            var height = _rCanvas.Height;
            var width= _rCanvas.Width;
           
            _guiDiffs = new GUIButton[3];
            _guiDiffs[0] = new GUIButton(width / 2 + (width / 80), height / 2, -2, 50, 20);
            _guiDiffs[1] = new GUIButton(40, 0, 40, 20);
            _guiDiffs[2] = new GUIButton(0, 0, 40, 20);

           // _guiText = new GUIText[3];
            _guiText1 = new GUIText("Scha(r)fschießen", _guiFontWESTERN30, (width / 2) - (width / 30), (height / 3));
            _guiText1.TextColor = new float4(0, 0, 0, 1);
            _guiText3 = new GUIText("Starten", _guiFontCabin12, (width / 2) + (width / 55), (height / 2) + (height / 10));
            _guiText3.TextColor = new float4(1, 1, 1, 1);
            _guiText4 = new GUIText("Game Over!", _guiFontWESTERN30, (width / 2), (height / 3));
            _guiText4.TextColor = new float4(0, 0, 0, 1);
            _guiText5 = new GUIText("Nochmal spielen", _guiFontCabin12, (width / 2) + (width / 55), (height / 2) + (height / 10));
            _guiText5.TextColor = new float4(1, 1, 1, 1);

            Console.WriteLine("Höhe = " + height);
            Console.WriteLine("Breite = " + width);

            _guiImage1 = new GUIImage("Assets/holz.png", width / 2, height / 2 - (height / 20), -1, 90, 40);
            _guiImage2 = new GUIImage("Assets/holz.png", width / 2 - (width / 140), height / 2 - (height / 20), -1, 150, 40);
            
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
                    Console.Write(_countdown);
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
            _mainmenuHandler.Add(_guiImage1);
            _mainmenuHandler.Add(_guiText1);
            _mainmenuHandler.Add(_guiText3);
            _guiHandler.Add(_guiDiffs[0]);
            _guiDiffs[0].OnGUIButtonDown += OnDiffButtonDown;

        }

        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _guiDiffs[0].OnGUIButtonDown -= OnDiffButtonDown;
            //Game.GetTime();
            _guiHandler = _inGameHandler;
            _guiText1 = new GUIText("Zeit: "+ _countdown, _guiFontCabin12, 10, 15);
            _guiText1.TextColor = new float4(0, 0, 0, 1);
            _inGameHandler.Add(_guiText1); 
            
        }

        internal void HighScoreGui()
        {
            //set guiHander für die Highscore Anzeige
            Console.WriteLine("HighScoreGui");
            _guiHandler = _highScoreHandler;
            _highScoreHandler.Add(_guiImage2);
            _highScoreHandler.Add(_guiText4);
            _highScoreHandler.Add(_guiText5);
            _guiHandler.Add(_guiDiffs[0]);
            _guiDiffs[0].OnGUIButtonDown += OnDiffButtonDown;

        }



        private void OnDiffButtonDown(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            _gameHandler.GameState.CurrentState = GameState.State.Playing;
        }
    }
}
