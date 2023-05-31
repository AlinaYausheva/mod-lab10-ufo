using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Ufo
{
    public partial class Form1 : Form
    {
        Point start;
        Point end;
        PointF current;
        double angle;
        int accuracy = 5;
        float step;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Visible = false;
            panel1.Visible = false;
        }

        double Factorial(int n)
        {
            if (n == 0)
                return 1;
            return n * Factorial(n - 1);
        }
        double Sin(double x, int n)
        {
            double ans = 0;
            for (int i = 1; i <= n; i++)
                ans += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 1) / Factorial(2 * i - 1);

            return ans;
        }

        double Cos(double x, int n)
        {
            double ans = 0;
            for (int i = 1; i <= n; i++)
                ans += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 2) / Factorial(2 * i - 2);

            return ans;
        }

        double Arctg(double x, int n)
        {
            double ans = 0;
            for (int i = 1; i <= n; i++)
                ans += Math.Pow(-1, i - 1) * Math.Pow(x, (2 * i - 1)) / (2 * i - 1);

            return ans;
        }

        private double Count_distance(int n)
        {
            double distance = 0;
            current = start;

            if (Math.Abs(end.Y - start.Y) <= Math.Abs(end.X - start.X))
            {
                angle = Arctg((double)Math.Abs(end.Y - start.Y) / Math.Abs(end.X - start.X), n);
            }
            else
            {
                angle = Math.PI / 2 - Arctg((double)Math.Abs(end.X - start.X) / Math.Abs(end.Y - start.Y), n);
            }
            while ((end.X > current.X) && (end.Y > current.Y))
            {
                distance = (double)Math.Sqrt(Math.Pow(current.X - end.X, 2) + Math.Pow(current.Y - end.Y, 2));
                current.X += (float)(step * Cos(angle, n));
                current.Y += (float)(step * Sin(angle, n));
            }
            return distance;
        }

        private void drawing_button_Click(object sender, EventArgs e)
        {
            start = new Point(50, 50);
            end = new Point(600, 250);
            step = 5;
            panel1.Visible = true;
            chart1.Visible = false;
            current = start;

            if (Math.Abs(end.Y - start.Y) <= Math.Abs(end.X - start.X))
            {
                angle = Arctg((double)Math.Abs(end.Y - start.Y) / Math.Abs(end.X - start.X), accuracy);
            }
            else
            {
                angle = Math.PI / 2 - Arctg((double)Math.Abs(end.X - start.X) / Math.Abs(end.Y - start.Y), accuracy);
            }
            timer1.Start();
        }

        private void graphics_button_Click(object sender, EventArgs e)
        {
            start = new Point(10, 10);
            end = new Point(350, 380);
            step = 1;
            chart1.Visible = true;
            panel1.Visible = true;

            chart1.Series.Clear();
            chart1.Series.Add("График зависимости количества членов ряда от радиуса зоны попадания");
            chart1.ChartAreas.Add("График зависимости количества членов ряда от радиуса зоны попадания");
            chart1.Series[0].ChartType = SeriesChartType.Spline;
            chart1.ChartAreas[0].AxisX.Title = "Радиус зоны попадания";
            chart1.ChartAreas[0].AxisY.Title = "Количество членов ряда";
            chart1.ChartAreas[0].AxisX.Minimum = 0;

            for (int i = 2; i <= 50; i ++)
                chart1.Series[0].Points.AddXY(Count_distance(i), i);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((end.X <= current.X)&& (end.Y <= current.Y)){
                timer1.Stop();
            }
            else
            {
                current.X += (float)(step * Cos(angle, accuracy));
                current.Y += (float)(step * Sin(angle, accuracy));
                panel1.Invalidate();
            }        
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GraphicsState gs;

            Pen pen = new Pen(Color.Black, 8);
            g.DrawEllipse(pen, start.X, start.Y, 2, 2);
            g.DrawEllipse(pen, end.X, end.Y, 2, 2);
            SolidBrush brush = new SolidBrush(Color.Red);
            g.FillEllipse(brush, current.X - step, current.Y - step, 2 * step, 2 * step);

            gs = g.Save();
            g.Restore(gs);
        }
    }
}