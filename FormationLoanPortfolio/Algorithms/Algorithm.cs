using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationLoanPortfolio
{
    abstract class Algorithm
    {
        protected int[] _k_j;
        protected double[] _d_j;
        protected double[] _t_j;
        protected double[] _P_j;
        protected double _F;
        protected int _countOfEra;
        protected int _lengthOfChromossome;


        public double A1 { get; set; }
        public double A2 { get; set; }
        public double R { get; set; }

        public virtual void Run()
        {

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

            result = result1 * a1 + Math.Round((result2 / result3) * a2, 6) + Math.Exp(r * (result4 - F))/r;

            return result;
        }


        private static double RFunction(short[] X_pop, double[] P_j, int[] k_j, double[] d_j, double[] t_j)
        {
            double result1 = 0;
            double resutl2 = 0;

            for (int i = 0; i < k_j.Length; i++)
            {
                resutl2 += X_pop[i];
                result1 += k_j[i] * (1 + d_j[i] * t_j[i]) * X_pop[i] * P_j[i];
            }

            if (resutl2 == 0)
                return 0;
            else
                return result1 / resutl2;
        }


        private static double EFunction(short[] X_pop, int[] k_j, double[] t_j, double[] d_j)
        {
            double result = 0;
            for (int i = 0; i < k_j.Length; i++)
            {
                result += k_j[i] * (1 + d_j[i] * t_j[i]) * X_pop[i];
            }

            return result;
        }


    }
}
