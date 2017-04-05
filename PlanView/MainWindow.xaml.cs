using System;
using System.Windows;
using System.Windows.Controls;
using PlanPresentation;
using PlanService;

namespace EvacuationPlanningSystem
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
            _timeOffsetDueLastReload = DateTimeOffset.Now;

            RegenertePlanButton.Click += (sender, args) => {
                if (!int.TryParse(WidthInput.Text, out int width) || !int.TryParse(HeightInput.Text, out int height))
                {
                    MessageBox.Show("Ivalid input");
                }
                else
                {
                    _planPresentor.RegeneratePlan(width, height);
                    _planPresentor.DrawPlan();
                }
            };

            SizeChanged += (sender, args) => {
                if((DateTimeOffset.Now - _timeOffsetDueLastReload).Milliseconds > 10)
                    _planPresentor.DrawPlan();
                _timeOffsetDueLastReload = DateTimeOffset.Now;
            };

            StartButton.Click += (sender, args) =>  {
                
            };
        }

        private readonly PlanPresentor _planPresentor;
        private DateTimeOffset _timeOffsetDueLastReload;
    }
}
