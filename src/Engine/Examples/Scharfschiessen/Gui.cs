using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Fusee.Engine;
using Fusee.Math;
using MouseButtons = Fusee.Engine.MouseButtons;


namespace Examples.Scharfschiessen
{

    public class Gui
    {
#region klassenvariable
        private RenderCanvas _rCanvas;
        private readonly GameHandler _gameHandler;
        private GUIHandler _guiHandler;
        private GUIHandler _mainmenuHandler;
        private GUIHandler _inGameHandler;
        private GUIHandler _highScoreHandler;
       
        //Schriften
        private IFont _guiFontCabin12;
        private IFont _guiFontAlphaWood18;
        private IFont _guiFontWESTERN30;
        private IFont _guiFontCabin24;
        private IFont _guiFontWESTERN18;
        
        //Texte:
        private GUIText _guiText1;
        private GUIText _guiText2;
        private GUIText _guiText3;
        private GUIText _guiText4;
        private GUIText _guiText5;
        private GUIText _guiText6;
        private GUIText _name;
        private GUIText _guiTextTitel;
        
        //Arrays für Buttons und Images
        private GUIButton[] _guiDiffs;
        private GUIImage[] _guiImages;
        private GUIImage[] _guiImageTomato;

        // Diverse public Variablen (für Datenbank )
        public int _points;
        public GUIText nameInput;   // Textfeld zur Namenseingabe
        public string playername;   //eingegebener name an DB

        //variablen für Anzeige des Highscores
        public GUIText _highscoreEintrag;
        public string[] highscoreEintraege;
        public int _hsYpos;
       // public GUIText _guiTexthighscore; 

        // Diverse private Variablen
        private double _countdown;
        private int _munition;
        private bool _highscore;
        private bool _neustart;
        private bool _inputToggle; // Eingabe des Spielernamens
        private string _hs;     // Empfangsversuch für die String-Daten aus der Datenbank
        private int _aimimage; // für die flexible Größe des Fadenkreuzes
#endregion

        private enum _buttons
        {
            btnStart,
            btnNochmal,
            btnHighscore,
        };

        private enum _btnimages
        {
            btniStart,
            btniNochmal,
            btniHighscore,
            btniMouse,
            btniFadenkreuz
        };

