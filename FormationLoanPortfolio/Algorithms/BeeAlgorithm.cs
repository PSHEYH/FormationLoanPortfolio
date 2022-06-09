using System;
using System.Collections.Generic;
using System.Text;



namespace FormationLoanPortfolio
{
    class BeeAlgorithm: Algorithm
    {

        private Bee[] bees;

        private int countLoyalBees = 0;
        public short[] BestSolution { get; private set; }


        public BeeAlgorithm(int stepsForSearch, int lengthOfSolution, int countOfBees, int[] k_j, double[] t_j, double[] d_j,
            double[] P_j, double a1, double a2, double r, double F) 
        {
            bees = new Bee[countOfBees];

            for (int i = 0; i < countOfBees; i++)
            {
                bees[i] = new Bee();
            }
            _countOfEra = stepsForSearch;
            _lengthOfChromossome = lengthOfSolution;
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
            BestSolution = bees[GetBestIndexBee()].Solution;
            return bees[GetBestIndexBee()].Solution;
        }

        public void GenerateSolutions()
        {
            for (int i = 0; i < bees.Length; i++)
            {
                short[] BeeSolution = new short[_lengthOfChromossome];

                for (int j = 0; j < _lengthOfChromossome; j++)
                {
                    Random rnd = new Random();
                    BeeSolution[j] = ((short)rnd.Next(0,2));
                }

                bees[i].Solution = BeeSolution;
            }
        }

        private short[] GenerateRandomSolution(Random rnd)
        {
            short[] BeeSolution = new short[_lengthOfChromossome];

            for (int j = 0; j < _lengthOfChromossome; j++)
            {
                BeeSolution[j] = ((short)rnd.Next(0, 2));
            }

            return BeeSolution;
        }

        public override void Run()
        {
            int countOfEra =_countOfEra;

            while (countOfEra != 0)
            {
                Random rnd = new Random();

                for (int i = 0; i < bees.Length; i++)
                {
                    if(!bees[i].isLoyalty)
                        bees[i].Solution = GenerateRandomSolution(rnd);
                    
                    for (int j = 0; j < 6; j++)
                    { 
                        bees[i].Move();
                    }
                }

                int min = GetBestIndexBee();

                SetLoyaltyToBees(bees[min].Solution);

                SetDirectionToBees();

                ////  Display cromossomes
                //DisplayResults();
                //Console.WriteLine("\n\n");

                for (int i = 0; i < bees.Length; i++)
                {
                    bees[i].Clear();
                }

                countOfEra--;
            }

            BestSolution = new short[bees[GetBestIndexBee()].Solution.Length];

            for (int i = 0; i < BestSolution.Length; i++)
            {
                BestSolution[i] = bees[GetBestIndexBee()].Solution[i];
            }

        }

        private int GetBestIndexBee()
        {
            int min = 0;

            for (int i = 1; i < bees.Length; i++)
            {
                if (F(bees[min].Solution, _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F) > F(bees[i].Solution, _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F))
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
