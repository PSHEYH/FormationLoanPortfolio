using System;
using System.Collections.Generic;
using System.Text;

namespace PracticeForGraduate
{
    class MemeticAlgorithm:Algorithm
    {
        private int _valueOfMutation;
        private List<short[]> _population;
        private int _countOfPopulation;

        public short[] BestSolution { get;private set; }


        public MemeticAlgorithm(int valutOfMutation, int lengthOfChrommossome, int countOfPopulation, int countOfEra,
            int[] k_j, double[] d_j, double[] t_j, double[] P_j, double a1, double a2, double r, double F)
        {
            _valueOfMutation = valutOfMutation;
            _lengthOfChromossome = lengthOfChrommossome;
            _countOfEra = countOfEra;
            _countOfPopulation = countOfPopulation;
            _population = new List<short[]>();

            _d_j = d_j;
            _k_j = k_j;
            _t_j = t_j;
            _P_j = P_j;
            A1 = a1;
            A2 = a2;
            R = r;
            _F = F;

        }

        private short[] GenerateIndividual()
        {
            short[] result = new short[_lengthOfChromossome];
            Random rnd = new Random();

            for (int i = 0; i < _lengthOfChromossome; i++)
            {
                result[i] = (short)rnd.Next(0, 2);
            }

            return result;
        }

        public void GeneratePopulation()
        {

            for (int i = 0; i < _countOfPopulation; i++)
            {
                _population.Add(GenerateIndividual());
            }
            Sort();
            BestSolution = _population[0];
        }

        public override void Run()
        {
            int average = (int)(_countOfPopulation / 2);

            GeneratePopulation();

            TabuSearch tbsearch = new TabuSearch(_lengthOfChromossome);

            for (int i = 0; i < _population.Count; i++)
            {
                tbsearch.Run(_population[i],_k_j,_t_j,_d_j,_P_j,A1,A2,R,_F);
            }

            Console.WriteLine(" Before algorithm ");
            DisplayResults();

            while (_countOfEra != 0)
            {
                Random rnd = new Random();

                int randomNumber = rnd.Next(0, _population.Count);

                int i = 0;

                Sort();

                while (i!= _countOfPopulation)
                {

                    for (int j = 0; j < average / 2; j++)
                    {
                        _population[i] = Crossover1(_population[j], _population[average - j - 1]);
                        i++;
                        _population[i] = Crossover1(_population[average - j - 1],_population[j]);
                        i++;
                        _population[i] = Crossover2(_population[j], _population[average - j - 1]);
                        i++;
                        _population[i] = Crossover2(_population[average - j - 1], _population[j]);
                        i++;
                    }
                }
                for (int j = 0; j < _valueOfMutation; j++)
                {
                    Mutation(_population[randomNumber]);
                    randomNumber = rnd.Next(0, _population.Count);
                }
                

                for (int j = 0; j < _population.Count; j++)
                {
                    tbsearch.Run(_population[j], _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F);
                }

                ///DisplayResults();

                _countOfEra--;
            }

            Sort();
            BestSolution = _population[0];
            _population.Clear();
        }

        private void Mutation(short[] individual)
        {
            Random rnd = new Random();
            int rndCount = rnd.Next(0, 8);
            int[] randNumbers = new int[rndCount];

            for (int j = 0; j < rndCount; j++)
            {
                randNumbers[j] = rnd.Next(0, _lengthOfChromossome);
            }

            for (int j = 0; j < _lengthOfChromossome; j++)
            {
                if (IsContain(randNumbers, j))
                {
                    individual[j] = individual[j] == (short)1 ? (short)0 : (short)1;
                }
            }

        }
        private static bool IsContain(int[] array, int point)
        {
            bool isContain = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == point)
                {
                    isContain = true;
                    break;
                }
            }
            return isContain;
        }

        private void Sort()
        {
            
            for (int i = 0; i < _countOfPopulation - 1; i++)
            {
                for (int j = i + 1; j < _countOfPopulation; j++)
                {
                    if (Program.F(_population[i],_k_j,_t_j,_d_j,_P_j,A1,A2,R,_F) > Program.F(_population[j], _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F))
                    {
                        short[] tmp = new short[_lengthOfChromossome];
                        for (int k = 0; k < _lengthOfChromossome; k++)
                        {
                            tmp[k] = _population[i][k];
                            _population[i][k] = _population[j][k];
                            _population[j][k] = tmp[k];
                        }
                    }
                }
            }
        }


        private short[] Crossover1(short[] XGreat, short[] XLight)
        {
            short[] result = new short[_lengthOfChromossome];

            for (int i = 0; i < _lengthOfChromossome; i++)
            {
                if (i < (Math.Round(_lengthOfChromossome* 0.6)))
                {
                    result[i] = XGreat[i];
                }
                else
                    result[i] = XLight[i];
            }

            return result;
        }

        private short[] Crossover2(short[] XGreat, short[] XLight)
        {
            short[] result = new short[_lengthOfChromossome];

            for (int i = 0; i < _lengthOfChromossome; i++)
            {
                if (i < (Math.Round(_lengthOfChromossome * 0.8)))
                {
                    result[i] = XGreat[i];
                }
                else
                    result[i] = XLight[i];
            }

            return result;
        }

        public void DisplayResults()
        {
            for (int i = 0; i < _population.Count; i++)
            {
                Console.Write(i + " Individual: ");
                DisplayIndividual(_population[i]);
            }
        }

        private void DisplayIndividual(short[] solution)
        {
            for (int j = 0; j < solution.Length; j++)
            {
                Console.Write(solution[j]);
            }
            Console.WriteLine(" ");
            Console.WriteLine(" ================== ");
        }

    }
}