        public Gui(RenderContext RC, RenderCanvas rCanvas, GameHandler gameHandler)
            //da initialisieren wir alles für den GuiHandler
        {
    #region Variablen
            _rCanvas = rCanvas;
            var height = _rCanvas.Height;
            var width = _rCanvas.Width;

            _gameHandler = gameHandler;
            _guiHandler = new GUIHandler(RC);
            _mainmenuHandler = new GUIHandler(RC);
            _inGameHandler = new GUIHandler(RC);
            _highScoreHandler = new GUIHandler(RC);

            _guiDiffs = new GUIButton[3];
            _guiImageTomato = new GUIImage[10];
            _guiImages = new GUIImage[5];

            float textwidth;
            float texthight;
            _highscore = false;
            _neustart = true;
            _hsYpos = 50;

      #endregion

    #region Bilder
            //Maus-Nutzerinfo in Mainmenü einblenden
            _guiImages[(int)_btnimages.btniMouse] = new GUIImage("Assets/Mouse.png", width / 4, 0, -1, (int)(height / 1.322), height);
            
            //Fadenkreuz flexibel (Falls es zwecks Schwierigkeitsstufe kleiner werden soll)
            _aimimage = 80;
            _guiImages[(int)_btnimages.btniFadenkreuz] = new GUIImage("Assets/Fadenkreuz.png", width / 2 - _aimimage / 2, height / 2 - _aimimage / 2, -2, _aimimage, _aimimage);
    #endregion

    #region Beschriftungen
            //Schriften
            _guiFontCabin12 = RC.LoadFont("Assets/Cabin.ttf", 12);
            _guiFontCabin24 = RC.LoadFont("Assets/Cabin.ttf", 24);
            _guiFontAlphaWood18 = RC.LoadFont("Assets/AlphaWood.ttf", 18);
            _guiFontWESTERN30 = RC.LoadFont("Assets/WESTERN.ttf", 30);
            _guiFontWESTERN18 = RC.LoadFont("Assets/WESTERN.ttf", 18);

            //Eingabetext Name für Highscore
            texthight = GUIText.GetTextHeight("Lorem ipsum", _guiFontCabin12);
            textwidth = GUIText.GetTextWidth("Write name: ", _guiFontCabin12);
            _name = new GUIText("Write name: ", _guiFontCabin12, (width / 2) - (int)(textwidth), (height / 2) - (int)(texthight), new float4(0, 0, 0, 1));
            nameInput = new GUIText("", _guiFontCabin12, (width / 2), (height / 2) - (int)(texthight), new float4(1, 0, 0, 1));
           
            //Text Mainmenü: Scha(r)fschießen
            textwidth = GUIText.GetTextWidth("Scha(r)fschießen", _guiFontWESTERN30);
            _guiTextTitel = new GUIText("Scha(r)fschießen", _guiFontWESTERN30, (width/2) - (int) (textwidth/2), (height/3));
            _guiTextTitel.TextColor = new float4(0, 0, 0, 1);

            // Text InGame
            textwidth = GUIText.GetTextWidth("Time: ", _guiFontCabin12);
            texthight = GUIText.GetTextHeight("Time: ", _guiFontCabin12);
            _guiText1 = new GUIText("Time: " + _countdown, _guiFontCabin12, (int)textwidth, (int)(texthight*2));
            _guiText1.TextColor = new float4(0, 0, 0, 1);
            textwidth = GUIText.GetTextWidth("Time: ", _guiFontCabin12);
            _guiText2 = new GUIText("Points: " + _points, _guiFontCabin12, width - (int)(textwidth * 3), (int)(texthight*2));
            _guiText2.TextColor = new float4(0, 0, 0, 1);

            // Text HighscoreMenü: Game Over
            textwidth = GUIText.GetTextWidth("Game Over!", _guiFontWESTERN30);
            _guiText4 = new GUIText("Game Over!", _guiFontWESTERN30, width/2 - (int) (textwidth/2), (height/3));
            _guiText4.TextColor = new float4(0, 0, 0, 1);
    #endregion

    #region Buttons
            // Button MainMenü: Starten
            textwidth = GUIText.GetTextWidth("Start", _guiFontCabin12);
            texthight = GUIText.GetTextHeight("Start", _guiFontCabin12);
            _guiText3 = new GUIText("Start", _guiFontCabin12, width / 2 - (int)(textwidth / 2), (height / 2));
            _guiText3.TextColor = new float4(1, 1, 1, 1);
            _guiDiffs[(int)_buttons.btnStart] = new GUIButton(_guiText3.PosX - (int)(texthight / 2), _guiText3.PosY - (int)texthight, -2,
                (int)(textwidth * 1.5), (int)(texthight * 1.5));
            _guiImages[(int)_btnimages.btniStart] = new GUIImage("Assets/holz.png", _guiText3.PosX - (int)textwidth / 2,
                _guiText3.PosY - (int)(texthight * 1.5), -1, (int)textwidth * 2, (int)texthight * 2);

            // Button HighscoreMenü: Nochmal spielen
            textwidth = GUIText.GetTextWidth("Nochmal spielen", _guiFontCabin12);
            _guiText5 = new GUIText("Nochmal spielen", _guiFontCabin12, (width/2) - (int) (textwidth*1.5),
                (height/2) + (height/10));
            _guiText5.TextColor = new float4(1, 1, 1, 1);
            texthight = GUIText.GetTextHeight("Nochmal spielen", _guiFontCabin12);
            _guiDiffs[(int) _buttons.btnNochmal] = new GUIButton(_guiText5.PosX, _guiText5.PosY - (int) texthight, -2,
                (int) textwidth, (int) texthight);
            _guiImages[(int) _btnimages.btniNochmal] = new GUIImage("Assets/holz.png",
                _guiText5.PosX - (int) textwidth/2, _guiText5.PosY - (int) (texthight*1.5), -1, (int) textwidth*2,
                (int) texthight*2);
           
            // Button HighscoreMenü: Highscore
            textwidth = GUIText.GetTextWidth("In Highscore eintragen", _guiFontCabin12);
            _guiText6 = new GUIText("In Highscore eintragen", _guiFontCabin12, (width / 2) + (int)(textwidth / 2),
                (height/2) + (height/10));
            _guiText6.TextColor = new float4(1, 1, 1, 1);
            texthight = GUIText.GetTextHeight("In Highscore eintragen", _guiFontCabin12);
            _guiDiffs[(int) _buttons.btnHighscore] = new GUIButton(_guiText6.PosX, _guiText6.PosY - (int) texthight, -2,
                (int) textwidth, (int) texthight);
            _guiImages[(int) _btnimages.btniHighscore] = new GUIImage("Assets/holz.png",
                _guiText6.PosX - (int) textwidth/2, _guiText6.PosY - (int) (texthight*1.5), -1, (int) textwidth*2,
                (int) texthight*2);  
        #endregion
        }

