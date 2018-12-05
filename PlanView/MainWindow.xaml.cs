using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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
        private Point _currentPoint;

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
                    _planPresentor.Drawer.DrawPlan();
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
                    _planPresentor.Drawer.DrawPlan();
                    _planPresentor.Drawer.DrawGatesAndPeople();
                    _planPresentor.Drawer.DrawSolution();
                }
                _timeOffsetDueLastReload = DateTimeOffset.Now;
            };

            StartButton.Click += (sender, args) =>
            {
                try
                {
                    _planPresentor.Drawer.DrawSolution();
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

            Canvas.MouseMove += (sender, e) =>
            {
                if (e.LeftButton != MouseButtonState.Pressed) return;
                var line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.DarkRed),
                    StrokeThickness = 2,
                    X1 = _currentPoint.X,
                    Y1 = _currentPoint.Y,
                    X2 = e.GetPosition(this).X,
                    Y2 = e.GetPosition(this).Y
                };


                _currentPoint = e.GetPosition(this);

                Canvas.Children.Add(line);
            };
            Canvas.MouseDown += (sender, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    _currentPoint = e.GetPosition(this);
            };

            ShowHintButton.Click += (sender, args) =>
            {
                MessageBox.Show($"Solution max length {_planPresentor.CurrentSolution.Max(way => way.WayOut.Count)}, " +
                                $"mean {_planPresentor.CurrentSolution.Average(solution => solution.WayOut.Count):#,##0}");
            };
        }

        private void ShowUnitsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manCount = int.Parse(ManCount.Text);
                var gatesCapasities = GateCapasities.Text.Split(',').Select(int.Parse).ToList();

                _planPresentor.RunRandomSimulation(gatesCapasities, manCount);
                _planPresentor.Drawer.DrawGatesAndPeople();
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Ivalid gates capasities or man count. It must be list of integer (for example \"1,2,4,4\")\n" +
                    "Man count must be integer");
            }
        }

        private void RelocateGats_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var gatesCapasities = GateCapasities.Text.Split(',').Select(int.Parse).ToList();
                
                _planPresentor.ReloadGates(gatesCapasities);
                _planPresentor.Drawer.DrawGatesAndPeople();
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Ivalid gates capasities. It must be list of integer (for example \"1,2,4,4\")");
            }
        }

        private void RelocatePeople_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manCount = int.Parse(ManCount.Text);

                _planPresentor.ReloadPeople(manCount);
                _planPresentor.Drawer.DrawGatesAndPeople();
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Ivalid number of people. It must be integer");
            }
        }
    }
}