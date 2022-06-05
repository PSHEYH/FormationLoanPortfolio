using System;
using System.Collections.Generic;
using System.Text;



namespace PracticeForGraduate
{
    class Bee
    {
        public short[] Solution { get; set; }
        private List<short[]> ListOfSolutions = new List<short[]>();
        public bool isLoyalty = false;


        public double ProbabilityOfSolution(short[] Cmin, short[] Cb)
        {
            return Math.Exp(-SolutionDifference(Cmin,Cb));
        }

        public void Move()
        {
            Random rnd = new Random();

            int randomPlace = rnd.Next(0, Solution.Length);

            for (int i = 0; i < Solution.Length; i++)
            {
                if (randomPlace == i)
                {
                    Solution[i] = Solution[i] == (short)1 ? (short)0 : (short)1;
                    ListOfSolutions.Add(Solution);
                    Solution[i] = Solution[i] == (short)1 ? (short)0 : (short)1;
                }
            }
        }

        private short[] GetBestSolution()
        {

            return Solution;
        }

        private void Sort(int[] k_j, double[] t_j, double[] d_j,
            double[] P_j, double a1, double a2, double r, double F)
        {
            short[] BestSolution = ListOfSolutions[0];

            for (int i = 1; i < ListOfSolutions.Count; i++)
            {
                if (Program.F(BestSolution, k_j, t_j, d_j, P_j, a1,a2,r,F) < Program.F(ListOfSolutions[i], k_j, t_j, d_j, P_j, a1, a2, r, F))
                {
                    BestSolution = ListOfSolutions[i];
                }
            }

            for (int i = 0; i < BestSolution.Length; i++)
            {
                Solution[i] = BestSolution[i];
            }

        }

        private double SolutionDifference(short[] mainSolution, short[] secondSolution)
        {
            double result = 0;
            for (int i = 0; i < mainSolution.Length; i++)
            {
                if (mainSolution[i] != secondSolution[i])
                {
                    result += 0.1;
                }
            }

            return result;
        }

        public void AsignSolution(short[] solution)
        {
            if(solution.Length != Solution.Length)
            {
                throw new Exception(" Length of Solution is different");
            }
            for (int i = 0; i < this.Solution.Length; i++)
            {
                Solution[i] = solution[i];
            }
        }
    }
}
