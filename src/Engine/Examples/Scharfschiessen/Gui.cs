using System;
using System.Windows;
using System.Windows.Forms;
using Fusee.Engine;
using Fusee.Math;
using MouseButtons = Fusee.Engine.MouseButtons;


namespace Examples.Scharfschiessen
{

    public class Gui
    {
        private RenderCanvas _rCanvas;
        private readonly GameHandler _gameHandler;
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
        private GUIText _name;
        private GUIText _guiTextTitel;
        // private GUIText[] _guiText;
        private GUIButton[] _guiDiffs;
        private GUIImage[] _guiImages;
        private double _countdown;
        private double _points;
        private int _munition ;
        public GUIText nameInput;
        private bool _inputToggle;
        private bool _highscore;
        public string playername;

        private enum _buttons
        {
            btnStart,
            btnNochmal,
            btnHighscore,
            btnInGame,
        };

        private enum _btnimages
        {
            btniStart,
            btniNochmal,
            btniHighscore,
            btniTomato1, btniTomato2, btniTomato3, btniTomato4, btniTomato5, btniTomato6, btniTomato7, btniTomato8, btniTomato9, btniTomato10 
        };

        public Gui(RenderContext RC, RenderCanvas rCanvas, GameHandler gameHandler)
            //da initialisieren wir alles für den GuiHandler
        {
            _highscore = false;
            _rCanvas = rCanvas;
            var height = _rCanvas.Height;
            var width = _rCanvas.Width;
            float textwidth;
            float texthight;
            _gameHandler = gameHandler;
            _guiHandler = new GUIHandler(RC);
            _mainmenuHandler = new GUIHandler(RC);
            _inGameHandler = new GUIHandler(RC);
            _highScoreHandler = new GUIHandler(RC);
            _guiDiffs = new GUIButton[4];
            _guiImages = new GUIImage[13];
            _munition = 10;

            _guiFontCabin12 = RC.LoadFont("Assets/Cabin.ttf", 12);
            _guiFontCabin24 = RC.LoadFont("Assets/Cabin.ttf", 24);
            _guiFontAlphaWood18 = RC.LoadFont("Assets/AlphaWood.ttf", 18);
            _guiFontWESTERN30 = RC.LoadFont("Assets/WESTERN.ttf", 30);

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

            // Button MainMenü: Starten
            textwidth = GUIText.GetTextWidth("Start", _guiFontCabin12);
            texthight = GUIText.GetTextHeight("Start", _guiFontCabin12);
            _guiText3 = new GUIText("Start", _guiFontCabin12, width/2 - (int) (textwidth/2), (height/2));
            _guiText3.TextColor = new float4(1, 1, 1, 1);
            _guiDiffs[(int) _buttons.btnStart] = new GUIButton(_guiText3.PosX, _guiText3.PosY - (int) texthight, -2,
                (int) textwidth, (int) texthight);
            _guiImages[(int) _btnimages.btniStart] = new GUIImage("Assets/holz.png", _guiText3.PosX - (int) textwidth/2,
                _guiText3.PosY - (int) (texthight*1.5), -1, (int) textwidth*2, (int) texthight*2);

            // Text HighscoreMenü: Game Over
            textwidth = GUIText.GetTextWidth("Game Over!", _guiFontWESTERN30);
            _guiText4 = new GUIText("Game Over!", _guiFontWESTERN30, width/2 - (int) (textwidth/2), (height/3));
            _guiText4.TextColor = new float4(0, 0, 0, 1);

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

            _guiDiffs[(int) _buttons.btnInGame] = new GUIButton(0, 0, -3, width, height);
            

            //Test-Dummies für Munition
            _guiImages[(int)_btnimages.btniTomato1] = new GUIImage("Assets/holz.png", 10, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato2] = new GUIImage("Assets/holz.png", 30, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato3] = new GUIImage("Assets/holz.png", 50, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato4] = new GUIImage("Assets/holz.png", 70, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato5] = new GUIImage("Assets/holz.png", 90, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato6] = new GUIImage("Assets/holz.png", 110, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato7] = new GUIImage("Assets/holz.png", 130, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato8] = new GUIImage("Assets/holz.png", 150, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato9] = new GUIImage("Assets/holz.png", 170, height / 2, -1, 10, 20);
            _guiImages[(int)_btnimages.btniTomato10] = new GUIImage("Assets/holz.png", 190, height / 2, -1, 10, 20);

        }


