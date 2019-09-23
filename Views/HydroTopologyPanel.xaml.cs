using DHOG_WPF.DataProviders;
using DHOG_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for HydroTopologyPanel.xaml
    /// </summary>
    public partial class HydroTopologyPanel : UserControl
    {

        int contador = 0;
        HydroTopologyDataProvider hydroTopologyDataProvider;
        EntitiesCollections entitiesCollections;
        public static string HydroSystemName;
        public HydroTopologyPanel(EntitiesCollections entitiesCollections)
        {
            InitializeComponent();
            this.entitiesCollections = entitiesCollections;
            hydroTopologyDataProvider = new HydroTopologyDataProvider();
            Binding binding = new Binding();
            binding.Source = entitiesCollections;
            //RowLoaded
            //RowLoaded += BaseDataGrid_Formating;
            SetBinding(DataContextProperty, binding);
            HydroSystemsListBox.SelectedIndex = 0;
        }
         
        public void BaseDataGrid_Formating(object sender, RowLoadedEventArgs e)
        {

            contador = contador + 1;
            System.Type Tipo = e.Row.Cells.GetType();

            if ((contador % 2) == 0)
                e.Row.Background = new SolidColorBrush(Colors.LightGray);
            else
                e.Row.Background = new SolidColorBrush(Colors.WhiteSmoke);


            bool Valor;
            Valor = Tipo.IsValueType;

            if (Valor)
            {
                e.Row.Cells.ToString();
                //        column.DataFormatString = "{0:F0}";

            }

        }



        private void HydroSystemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HydroSystemViewModel system = HydroSystemsListBox.SelectedItem as HydroSystemViewModel;
            if (system != null)
            {
                HydroSystemName = system.Name;
            }
            else
            {
                HydroSystemsListBox.SelectedIndex = 0;
                return;
            }
            HydroTopologyCollectionViewModel systemTopology = hydroTopologyDataProvider.GetSystemTopology(HydroSystemName);
            SystemTopologyGrid.ItemsSource = systemTopology;
            //RowLoaded += BaseDataGrid_Formating;
        }

        private void SystemTopologyGrid_Pasting(object sender, Telerik.Windows.Controls.GridViewClipboardEventArgs e)
        {

        }
    }
}
