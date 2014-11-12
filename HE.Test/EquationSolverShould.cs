using System;
using HE.Logic;
using NUnit.Framework;

namespace HE.Test
{
    [TestFixture]
    public class EquationSolverShould
    {
        [Test]
        public void SolveTestExample()
        {
            var exactAnswer =
                new Func<double, double, double>((x, t) => Math.Sin(Math.PI * x) * Math.Exp(-Math.PI * Math.PI * t));

            var solver = new HeatEquationSolver
            {
                StartCondition = x => Math.Sin(Math.PI * x),
            };

            const int n = 20000;
            const int k = 200;
            var answer1 = solver.Solve(0.001, n, k);

            var maximumOfDifference = Compute.MaximumOfDifference(answer1, exactAnswer);
            Expect.FloatsAreEqual(0, maximumOfDifference);
        }

        [Test]
        public void SolveTestExampleWithSecondOrderOfConvergenceForSpaceStep()
        {
            var solver = new HeatEquationSolver { StartCondition = x => Math.Sin(Math.PI*x) };
            var exactAnswer = new Func<double, double, double>((x, t) => Math.Sin(Math.PI * x) * Math.Exp(-Math.PI * Math.PI * t));

            Func<int, EquationSolveAnswer> testFunction = num => solver.Solve(timeOfEnd: 0.001, spaceIntervals: num, timeIntervals: 20);

            Expect.ExpectOrderOfConvergence(2, forFunction: testFunction, toFunction: exactAnswer, startParameter: 40);
        }


        [Test]
        public void SolveTrivialEquationExactly()
        {
            var solver = new HeatEquationSolver();

            const int n = 10;
            const int k = 20;
            var answer = solver.Solve(5.0, n, k);

            Assert.AreEqual(n + 1, answer.LastLayer.Length);
            foreach (double temperature in answer.LastLayer)
            {
                Assert.AreEqual(0, temperature);
            }
            Assert.AreEqual(n + 1, answer.Nodes.Length);
            Assert.AreEqual(0, answer.Nodes[0]);
            Assert.AreEqual(1, answer.Nodes[n]);
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(answer.Nodes[i], i/(double)n);
            }
        }
    }
}