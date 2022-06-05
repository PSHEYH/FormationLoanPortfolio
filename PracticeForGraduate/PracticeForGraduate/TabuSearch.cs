using System;
using System.Collections.Generic;
using System.Text;

namespace PracticeForGraduate
{
    class TabuSearch
    {
        private List<short[]> _passedVariants;
        private int _lengthOfIndivid;
        private int _countOfEra = 50;
        private short[] _resultVector;

        private List<short[]> _probableVariants;

        public TabuSearch(int length)
        {
            _passedVariants = new List<short[]>();
            _probableVariants = new List<short[]>();
            _lengthOfIndivid = length;
        }

        private short[] GenerateIndividual()
        {
            short[] result = new short[_lengthOfIndivid];
            Random rnd = new Random();

            for (int i = 0; i < _lengthOfIndivid; i++)
            {
                result[i] = (short)rnd.Next(0, 2);
            }

            return result;
        }

        public void Run(short[] solution,int[] k_j, double[] t_j, double[] d_j,
            double[] P_j, double a1, double a2, double r, double F)
        {

            while (_countOfEra!=0)
            {

                for (int i = 0; i < 8; i++)
                {
                    _probableVariants.Add(Move(solution));
                }

                Sort(k_j, t_j, d_j, P_j, a1, a2, r, F);

                _passedVariants.Add(_probableVariants[0]);
                
                if(Program.F(_probableVariants[0],k_j, t_j, d_j, P_j, a1, a2, r, F) < Program.F(solution, k_j, t_j, d_j, P_j, a1, a2, r, F))
                {
                    for (int i = 0; i < _lengthOfIndivid; i++)
                    {
                        solution[i] = _probableVariants[0][i];
                    }
                }

                _probableVariants.Clear();

                _countOfEra--;
            }
        }

        private short[] Move(short[] O)
        {
            short[] result = new short[_lengthOfIndivid];

            for (int i = 0; i < _lengthOfIndivid; i++)
            {
                result[i] = O[i];
            }

            Random rnd = new Random();
            int randomPlace = rnd.Next(0, _lengthOfIndivid);

            result[randomPlace] = O[randomPlace] == (short)1 ? (short)0 : (short)1;

            return result;
        }


        private void Sort(int[] k_j, double[] t_j, double[] d_j,
            double[] P_j, double a1, double a2, double r, double F)
        {

            for (int i = 0; i < _probableVariants.Count - 1; i++)
            {
                for (int j = i + 1; j < _probableVariants.Count; j++)
                {
                    if (Program.F(_probableVariants[i], k_j, t_j, d_j, P_j, a1, a2, r, F) > Program.F(_probableVariants[j], k_j, t_j, d_j, P_j, a1, a2, r, F))
                    {
                        short[] tmp = new short[_lengthOfIndivid];
                        for (int k = 0; k < _lengthOfIndivid; k++)
                        {
                            tmp[k] = _probableVariants[i][k];
                            _probableVariants[i][k] = _probableVariants[j][k];
                            _probableVariants[j][k] = tmp[k];
                        }
                    }
                }
            }
        }
    }
}
