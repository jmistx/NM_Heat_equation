using System;

namespace HE.Logic
{
    public class EquationSolver
    {
        public EquationSolver(double leftBoundary, double rightBoundary, Func<double, double> leftBoundCondition, Func<double, double> rightBoundCondition, Func<double, double> startCondition, Func<double, double> function)
        {
        }

        public EquationSolveAnswer Solve(double time, int spaceIntervals, int timeIntervals)
        {
            var spaceNodesCount = spaceIntervals + 1;
            var equationSolveAnswer = new EquationSolveAnswer
            {
                LastLayer = new double[spaceNodesCount]
            };
            return equationSolveAnswer;
        }
    }
}