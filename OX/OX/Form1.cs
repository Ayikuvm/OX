namespace OX
{
    public partial class Form1 : Form
    {
        bool turn = true;
        int turn_count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_2(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By KruFern", "About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                if (turn)
                    b.Text = "X";
                else
                    b.Text = "O";

                turn = !turn;
                b.Enabled = false;
                turn_count++;

                checkForWinner();
            }
        }
        private void checkForWinner()
        {
            bool there_is_a_winner = false;

            //ตรวจสอบแนวนอน
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))
                there_is_a_winner = true;
            else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))
                there_is_a_winner = true;
            else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
                there_is_a_winner = true;

            //ตรวจสอบแนวตั้ง
            else if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
                there_is_a_winner = true;
            else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
                there_is_a_winner = true;
            else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
                there_is_a_winner = true;

            //ตรวจสอบแนวทแยงมุม
            else if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
                there_is_a_winner = true;
            else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!A3.Enabled))
                there_is_a_winner = true;

            if (there_is_a_winner)
            {
                disableButtons();

                String winner = "";
                if (turn)
                    winner = "O";
                else
                    winner = "X";

                MessageBox.Show(winner + " Wins!", "Wow!");
            }
            else
            {
                if (turn_count == 9)
                    MessageBox.Show("OOP! Try again", "Game Over!");
            }
        }
        private void disableButtons()
        {
            foreach (Control c in Controls)
            {
                if (c is Button b)
                    b.Enabled = false;
            }
        }
        private void newGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;

            foreach (Control c in Controls)
            {
                if (c is Button b)
                {
                    b.Enabled = true;
                    b.Text = "";
                }
            }
        }

        private void button_enter(object sender, EventArgs e)
        {
            if (sender is Button b && b.Enabled)
            {
                if (turn)
                    b.Text = "X";
                else
                    b.Text = "O";
            }
        }

        private void button_leave(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                b.Text = "";
            }
        }
    }
}
