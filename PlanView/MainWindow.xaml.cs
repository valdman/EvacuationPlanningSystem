using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using PlanPresentation;

namespace EvacuationPlanningSystem
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ScaleRate = 1.1;
        private readonly PlanPresentor _planPresentor;
        private DateTimeOffset _timeOffsetDueLastReload;

        public MainWindow()
        {
            InitializeComponent();
            _planPresentor = new PlanPresentor(ref Canvas);
            _timeOffsetDueLastReload = DateTimeOffset.Now;

            RegenertePlanButton.Click += (sender, args) =>
            {
                if (!int.TryParse(WidthInput.Text, out int width) || !int.TryParse(HeightInput.Text, out int height))
                {
                    MessageBox.Show("Ivalid plan size. Every dimention must be integer");
                }
                else
                {
                    _planPresentor.RegeneratePlan(width, height);
                    _planPresentor.DrawPlan();
                }
            };

            SizeChanged += (sender, args) =>
            {
                var heigthDifference = Math.Abs(args.NewSize.Height - args.PreviousSize.Height);
                var weightDifference = Math.Abs(args.NewSize.Width - args.PreviousSize.Width);
                if ((DateTimeOffset.Now - _timeOffsetDueLastReload).Milliseconds > 100 ||
                    heigthDifference > 10 ||
                    weightDifference > 10)
                {
                    _planPresentor.DrawPlan();
                    _planPresentor.DrawGatesAndPeople();
                }
                _timeOffsetDueLastReload = DateTimeOffset.Now;
            };

            StartButton.Click += (sender, args) =>
            {
                try
                {
                    var manCount = int.Parse(ManCount.Text);
                    var gatesCapasities = GateCapasities.Text.Split(',').Select(int.Parse).ToList();

                    _planPresentor.RunRandomSimulation(gatesCapasities, manCount);
                    //_planPresentor.DrawSolution();
                }
                catch (FormatException)
                {
                    MessageBox.Show(
                        "Ivalid gates capasities or man count. It must be list of integer (for example \"1,2,4,4\")\n" +
                        "Man count must be integer");
                }
            };

            Canvas.MouseWheel += (sender, args) =>
            {
                var st = new ScaleTransform();
                if (args.Delta > 0)
                {
                    st.ScaleX *= ScaleRate;
                    st.ScaleY *= ScaleRate;
                }
                else
                {
                    st.ScaleX /= ScaleRate;
                    st.ScaleY /= ScaleRate;
                }
            };
        }
    }
}