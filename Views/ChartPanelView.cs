using DHOG_WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.Views
{
    public class ChartPanelView : Grid
    {
        Grid chartContainer;
        BaseChart chart;
        EntitiesCollections entitiesCollections;
        ChartOptionsType optionsType;
        string chartTitle;

        public ChartPanelView(string chartTitle, ChartOptionsType optionsType, BaseChart chart, EntitiesCollections entitiesCollections)
        {

            this.entitiesCollections = entitiesCollections;
            entitiesCollections.DHOGCaseViewModel.SelectedReservoirChanged += DHOGCaseViewModel_SelectedReservoirChanged;
            entitiesCollections.DHOGCaseViewModel.SelectedCompanyChanged += DHOGCaseViewModel_SelectedCompanyChanged;
            entitiesCollections.DHOGCaseViewModel.CaseScenarioChanged += DHOGCaseViewModel_SelectedScenarioChanged;

            this.chartTitle = chartTitle;
            this.optionsType = optionsType;
            Background = new SolidColorBrush(Colors.White);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(1.2, GridUnitType.Star);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(8.8, GridUnitType.Star);
            RowDefinitions.Add(row1);
            RowDefinitions.Add(row2);

            this.chart = chart;
            chartContainer = CreateChartContainer();
            SetColumn(chartContainer, 0);
            SetRow(chartContainer, 1);
            Children.Add(chartContainer);

            Grid chartOptionsPanel = CreateChartOptionsPanel();
            SetColumn(chartOptionsPanel, 0);
            SetRow(chartOptionsPanel, 0);
            Children.Add(chartOptionsPanel);
           
        }

        private void DHOGCaseViewModel_SelectedCompanyChanged()
        {
            UpdateChart();
        }

        private void DHOGCaseViewModel_SelectedReservoirChanged()
        {
            UpdateChart();
        }

        private void DHOGCaseViewModel_SelectedScenarioChanged()
        {
            UpdateChart();
        }

        public Grid CreateChartOptionsPanel()
        {

                     
            Grid chartOptionsPanel = new Grid();

            if (!optionsType.Equals(ChartOptionsType.CandleStick))
            {

                if (optionsType.Equals(ChartOptionsType.Scenario))
                {
                    ColumnDefinition column1 = new ColumnDefinition();
                    column1.Width = new GridLength(4.5, GridUnitType.Star);
                    ColumnDefinition column2 = new ColumnDefinition();
                    column2.Width = new GridLength(4.5, GridUnitType.Star);
                    ColumnDefinition column3 = new ColumnDefinition();
                    column3.Width = new GridLength(1, GridUnitType.Star);
                    chartOptionsPanel.ColumnDefinitions.Add(column1);
                    chartOptionsPanel.ColumnDefinitions.Add(column2);
                    chartOptionsPanel.ColumnDefinitions.Add(column3);
                }
                else
                {
                    ColumnDefinition column1 = new ColumnDefinition();
                    column1.Width = new GridLength(2.5, GridUnitType.Star);
                    ColumnDefinition column2 = new ColumnDefinition();
                    column2.Width = new GridLength(2, GridUnitType.Star);
                    ColumnDefinition column3 = new ColumnDefinition();
                    column3.Width = new GridLength(1, GridUnitType.Star);
                    ColumnDefinition column4 = new ColumnDefinition();
                    column4.Width = new GridLength(2, GridUnitType.Star);
                    ColumnDefinition column5 = new ColumnDefinition();
                    column5.Width = new GridLength(2.5, GridUnitType.Star);
                    chartOptionsPanel.ColumnDefinitions.Add(column1);
                    chartOptionsPanel.ColumnDefinitions.Add(column2);
                    chartOptionsPanel.ColumnDefinitions.Add(column3);
                    chartOptionsPanel.ColumnDefinitions.Add(column4);
                    chartOptionsPanel.ColumnDefinitions.Add(column5);
                }

                TextBlock scenarioTextBlock = new TextBlock();
                scenarioTextBlock.VerticalAlignment = VerticalAlignment.Center;
                scenarioTextBlock.HorizontalAlignment = HorizontalAlignment.Right;


                SetRow(scenarioTextBlock, 0);
                SetColumn(scenarioTextBlock, 0);
                chartOptionsPanel.Children.Add(scenarioTextBlock);

                RadComboBox scenarioComboBox = new RadComboBox();

                bool b = chartTitle.Contains("COSTO MARGINAL POR ESCENARIOS");

                if (b)
                { 
                ////  if (chartTitle == "COSTO MARGINAL POR BLOQUES")
                //{
                //    //    scenarioTextBlock.Text = "BLOQUES";
                    scenarioTextBlock.Visibility = Visibility.Hidden;
                    scenarioComboBox.Visibility = Visibility.Hidden;
                }
                //else
                    scenarioTextBlock.Text = "ESCENARIO";

                Binding binding = new Binding("ActiveScenarios");
                binding.Source = entitiesCollections;
                scenarioComboBox.SetBinding(RadComboBox.ItemsSourceProperty, binding);

              
                binding = new Binding("Scenario");

                binding.Source = entitiesCollections.DHOGCaseViewModel;

                scenarioComboBox.SetBinding(RadComboBox.SelectedItemProperty, binding);
                scenarioComboBox.Width = 100;
                scenarioComboBox.Height = 20;
                scenarioComboBox.Margin = new Thickness(10, 0, 0, 0);
                scenarioComboBox.HorizontalAlignment = HorizontalAlignment.Left;
                scenarioComboBox.Focusable = true;
                SetRow(scenarioComboBox, 0);
                //MessageBox.Show("valor", scenarioComboBox.ToString());

                SetColumn(scenarioComboBox, 1);


                //MessageBox.Show("valor", scenarioComboBox.ToString());
                //scenarioComboBox.SelectedIndex = 0;
                chartOptionsPanel.Children.Add(scenarioComboBox);
             

                if (!optionsType.Equals(ChartOptionsType.Scenario))
                {
                    TextBlock optionTextBlock = new TextBlock();
                    optionTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    optionTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    switch (optionsType)
                    {
                        case ChartOptionsType.Reservoir:
                            optionTextBlock.Text = "EMBALSE";
                            break;
                        case ChartOptionsType.Company:
                            optionTextBlock.Text = "EMPRESA";
                            break;
                    }
                    SetRow(optionTextBlock, 0);
                    SetColumn(optionTextBlock, 2);
                    chartOptionsPanel.Children.Add(optionTextBlock);

                    RadComboBox optionComboBox = new RadComboBox();
                    switch (optionsType)
                    {
                        case ChartOptionsType.Reservoir:
                            binding = new Binding("ReservoirsScenario1");
                            break;
                        case ChartOptionsType.Company:
                            binding = new Binding("CompaniesScenario1");
                            break;
                    }
                    binding.Source = entitiesCollections;
                    optionComboBox.SetBinding(RadComboBox.ItemsSourceProperty, binding);

                    switch (optionsType)
                    {
                        case ChartOptionsType.Reservoir:
                            binding = new Binding("SelectedReservoir");
                            break;
                        case ChartOptionsType.Company:
                            binding = new Binding("SelectedCompany");
                            break;
                    }
                    binding.Source = entitiesCollections.DHOGCaseViewModel;
                    optionComboBox.SetBinding(RadComboBox.SelectedItemProperty, binding);

                    optionComboBox.SelectedIndex = 0;
                    optionComboBox.Width = 150;
                    optionComboBox.Height = 20;
                    optionComboBox.Margin = new Thickness(10, 0, 0, 0);
                    optionComboBox.HorizontalAlignment = HorizontalAlignment.Left;
                    optionComboBox.Focusable = false;
                    SetRow(optionComboBox, 0);
                    SetColumn(optionComboBox, 3);
                    chartOptionsPanel.Children.Add(optionComboBox);

                    //if (String.IsNullOrEmpty(scenarioComboBox.SelectedValue.ToString()))
                    //{
                    //  MessageBox.Show("Valor escenario","Valor vacio");
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Valor escenario", scenarioComboBox.SelectedValue.ToString());
                    //}

                }

            }

            RadButton saveAsImageButton = new RadButton
            {
                BorderThickness = new Thickness(0, 0, 0, 0),
                Content = new Image()
                {
                    Source = new BitmapImage(new Uri(@"Images/download_image_icon.png", UriKind.Relative)),
                    Width = 40,
                    Height = 40,
                },
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 25, 30, 0),
                ToolTip = "Guardar Imagen"
            };
            saveAsImageButton.Click += SaveAsImageButton_Click;
            SetRow(saveAsImageButton, 0);
            if (optionsType.Equals(ChartOptionsType.Scenario))
                SetColumn(saveAsImageButton, 2);
            else
                SetColumn(saveAsImageButton, 5);
            chartOptionsPanel.Children.Add(saveAsImageButton);

            return chartOptionsPanel;
        }

        public Grid CreateChartContainer()
        {
            Grid chartGrid = new Grid();
            chartGrid.Background = new SolidColorBrush(Colors.White);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(0.7, GridUnitType.Star);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(9.3, GridUnitType.Star);
            chartGrid.RowDefinitions.Add(row1);
            chartGrid.RowDefinitions.Add(row2);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(8.2, GridUnitType.Star);
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(0.2, GridUnitType.Star);
            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(1.6, GridUnitType.Star);
            chartGrid.ColumnDefinitions.Add(column1);
            chartGrid.ColumnDefinitions.Add(column2);
            chartGrid.ColumnDefinitions.Add(column3);
            chartGrid.Margin = new Thickness(10);

            TextBlock chartTitleTextBlock = new TextBlock
            {
                Text = chartTitle,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            SetRow(chartTitleTextBlock, 0);
            SetColumn(chartTitleTextBlock, 0);
            SetColumnSpan(chartTitleTextBlock, 2);
            chartGrid.Children.Add(chartTitleTextBlock);

            SetRow(chart, 1);
            SetColumn(chart, 0);
            chartGrid.Children.Add(chart);

            RadLegend legendControl = new RadLegend();
            legendControl.Items = chart.LegendItems;
            SetRow(legendControl, 1);
            SetColumn(legendControl, 2);
            chartGrid.Children.Add(legendControl);

            return chartGrid;
        }

        private void SaveAsImageButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Images (*.png)|*.png";
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = File.OpenWrite(dialog.FileName))
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(chartContainer, fileStream, new PngBitmapEncoder());
                }
            }
        }

        public void UpdateChart()
        {
            try
            {
                switch (optionsType)
                {
                    case ChartOptionsType.Scenario:
                        chart.Update(entitiesCollections.DHOGCaseViewModel.Scenario);
                        break;
                    case ChartOptionsType.Reservoir:
                        chart.Update(entitiesCollections.DHOGCaseViewModel.Scenario, entitiesCollections.DHOGCaseViewModel.SelectedReservoir);
                        break;
                    case ChartOptionsType.Company:
                        chart.Update(entitiesCollections.DHOGCaseViewModel.Scenario, entitiesCollections.DHOGCaseViewModel.SelectedCompany);
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
    }

}
