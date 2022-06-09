using System;

namespace PracticeForGraduate
{
    public class Program
    {   

        static void Main(string[] args)
        {

            int[] k_j = new int[20] { 30, 45, 28, 80, 36, 57, 46, 30, 25, 65, 75, 25, 41, 20, 25, 44, 46, 30, 38, 45 };
            double[] t_j = new double[20] { 1, 2, 1, 3, 1.5, 2, 2, 1.5, 1, 2, 2, 1, 1.5, 1, 1, 2, 2, 1.4, 1, 2 };
            double[] P_j = new double[20] { 0.1, 0.3, 0.25, 0.45, 0.33, 0.4, 0.33, 0.25, 0.25, 0.3, 0.33, 0.25, 0.45, 0.33, 0.33, 0.25, 0.35, 0.20, 0.466, 0.33 };
            double[] d_j = new double[20] { 0.07, 0.095, 0.075, 0.095, 0.08, 0.095, 0.1, 0.09, 0.085, 0.095, 0.1, 0.08, 0.1, 0.09, 0.08, 0.085, 0.09, 0.07, 0.12, 0.09 };
            double a1 = 0.5, a2 = 0.5, r = 0.1, e = 0.01, F = 650;

            //////////////////////////////////////////////////////////////////////////////


            Console.WriteLine("    == Использование метода штрафных функций и генетического алгоритма  == " +
                "\n  Задайте количество популяции : ");


            Console.WriteLine("  +====   Genetic algorithm  ====+ \n\n");
            Console.WriteLine("  Задайте размер популяции: ");
            int countOfPopulationGa = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("  Задайте количество эпох: ");
            int erasForGA = Convert.ToInt32(Console.ReadLine());

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(3, k_j.Length, countOfPopulationGa, erasForGA, k_j, t_j, d_j, P_j, a1, a2, r, F);
            Console.WriteLine("  Результат роботы: ");
            var watchForGa = System.Diagnostics.Stopwatch.StartNew();

            geneticAlgorithm.GeneratePopulation();

            a1 = 0.5;
            a2 = 0.5;
            r = 0.1;

            while (P(k_j, geneticAlgorithm.BestSolution, geneticAlgorithm.R, F) > e)
            {
                geneticAlgorithm.R = 1.5 * geneticAlgorithm.R;
                geneticAlgorithm.A1 = 1.5 * geneticAlgorithm.A1;
                geneticAlgorithm.A2 = 1.5 * geneticAlgorithm.A2;

                geneticAlgorithm.Run();

            }

            watchForGa.Stop();

            short[] solutionForGa = geneticAlgorithm.BestSolution;

            Console.Write(" Набор клиентов таков: ");

            DisplayResult(solutionForGa);

            Console.WriteLine(" E(x): " + E(solutionForGa, k_j, t_j, d_j));
            Console.WriteLine(" R(x): " + R(solutionForGa, P_j, k_j, d_j, t_j));
            Console.WriteLine(" g(x): " + g(solutionForGa, k_j));
            Console.WriteLine("Time: " + watchForGa.ElapsedMilliseconds + "ms");


            //////////////////////////////////////////////////////////
            ///


            Console.WriteLine("  +==== Algorithm of bee colony ====+ \n\n");
            Console.WriteLine("  Задайте кол-во пчел: ");
            int countOfBees = Convert.ToInt32(Console.ReadLine());

            a1 = 0.5;
            a2 = 0.5;
            r = 0.1;

            BeeAlgorithm beeAlgorithm = new BeeAlgorithm(40, k_j.Length, countOfBees, k_j, t_j, d_j, P_j, a1, a2, r, F);

            var watchBee = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("  Результат роботы: ");

            beeAlgorithm.Run();

            while (P(k_j, beeAlgorithm.GetBestSolution(), beeAlgorithm.R, F) > e)
            {
                beeAlgorithm.R = 2.5 * beeAlgorithm.R;
                beeAlgorithm.A1 = 2.5 * beeAlgorithm.A1;
                beeAlgorithm.A2 = 2.5 * beeAlgorithm.A2;

                beeAlgorithm.Run();

            }

            watchBee.Stop();
            

            short[] solution = beeAlgorithm.GetBestSolution();
            Console.Write(" Набор клиентов таков: ");
            DisplayResult(solution);
            Console.WriteLine(" E(x): " + E(solution, k_j, t_j, d_j));
            Console.WriteLine(" R(x): " + R(solution, P_j, k_j, d_j, t_j));
            Console.WriteLine(" g(x): " + g(solution, k_j));
            Console.WriteLine("Time : "+ watchBee.ElapsedMilliseconds);


            /////////////////////////////////////////////////////////////////



            Console.WriteLine("  +====   Memetic algorithm  ====+ \n\n");
            Console.WriteLine("  Задайте размер популяции: ");
            int countOfPopulationMemetic = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("  Задайте количество эпох: ");
            int eras = Convert.ToInt32(Console.ReadLine());

            a1 = 0.5;
            a2 = 0.5;
            r = 0.1;

            MemeticAlgorithm memeticAlgorithm = new MemeticAlgorithm(3, k_j.Length, countOfPopulationMemetic, eras, k_j, t_j, d_j, P_j, a1, a2, r, F);
            Console.WriteLine("  Результат роботы: ");
            var watchForMA = System.Diagnostics.Stopwatch.StartNew();

            memeticAlgorithm.Run();

            while (P(k_j, memeticAlgorithm.BestSolution, memeticAlgorithm.R, F) > e)
            {
                
                memeticAlgorithm.R = 2.5 * memeticAlgorithm.R;
                memeticAlgorithm.A1 = 2.5 * memeticAlgorithm.A1;
                memeticAlgorithm.A2 = 2.5 * memeticAlgorithm.A2;

                memeticAlgorithm.Run();

            }

            watchForMA.Stop();
            Console.Write(" Набор клиентов таков: ");
            
            DisplayResult(memeticAlgorithm.BestSolution);

            Console.WriteLine(" E(x): " + E(memeticAlgorithm.BestSolution, k_j, t_j, d_j));
            Console.WriteLine(" R(x): " + R(memeticAlgorithm.BestSolution, P_j, k_j, d_j, t_j));
            Console.WriteLine(" g(x): " + g(memeticAlgorithm.BestSolution, k_j));
            Console.WriteLine("Time : " + watchForMA.ElapsedMilliseconds);


            Console.WriteLine("  +====   PSO algorithm  ====+ \n\n");
            Console.WriteLine("  Задайте размер популяции: ");
            int countOfPopulationPSO = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("  Задайте количество эпох: ");
            int erasPSO = Convert.ToInt32(Console.ReadLine());

            a1 = 0.5;
            a2 = 0.5;
            r = 0.1;

            PSOAlgorithm psoAlgorithm = new PSOAlgorithm(k_j.Length, countOfPopulationPSO, erasPSO, k_j, t_j, d_j, P_j, a1, a2, r, F);
            Console.WriteLine("  Результат роботы: ");
            var watchForPSO = System.Diagnostics.Stopwatch.StartNew();
            psoAlgorithm.Run();

            while (P(k_j, psoAlgorithm.BestSolution, psoAlgorithm.R, F) > e)
            {
                psoAlgorithm.R = 2.5 * psoAlgorithm.R;
                psoAlgorithm.A1 = 2.5 * psoAlgorithm.A1;
                psoAlgorithm.A2 = 2.5 * psoAlgorithm.A2;

                psoAlgorithm.Run();

            }
            //psoAlgorithm.Run();

            watchForPSO.Stop();
            Console.Write(" Набор клиентов таков: ");

            DisplayResult(psoAlgorithm.BestSolution);

            Console.WriteLine(" E(x): " + E(psoAlgorithm.BestSolution, k_j, t_j, d_j));
            Console.WriteLine(" R(x): " + R(psoAlgorithm.BestSolution, P_j, k_j, d_j, t_j));
            Console.WriteLine(" g(x): " + g(psoAlgorithm.BestSolution, k_j));
            Console.WriteLine("Time : " + watchForPSO.ElapsedMilliseconds);



        }

        public static double F(short[] Solution, int[] k_j, double[] t_j, double[] d_j,
            double[] P_j, double a1, double a2, double r, double F)
        {
            double result = 0;
            double result1 = 0;
            double result2 = 0;
            double result3 = 0;
            double result4 = 0;

            for (int i = 0; i < k_j.Length; i++)
            {
                result1 -= k_j[i] * (1 + d_j[i] * t_j[i]) * Solution[i];
                result2 += P_j[i] * k_j[i] * (1 + d_j[i] * t_j[i]) * Solution[i];
                result3 += Solution[i];
                result4 += k_j[i] * Solution[i];
            }

            result = result1 * a1 + Math.Round((result2 / result3) * a2, 6) + r * (result4 - F);

            return result;
        }

        public static double P(int[] k_j, short[] X_pop, double r, double F)
        {
            double result = 0;
            for (int i = 0; i < k_j.Length; i++)
            {
                result += k_j[i] * X_pop[i];
            }
            result = r * (result - F);
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
    }
}
