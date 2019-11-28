using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labirint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\dragos\Desktop\Labirint\Labirint\Labirint.txt");
        int i = 0, j, lungime, latime, lin = 0, col = 0, x, y, xurmator, yurmator, nmax, mmax, my_x, my_y;

        int[] dx = new int[] { 0, 1, 0, -1 };
        int[] dy = new int[] { 1, 0, -1, 0 };
        int[,] mv = new int[1000, 1000];
        int[] frecv = new int[] { 0, 0, 0, 0 };

        Graphics grp;
        Bitmap bmp;

        Queue<int> linie = new Queue<int>();
        Queue<int> coloana = new Queue<int>();

        public void movement(int dir_x, int dir_y)
        {
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            grp.FillRectangle(whiteBrush, my_y * lungime, my_x * latime, lungime, latime);

            mv[my_x, my_y] = 0;

            SolidBrush greenBrush = new SolidBrush(Color.Green);
            grp.FillRectangle(greenBrush, (my_y + dir_y) * lungime, (my_x + dir_x) * latime, lungime, latime);

            mv[my_x + dir_x, my_y + dir_y] = 3;

            my_x += dir_x;
            my_y += dir_y;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            grp = Graphics.FromImage(bmp);

            latime = 500 / lines.Length;
            nmax = lines.Length;

            foreach (string line in lines)
            {
                lungime = 500 / line.Length;
                col = 0;
                mmax = line.Length;

                for (j = 0; j < line.Length; j++)
                {
                    if (line[j] == '1') {
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        grp.FillRectangle(blackBrush, col, lin, lungime, latime);

                        mv[i, j] = 1;
                    }
                    else if(line[j] == '0')
                    {
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        grp.FillRectangle(whiteBrush, col, lin, lungime, latime);

                        mv[i, j] = 0;
                    }
                    else if (line[j] == '2')
                    {
                        SolidBrush redBrush = new SolidBrush(Color.Red);
                        grp.FillRectangle(redBrush, col, lin, lungime, latime);

                        x = i;
                        y = j;
                        mv[i, j] = 2;

                        linie.Enqueue(i);
                        coloana.Enqueue(j);
                    }
                    else if (line[j] == '3')
                    {
                        SolidBrush greenBrush = new SolidBrush(Color.Green);
                        grp.FillRectangle(greenBrush, col, lin, lungime, latime);

                        my_x = i;
                        my_y = j;
                        mv[i, j] = 3;
                    }

                    col += lungime;
                }

                lin += latime;
                i++;
            }

            pictureBox1.Image = bmp;

        }

        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                timer1.Enabled = true;

            if (e.KeyCode == Keys.Up && my_x - 1 >= 0)
                if (mv[my_x - 1, my_y] == 0)
                    movement(-1, 0);

            if (e.KeyCode == Keys.Down && my_x + 1 < nmax)
                if (mv[my_x + 1, my_y] == 0)
                    movement(1, 0);

            if (e.KeyCode == Keys.Right && my_y + 1 < mmax)
                if (mv[my_x, my_y + 1] == 0)
                    movement(0, 1);

            if (e.KeyCode == Keys.Left && my_y - 1 >= 0)
                if (mv[my_x, my_y - 1] == 0)
                    movement(0, -1);

            pictureBox1.Image = bmp;
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            bool ok = false;

            x = linie.Dequeue();
            y = coloana.Dequeue();

            for (i = 0; i < 4; i++)
                frecv[i] = 0;

            while (ok == false) {
                int d = rand.Next(4);
                frecv[d]++;

                xurmator = x + dx[d];
                yurmator = y + dy[d];

                if (xurmator >= 0 && yurmator >= 0 && xurmator < nmax && yurmator < mmax && (mv[xurmator, yurmator] == 0 || mv[xurmator, yurmator] == 3)) 
                {
                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                    grp.FillRectangle(whiteBrush, y * lungime, x * latime, lungime, latime);

                    mv[x, y] = 0;

                    SolidBrush redBrush = new SolidBrush(Color.Red);
                    grp.FillRectangle(redBrush, yurmator * lungime, xurmator * latime, lungime, latime);

                    if (mv[xurmator, yurmator] == 3)
                    {
                        mv[xurmator, yurmator] = 2;
                        pictureBox1.Image = bmp;
                        timer1.Enabled = false;
                        MessageBox.Show("Ai pierdut!");      
                    }

                    mv[xurmator, yurmator] = 2;


                    linie.Enqueue(xurmator);
                    coloana.Enqueue(yurmator);

                    ok = true;
                }
                else if (frecv[0] + frecv[1] + frecv[2] + frecv[3] == 4)
                {
                    linie.Enqueue(x);
                    coloana.Enqueue(y);
                    ok = true;
                }
            }

            pictureBox1.Image = bmp;
        }
    }
}
