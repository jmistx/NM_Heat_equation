using System;
using System.Data;
using System.Globalization;
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
            LeftBoundaryCondition = "0";
            RightBoundaryCondition = "0";
            InitialCondition = "0";
            Function = "0";
        }

        private void PopulateSecondExample()
        {
            LeftBoundary = 0;
            RightBoundary = 1;

            EndTime = 0.7;

            NumberOfSpaceIntervals = 10;
            NumberOfTimeIntervals = 100;

            LeftBoundaryCondition = "0";
            RightBoundaryCondition = "0";

            InitialCondition = "0";
            Function = "sin(pi*x)";

            RaisePropertyChanged(null);
        }

        private void PopulateFirstExample()
        {
            LeftBoundary = 0;
            RightBoundary = 1;

            EndTime = 0.7;

            NumberOfSpaceIntervals = 10;
            NumberOfTimeIntervals = 100;

            LeftBoundaryCondition = "0";
            RightBoundaryCondition = "0";

            InitialCondition = "sin(pi* x)";

            RaisePropertyChanged(null);
        }

        private void Calculate()
        {
            var solver = new HeatEquationSolver
            {
                LeftBoundary = LeftBoundary,
                RightBoundary = RightBoundary,
                LeftBoundCondition = t => 0,
                RightBoundCondition = t => 0,
                StartCondition = x => Math.Sin(x),
                Function = (x, t) => 0
            };
            var answer = solver.Solve(EndTime, NumberOfSpaceIntervals, NumberOfTimeIntervals);
            LastLayer = Populate(answer);
            RaisePropertyChanged("LastLayer");

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
}