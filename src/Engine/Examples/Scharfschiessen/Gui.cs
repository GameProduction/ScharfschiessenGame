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
        // private GUIElement _timer;
        private IFont _guiFontCabin12;
        private IFont _guiFontAlphaWood18;
        private IFont _guiFontWESTERN30;
        private IFont _guiFontCabin24;
        private GUIText _guiText1;
        private GUIText _guiText2;
        private GUIText _guiText3;
        private GUIText _guiText4;
        private GUIText _guiText5;
        private GUIText _guiText6;
       // private GUIText[] _guiText;
        private GUIButton[] _guiDiffs;
        private GUIImage[] _guiImages;
        private double _countdown;
        enum _buttons{ btnStart, btnNochmal, btnHighscore};
        enum _btnimages { btniStart, btniNochmal, btniHighscore };


        public Gui(RenderContext RC, RenderCanvas rCanvas, GameHandler gameHandler/*, Game g*/) //da initialisieren wir alles für den GuiHandler
        {
            float textwidth;
            float texthight;
            _rCanvas = rCanvas;
            _gameHandler = gameHandler;
            _guiHandler = new GUIHandler(RC);
            _mainmenuHandler = new GUIHandler(RC);
            _inGameHandler = new GUIHandler(RC);
            _highScoreHandler = new GUIHandler(RC);

            var height = _rCanvas.Height;
            var width = _rCanvas.Width;

            _guiFontCabin12 = RC.LoadFont("Assets/Cabin.ttf", 12);
            _guiFontCabin24 = RC.LoadFont("Assets/Cabin.ttf", 24);
            _guiFontAlphaWood18 = RC.LoadFont("Assets/AlphaWood.ttf", 18);
            _guiFontWESTERN30 = RC.LoadFont("Assets/WESTERN.ttf", 30);

            _guiDiffs = new GUIButton[3];
            _guiImages = new GUIImage[3];

            //Text Mainmenü: Scha(r)fschießen
            textwidth = GUIText.GetTextWidth("Scha(r)fschießen", _guiFontWESTERN30);
            _guiText1 = new GUIText("Scha(r)fschießen", _guiFontWESTERN30, (width/2) - (int)(textwidth/2), (height/3));
            _guiText1.TextColor = new float4(0, 0, 0, 1);

            // Button MainMenü: Starten
            textwidth = GUIText.GetTextWidth("Starten", _guiFontCabin12);
            texthight = GUIText.GetTextHeight("Starten", _guiFontCabin12);
            _guiText3 = new GUIText("Starten", _guiFontCabin12, width / 2 - (int)(textwidth / 2), (height / 2));
            _guiText3.TextColor = new float4(1, 1, 1, 1);
            _guiDiffs[(int)_buttons.btnStart] = new GUIButton(_guiText3.PosX, _guiText3.PosY - (int)texthight, -2, (int)textwidth, (int)texthight);
            _guiImages[(int)_btnimages.btniStart] = new GUIImage("Assets/holz.png", _guiText3.PosX - (int)textwidth/2, _guiText3.PosY - (int)(texthight*1.5), -1, (int)textwidth * 2, (int)texthight * 2);


            // Text HighscoreMenü: Game Over
            textwidth = GUIText.GetTextWidth("Game Over!", _guiFontWESTERN30);
            _guiText4 = new GUIText("Game Over!", _guiFontWESTERN30, width/2 - (int)(textwidth/2), (height/3));
            _guiText4.TextColor = new float4(0, 0, 0, 1);

            // Button HighscoreMenü: Nochmal spielen
            textwidth = GUIText.GetTextWidth("Nochmal spielen", _guiFontCabin12);
            _guiText5 = new GUIText("Nochmal spielen", _guiFontCabin12, (width/2) -(int)(textwidth*1.5), (height/2) + (height/10));
            _guiText5.TextColor = new float4(1, 1, 1, 1);
            texthight = GUIText.GetTextHeight("Nochmal spielen", _guiFontCabin12);
            _guiDiffs[(int)_buttons.btnNochmal] = new GUIButton(_guiText5.PosX, _guiText5.PosY - (int)texthight, -2, (int)textwidth, (int)texthight);
            _guiImages[(int)_btnimages.btniNochmal] = new GUIImage("Assets/holz.png", _guiText5.PosX - (int)textwidth/2, _guiText5.PosY - (int)(texthight*1.5), -1, (int)textwidth * 2, (int)texthight * 2);

            // Button HighscoreMenü: Highscore
            textwidth = GUIText.GetTextWidth("Highscore anzeigen", _guiFontCabin12);
            _guiText6 = new GUIText("Highscore anzeigen", _guiFontCabin12, (width / 2) + (int)(textwidth/2), (height / 2) + (height / 10));
            _guiText6.TextColor = new float4(1, 1, 1, 1);
            texthight = GUIText.GetTextHeight("Highscore anzeigen", _guiFontCabin12);
            _guiDiffs[(int)_buttons.btnHighscore] = new GUIButton(_guiText6.PosX, _guiText6.PosY - (int)texthight, -2, (int)textwidth, (int)texthight);
            _guiImages[(int)_btnimages.btniHighscore] = new GUIImage("Assets/holz.png", _guiText6.PosX - (int)textwidth / 2, _guiText6.PosY - (int)(texthight * 1.5), -1, (int)textwidth * 2, (int)texthight * 2);
        }

        public void DrawGui()
        {
            //Gui Rendern
            _guiHandler.RenderGUI();
            if (_gameHandler.Game != null) //Countdown nur während dem Spiel
            {
                _countdown = (int) _gameHandler.Game.Countdown;
                _guiText1.Text = "Zeit:  " + _countdown;
            }
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
            _mainmenuHandler.Add(_guiImages[(int)_btnimages.btniStart]);
            _mainmenuHandler.Add(_guiText1);
            _mainmenuHandler.Add(_guiText3);
            _guiHandler.Add(_guiDiffs[(int)_buttons.btnStart]);
            _guiDiffs[(int)_buttons.btnStart].OnGUIButtonDown += OnbtnPlay;

        }

        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _guiDiffs[0].OnGUIButtonDown -= OnbtnHighscore;
            _inGameHandler.Remove(_guiText1);
            _guiHandler = _inGameHandler;
            _countdown = _gameHandler.Game.Countdown;
            _guiText1 = new GUIText("Zeit: " + _countdown, _guiFontCabin12, 10, 15);
            _guiText1.TextColor = new float4(0, 0, 0, 1);
            _inGameHandler.Add(_guiText1);
 
        }

        internal void HighScoreGui()
        {
            //set guiHander für die Highscore Anzeige
            Console.WriteLine("HighScoreGui");
            _guiHandler = _highScoreHandler;
            _highScoreHandler.Add(_guiImages[(int)_btnimages.btniNochmal]);
            _highScoreHandler.Add(_guiImages[(int)_btnimages.btniHighscore]);
            _highScoreHandler.Add(_guiText4);
            _highScoreHandler.Add(_guiText5);
            _highScoreHandler.Add(_guiText6);
            _guiHandler.Add(_guiDiffs[(int)_buttons.btnNochmal]);
            _guiDiffs[(int)_buttons.btnNochmal].OnGUIButtonDown += OnbtnPlay;
            _guiHandler.Add(_guiDiffs[(int)_buttons.btnHighscore]);
            _guiDiffs[(int)_buttons.btnHighscore].OnGUIButtonDown += OnbtnHighscore;

        }

        private void OnbtnHighscore(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            //To-Do: Hier verlinken zur Datenbank
            _gameHandler.GameState.CurrentState = GameState.State.MainMenu;
        }

        private void OnbtnPlay(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            _gameHandler.GameState.CurrentState = GameState.State.Playing;
        }
    }
}