        public void DrawGui()
        {
            //Gui Rendern
            _guiHandler.RenderGUI();
         
           // if (_gameHandler.Game != null) //Countdown nur während dem Spiel
            if (_gameHandler.GameState.CurrentState == GameState.State.Playing) 
            {
                _countdown = (int) _gameHandler.Game.Countdown;
                _guiText1.Text = "Time:  " + _countdown;
                _points = (int)_gameHandler.Game.Points;
                _guiText2.Text = "Points: " + _points;
                if (_munition != _gameHandler.Game.Weapon.Magazin)
                {
                    UpdateMunition(_gameHandler.Game.Weapon.Magazin);   
                }
                _munition = _gameHandler.Game.Weapon.Magazin;
            }

            if (_highscore = true) //Namen eingeben nur nach dem Spiel
            {
                UpdateCustomText();
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
                    // keine Gui anzeigen, da sonst Null-Referenz-Exeption auftreten kann wenn zb. im MainMenu pausiert wird.
                    // oder DummyFunktion() ist aber eigendlich nciht nötig.
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
            //_guiDiffs[(int)_buttons.btnHighscore].OnGUIButtonDown -= OnbtnHighscore;
            var height = _rCanvas.Height;
            var width = _rCanvas.Width;
            _guiHandler = _mainmenuHandler;
            _mainmenuHandler.Add(_guiImages[(int) _btnimages.btniStart]);
            _mainmenuHandler.Add(_guiTextTitel);
            _mainmenuHandler.Add(_guiText3);
            _mainmenuHandler.Add(_guiDiffs[(int) _buttons.btnStart]);
            _guiDiffs[(int) _buttons.btnStart].OnGUIButtonDown += OnbtnPlay;
            _mainmenuHandler.Add(_guiImages[(int)_btnimages.btniMouse]);
        }

        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _countdown = _gameHandler.Game.Countdown;

            // Erst if-Abfrage, ob ich grad aus der Pause komme, weil ich dann nichts überschreiben will
            if (_gameHandler.GameState.LastState == GameState.State.HiddenPause && _gameHandler.GameState.CurrentState == GameState.State.Playing)
            {
                _neustart = false;
                Console.WriteLine("Neustart = false");
            }
            else
            {
                _neustart = true;
                for (int i = 0; i < 10; i++)
                {
                    _guiHandler.Remove(_guiImageTomato[i]);
                }
                Console.WriteLine("Neustart = true");
            }

            if (_neustart)
            {
            _guiHandler.Clear();
            _guiHandler = _inGameHandler; 
            _inGameHandler.Remove(_guiTextTitel);
            _inGameHandler.Add(_guiText1);
            _inGameHandler.Add(_guiText2);
            _inGameHandler.Add(_guiImages[(int)_btnimages.btniFadenkreuz]);
            _munition = _gameHandler.Game.Weapon.Magazin;
            //_countdown = _gameHandler.Game.Countdown;
            DrawMunition();
            }
        }

        internal void HighScoreGui()
        {
            //set guiHander für die Highscore Anzeige
            Console.WriteLine("HighScoreGui");
            _highscore = true;
            _guiHandler = _highScoreHandler;
            for (int i = 0; i < 10; i++)
            {
                _guiHandler.Remove(_guiImageTomato[i]);
            }
            _highScoreHandler.Add(_guiImages[(int)_btnimages.btniNochmal]);
            _highScoreHandler.Add(_guiImages[(int)_btnimages.btniHighscore]);
            _highScoreHandler.Add(_guiText4);
            _highScoreHandler.Add(_guiText5);
            _highScoreHandler.Add(_guiText6);
            _highScoreHandler.Add(_guiDiffs[(int)_buttons.btnNochmal]);
            _guiDiffs[(int)_buttons.btnNochmal].OnGUIButtonDown += OnbtnPlay;
            _highScoreHandler.Add(_guiDiffs[(int)_buttons.btnHighscore]);
            _guiDiffs[(int)_buttons.btnHighscore].OnGUIButtonDown += OnbtnHighscore;
            _highScoreHandler.Add(nameInput);
            _highScoreHandler.Add(_name);
            //_highScoreHandler.Add(_guiTexthighscore); ----------------------------------------------------------------------
        }

        public void DrawMunition() // Alle 10 Tomaten einmalig hinmalen
        {
             int tomatoposition = 10;

            for (int i = 0; i < 10; i++)
            {
                var height = _rCanvas.Height;
                 _guiImageTomato[i]=new GUIImage("Assets/tomateAmmoTexture.png", tomatoposition, height - 30, -1, 20, 20);
                _guiHandler.Add(_guiImageTomato[i]);
                tomatoposition = tomatoposition + 20;
            }
        }

        public void UpdateMunition(int munition)
        {
            if (munition < _munition) // wenn die Munition weniger wurde (= Schuss), dann entferne letzte Tomate
            {
                //Debug.WriteLine("neu: " + munition + "alt: " + _munition);
                //Debug.WriteLine("_guiImageTomato.count: " + _guiImageTomato.Length);
                _guiHandler.Remove(_guiImageTomato[munition]);
            }
            else
            { 
                for (int i = 0; i < 10; i++)
                {
                    _guiHandler.Remove(_guiImageTomato[i]);
                }
                DrawMunition();
            }
        }

