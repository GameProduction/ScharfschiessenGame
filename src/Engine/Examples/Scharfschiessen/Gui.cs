﻿using System;
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
        private IFont _guiFontCabin14;
        private IFont _guiFontCabin18;
        private IFont _guiFontWESTERN30;
        private IFont _guiFontCabin100;
        
        //Texte:
        private GUIText _guiText1;
        private GUIText _guiText2;
        private GUIText _guiText3;
        private GUIText _guiText4;
        private GUIText _guiText5;
        private GUIText _guiText6;
        private GUIText _guiText7;
        private GUIText _guiCredits;
        private GUIText _name;
        private GUIText _guiTextTitel;
        private GUIText _guiTextCredits1;
        private GUIText _guiTextCredits2;
        private GUIText _guiTextCredits3;
        private GUIText _guiTextCredits4;
        private GUIText _guiTextCredits5;
        
        //Arrays für Buttons und Images
        private GUIButton[] _guiDiffs;
        private GUIImage[] _guiImages;
        private GUIImage[] _guiImageTomato;
        private GUIImage[] _highscoreBretter;

        // Diverse public Variablen (für Datenbank )
        public int _points;
        public int _level;
        public int endlevel;
        public GUIText nameInput;   // Textfeld zur Namenseingabe
        public string playername;   //eingegebener name an DB

        //variablen für Anzeige des Highscores
        public GUIText _highscoreEintrag;
        public string[] highscoreEintraege;
        public int _hsYpos;

        // Diverse private Variablen
        private double _countdown;
        private int _munition;
        private bool _showLevelUp;
        private double time;
        private bool _highscore; // Eingabe des Spielernamens
        private bool _neustart;
        private bool _pause;
        private int _aimimage; // für die flexible Größe des Fadenkreuzes
