using System.Windows.Input;
using HE.Logic;
using Microsoft.TeamFoundation.MVVM;

namespace HE.Gui
{
    public class MainViewModel : ViewModelBase
    {
        public HeatEquationSolver Solver { get; set; }

        public double LeftBoundary { get; set; }
        public double RightBoundary { get; set; }
        public ICommand CalculateCommand { get; set; }
        public double EndTime { get; set; }
        public int NumberOfSpaceIntervals { get; set; }
        public int NumberOfTimeIntervals { get; set; }
        public string LeftBoundaryCondition { get; set; }
        public object RightBoundaryCondition { get; set; }
        public string InitialCondition { get; set; }

        public MainViewModel()
        {
            Solver = new HeatEquationSolver();
            CalculateCommand = new RelayCommand(Calculate);
        }

        private void Calculate()
        {
            var a = 1;
        }
    }
}