        public void UpdateCustomText()
        {
            _inputToggle = true;
            //Console.Write("Bitte Namen eingeben");

                if (Input.Instance.IsKeyDown(KeyCodes.Enter))
                {
                    _inputToggle = !_inputToggle;
                    _highscore = false;
                    if (nameInput.Text.Length <= 0)
                    {
                        nameInput.Text = "Player1";
                    }
                    Console.Write("Name = " + nameInput.Text);
                    playername = nameInput.Text;
                   // System.Windows.MessageBox.Show("Name = " + nameInput.Text + "\n" + "Points: " + _points + "\n" + "Hier könnte Ihr Highscore stehen!!!");
                }

                if (_inputToggle)
                {
                    if (Input.Instance.IsKeyDown(KeyCodes.A))
                    {
                        nameInput.Text = nameInput.Text + "A";
                        Console.WriteLine("A");
                    }
                        /*else if (Input.Instance.IsKeyDown(KeyCodes.B))
                {
                    _customText.Text = _customText.Text + "B";
                }
               */
                    else if (Input.Instance.IsKeyDown(KeyCodes.C))
                    {
                        nameInput.Text = nameInput.Text + "C";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.D))
                    {
                        nameInput.Text = nameInput.Text + "D";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.E))
                    {
                        nameInput.Text = nameInput.Text + "E";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.F))
                    {
                        nameInput.Text = nameInput.Text + "F";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.G))
                    {
                        nameInput.Text = nameInput.Text + "G";
                    }
                        /*else if (Input.Instance.IsKeyDown(KeyCodes.H))
                {
                    _customText.Text = _customText.Text + "H";
                }
                else if (Input.Instance.IsKeyDown(KeyCodes.I))
                {
                    _customText.Text = _customText.Text + "I";
                }
               */
                    else if (Input.Instance.IsKeyDown(KeyCodes.J))
                    {
                        nameInput.Text = nameInput.Text + "J";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.K))
                    {
                        nameInput.Text = nameInput.Text + "K";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.L))
                    {
                        nameInput.Text = nameInput.Text + "L";
                    }
                        /* else if (Input.Instance.IsKeyDown(KeyCodes.M))
                {
                    _customText.Text = _customText.Text + "M";
                }
               */
                    else if (Input.Instance.IsKeyDown(KeyCodes.N))
                    {
                        nameInput.Text = nameInput.Text + "N";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.O))
                    {
                        nameInput.Text = nameInput.Text + "O";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.P))
                    {
                        nameInput.Text = nameInput.Text + "P";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.Q))
                    {
                        nameInput.Text = nameInput.Text + "Q";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.R))
                    {
                        nameInput.Text = nameInput.Text + "R";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.S))
                    {
                        nameInput.Text = nameInput.Text + "S";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.T))
                    {
                        nameInput.Text = nameInput.Text + "T";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.U))
                    {
                        nameInput.Text = nameInput.Text + "U";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.V))
                    {
                        nameInput.Text = nameInput.Text + "V";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.W))
                    {
                        nameInput.Text = nameInput.Text + "W";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.X))
                    {
                        nameInput.Text = nameInput.Text + "X";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.Y))
                    {
                        nameInput.Text = nameInput.Text + "Y";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.Z))
                    {
                        nameInput.Text = nameInput.Text + "Z";
                    }
                    else if (Input.Instance.IsKeyDown(KeyCodes.Back))
                    {
                        if (nameInput.Text.Length > 0)
                        {
                            nameInput.Text = nameInput.Text.Substring(0, nameInput.Text.Length - 1);
                        }
                    }
                }
        }

        private void OnbtnHighscore(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
           // System.Windows.MessageBox.Show("Name = " + nameInput.Text + "\n" + "Hier könnte Ihr Highscore stehen!!!");
            Console.Write("Name = " + nameInput.Text);
            playername = nameInput.Text; 
            _gameHandler.DbConnection.Insert(playername, _points);
           
             highscoreEintraege = _gameHandler.DbConnection.ShowFirstFiveHighScore();
             foreach (string _highscoreEintrag in highscoreEintraege)
             {
                 _highScoreHandler.Add(new GUIText(_highscoreEintrag, _guiFontWESTERN18, 50, _hsYpos, new float4(0, 0, 1, 1)));
                    _hsYpos = _hsYpos + 20;
                 Console.WriteLine("bla");
             }
        }

        private void OnbtnPlay(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            if (_gameHandler.GameState.CurrentState != GameState.State.Playing)
            {
                _gameHandler.GameState.CurrentState = GameState.State.Playing;
            }
        }
    }
}
