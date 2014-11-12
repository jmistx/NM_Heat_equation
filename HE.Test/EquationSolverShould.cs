using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Logic;
using NUnit.Framework;

namespace HE.Test
{
    [TestFixture]
    public class EquationSolverShould    
    {
        [Test]
        public void SolveTrivialEquationExactly()
        {
            var leftBoundCondition = new Func<double, double>(x => 0);
            var rightBoundCondition = new Func<double, double>(x => 0);
            var startCondition = new Func<double, double>(x => 0);
            var function = new Func<double, double>(x => 0);
            const int leftBoundary = 0;
            const double rightBoundary = 1.0;
            const int n = 10;
            const int k = 20;

            var solver = new EquationSolver(leftBoundary, rightBoundary, leftBoundCondition, rightBoundCondition, startCondition, function);
            var answer = solver.Solve(time: 5.0, spaceIntervals: n, timeIntervals: k);
            Assert.AreEqual(n + 1, answer.LastLayer.Length);
            foreach (var temperature in answer.LastLayer)
            {
                Assert.AreEqual(0, temperature);
            }
        }
    }
}
