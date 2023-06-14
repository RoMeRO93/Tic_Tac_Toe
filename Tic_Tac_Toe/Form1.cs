using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        public enum Player
        {
            X, O
        }

        Player currentPlayer;
        List<Button> buttonsList;
        Random random = new Random();
        int playerWins = 0;
        int computerWins = 0;
        Timer AIMoveTimer; // Adăugat obiectul Timer

        public Form1()
        {
            InitializeComponent();
            currentPlayer = Player.X;
            resetGame();
            AIMoveTimer = new Timer(); // Inițializat obiectul Timer
            AIMoveTimer.Tick += new EventHandler(AIMove_Tick);
            AIMoveTimer.Interval = 1000; // Setat intervalul la valoarea dorită (în milisecunde)
        }

        private void playerClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.Text = currentPlayer.ToString();
            button.Enabled = false;
            button.BackColor = System.Drawing.Color.Cyan;
            buttonsList.Remove(button);
            CheckWin();

            if (playerWins == 0) // Verificați că jucătorul nu a câștigat deja
            {
                AIMoveTimer.Start(); // Începe Timer-ul pentru mișcarea AI
            }
        }


        private void AIMove_Tick(object sender, EventArgs e)
{
    if (buttonsList.Count > 0)
    {
        int index = random.Next(buttonsList.Count);
        if (buttonsList[index].Text == " ") // Verifică dacă poziția este goală
        {
            buttonsList[index].Enabled = false;
            buttonsList[index].Text = Player.O.ToString();
            buttonsList[index].BackColor = System.Drawing.Color.DarkSalmon;
            buttonsList.RemoveAt(index);
            CheckWin();
            AIMoveTimer.Stop(); // Oprește Timer-ul după ce AI a făcut o mișcare
        }
    }
}



        private void restartGame(object sender, EventArgs e)
        {
            resetGame();
            AIMoveTimer.Stop(); // Oprește Timer-ul
            AIMoveTimer.Start();
        }

        private void LoadButtons()
        {
            buttonsList = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button9, button8 };
        }

        private void resetGame()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button && control.Tag == "play")
                {
                    ((Button)control).Enabled = true;
                    ((Button)control).Text = "?";
                    ((Button)control).BackColor = default(Color);
                }
            }
            LoadButtons();
        }

        private void CheckWin()
        {
            string[,] board = new string[3, 3]
            {
                { button1.Text, button2.Text, button3.Text },
                { button4.Text, button5.Text, button6.Text },
                { button7.Text, button9.Text, button8.Text }
            };

            string[] winningCombinations = new string[]
            {
                "012", "345", "678",  // linii
                "036", "147", "258",  // coloane
                "048", "246"           // diagonale
            };

            foreach (string combination in winningCombinations)
            {
                char firstSymbol = board[int.Parse(combination[0].ToString()) / 3, int.Parse(combination[0].ToString()) % 3][0];
                bool isWinningCombination = true;

                foreach (char position in combination.Skip(1))
                {
                    if (board[int.Parse(position.ToString()) / 3, int.Parse(position.ToString()) % 3][0] != firstSymbol)
                    {
                        isWinningCombination = false;
                        break;
                    }
                }

                if (isWinningCombination)
                {
                    if (firstSymbol == 'X')
                    {
                        MessageBox.Show("Player Wins");
                        playerWins++;
                        label1.Text = "Player Wins - " + playerWins;
                    }
                    else if (firstSymbol == 'O')
                    {
                        MessageBox.Show("Computer Wins");
                        computerWins++;
                        label2.Text = "AI Wins - " + computerWins;
                    }

                    resetGame();

                    AIMoveTimer.Stop(); // Opriți Timer-ul după ce jucătorul câștigă

                    break;

                }
            }
        }
    }
}