#endregion

        private enum _buttons
        {
            btnStart,
            btnNochmal,
            btnHighscore,
            btnCredits
        };

        private enum _btnimages
        {
            btniStart,
            btniNochmal,
            btniHighscore,
            btniMouse,
            btniFadenkreuz,
            Startbild,
            Endbild,
            btniCreditsA,
            btniCreditsB,
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

            _guiDiffs = new GUIButton[4];
            _guiImageTomato = new GUIImage[10];
            _guiImages = new GUIImage[9];
            _highscoreBretter = new GUIImage[5];

            float textwidth;
            float texthight;
            _highscore = false;
            _neustart = true;
            _pause = true;
            _level = 1;

      #endregion

    #region Bilder
            //Hintergrund Startscreen
            _guiImages[(int)_btnimages.Startbild] = new GUIImage("Assets/startbild.png", 0, 0,-3, width, height);

            //Hintergrund Endscreen
            _guiImages[(int)_btnimages.Endbild] = new GUIImage("Assets/endbild.png", 0, 0, -3, width, height);

            //Maus-Nutzerinfo in Mainmenü einblenden
            _guiImages[(int)_btnimages.btniMouse] = new GUIImage("Assets/Mouse.png", width / 4, 0, -1, (int)(height / 1.322), height);
            
            //Fadenkreuz flexibel (Falls es zwecks Schwierigkeitsstufe kleiner werden soll)
            _aimimage = 80;
            _guiImages[(int)_btnimages.btniFadenkreuz] = new GUIImage("Assets/Fadenkreuz.png", width / 2 - _aimimage / 2, height / 2 - _aimimage / 2, -2, _aimimage, _aimimage);

            // Credits Klohaus
            _guiImages[(int)_btnimages.btniCreditsA] = new GUIImage("Assets/kloClosed.png", width - height, 0, -1, height, height);
            _guiImages[(int)_btnimages.btniCreditsB] = new GUIImage("Assets/kloOpen.png", width - height, 0, -1, height, height);
            
            #endregion

    #region Beschriftungen
            //Schriften
            _guiFontCabin12 = RC.LoadFont("Assets/Cabin.ttf", 12);
            _guiFontCabin14 = RC.LoadFont("Assets/Cabin.ttf", 14);
            _guiFontCabin18 = RC.LoadFont("Assets/Cabin.ttf", 18);
            _guiFontCabin100 = RC.LoadFont("Assets/Cabin.ttf", 100);
            _guiFontWESTERN30 = RC.LoadFont("Assets/WESTERN.ttf", 30);

            //Credits
            texthight = GUIText.GetTextHeight("So hoch.", _guiFontCabin18);
            textwidth = GUIText.GetTextWidth("Sooooooooo laaaaaang", _guiFontCabin14);
            _guiTextCredits1 = new GUIText("Ramazan Gündogdu", _guiFontCabin14, (width) - (int)(textwidth), (height / 3)+ (int)(texthight/2), new float4(1, 1, 1, 1));
            _guiTextCredits2 = new GUIText("Kathleen Hübel", _guiFontCabin14, (width) - (int)(textwidth), (int)(_guiTextCredits1.PosY + texthight), new float4(1, 1, 1, 1));
            _guiTextCredits3 = new GUIText("Linda Schey", _guiFontCabin14, (width) - (int)(textwidth), (int)(_guiTextCredits2.PosY + texthight), new float4(1, 1, 1, 1));
            _guiTextCredits4 = new GUIText("Susanne Schmidt", _guiFontCabin14, (width) - (int)(textwidth), (int)(_guiTextCredits3.PosY + texthight), new float4(1, 1, 1, 1));
            _guiTextCredits5 = new GUIText("Tobias Winterhalder ", _guiFontCabin14, (width) - (int)(textwidth), (int)(_guiTextCredits4.PosY + texthight), new float4(1, 1, 1, 1));

            //Eingabetext Name für Highscore
            texthight = GUIText.GetTextHeight("Lorem ipsum", _guiFontCabin18);
            textwidth = GUIText.GetTextWidth("Write name: ", _guiFontCabin18);
            _name = new GUIText("Write name: ", _guiFontCabin18, (width / 2) - (int)(textwidth), (height / 2), new float4(0, 0, 0, 1));
            nameInput = new GUIText("", _guiFontCabin18, (width / 2), (height / 2), new float4(1, 1, 1, 1));
           
            //Text Mainmenü: Scha(r)fschießen
            textwidth = GUIText.GetTextWidth("Scha(r)fschießen", _guiFontWESTERN30);
            _guiTextTitel = new GUIText("Scha(r)fschießen", _guiFontWESTERN30, (width/2) - (int) (textwidth/2), (height/3));
            _guiTextTitel.TextColor = new float4(0, 0, 0, 1);

            // Text InGame
            textwidth = GUIText.GetTextWidth("Time: ", _guiFontCabin18);
            texthight = GUIText.GetTextHeight("Time: ", _guiFontCabin18);
            _guiText1 = new GUIText("Time: " + _countdown, _guiFontCabin18, (int)textwidth, (int)(texthight*2));
            _guiText1.TextColor = new float4(0, 0, 0, 1);
            textwidth = GUIText.GetTextWidth("Time: ", _guiFontCabin18);
            _guiText2 = new GUIText("Points: " + _points, _guiFontCabin18, width - (int)(textwidth * 3), (int)(texthight*2));
            _guiText2.TextColor = new float4(0, 0, 0, 1);

            //Text LevelUp
            textwidth = GUIText.GetTextWidth("Level Up!", _guiFontCabin100);
            _guiText7 = new GUIText("Level Up!", _guiFontCabin100, (width / 2) - (int)(textwidth/2), (height / 2));
            _guiText7.TextColor = new float4(1, 0, 0, 0.5f);

    #endregion

    #region Buttons
            // Button MainMenü: Starten
            textwidth = GUIText.GetTextWidth("Start", _guiFontWESTERN30);
            texthight = GUIText.GetTextHeight("Start", _guiFontWESTERN30);
            _guiText3 = new GUIText("Start", _guiFontWESTERN30, width / 2 - (int)(textwidth / 2), (int)(height / 1.5));
            _guiText3.TextColor = new float4(1, 1, 1, 1);
            _guiDiffs[(int)_buttons.btnStart] = new GUIButton(_guiText3.PosX - (int)(texthight / 2), _guiText3.PosY - (int)texthight, -2,
                (int)(textwidth * 1.5), (int)(texthight * 1.5));
            _guiImages[(int)_btnimages.btniStart] = new GUIImage("Assets/holz.png", _guiText3.PosX - (int)textwidth / 2,
                _guiText3.PosY - (int)(texthight * 1.5), -1, (int)textwidth * 2, (int)texthight * 2);

            // Button HighscoreMenü: Nochmal spielen
            textwidth = GUIText.GetTextWidth("Play again", _guiFontCabin18);
            _guiText5 = new GUIText("Play again", _guiFontCabin18, (width / 2) - (int)(textwidth * 1.5),
                (height / 2 + height / 3));
            _guiText5.TextColor = new float4(1, 1, 1, 1);
            texthight = GUIText.GetTextHeight("Play again", _guiFontCabin18);
            _guiDiffs[(int) _buttons.btnNochmal] = new GUIButton(_guiText5.PosX, _guiText5.PosY - (int) texthight, -2,
                (int) textwidth, (int) texthight);
            _guiImages[(int) _btnimages.btniNochmal] = new GUIImage("Assets/holz.png",
                _guiText5.PosX - (int) textwidth/2, _guiText5.PosY - (int) (texthight*1.5), -1, (int) textwidth*2,
                (int) texthight*2);
           
            // Button HighscoreMenü: Highscore
            textwidth = GUIText.GetTextWidth("Enter in Highscore", _guiFontCabin18);
            _guiText6 = new GUIText("Enter in Highscore", _guiFontCabin18, (width / 2) + (int)(textwidth / 5), (height / 2 + height / 3));
            _guiText6.TextColor = new float4(1, 1, 1, 1);
            texthight = GUIText.GetTextHeight("Enter in Highscore", _guiFontCabin18);
            _guiDiffs[(int) _buttons.btnHighscore] = new GUIButton(_guiText6.PosX, _guiText6.PosY - (int) texthight, -2, (int) textwidth, (int) texthight);
            _guiImages[(int) _btnimages.btniHighscore] = new GUIImage("Assets/holz.png",_guiText6.PosX - (int)textwidth / 8, _guiText6.PosY - (int)(texthight * 1.5), -1, (int)(textwidth * 1.2), (int) texthight*2);  

            //Button Credits
            textwidth = GUIText.GetTextWidth("Credits", _guiFontCabin18);
            texthight = GUIText.GetTextWidth("Credits", _guiFontCabin18);
            _guiCredits = new GUIText("Credits", _guiFontCabin18, width - (int)(textwidth*1.5), (height / 2), new float4(1, 1, 1, 1));
            _guiDiffs[(int)_buttons.btnCredits] = new GUIButton(_guiCredits.PosX, _guiCredits.PosY - (int)texthight, -2,
               (int)textwidth, (int)texthight);
        #endregion
        }

        public void DrawGui() // Wird jeden Frame aufgerufen
        {
            //Gui Rendern
            _guiHandler.RenderGUI();

            if (_gameHandler.GameState.CurrentState == GameState.State.Playing)  //Countdown nur während dem Spiel
            {
                _countdown = (int) _gameHandler.Game.Countdown; //Hole Info aus Game-Klasse
                _guiText1.Text = "Time:  " + _countdown;    //Zeige Zeit an
                _points = _gameHandler.Game.Points;     //Hole Info aus Game-Klasse
                _guiText2.Text = "Points: " + _points;  //Zeige Countdown an
                if (_munition != _gameHandler.Game.Weapon.Magazin) //Wenn sich die Munition verändert hat = Wenn geschossen wurde
                {
                    UpdateMunition(_gameHandler.Game.Weapon.Magazin);   
                }
                _munition = _gameHandler.Game.Weapon.Magazin; // Munition nimmt den neuen Wert an um wieder auf Veränderung getestet werden zu können
                if (_showLevelUp) // Wenn LevelUp-Info Angezeigt wird...
                {
                    TimerLevelUp(); //... dann starte den Timer, wie lange die Info angezeigt wird
                }
            }

            if (_highscore == true) //Eingegebener Name nur nach dem Spiel sichtbar
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
                    // oder DummyFunktion() ist aber eigendlich nicht nötig.
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
            var height = _rCanvas.Height;
            var width = _rCanvas.Width;
            _guiHandler = _mainmenuHandler;
            _mainmenuHandler.Add(_guiImages[(int) _btnimages.Startbild]);
            _mainmenuHandler.Add(_guiImages[(int) _btnimages.btniStart]);
            _mainmenuHandler.Add(_guiTextTitel);
            _mainmenuHandler.Add(_guiText3);
            _mainmenuHandler.Add(_guiImages[(int)_btnimages.btniMouse]); //Mouse-Anleitung wird angezeigt
            _mainmenuHandler.Add(_guiDiffs[(int) _buttons.btnStart]);
            _guiDiffs[(int) _buttons.btnStart].OnGUIButtonDown += OnbtnPlay;
        }

        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _countdown = _gameHandler.Game.Countdown;

            // Erst if-Abfrage, ob ich grad aus der Pause komme, weil ich dann nichts überschreiben will
            if (_gameHandler.GameState.LastState == GameState.State.HiddenPause && _gameHandler.GameState.CurrentState == GameState.State.Playing)
                {
                 _neustart = false; // Komme also aus der Pause und will nichts verändern
                }
            else // Starte neues Spiel
            {
                _neustart = true;
                for (int i = 0; i < 10; i++) //alle Tomaten noch weg
                {
                    _guiHandler.Remove(_guiImageTomato[i]);
                }
            }
            #region Neustart
            if (_neustart)
            {
            _guiHandler.Clear();
            _guiHandler = _inGameHandler; 
            _inGameHandler.Remove(_guiTextTitel);
            _inGameHandler.Add(_guiText1);
            _inGameHandler.Add(_guiText2);
            _inGameHandler.Add(_guiImages[(int)_btnimages.btniFadenkreuz]);
            _munition = _gameHandler.Game.Weapon.Magazin;
            for (int i = 0; i < 10; i++)
            {
                _guiHandler.Remove(_guiImageTomato[i]);
            }
            DrawMunition();
            }
            #endregion

            _pause = true;
        }

        public void ShowLevelUp()
        {
            _showLevelUp = true;
            _level++;
            endlevel = _level;
            _inGameHandler.Add(_guiText7);
            time = 1;
        }

        private void RemoveLevelUp()
        {
            _inGameHandler.Remove(_guiText7);
        }

        private void TimerLevelUp()
        {
            if (time > 0)
            {
                time -= Time.Instance.DeltaTime;
            }
            else
            {
                RemoveLevelUp();
                _showLevelUp = false;
                time = 1;
            }
        }

        internal void HighScoreGui()
        {
            var height = _rCanvas.Height;
            var width = _rCanvas.Width;
            _highscore = true;
            _guiHandler = _highScoreHandler;

            // Erst if-Abfrage, ob ich grad aus der Pause komme, weil ich dann nichts überschreiben will
            if (_gameHandler.GameState.LastState != GameState.State.HiddenPause &&
                _gameHandler.GameState.CurrentState == GameState.State.Highscore)
            {
                Console.WriteLine("HighScoreGui");
                _guiHandler.Clear(); // Alte Infos aus dem GameHandler entfernen (z.B. Stand der Tomaten)
                for (int i = 0; i < 10; i++) //Restliche Tomaten aus dem Spiel noch entfernen
                {
                    _highScoreHandler.Remove(_guiImageTomato[i]);
                }

                // Bilder&Texte&Buttons hinzufügen
                _highScoreHandler.Add(_guiImages[(int) _btnimages.Endbild]);
                _highScoreHandler.Add(_guiImages[(int) _btnimages.btniCreditsA]);
                _highScoreHandler.Add(_guiImages[(int) _btnimages.btniNochmal]);
                _highScoreHandler.Add(_guiImages[(int) _btnimages.btniHighscore]);
                _highScoreHandler.Add(_guiText5);
                _highScoreHandler.Add(_guiText6);
                _highScoreHandler.Add(nameInput);
                _highScoreHandler.Add(_name);
                _highScoreHandler.Add(_guiCredits);
                _highScoreHandler.Add(_guiDiffs[(int) _buttons.btnCredits]);
                _highScoreHandler.Add(_guiDiffs[(int) _buttons.btnNochmal]);
                _highScoreHandler.Add(_guiDiffs[(int) _buttons.btnHighscore]);
                _guiDiffs[(int)_buttons.btnNochmal].OnGUIButtonDown += OnbtnPlay;
                _guiDiffs[(int)_buttons.btnHighscore].OnGUIButtonDown += OnbtnHighscore;
                _guiDiffs[(int)_buttons.btnCredits].OnGUIButtonDown += Credits;
              
                // Punkte
                if (_highScoreHandler.Contains(_guiText4))
                {
                    _highScoreHandler.Remove(_guiText4);
                }
                float textwidth;
                textwidth = GUIText.GetTextWidth("You reached --- Points!", _guiFontWESTERN30);
                _guiText4 = new GUIText("You reached " + _points + " Points!", _guiFontWESTERN30, width / 2 - (int)(textwidth / 2), (height / 3), new float4(1, 0, 0, 1));
                _highScoreHandler.Add(_guiText4);
            }
        }

        public void DrawMunition() // Alle 10 Tomaten einmalig hinmalen
        {
            int tomatoposition = 10;

            for (int i = 0; i < 10; i++)
            {
                var height = _rCanvas.Height;
                 _guiImageTomato[i]=new GUIImage("Assets/tomateAmmoTexture.png", tomatoposition, height - 50, -1, 40, 40);
                _guiHandler.Add(_guiImageTomato[i]);
                tomatoposition = tomatoposition + 40;
            }
        }

        public void UpdateMunition(int munition)
        {
            if (munition < _munition) // wenn die Munition weniger wurde (= Schuss), dann entferne letzte Tomate
            {
                _guiHandler.Remove(_guiImageTomato[munition]);
            }
            else // wenn aufgeladen wurde, alle weg verbleibenden weg und neu zeichnen
            { 
                for (int i = 0; i < 10; i++)
                {
                    _guiHandler.Remove(_guiImageTomato[i]);
                }
                for (int i = 0; i < 10; i++)
                {
                    _guiHandler.Add(_guiImageTomato[i]);
                }
               // DrawMunition();
            }
        }

        public void UpdateCustomText()
        {
            //_highscore = true;

            #region Name mit Enter eintragen

            if (Input.Instance.IsKeyDown(KeyCodes.Enter))
            {
                _highscore = false;
                if (nameInput.Text.Length <= 0)
                {
                    nameInput.Text = "Anonymous";
                }
            }

            #endregion

            #region Namenseingabe

            if (_highscore && nameInput.Text.Length < 9)
            {
                if (Input.Instance.IsKeyDown(KeyCodes.A))
                {
                    nameInput.Text = nameInput.Text + "A";
                }
                else if (Input.Instance.IsKeyDown(KeyCodes.B))
                {
                    nameInput.Text = nameInput.Text + "B";
                }
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
                else if (Input.Instance.IsKeyDown(KeyCodes.H))
                {
                    nameInput.Text = nameInput.Text + "H";
                }
                else if (Input.Instance.IsKeyDown(KeyCodes.I))
                {
                    nameInput.Text = nameInput.Text + "I";
                }
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
                else if (Input.Instance.IsKeyDown(KeyCodes.M))
                {
                    nameInput.Text = nameInput.Text + "M";
                }
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
            }
            if (Input.Instance.IsKeyDown(KeyCodes.Back))
            {
                if (nameInput.Text.Length > 0)
                {
                    nameInput.Text = nameInput.Text.Substring(0, nameInput.Text.Length - 1);
                }
            }
                #endregion    
        }

        private void OnbtnHighscore(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            PlayerDataDb[] playerData = new PlayerDataDb[5];
            if (nameInput.Text.Length <= 0)
            {
                nameInput.Text = "Anonymous";
            }
            playername = nameInput.Text; 
            // _gameHandler.DbConnection.Insert(playername, _points, endlevel); //Übergebe Daten an DB
             _hsYpos = 50;  //Schleifenvariable um die Einträge zeilenweise untereinander zu setzen
           //  playerData = _gameHandler.DbConnection.ShowFirstFiveHighScore(); //Hole Daten von DB
             foreach (PlayerDataDb data in playerData) // Setze Strings aus Datenbank in Zeilen untereinander
             {
                 _guiHandler.Add(new GUIText("Name: "+data.Name, _guiFontCabin12, 120, _hsYpos, -1, new float4(1, 1, 1, 1))); // oder highscoreHandler???
                 _guiHandler.Add(new GUIText("                                      Level: "+data.Level.ToString(), _guiFontCabin12, 120, _hsYpos, -1, new float4(1, 1, 1, 1))); // oder highscoreHandler???
                 _guiHandler.Add(new GUIText("                                                           Points: "+data.Score.ToString(), _guiFontCabin12, 120, _hsYpos, -1, new float4(1, 1, 1, 1))); // oder highscoreHandler???
                 _hsYpos = _hsYpos + 20;
             }
             int _brettposition = 38; //Schleifenvariable um die Bretter zeilenweise untereinander zu setzen
             for (int i = 0; i < 5; i++)
             {
                 _highscoreBretter[i] = new GUIImage("Assets/Holz.png", 115, _brettposition, -2, 300, 15); //Bretterbilder anlegen
                 _guiHandler.Add(_highscoreBretter[i]); // oder highscoreHandler???
                 _brettposition = _brettposition + 20;
             }
        }

        private void OnbtnPlay(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            if (_gameHandler.GameState.CurrentState != GameState.State.Playing)
            {
                _gameHandler.GameState.CurrentState = GameState.State.Playing;
            }
        }

        private void Credits(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            if (_gameHandler.GameState.CurrentState == GameState.State.Highscore)
            {
                _guiHandler.Remove(_guiCredits);
                _guiHandler.Remove(_guiDiffs[(int)_buttons.btnCredits]);
                _guiHandler.Remove(_guiImages[(int)_btnimages.btniCreditsA]); 
                _guiDiffs[(int)_buttons.btnCredits].OnGUIButtonDown -= Credits;
                _guiHandler.Add(_guiImages[(int) _btnimages.btniCreditsB]);
                _guiHandler.Add(_guiTextCredits1);
                _guiHandler.Add(_guiTextCredits2);
                _guiHandler.Add(_guiTextCredits3);
                _guiHandler.Add(_guiTextCredits4);
                _guiHandler.Add(_guiTextCredits5);
            }
        }
    }
}
