using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ChestWinFormsApp
{
    public partial class GameForm : Form
    {
        public int[,] gameMap;
        public Button[,] buttons;
        public Button prevButton;
        public bool isFigureInMoving;
        public int currentPlayer;
        public Color mapColor;
        public int newFigureVal;
        public GameForm()
        {
            InitializeComponent();
            NewGame();
            ClientSize = new Size(825, 680);
        }

        public void NewGame()
        {
            currentPlayer = 1;
            gameMap = new int[8, 8]
            {
                {21,22,23,24,25,23,22,21},
                {26,26,26,26,26,26,26,26},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {16,16,16,16,16,16,16,16},
                {11,12,13,14,15,13,12,11},
            };
            buttons = new Button[8, 8];
            isFigureInMoving = false;
            CreatMap();
        }
        public void ButtonInFocus(object sender, EventArgs e)
        {
            Button buttonPressed = sender as Button;
            int rowCurrFigure = buttonPressed.Location.Y / 85;
            int columnCurrFigure = buttonPressed.Location.X / 85;
            int currFigure = gameMap[rowCurrFigure, columnCurrFigure] % 10;
            if (prevButton != null)
            {
                prevButton.BackgroundImage = null;
            }
            if (gameMap[rowCurrFigure, columnCurrFigure] != 0 && gameMap[rowCurrFigure, columnCurrFigure] / 10 == currentPlayer)
            {
                ClearStepsForMoving();
                buttonPressed.BackgroundImage = Properties.Resources.red;
                isFigureInMoving = true;
                ShowMovingSteps(rowCurrFigure, columnCurrFigure, currFigure);
                prevButton = buttonPressed;
            }
            else
            {
                if (isFigureInMoving && buttonPressed.BackgroundImage != null)
                {
                    MakeAMove(buttonPressed);
                    Task.Delay(500);
                    ClearStepsForMoving();
                    CheckForPawnChenging(rowCurrFigure, columnCurrFigure, gameMap[rowCurrFigure, columnCurrFigure] % 10);                    
                    SwitchPlayer();
                    prevButton = buttonPressed;
                }
            }
        }

        private void CheckForPawnChenging(int rowCurrFigure, int columnCurrFigure, int currFigure)
        {
            if (currentPlayer == 1)
            {
                if(currFigure == 6 && rowCurrFigure == 0)
                {
                    var newFigure = new WhitePawnChengingFiguresForm(this, rowCurrFigure, columnCurrFigure);
                    newFigure.Show();
                    this.Enabled = false;
                }
            }
            else
            {
                if (currFigure == 6 && rowCurrFigure == 7)
                {
                    var newFigure = new BlackPawnChengingFiguresForm(this, rowCurrFigure, columnCurrFigure);
                    newFigure.Show();
                    this.Enabled = false;
                }
            }
        }

        public void MakeAMove(Button buttonPressed)
        {
            buttonPressed.Image = prevButton.Image;
            prevButton.Image = null;
            buttonPressed.ForeColor = Color.Black;
            prevButton.ForeColor = Color.Black;
            if(currentPlayer == 1 && gameMap[buttonPressed.Location.Y / 85, buttonPressed.Location.X / 85] % 10 == 5)
            {
                EndGame(1);
            }
            else if(currentPlayer == 2 && gameMap[buttonPressed.Location.Y / 85, buttonPressed.Location.X / 85] % 10 == 5)
            {
                EndGame(2);
            }
            else
            {
                var tempMapVal = 0;
                gameMap[buttonPressed.Location.Y / 85, buttonPressed.Location.X / 85] = gameMap[prevButton.Location.Y / 85, prevButton.Location.X / 85];
                gameMap[prevButton.Location.Y / 85, prevButton.Location.X / 85] = tempMapVal;
                isFigureInMoving = false;
            }                        
        }

        private void EndGame(int currPlayer)
        {
            if (currPlayer == 1)
            {
                MessageBox.Show("The white figures won!", "End game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The black figures won!", "End game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            restartButton.PerformClick();

        }

        public void ShowMovingSteps(int rowCurrFigure, int columnCurrFigure, int currFigure)
        {
            int direction = currentPlayer == 1 ? 1 : -1;
            switch (currFigure % 10)
            {
                case 1:
                    ShowGorizontalVerticalSteps(rowCurrFigure, columnCurrFigure, false);
                    break;
                case 2:
                    ShowHorseSteps(rowCurrFigure, columnCurrFigure);
                    break;
                case 3:
                    ShowAngleSteps(rowCurrFigure, columnCurrFigure, false);
                    break;
                case 4:
                    ShowAngleSteps(rowCurrFigure, columnCurrFigure, false);
                    ShowGorizontalVerticalSteps(rowCurrFigure, columnCurrFigure, false);
                    break;
                case 5:
                    ShowAngleSteps(rowCurrFigure, columnCurrFigure, true);
                    ShowGorizontalVerticalSteps(rowCurrFigure, columnCurrFigure, true);
                    break;
                case 6:
                    if (IsOnMap(rowCurrFigure - 1 * direction, columnCurrFigure))
                    {
                        if (gameMap[rowCurrFigure - 1 * direction, columnCurrFigure] == 0)
                        {
                            buttons[rowCurrFigure - 1 * direction, columnCurrFigure].BackgroundImage = Properties.Resources.Green;
                            buttons[rowCurrFigure - 1 * direction, columnCurrFigure].BackgroundImageLayout = ImageLayout.Stretch;
                        }
                        if (IsOnMap(rowCurrFigure - 2 * direction, columnCurrFigure) && gameMap[rowCurrFigure - 2 * direction, columnCurrFigure] == 0 && buttons[rowCurrFigure, columnCurrFigure].ForeColor == Color.Green)
                        {
                            buttons[rowCurrFigure - 2 * direction, columnCurrFigure].BackgroundImage = Properties.Resources.Green;
                            buttons[rowCurrFigure - 2 * direction, columnCurrFigure].BackgroundImageLayout = ImageLayout.Stretch;
                        }

                    }
                    if (IsOnMap(rowCurrFigure - 1 * direction, columnCurrFigure + 1))
                    {
                        if (gameMap[rowCurrFigure - 1 * direction, columnCurrFigure + 1] != 0 && gameMap[rowCurrFigure + 1 * direction, columnCurrFigure + 1] / 10 != currentPlayer)
                        {
                            buttons[rowCurrFigure - 1 * direction, columnCurrFigure + 1].BackgroundImage = Properties.Resources.Green;
                            buttons[rowCurrFigure - 1 * direction, columnCurrFigure + 1].BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                    if (IsOnMap(rowCurrFigure - 1 * direction, columnCurrFigure - 1))
                    {
                        if (gameMap[rowCurrFigure - 1 * direction, columnCurrFigure - 1] != 0 && gameMap[rowCurrFigure + 1 * direction, columnCurrFigure - 1] / 10 != currentPlayer)
                        {
                            buttons[rowCurrFigure - 1 * direction, columnCurrFigure - 1].BackgroundImage = Properties.Resources.Green;
                            buttons[rowCurrFigure - 1 * direction, columnCurrFigure - 1].BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                    break;

            }
        }
        private void ShowHorseSteps(int rowCurrFigure, int columnCurrFigure)
        {
            if (IsOnMap(rowCurrFigure + 2, columnCurrFigure + 1))
            {
                if (gameMap[rowCurrFigure + 2, columnCurrFigure + 1] == 0 || gameMap[rowCurrFigure + 2, columnCurrFigure + 1] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure + 2, columnCurrFigure + 1].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure + 2, columnCurrFigure + 1].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure + 1, columnCurrFigure + 2))
            {
                if (gameMap[rowCurrFigure + 1, columnCurrFigure + 2] == 0 || gameMap[rowCurrFigure + 1, columnCurrFigure + 2] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure + 1, columnCurrFigure + 2].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure + 1, columnCurrFigure + 2].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure - 2, columnCurrFigure + 1))
            {
                if (gameMap[rowCurrFigure - 2, columnCurrFigure + 1] == 0 || gameMap[rowCurrFigure - 2, columnCurrFigure + 1] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure - 2, columnCurrFigure + 1].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure - 2, columnCurrFigure + 1].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure - 1, columnCurrFigure + 2))
            {
                if (gameMap[rowCurrFigure - 1, columnCurrFigure + 2] == 0 || gameMap[rowCurrFigure - 1, columnCurrFigure + 2] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure - 1, columnCurrFigure + 2].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure - 1, columnCurrFigure + 2].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure - 2, columnCurrFigure - 1))
            {
                if (gameMap[rowCurrFigure - 2, columnCurrFigure - 1] == 0 || gameMap[rowCurrFigure - 2, columnCurrFigure - 1] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure - 2, columnCurrFigure - 1].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure - 2, columnCurrFigure - 1].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure - 1, columnCurrFigure - 2))
            {
                if (gameMap[rowCurrFigure - 1, columnCurrFigure - 2] == 0 || gameMap[rowCurrFigure - 1, columnCurrFigure - 2] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure - 1, columnCurrFigure - 2].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure - 1, columnCurrFigure - 2].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure + 2, columnCurrFigure - 1))
            {
                if (gameMap[rowCurrFigure + 2, columnCurrFigure - 1] == 0 || gameMap[rowCurrFigure + 2, columnCurrFigure - 1] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure + 2, columnCurrFigure - 1].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure + 2, columnCurrFigure - 1].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            if (IsOnMap(rowCurrFigure + 1, columnCurrFigure - 2))
            {
                if (gameMap[rowCurrFigure + 1, columnCurrFigure - 2] == 0 || gameMap[rowCurrFigure + 1, columnCurrFigure - 2] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure + 1, columnCurrFigure - 2].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure + 1, columnCurrFigure - 2].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }
        private void ShowAngleSteps(int rowCurrFigure, int columnCurrFigure, bool isOneStep)
        {
            int i = rowCurrFigure + 1;
            for (int j = columnCurrFigure + 1; j < 8 && i < 8; j++, i++)
            {
                if (IsOnMap(i, j))
                {
                    if (!CanShow(i, j))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
            i = rowCurrFigure - 1;
            for (int j = columnCurrFigure + 1; j < 8 && i >= 0; j++, i--)
            {
                if (IsOnMap(i, j))
                {
                    if (!CanShow(i, j))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
            i = rowCurrFigure + 1;
            for (int j = columnCurrFigure - 1; j >= 0 && i < 8; j--, i++)
            {
                if (IsOnMap(i, j))
                {
                    if (!CanShow(i, j))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
            i = rowCurrFigure - 1;
            for (int j = columnCurrFigure - 1; j >= 0 && i >= 0; j--, i--)
            {
                if (IsOnMap(i, j))
                {
                    if (!CanShow(i, j))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
        }
        private void ShowGorizontalVerticalSteps(int rowCurrFigure, int columnCurrFigure, bool isOneStep)
        {
            for (int j = columnCurrFigure + 1; j < 8; j++)
            {
                if (IsOnMap(rowCurrFigure, j))
                {
                    if (!CanShow(rowCurrFigure, j))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
            for (int j = columnCurrFigure - 1; j >= 0; j--)
            {
                if (IsOnMap(rowCurrFigure, j))
                {
                    if (!CanShow(rowCurrFigure, j))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
            for (int i = rowCurrFigure + 1; i < 8; i++)
            {
                if (IsOnMap(i, columnCurrFigure))
                {
                    if (!CanShow(i, columnCurrFigure))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
            for (int i = rowCurrFigure - 1; i >= 0; i--)
            {
                if (IsOnMap(i, columnCurrFigure))
                {
                    if (!CanShow(i, columnCurrFigure))
                    {
                        break;
                    }
                }
                if (isOneStep)
                {
                    break;
                }
            }
        }

        private bool CanShow(int rowCurrFigure, int j)
        {
            if (gameMap[rowCurrFigure, j] == 0)
            {
                buttons[rowCurrFigure, j].BackgroundImage = Properties.Resources.Green;
                buttons[rowCurrFigure, j].BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                if (gameMap[rowCurrFigure, j] / 10 != currentPlayer)
                {
                    buttons[rowCurrFigure, j].BackgroundImage = Properties.Resources.Green;
                    buttons[rowCurrFigure, j].BackgroundImageLayout = ImageLayout.Stretch;
                }
                return false;
            }
            return true;
        }

        public bool IsOnMap(int i, int j)
        {
            return i >= 0 && j >= 0 && i < 8 && j < 8;
        }
        public void ClearStepsForMoving()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttons[i, j].BackgroundImage = null;
                }
            }
        }
        public void CreatMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var button = new Button();
                    button.Size = new Size(85, 85);
                    button.Location = new Point(j * 85, i * 85);
                    if ((i + j) % 2 == 0)
                    {
                        button.BackColor = Color.LightGray;
                    }
                    else
                    {
                        button.BackColor = Color.White;
                    }
                    switch (gameMap[i, j] / 10)
                    {
                        case 1:
                            {
                                switch (gameMap[i, j] % 10)
                                {
                                    case 1:
                                        {
                                            button.Image = Properties.Resources.wR;
                                        }
                                        break;
                                    case 2:
                                        {
                                            button.Image = Properties.Resources.wN;
                                        }
                                        break;
                                    case 3:
                                        {
                                            button.Image = Properties.Resources.wB;
                                        }
                                        break;
                                    case 4:
                                        {
                                            button.Image = Properties.Resources.wQ;
                                        }
                                        break;
                                    case 5:
                                        {
                                            button.Image = Properties.Resources.wK;
                                        }
                                        break;
                                    case 6:
                                        {
                                            button.Image = Properties.Resources.wP;
                                            button.ForeColor = Color.Green;
                                        }
                                        break;
                                }
                            }
                            break;
                        case 2:
                            {
                                switch (gameMap[i, j] % 10)
                                {
                                    case 1:
                                        {
                                            button.Image = Properties.Resources.bR;
                                        }
                                        break;
                                    case 2:
                                        {
                                            button.Image = Properties.Resources.bN;
                                        }
                                        break;
                                    case 3:
                                        {
                                            button.Image = Properties.Resources.bB;
                                        }
                                        break;
                                    case 4:
                                        {
                                            button.Image = Properties.Resources.bQ;
                                        }
                                        break;
                                    case 5:
                                        {
                                            button.Image = Properties.Resources.bK;
                                        }
                                        break;
                                    case 6:
                                        {
                                            button.Image = Properties.Resources.bP;
                                            button.ForeColor = Color.Green;
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    button.Click += new EventHandler(ButtonInFocus);
                    Controls.Add(button);
                    buttons[i, j] = button;
                }
            }
        }
        public void ClearMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Controls.Remove(buttons[i, j]);
                }
            }
        }
        public void SwitchPlayer()
        {
            if (currentPlayer == 1)
            {
                currentPlayer = 2;
            }
            else
            {
                currentPlayer = 1;
            }
        }
        private void restartButton_Click(object sender, EventArgs e)
        {
            ClearMap();
            NewGame();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}