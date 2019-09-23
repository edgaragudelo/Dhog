using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace DHOG_WPF.Views
{
    public abstract class BaseChart: RadCartesianChart
    {
        public BaseChart()
        {
            Background = new SolidColorBrush(Colors.White);
            VerticalAxis = new LinearAxis();
            HorizontalAxis = new CategoricalAxis
            {
                SmartLabelsMode = AxisSmartLabelsMode.SmartStep,
                LabelFitMode = AxisLabelFitMode.Rotate,
                LabelRotationAngle = 270
            };
            
            ChartTrackBallBehavior chartTrackBallBehavior = new ChartTrackBallBehavior
            {
                ShowIntersectionPoints = true,               
                ShowTrackInfo = true,                  
            };
            //System.Windows.Point position = chartTrackBallBehavior.Position;
            //position.X = (23);
            //position.Y = (45);

            //chartTrackBallBehavior.Position = position;
            

            ChartPanAndZoomBehavior panAndZoomBehavior = new ChartPanAndZoomBehavior
            {
                ZoomMode = ChartPanZoomMode.Both,                
                PanMode = ChartPanZoomMode.Both
            };

            Behaviors.Add(panAndZoomBehavior);
            Behaviors.Add(chartTrackBallBehavior);
        }

        public virtual void Update(int scenario) { }
        public virtual void Update(int scenario, string selectedOption) { }
    }
}
