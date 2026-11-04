using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        // สร้างรายการสำหรับเก็บตัวงู และอาหาร
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            // ตั้งค่าเริ่มต้นเกม
            new Settings();

            // ตั้งค่าความเร็วนาฬิกา
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen; // เชื่อมนาฬิกากับฟังก์ชัน UpdateScreen
            gameTimer.Start();

            // เริ่มเกม
            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false; // ซ่อนป้าย Game Over
            new Settings(); // รีเซ็ตค่าต่างๆ

            // สร้างงูตัวแรก (หัวงู)
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood(); // สุ่มอาหาร
        }

        // ฟังก์ชันสุ่มตำแหน่งอาหาร
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) };
        }

        // ฟังก์ชันนี้จะทำงานทุกครั้งที่นาฬิกาเดิน (Game Loop)
        private void UpdateScreen(object sender, EventArgs e)
        {
            // เช็คว่าเกมจบหรือยัง
            if (Settings.GameOver == true)
            {
                // ถ้ากด Enter ให้เริ่มเกมใหม่
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                // เช็คปุ่มกดเพื่อเปลี่ยนทิศทาง
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer(); // ขยับตัวงู
            }

            pbCanvas.Invalidate(); // สั่งวาดภาพใหม่ (Refresh จอ)
        }

        // ฟังก์ชันวาดรูป (Graphics)
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                // วาดตัวงู
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Yellow; // หัวงูสีดำ
                    else
                        snakeColour = Brushes.Green;// ตัวงูสีเขียว

                    // วาดวงกลม
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                    // วาดอาหาร (วงกลมสีแดง)
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height,
                                      Settings.Width, Settings.Height));
                }
            }
            else
            {
                // ถ้าจบเกม ให้โชว์ป้าย Game Over
                string gameOver = "Game over \nYour final score is: " + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        // ฟังก์ชันขยับตัวงู
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                // ขยับหัว
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    // --- เช็คการชนขอบจอ ---
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    // --- เช็คการชนตัวเอง ---
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }


                    // --- เช็คการกินอาหาร ---
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    // ขยับตัวตามหัว
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        // ฟังก์ชันเมื่อกดปุ่มคีย์บอร์ดลง
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        // ฟังก์ชันเมื่อปล่อยปุ่มคีย์บอร์ด
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        // ฟังก์ชันกินอาหาร
        private void Eat()
        {
            // เพิ่มตัวงู
            Circle circle = new Circle();
            circle.X = Snake[Snake.Count - 1].X;
            circle.Y = Snake[Snake.Count - 1].Y;
            Snake.Add(circle);

            // บวกคะแนน
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        // ฟังก์ชันตาย
        private void Die()
        {
            Settings.GameOver = true;
        }

        private void pbCanvas_Click(object sender, EventArgs e)
        {

        }

        private void lblScore_Click(object sender, EventArgs e)
        {

        }

        private void lblGameOver_Click(object sender, EventArgs e)
        {

        }
    }
}