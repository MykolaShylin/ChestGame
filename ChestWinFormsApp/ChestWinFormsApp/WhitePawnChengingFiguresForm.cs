using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChestWinFormsApp
{
    public partial class WhitePawnChengingFiguresForm : Form
    {
        public int[] numberOfFigures;
        public Button prevButton;
        public GameForm playDeskForm;
        public int rowCurrFigure;
        public int columnCurrFigure;
        public WhitePawnChengingFiguresForm(GameForm playDeskForm, int rowCurrFigure, int columnCurrFigure)
        {
            InitializeComponent();
            numberOfFigures = new int[5] { 1, 2, 3, 4, 5 };
            this.playDeskForm= playDeskForm;
            this.rowCurrFigure = rowCurrFigure;
            this.columnCurrFigure = columnCurrFigure;
        }

        private void WhitePawnChengingFiguresForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                var button = new Button();
                button.Size = new Size(85, 85);
                button.Location = new Point(i * 85, 55);
                switch (numberOfFigures[i])
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
                            button.Image = Properties.Resources.wP;
                        }
                        break;
                }
                button.Click += new EventHandler(ButtonInFocus);
                Controls.Add(button);
            }
        }
        public void ButtonInFocus(object sender, EventArgs e)
        {
            Button buttonPressed = sender as Button;
            if (prevButton != null)
            {
                prevButton.BackgroundImage = null;
                buttonPressed.BackgroundImage = Properties.Resources.red;
                prevButton = buttonPressed;
            }
            else
            {

                buttonPressed.BackgroundImage = Properties.Resources.red;
                prevButton = buttonPressed;
            }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (prevButton != null)
            {
                var currFigure = numberOfFigures[prevButton.Location.X / 85];
                switch (currFigure)
                {
                    case 1:
                        {
                            playDeskForm.buttons[rowCurrFigure, columnCurrFigure].Image = Properties.Resources.wR;
                            playDeskForm.gameMap[rowCurrFigure, columnCurrFigure] = 11;
                        }
                        break;
                    case 2:
                        {
                            playDeskForm.buttons[rowCurrFigure, columnCurrFigure].Image = Properties.Resources.wN;
                            playDeskForm.gameMap[rowCurrFigure, columnCurrFigure] = 12;
                        }
                        break;
                    case 3:
                        {
                            playDeskForm.buttons[rowCurrFigure, columnCurrFigure].Image = Properties.Resources.wB;
                            playDeskForm.gameMap[rowCurrFigure, columnCurrFigure] = 13;
                        }
                        break;
                    case 4:
                        {
                            playDeskForm.buttons[rowCurrFigure, columnCurrFigure].Image = Properties.Resources.wQ;
                            playDeskForm.gameMap[rowCurrFigure, columnCurrFigure] = 14;
                        }
                        break;
                    case 5:
                        {
                            playDeskForm.buttons[rowCurrFigure, columnCurrFigure].Image = Properties.Resources.wP;
                            playDeskForm.gameMap[rowCurrFigure, columnCurrFigure] = 16;
                        }
                        break;
                }
                playDeskForm.Enabled = true;
                Close();
            }
            else
            {
                MessageBox.Show("You didn't select a figure", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

