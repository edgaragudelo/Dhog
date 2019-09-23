using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using static DHOG_WPF.DataTypes.Types;
using DHOG_WPF.Views;
using System.Linq;

namespace DHOG_WPF.CustomControls
{
    

    public class CloseableTab : RadTabItem
    {

        // Constructor
        public CloseableTab()
        {
            // Create an instance of the usercontrol
            CloseableHeader closableTabHeader = new CloseableHeader();

            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;

            // Attach to the CloseableHeader events (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter += new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave += new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click += new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
            //closableTabHeader.MouseDoubleClick += new RoutedEventHandler(MouseDoubleClick1);
            closableTabHeader.MouseLeftButtonDown += new MouseButtonEventHandler(MouseDoubleClick1);
        }



        /// <summary>
        /// Property - Set the Title of the Tab
        /// </summary>
        public string Title
        {
            get
            {
                return ((CloseableHeader)this.Header).label_TabTitle.Content.ToString();
            }
            set
            {
                ((CloseableHeader)this.Header).label_TabTitle.Content = value;
            }
        }

        public InfoType Type { get; set; }

        
        //
        // - - - Overrides  - - -
        //
        protected override void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            base.OnIsSelectedChanged(oldValue, newValue);
            if (newValue)
            {
                ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
                ((CloseableHeader)this.Header).label_TabTitle.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
                ((CloseableHeader)this.Header).label_TabTitle.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
                ((CloseableHeader)this.Header).IsEnabled = true;
            }
        }

        //
        // - - - Event Handlers  - - -
        //
        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Foreground = Brushes.Red;
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Foreground = Brushes.Black;
        }

        void MouseDoubleClick1(object sender, RoutedEventArgs e)
        {
            RadTabControl tabControl = (RadTabControl)this.Parent;
            if (Type.Equals(InfoType.Chart))
            {
                Grid grid = Content as Grid;
                BaseChart chart = grid.ChildrenOfType<BaseChart>().FirstOrDefault();
                grid.Children.Remove(chart);
            }

        }
            // Button Close Click - Remove the Tab - (or raise an event indicating a "CloseTab" event has occurred)
            void button_close_Click(object sender, RoutedEventArgs e)
        {
            RadTabControl tabControl = (RadTabControl)this.Parent;
            if (Type.Equals(InfoType.Chart))
            {
                Grid grid = Content as Grid;
                BaseChart chart = grid.ChildrenOfType<BaseChart>().FirstOrDefault();
                grid.Children.Remove(chart);
            }


            tabControl.Items.Remove(this);
     
            //if (tabControl.Items.Count == 1)
            //{
            //    RadTabItem item = (RadTabItem)(tabControl.Items[0]);
            //    item.IsSelected = true; // true;
            //}
        }


        // Label SizeChanged - When the Size of the Label changes (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Margin = new Thickness(((CloseableHeader)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }

    }
}
