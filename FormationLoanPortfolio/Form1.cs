using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ExcelDataReader;



namespace FormationLoanPortfolio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string fileName = string.Empty;

        private int[] k_j;
        private double[] d_j;
        private double[] t_j;
        private double[] P_j;
        private double F;
        private double a1 = 0.5;
        private double a2 = 0.5;
        private double r = 0.1;
        private double epsilon = 0.1;

        private short[] _geneticSolution;
        private short[] _memeticSolution;
        private short[] _psoSolution;
        private short[] _beeSolution;

        private DataTable table;

        private void Form1_Load(object sender, EventArgs e)
        {


        }



        private void button3_Click(object sender, EventArgs e)
        {

            if (k_j == null || t_j == null || d_j == null || P_j == null)
            {
                MessageBox.Show("Таблиця даних клієнтів є не заповненою", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (k_j.Length != d_j.Length || k_j.Length != t_j.Length || k_j.Length != P_j.Length)
            {
                MessageBox.Show("Довжина рядків даних кредитування клієнтів не є однаковою ", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MemeticAlgorithm memeticAlgorithm = new MemeticAlgorithm(5, k_j.Length, 20, 60, k_j, t_j, d_j, P_j, a1, a2, r, F);

            var watchForGa = System.Diagnostics.Stopwatch.StartNew();

            memeticAlgorithm.Run();

            while (P(k_j, memeticAlgorithm.BestSolution, memeticAlgorithm.R, F) > epsilon)
            {
                memeticAlgorithm.R = 5 * memeticAlgorithm.R;

                memeticAlgorithm.Run();

            }


            watchForGa.Stop();
            _memeticSolution = memeticAlgorithm.BestSolution;

            table.Rows.Add("Меметичний алгоритм");

            for (int i = 1; i < memeticAlgorithm.BestSolution.Length + 1; i++)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i].Value = memeticAlgorithm.BestSolution[i - 1];

                if (memeticAlgorithm.BestSolution[i - 1] == 1)
                {
                    comboBox3.Items.Add(dataGridView1.Rows[4].Cells[i].Value);
                }
            }

            chart1.Series[2].Points.AddXY(Math.Round(R(memeticAlgorithm.BestSolution, P_j, k_j, d_j, t_j),5), Math.Round(E(memeticAlgorithm.BestSolution, k_j, t_j, d_j),5));

            label10.Text = "E(x)=" + Math.Round(E(memeticAlgorithm.BestSolution, k_j, t_j, d_j), 5).ToString();
            label11.Text = "R(x)=" + Math.Round(R(memeticAlgorithm.BestSolution, P_j, k_j, d_j, t_j), 5).ToString();
            label16.Text = "g(x)=" + g(memeticAlgorithm.BestSolution, k_j);
            label20.Text = "Час виконання міметичного алгоритму = " + watchForGa.ElapsedMilliseconds;
        }


        private void OpenExcelFile(string path, DataGridView dataGrid)
        {
            FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);

            
            IExcelDataReader reader = ExcelReaderFactory.CreateReader(fileStream);

            DataSet db = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }

            });

            table = db.Tables[0];
            dataGrid.DataSource = table;

        }


        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                    Text = fileName;
                    OpenExcelFile(fileName, dataGridView1);

                    SetData();
                }
                else
                {
                    throw new Exception(" Файл не був вибраний");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static double P(int[] k_j, short[] X_pop, double r, double F)
        {
            double result = 0;
            for (int i = 0; i < k_j.Length; i++)
            {
                result += k_j[i] * X_pop[i];
            }
            result = Math.Exp(r * Math.Abs(result - F)) / r;

            return result;
        }


        private static double R(short[] X_pop, double[] P_j, int[] k_j, double[] d_j, double[] t_j)
        {
            double result1 = 0;
            double resutl2 = 0;

            for (int i = 0; i < k_j.Length; i++)
            {
                resutl2 += X_pop[i];
                result1 += k_j[i] * (1 + d_j[i] * t_j[i]) * X_pop[i] * P_j[i];
            }

            if(resutl2 == 0)
                return 0;
            else
                return result1 / resutl2;
        }


        private static double E(short[] X_pop, int[] k_j, double[] t_j, double[] d_j)
        {
            double result = 0;
            for (int i = 0; i < k_j.Length; i++)
            {
                result += k_j[i] * (1 + d_j[i] * t_j[i]) * X_pop[i];
            }

            return result;
        }

        private static double g(short[] X_pop, int[] k_j)
        {
            double result = 0;
            for (int i = 0; i < k_j.Length; i++)
            {
                result += X_pop[i] * k_j[i];
            }
            return result;
        }


        private static void DisplayResult(short[] solution)
        {
            for (int i = 0; i < solution.Length; i++)
            {
                Console.Write(solution[i]);
            }
            Console.WriteLine();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;

            table = new DataTable();

            for (int i = 0; i < numericUpDown1.Value + 1; i++)
            {
                if (i == 0)
                {
                    table.Columns.Add("№");
                }
                else
                {
                    table.Columns.Add(i.ToString());
                }
            }

            table.Rows.Add("k_j");
            table.Rows.Add("t_j");
            table.Rows.Add("P_j");
            table.Rows.Add("d_j");
            table.Rows.Add("Імена клієнтів");

            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (k_j == null || t_j == null || d_j == null || P_j == null)
            {
                MessageBox.Show("Таблиця даних клієнтів є не заповненою", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (k_j.Length != d_j.Length || k_j.Length != t_j.Length || k_j.Length != P_j.Length)
            {
                MessageBox.Show("Довжина рядків даних кредитування клієнтів не є однаковою ", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(5, k_j.Length, 20, 40, k_j, t_j, d_j, P_j, a1, a2, r, F);

            var watchForGa = System.Diagnostics.Stopwatch.StartNew();

            geneticAlgorithm.Run();


            while (P(k_j, geneticAlgorithm.BestSolution, geneticAlgorithm.R, F) > epsilon)
            {
                geneticAlgorithm.R = 5 * geneticAlgorithm.R;

                geneticAlgorithm.Run();

            }

            watchForGa.Stop();

            _geneticSolution = geneticAlgorithm.BestSolution;

            table.Rows.Add("Генетичний алгоритм");


            for (int i = 1; i < geneticAlgorithm.BestSolution.Length + 1 ; i++)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i].Value = geneticAlgorithm.BestSolution[i-1];
                if(geneticAlgorithm.BestSolution[i - 1] == 1)
                {
                    comboBox1.Items.Add(dataGridView1.Rows[4].Cells[i].Value);
                }
            }

            chart1.Series[0].Points.AddXY(Math.Round(R(geneticAlgorithm.BestSolution,P_j,k_j,d_j,t_j),5),Math.Round(E(geneticAlgorithm.BestSolution, k_j, t_j, d_j), 5));

            label6.Text = "E(x)=" + Math.Round(E(geneticAlgorithm.BestSolution, k_j, t_j, d_j), 5);
            label7.Text = "R(x)=" + Math.Round(R(geneticAlgorithm.BestSolution, P_j, k_j, d_j, t_j), 5);
            label14.Text = "g(x)=" + Math.Round(g(geneticAlgorithm.BestSolution,k_j));
            label18.Text = "Час виконання генетичного алгоритму = " + watchForGa.ElapsedMilliseconds;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (k_j == null || t_j == null || d_j == null || P_j == null)
            {
                MessageBox.Show("Таблиця даних клієнтів є не заповненою", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (k_j.Length != d_j.Length || k_j.Length != t_j.Length || k_j.Length != P_j.Length)
            {
                MessageBox.Show("Довжина рядків даних кредитування клієнтів не є однаковою ", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BeeAlgorithm beeAlgorithm = new BeeAlgorithm(40, k_j.Length, 40, k_j, t_j, d_j, P_j, a1, a2, r, F);

            var watchBee = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("  Результат роботы: ");

            beeAlgorithm.Run();


            while (P(k_j, beeAlgorithm.GetBestSolution(), beeAlgorithm.R, F) > epsilon)
            {
                beeAlgorithm.R = 5 * beeAlgorithm.R;

                beeAlgorithm.Run();

            }
            watchBee.Stop();

            _beeSolution = beeAlgorithm.BestSolution;


            table.Rows.Add("Бджолиний алгоритм");

            for (int i = 1; i < beeAlgorithm.BestSolution.Length + 1; i++)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i].Value = beeAlgorithm.BestSolution[i - 1];
                if (beeAlgorithm.BestSolution[i - 1] == 1)
                {
                    comboBox2.Items.Add(dataGridView1.Rows[4].Cells[i].Value);
                }
            }

            chart1.Series[1].Points.AddXY(Math.Round(R(beeAlgorithm.BestSolution, P_j, k_j, d_j, t_j), 5), Math.Round(E(beeAlgorithm.BestSolution, k_j, t_j, d_j), 5));

            label8.Text = "E(x)=" + Math.Round(E(beeAlgorithm.BestSolution, k_j, t_j, d_j), 5).ToString();
            label9.Text = "R(x)=" + Math.Round(R(beeAlgorithm.BestSolution, P_j, k_j, d_j, t_j), 5).ToString();
            label15.Text = "g(x)="+ g(beeAlgorithm.BestSolution,k_j);
            label19.Text = "Час виконання бджолиного алгоритму = " + watchBee.ElapsedMilliseconds;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (k_j == null || t_j == null || d_j == null || P_j == null)
            {
                MessageBox.Show("Таблиця даних клієнтів є не заповненою", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (k_j.Length != d_j.Length || k_j.Length != t_j.Length || k_j.Length != P_j.Length)
            {
                MessageBox.Show("Довжина рядків даних кредитування клієнтів не є однаковою ", "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            PSOAlgorithm psoAlgorithm = new PSOAlgorithm(k_j.Length, 20, 40, k_j, t_j, d_j, P_j, a1, a2, r, F);
            Console.WriteLine("  Результат роботы: ");
            var watchForPSO = System.Diagnostics.Stopwatch.StartNew();

            psoAlgorithm.Run();

            while (P(k_j, psoAlgorithm.BestSolution, psoAlgorithm.R, F) > epsilon)
            {
                psoAlgorithm.R = 10 * psoAlgorithm.R;
                psoAlgorithm.Run();

            }

            watchForPSO.Stop();

            _psoSolution = psoAlgorithm.BestSolution;


            table.Rows.Add("Алгоритм рою частинок");

            for (int i = 1; i < psoAlgorithm.BestSolution.Length + 1; i++)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i].Value = psoAlgorithm.BestSolution[i - 1];
                if (psoAlgorithm.BestSolution[i - 1] == 1)
                {
                    comboBox4.Items.Add(dataGridView1.Rows[4].Cells[i].Value);
                }
            }

            chart1.Series[3].Points.AddXY(Math.Round(R(psoAlgorithm.BestSolution, P_j, k_j, d_j, t_j), 5), Math.Round(E(psoAlgorithm.BestSolution, k_j, t_j, d_j), 5));

            label12.Text = "E(x)=" + Math.Round(E(psoAlgorithm.BestSolution, k_j, t_j, d_j), 5).ToString();
            label13.Text = "R(x)=" + Math.Round(R(psoAlgorithm.BestSolution, P_j, k_j, d_j, t_j), 5).ToString();
            label17.Text = "g(x)=" + g(psoAlgorithm.BestSolution, k_j);
            label21.Text = "Час виконання алгоритму рою частинок = " + watchForPSO.ElapsedMilliseconds;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void SetData()
        {
            k_j = new int[dataGridView1.Columns.Count - 1];
            t_j = new double[dataGridView1.Columns.Count - 1];
            P_j = new double[dataGridView1.Columns.Count - 1];
            d_j = new double[dataGridView1.Columns.Count - 1];

            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                k_j[i-1] = Convert.ToInt32(dataGridView1.Rows[0].Cells[i].Value);
                t_j[i-1] = Convert.ToDouble(dataGridView1.Rows[1].Cells[i].Value);
                P_j[i-1] = Convert.ToDouble(dataGridView1.Rows[2].Cells[i].Value);
                d_j[i-1] = Convert.ToDouble(dataGridView1.Rows[3].Cells[i].Value);
            }

            F = Convert.ToDouble(dataGridView1.Rows[5].Cells[1].Value);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            a2 = (double)numericUpDown3.Value;
            numericUpDown2.Value = Convert.ToDecimal(1.0 - a2);
            a1 = 1.0 - a2;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            a1 = (double)numericUpDown2.Value;
            a2 = 1.0 - a1;
            numericUpDown3.Value = Convert.ToDecimal(1.0 - a1);
        }
    }
}
