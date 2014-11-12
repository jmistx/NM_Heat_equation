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
            var leftBoundCondition = new Func<double, double>(t => 0);
            var rightBoundCondition = new Func<double, double>(t => 0);
            var startCondition = new Func<double, double>(x => Math.Sin(Math.PI * x));
            var function = new Func<double, double, double>((x, t) => 0);
            const int leftBoundary = 0;
            const double rightBoundary = 1.0;
            var exactAnswer =
                new Func<double, double, double>((x, t) => Math.Sin(Math.PI * x) * Math.Exp(-Math.PI * Math.PI * t));

            var solver = new EquationSolver(leftBoundary, rightBoundary, leftBoundCondition, rightBoundCondition,
                startCondition, function);

            const int n = 20000;
            const int k = 200;
            var answer1 = solver.Solve(0.001, n, k);

            var maximumOfDifference = MaximumOfDifference(answer1, exactAnswer);
            Expect.FloatsAreEqual(0, maximumOfDifference);
        }

        [Test]
        public void SolveTestExampleWithSecondOrderOfConvergenceForSpaceStep()
        {
            var leftBoundCondition = new Func<double, double>(t => 0);
            var rightBoundCondition = new Func<double, double>(t => 0);
            var startCondition = new Func<double, double>(x => Math.Sin(Math.PI*x));
            var function = new Func<double, double, double>((x, t) => 0);
            const int leftBoundary = 0;
            const double rightBoundary = 1.0;
            var exactAnswer =
                new Func<double, double, double>((x, t) => Math.Sin(Math.PI*x)*Math.Exp(-Math.PI*Math.PI*t));

            var solver = new EquationSolver(leftBoundary, rightBoundary, leftBoundCondition, rightBoundCondition,
                startCondition, function);

            var n = 40;
            const int k = 20;
            var answer1 = solver.Solve(0.001, n, k);

            n = 400;
            var answer2 = solver.Solve(0.001, n, k);

            var maximumOfDifference1 = MaximumOfDifference(answer1, exactAnswer);
            var exponent1 = Math.Floor(Math.Abs(Math.Log10(Math.Abs(maximumOfDifference1))));

            var maximumOfDifference2 = MaximumOfDifference(answer2, exactAnswer);
            var exponent2 = Math.Floor(Math.Abs(Math.Log10(Math.Abs(maximumOfDifference2))));

            Assert.AreEqual(2, Math.Abs(exponent2 - exponent1));
        }

        private double MaximumOfDifference(EquationSolveAnswer answer, Func<double, double, double> exactAnswer)
        {
            double maximumDifference = 0;
            for (int i = 0; i < answer.LastLayer.Length; i++)
            {
                var difference = Math.Abs(answer.LastLayer[i] - exactAnswer(answer.Nodes[i], answer.TimeOfEnd));
                if (difference > maximumDifference)
                {
                    maximumDifference = difference;
                }
            }
            return maximumDifference;
        }

        [Test]
        public void SolveTrivialEquationExactly()
        {
            var leftBoundCondition = new Func<double, double>(x => 0);
            var rightBoundCondition = new Func<double, double>(x => 0);
            var startCondition = new Func<double, double>(x => 0);
            var function = new Func<double, double, double>((x, t) => 0);
            const int leftBoundary = 0;
            const double rightBoundary = 1.0;

            var solver = new EquationSolver(leftBoundary, rightBoundary, leftBoundCondition, rightBoundCondition,
                startCondition, function);

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