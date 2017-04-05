using System.Windows;
using PlanPresentation;
using PlanService;

namespace TaskMaze
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _planPresentor = new PlanPresentor(ref Canvas);

            RegenertePlanButton.Click += (sender, args) =>
            {
                if (!int.TryParse(PeopleInput.Text, out int input))
                {
                    MessageBox.Show("Ivalid input");
                }
                else
                {
                    _planPresentor.RegeneratePlan(input, input);
                    _planPresentor.DrawPlan();
                }
            };

            StartButton.Click += (sender, args) =>
            {
                
            };
        }

        private readonly PlanPresentor _planPresentor;
    }
}
