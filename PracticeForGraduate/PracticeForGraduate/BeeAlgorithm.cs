using System;
using System.Collections.Generic;
using System.Text;



namespace PracticeForGraduate
{
    class BeeAlgorithm
    {
        public int StepsForSearch { get; set; }
        public int LengthOfSolution { get; set; }

        private Bee[] bees;

        private int countLoyalBees = 0;

        private int[] _k_j;
        private double[] _t_j;
        private double[] _d_j;
        private double[] _P_j;
        public double A1 { get; set; }
        public double A2 { get; set; }
        public double R { get; set; }
        private double _F;

        public BeeAlgorithm(int stepsForSearch, int lengthOfSolution, int countOfBees, int[] k_j, double[] t_j, double[] d_j,
            double[] P_j, double a1, double a2, double r, double F) 
        {
            bees = new Bee[countOfBees];

            for (int i = 0; i < countOfBees; i++)
            {
                bees[i] = new Bee();
            }
            StepsForSearch = stepsForSearch;
            LengthOfSolution = lengthOfSolution;
            _k_j = k_j;
            _t_j = t_j;
            _d_j = d_j;
            _P_j = P_j;
            A1 = a1;
            A2 = a2;
            R = r;
            _F = F;
        }

        public short[] GetBestSolution()
        {
            return bees[GetBestIndexBee()].Solution;
        }

        private void GenerateSolutions()
        {
            for (int i = 0; i < bees.Length; i++)
            {
                short[] BeeSolution = new short[LengthOfSolution];

                for (int j = 0; j < LengthOfSolution; j++)
                {
                    Random rnd = new Random();
                    BeeSolution[j] = ((short)rnd.Next(0,2));
                }

                bees[i].Solution = BeeSolution;
            }
        }

        private short[] GenerateRandomSolution()
        {
            short[] BeeSolution = new short[LengthOfSolution];

            for (int j = 0; j < LengthOfSolution; j++)
            {
                Random rnd = new Random();
                BeeSolution[j] = ((short)rnd.Next(0, 2));
            }

            return BeeSolution;
        }

        public void Run()
        {
            while (StepsForSearch != 0)
            {
                for (int i = 0; i < bees.Length; i++)
                {
                    if(!bees[i].isLoyalty)
                        bees[i].Solution = GenerateRandomSolution();
                    
                    for (int j = 0; j < 6; j++)
                    { 
                        bees[i].Move();
                    }
                }

                int min = GetBestIndexBee();

                SetLoyaltyToBees(bees[min].Solution);

                SetDirectionToBees();

                DisplayResults();
                Console.WriteLine("\n\n");

                StepsForSearch--;
            }
        }

        private int GetBestIndexBee()
        {
            int min = 0;

            for (int i = 1; i < bees.Length; i++)
            {
                if (Program.F(bees[min].Solution, _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F) > Program.F(bees[i].Solution, _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F))
                {
                    min = i;
                }
            }

            return min;
        }

        private void SetLoyaltyToBees(short[] Cmin)
        {
            Random rnd = new Random();

            for (int i = 0; i < bees.Length; i++)
            {
                if (bees[i].ProbabilityOfSolution(Cmin, bees[i].Solution) > rnd.NextDouble())
                {
                    bees[i].isLoyalty = true;
                    countLoyalBees++;
                }
                else
                {
                    bees[i].isLoyalty = false;
                }
            }
        }

        private void SetDirectionToBees()
        {
            Random rnd = new Random();
            List<Bee> loyaltyBees = new List<Bee>();

            for (int i = 0; i < bees.Length; i++)
            {
                if (bees[i].isLoyalty)
                    loyaltyBees.Add(bees[i]);

            }

            for (int i = 0; i < loyaltyBees.Count; i++)
            {
                for (int j = 0; j < bees.Length; j++)
                {
                    if (!bees[j].isLoyalty)
                    {
                        if (1.0 / countLoyalBees < rnd.NextDouble()/2.0)
                            bees[j].AsignSolution(loyaltyBees[i].Solution);
                    }
                }
            }
        }

        public void DisplayResults()
        {
            for (int i = 0; i < bees.Length; i++)
            {
                Console.Write(i + " Bee: ");
                DisplayIndividual(bees[i]);
            }
        }

        private void DisplayIndividual(Bee bee)
        {
            for (int j = 0; j < bee.Solution.Length; j++)
            {
                Console.Write(bee.Solution[j]);
            }
            Console.WriteLine(" ");
            Console.WriteLine(" ================== ");
        }
    }
}
