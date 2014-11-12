using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using ExpressionEvaluator;
using HE.Logic;
using Microsoft.TeamFoundation.MVVM;

namespace HE.Gui
{
    public class MainViewModel : ViewModelBase
    {
        public string Function { get; set; }

        public ICommand CalculateCommand { get; set; }
        public ICommand PopulateSecondExampleCommand { get; set; }
        public ICommand PopulateFirstExampleCommand { get; set; }

        public double LeftBoundary { get; set; }
        public double RightBoundary { get; set; }
        public double EndTime { get; set; }
        public int NumberOfSpaceIntervals { get; set; }
        public int NumberOfTimeIntervals { get; set; }
        public string LeftBoundaryCondition { get; set; }
        public string RightBoundaryCondition { get; set; }
        public string InitialCondition { get; set; }

        public DataTable LastLayer { get; set; }

        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
            PopulateFirstExampleCommand = new RelayCommand(PopulateFirstExample);
            PopulateSecondExampleCommand = new RelayCommand(PopulateSecondExample);
            RightBoundary = 1;
            EndTime = 0.7;
            NumberOfTimeIntervals = 100;
            NumberOfSpaceIntervals = 10;
            LeftBoundaryCondition = "0.0";
            RightBoundaryCondition = "0.0";
            InitialCondition = "0.0";
            Function = "0.0";
        }

        private void PopulateSecondExample()
        {
            LeftBoundary = 0;
            RightBoundary = 1;

            EndTime = 0.7;

            NumberOfSpaceIntervals = 10;
            NumberOfTimeIntervals = 100;

            LeftBoundaryCondition = "0.0";
            RightBoundaryCondition = "0.0";

            InitialCondition = "0.0";
            Function = "m.Sin(m.PI * p.x)";

            RaisePropertyChanged(null);
        }

        private void PopulateFirstExample()
        {
            LeftBoundary = 0;
            RightBoundary = 1;

            EndTime = 0.7;

            NumberOfSpaceIntervals = 10;
            NumberOfTimeIntervals = 100;

            LeftBoundaryCondition = "0.0";
            RightBoundaryCondition = "0.0";

            InitialCondition = "m.Sin(m.PI * p.x)";

            RaisePropertyChanged(null);
        }

        private void Calculate()
        {

            var solver = new HeatEquationSolver
            {
                LeftBoundary = LeftBoundary,
                RightBoundary = RightBoundary,
                LeftBoundCondition = ParseTimeArgMethod(LeftBoundaryCondition),
                RightBoundCondition = ParseTimeArgMethod(RightBoundaryCondition),
                StartCondition = ParsePositionArgMethod(InitialCondition),
                Function = ParseTwoArgsMethod(Function)
            };
            var answer = solver.Solve(EndTime, NumberOfSpaceIntervals, NumberOfTimeIntervals);
            LastLayer = Populate(answer);
            RaisePropertyChanged("LastLayer");

        }

        private static Func<double, double> ParsePositionArgMethod(string textExpression)
        {
            var typeRegistry = new TypeRegistry();
            var param = new PositionArg();
            typeRegistry.RegisterType("m", typeof(Math));
            typeRegistry.RegisterSymbol("p", param);

            var expression = new CompiledExpression<double>(textExpression)
            {
                TypeRegistry = typeRegistry
            };

            Func<double, double> f = x =>
            {
                param.x = x;
                return expression.Eval();
            };
            return f;
        }

        private static Func<double, double> ParseTimeArgMethod(string textExpression)
        {
            var typeRegistry = new TypeRegistry();
            var param = new TimeArg();
            typeRegistry.RegisterType("m", typeof(Math));
            typeRegistry.RegisterSymbol("p", param);

            var expression = new CompiledExpression<double>(textExpression)
            {
                TypeRegistry = typeRegistry
            };

            Func<double, double> f = t =>
            {
                param.t = t;
                return expression.Eval();
            };
            return f;
        }

        private static Func<double, double, double> ParseTwoArgsMethod(string textExpression)
        {
            var typeRegistry = new TypeRegistry();
            var param = new TwoArgs();
            typeRegistry.RegisterType("m", typeof (Math));
            typeRegistry.RegisterSymbol("p", param);

            var expression = new CompiledExpression<double>(textExpression)
            {
                TypeRegistry = typeRegistry
            };

            Func<double, double, double> f = (x, t) =>
            {
                param.x = x;
                param.t = t;
                return expression.Eval();
            };
            return f;
        }

        private DataTable Populate(EquationSolveAnswer answer)
        {
            var dataTable = new DataTable();
            var columnNames = new string[answer.LastLayer.Length];
            for (int i = 0; i < answer.LastLayer.Length; i++)
            {
                columnNames[i] = answer.Nodes[i].ToString("#0.###");
                dataTable.Columns.Add(new DataColumn(columnNames[i]));
            }

            var newRow = dataTable.NewRow();
            for (int i = 0; i < answer.LastLayer.Length; i++)
            {
                newRow[i] = answer.LastLayer[i];
            }
            dataTable.Rows.Add(newRow);
            return dataTable;
        }
    }

    public class TwoArgs
    {
        public double x { get; set; }
        public double t { get; set; }
    }

    public class TimeArg
    {
        public double t { get; set; }
    }

    public class PositionArg
    {
        public double x { get; set; }
    }
}