        public void DrawGui()
        {
            //Gui Rendern
            _guiHandler.RenderGUI();

            if (_gameHandler.Game != null) //Countdown nur während dem Spiel
            {
                _countdown = (int) _gameHandler.Game.Countdown;
                _guiText1.Text = "Time:  " + _countdown;
                _points = (int)_gameHandler.Game.Points;
                _guiText2.Text = "Points: " + _points;

                if (Input.Instance.IsKeyDown(KeyCodes.Back))
                {
                    _munition = 10;
                    Console.WriteLine("Munition aufgeladen: " + _munition);
                    DrawMunition();
                    return;
                }

               
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
                    //InGameGui(); 
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
            _guiHandler = _mainmenuHandler;
            _mainmenuHandler.Add(_guiImages[(int) _btnimages.btniStart]);
            _mainmenuHandler.Add(_guiTextTitel);
            _mainmenuHandler.Add(_guiText3);
            _guiHandler.Add(_guiDiffs[(int) _buttons.btnStart]);
            _guiDiffs[(int) _buttons.btnStart].OnGUIButtonDown += OnbtnPlay;
        }


        internal void InGameGui()
        {
            //set guiHander für während das Spiel läuft (während der Pause?)
            Console.WriteLine("InGameGui");
            _guiDiffs[(int)_buttons.btnHighscore].OnGUIButtonDown -= OnbtnHighscore;
            _guiDiffs[(int)_buttons.btnInGame].OnGUIButtonDown += Shoot;
            _inGameHandler.Remove(_guiTextTitel);
            _guiHandler = _inGameHandler;
            _countdown = _gameHandler.Game.Countdown;
            _inGameHandler.Add(_guiText1);
            _inGameHandler.Add(_guiText2);
            DrawMunition();
        }

        public void DrawMunition()
        {
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato1]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato2]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato3]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato4]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato5]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato6]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato7]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato8]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato9]);
            _guiHandler.Add(_guiImages[(int)_btnimages.btniTomato10]); 
            }

        private void Shoot(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            if (_munition > 0)
            {
                _munition--;
                _guiHandler.Remove(_guiImages[_munition+3]);
                Console.WriteLine("Munition: " + _munition);
            }
            else
            {
                Console.WriteLine("Keine Munition mehr!");
            }
        }

        internal void HighScoreGui()
        {
            //set guiHander für die Highscore Anzeige
            Console.WriteLine("HighScoreGui");
            _highscore = true;
            _guiHandler = _highScoreHandler;
            _highScoreHandler.Add(_guiImages[(int) _btnimages.btniNochmal]);
            _highScoreHandler.Add(_guiImages[(int) _btnimages.btniHighscore]);
            _highScoreHandler.Add(_guiText4);
            _highScoreHandler.Add(_guiText5);
            _highScoreHandler.Add(_guiText6);
            _guiHandler.Add(_guiDiffs[(int) _buttons.btnNochmal]);
            _guiDiffs[(int) _buttons.btnNochmal].OnGUIButtonDown += OnbtnPlay;
            _guiHandler.Add(_guiDiffs[(int) _buttons.btnHighscore]);
            _guiDiffs[(int) _buttons.btnHighscore].OnGUIButtonDown += OnbtnHighscore;
            _highScoreHandler.Add(nameInput);
            _highScoreHandler.Add(_name); 
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
                    System.Windows.MessageBox.Show("Name = " + nameInput.Text + "\n" + "Points: " + _points + "\n" + "Hier könnte Ihr Highscore stehen!!!");
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
            //To-Do: Hier verlinken zur Datenbank
            System.Windows.MessageBox.Show("Name = " + nameInput.Text + "\n" + "Hier könnte Ihr Highscore stehen!!!");
            Console.Write("Name = " + nameInput.Text);
            playername = nameInput.Text;
           _gameHandler.GameState.CurrentState = GameState.State.MainMenu;
        }

        private void OnbtnPlay(GUIButton sender, Fusee.Engine.MouseEventArgs mea)
        {
            _gameHandler.GameState.CurrentState = GameState.State.Playing;
        }
    }
}
