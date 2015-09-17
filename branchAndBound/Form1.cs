using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace branchAndBound
{
    /// <summary>
    /// Это мой крутой класс FORM1
    /// </summary>
    public partial class Form1 : Form
    {

        static List<char> _chars = new List<char>()
            {
                'A',
                'B',
                'C',
                'D',
                'E',
                'F'
            };

        private List<List<TextBox>> _boxes;


        public Form1()
        {
            InitializeComponent();

            _boxes = new List<List<TextBox>>();
            int j = 0;
            for (int i = 0; i < 6; i++)
            {
                _boxes.Add(new List<TextBox>());
                for (int k = 0; k < 6; k++)
                {
                    j++;
                    Control t = tableLayoutPanel1.Controls.Find("textBox" + j.ToString(), false)[0];
                    _boxes[i].Add((TextBox)t);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Random r = new Random();

            List<List<int>> arr = LoadFromTextBoxes();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            label22.Text = BranchAndBound.Calculate(arr);
            label23.Text = stopwatch.Elapsed.TotalSeconds.ToString("0.00000 'сек'");
            stopwatch.Restart();
            arr = LoadFromTextBoxes();
            label20.Text = BranchAndBound.Calculate(arr);
            label21.Text = stopwatch.Elapsed.TotalSeconds.ToString("0.00000 'сек'");
            stopwatch.Restart();

            arr = LoadFromTextBoxes();
            label24.Text = BranchAndBound.Calculate(arr);
            label25.Text = stopwatch.Elapsed.TotalSeconds.ToString("0.00000 'сек'");
            stopwatch.Restart();



            groupBox1.Show();
            groupBox2.Show();
            groupBox3.Show();

        }

        private List<List<int>> LoadFromTextBoxes()
        {
            List<List<int>>  list = new List<List<int>>();
            for (int i = 0; i < _boxes.Count; i++)
            {
                list.Add(new List<int>());
                for (int j = 0; j < _boxes[i].Count; j++)
                {
                    list[i].Add(Convert.ToInt32(_boxes[i][j].Text));
                }
            }
            return list;
        }

        private static string GeneratePath()
        {
            Random r = new Random();
            List<char> result = new List<char>();
            List<char> myList = _chars.ToList();

            while (myList.Any())
            {
                int num = r.Next(myList.Count);
                result.Add(myList[num]);
                myList.RemoveAt(num);
            }

            if(result.Count > 0)
                result.Add(result[0]);

            return string.Join(" -> ", result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                Control control = tableLayoutPanel1.Controls[i];
                if (control is TextBox)
                {
                    if (i%7 != 0)
                    {
                        int r = random.Next(1, 10);
                        (control as TextBox).Text = r.ToString();
                    }
                    else
                    {
                        (control as TextBox).Text = (-1).ToString();
                    }
                }
            }

            Singleton s = Singleton.Instance;
            Debug.WriteLine(s.Id);

            Singleton c = Singleton.Instance;
            Debug.WriteLine(c.Id);
        }
    }
}
