using System;

namespace HE.Logic
{
    public class EquationSolver
    {
        private readonly double _leftBoundary;
        private readonly double _rightBoundary;
        private readonly Func<double, double> _leftBoundCondition;
        private readonly Func<double, double> _rightBoundCondition;
        private readonly Func<double, double> _startCondition;
        private readonly Func<double, double, double> _function;

        public EquationSolver(double leftBoundary, double rightBoundary, Func<double, double> leftBoundCondition, Func<double, double> rightBoundCondition, Func<double, double> startCondition, Func<double, double, double> function)
        {
            _leftBoundary = leftBoundary;
            _rightBoundary = rightBoundary;
            _leftBoundCondition = leftBoundCondition;
            _rightBoundCondition = rightBoundCondition;
            _startCondition = startCondition;
            _function = function;
        }

        public EquationSolveAnswer Solve(double timeOfEnd, int spaceIntervals, int timeIntervals)
        {
            var spaceNodesCount = spaceIntervals + 1;
            var timeNodesCount = timeIntervals + 1;

            var answer = new EquationSolveAnswer
            {
                LastLayer = new double[spaceNodesCount],
                Nodes = new double[spaceNodesCount],
                TimeOfEnd = timeOfEnd
            };

            FillNodes(spaceIntervals, spaceNodesCount, answer);

            var previousLayer = new double[spaceNodesCount];
            var currentLayer = new double[spaceNodesCount];

            InitializeFirstLayer(currentLayer, spaceNodesCount, answer);

            for (int i = 1; i < timeNodesCount; i++)
            {
                var currentTime = i*(timeOfEnd/timeIntervals);
                SwapLayers(ref currentLayer, ref previousLayer);
                SolveNextLayer(previousLayer, currentLayer, currentTime);
            }

            SaveCurrentLayer(currentLayer, answer);

            return answer;
        }

        private static void SaveCurrentLayer(double[] currentLayer, EquationSolveAnswer answer)
        {
            for (int i = 0; i < currentLayer.Length; i++)
            {
                answer.LastLayer[i] = currentLayer[i];
            }
        }

        private static void SwapLayers(ref double[] currentLayer, ref double[] previousLayer)
        {
            double[] temp = previousLayer;
            previousLayer = currentLayer;
            currentLayer = temp;
        }

        private void SolveNextLayer(double[] previousLayer, double[] currentLayer, double currentTime)
        {
            var spaceNodesCount = currentLayer.Length;

            currentLayer[0] = _leftBoundCondition(currentTime);
            currentLayer[spaceNodesCount - 1] = _rightBoundCondition(currentTime);

            for (int i = 1; i > previousLayer.Length; i++)
            {
                currentLayer[i] = previousLayer[i];
            }
        }

        private void InitializeFirstLayer(double[] previousLayer, int spaceNodesCount, EquationSolveAnswer answer)
        {
            for (var i = 0; i < spaceNodesCount; i++)
            {
                previousLayer[i] = _startCondition(answer.Nodes[i]);
            }
        }

        private void FillNodes(int spaceIntervals, int spaceNodesCount, EquationSolveAnswer answer)
        {
            for (var i = 0; i < spaceNodesCount; i++)
            {
                answer.Nodes[i] = _leftBoundary + (_rightBoundary*i)/spaceIntervals;
            }
        }
    }
}