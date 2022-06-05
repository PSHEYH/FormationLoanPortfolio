using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PracticeForGraduate
{
    class PSOAlgorithm
    {
        private int _lengthOfChromossome;
        private List<short[]> _population;
        private List<short[]> _posibleVariants;
        private List<short[]> _V;
        private List<short[]> _P;
        private short[] _G;

        private int _countOfPopulation;
        private int _countOfEra;

        public short[] BestSolution { get;private set; }

        private int[] _k_j;
        private double[] _d_j;
        private double[] _t_j;
        private double[] _P_j;
        public double A1 { get; set; }
        public double A2 { get; set; }
        public double R { get; set; }
        private double _F;

        public PSOAlgorithm(int lengthOfChrommossome, int countOfPopulation, int countOfEra,
           int[] k_j, double[] d_j, double[] t_j, double[] P_j, double a1, double a2, double r, double F)
        {
            _lengthOfChromossome = lengthOfChrommossome;
            _countOfEra = countOfEra;
            _countOfPopulation = countOfPopulation;
            _population = new List<short[]>();
            _V = new List<short[]>();
            _P = new List<short[]>();
            _posibleVariants = new List<short[]>();

            _d_j = d_j;
            _k_j = k_j;
            _t_j = t_j;
            _P_j = P_j;
            A1 = a1;
            A2 = a2;
            R = r;
            _F = F;

        }

        public void Run()
        {
            GeneratePopulation();
            InitializeSpeed();

            while (_countOfEra!=0)
            {
                for (int i = 0; i < _population.Count; i++)
                {
                    Move(i);
                }
                SetV();
                SetX();

                _P.Clear();

                _countOfEra--;
            }

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
            Sort(_population);
            BestSolution = _population[0];
        }


        public short[] InitializeOneSpeed()
        {
            Random rnd = new Random();

            int randomLength = rnd.Next(0,_lengthOfChromossome);

            short[] randomPlaces = new short[randomLength];

            for (int i = 0; i < randomLength; i++)
            {
                randomPlaces[i] = (short)rnd.Next(0, randomLength);
            }

            IEnumerable<short> result = randomPlaces.Distinct();

            short[] arrayresult = new short[result.Count()];

            int j = 0;
            foreach (short item in result)
            {
                arrayresult[j] = item;
                j++;
            }


            return arrayresult;
        }

        public void InitializeSpeed()
        {
            for (int i = 0; i < _countOfPopulation; i++)
            {
                _V.Add(InitializeOneSpeed());
            }
        }

        private void SetV()
        {
            for (int i = 0; i < _population.Count; i++)
            {
                _V[i] = SumV(Multiply(0.4, _V[i], true), Multiply(0.6, _P[i], true), Multiply(0.5, _G, false));
            }
        }

        private void SetX()
        {
            for (int i = 0; i < _countOfPopulation; i++)
            {
                Sum(i, i);
            }
        }

        private void Move(int xIndex)
        {
            for (int i = 0; i < 8; i++)
            {
                while (true)
                {
                    Random rnd = new Random();
                    int index = rnd.Next(0, _lengthOfChromossome);

                    _population[xIndex][index] = _population[xIndex][index] == (short)1 ? (short)0 : (short)1;

                    if (!IsContain(_population[xIndex], xIndex))
                    {
                        _posibleVariants.Add(_population[xIndex]);
                        _population[xIndex][index] = _population[xIndex][index] == (short)1 ? (short)0 : (short)1;
                        break;
                    }

                }
            }


            _P[xIndex] = GetBestPosibleVariant();
            _posibleVariants.Clear();

        }

        private short[] GetBestPosibleVariant()
        {
            Sort(_posibleVariants);

            return _posibleVariants[0];
        }

        public bool IsEqual(short[] first, short[] second)
        {
            if (first == null || second == null)
                return false;

            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    return false;
            }

            return true;
        }

        public bool IsContain(short[] result, int index)
        {
            for (int i = 0; i < _population.Count; i++)
            {
                if (index == i)
                    continue;
                if (IsEqual(result,_population[i]))
                    return true;
            }

            return false;
        }

        private void Sort(List<short[]> population)
        {

            for (int i = 0; i < population.Count - 1; i++)
            {
                for (int j = i + 1; j < population.Count; j++)
                {
                    if (Program.F(population[i], _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F) > Program.F(population[j], _k_j, _t_j, _d_j, _P_j, A1, A2, R, _F))
                    {
                        short[] tmp = new short[_lengthOfChromossome];
                        for (int k = 0; k < _lengthOfChromossome; k++)
                        {
                            tmp[k] = population[i][k];
                            population[i][k] = population[j][k];
                            population[j][k] = tmp[k];
                        }
                    }
                }
            }
        }

        private void Sum(int xIndex, int vIndex)
        {
            for (int i = 0; i < _V[vIndex].Length; i++)
            {
                _population[xIndex][_V[vIndex][i]] = _population[xIndex][_V[vIndex][i]] == (short)1 ? (short)0 : (short)1;
            }
        }

        private short[] SumV(short[] first, short[] second, short[] third)
        {

            if (first.Length >= second.Length && first.Length >= third.Length)
                return ComplexSum(first, second, third);
            else if (second.Length >= first.Length && second.Length >= third.Length)
                return ComplexSum(second, third, first);
            else
                return ComplexSum (third, first, second);
        }

        private short[] ComplexSum(short[] biggest, short[] low1, short[] low2)
        {
            List<short> result = new List<short>();

            for (int i = 0; i < biggest.Length; i++)
            {
                result.Add(biggest[i]);
            }

            SetNoMeetElements(result, biggest, low1);
            SetNoMeetElements(result, biggest, low2);

            short[] arrayResult = new short[result.Count];

            for (int i = 0; i < arrayResult.Length; i++)
            {
                arrayResult[i] = result[i];
            }

            result.Clear();
            return arrayResult;
        }

        private void SetNoMeetElements(List<short> result,short[] biggest, short[] low)
        {
            for (int i = 0; i < low.Length; i++)
            {
                bool isContain = false;
                for (int j = 0; j < biggest.Length; j++)
                {
                    if (low[i] == biggest[j])
                        isContain = true;

                }

                if (!isContain)
                    result.Add(low[i]);
            }
        }



        private short[] Difference(short[] first, short[] second)
        {
            List<short> result = new List<short>();
            for (short i = 0; i < first.Length; i++)
            {
                if(first[i] != second[i])
                {
                    result.Add(i);
                }
            }

            short[] arrayResult = new short[result.Count];

            for (int i = 0; i < result.Count; i++)
            {
                arrayResult[i] = result[i];
            }

            result.Clear();
            return arrayResult;
        }


        private short[] Multiply(double c, short[] V, bool isFirstHalf)
        {
            int length = (int)Math.Round(V.Length * c);

            short[] resultV = new short[length];

            if (isFirstHalf)
            {
                for (int i = 0; i < length; i++)
                {
                    resultV[i] = V[i];
                }
            }
            else
            {
                int j = 0;
                for (int i = length; i < V.Length; i++)
                {
                    resultV[j] = V[i];
                }
            }
            return resultV;

        }
    }
}
