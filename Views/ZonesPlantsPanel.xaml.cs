using DHOG_WPF.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using Telerik.Windows.Controls;
using DHOG_WPF.DataAccess;

namespace DHOG_WPF.Views
{
    /// <summary>
    /// Interaction logic for ZonesControl.xaml
    /// </summary>
    public partial class ZonesPlantsPanel : UserControl
    {
        EntitiesCollections entitiesCollections;

        public ZonesPlantsPanel(EntitiesCollections entitiesCollections)
        {
            InitializeComponent();
            this.entitiesCollections = entitiesCollections;

            Binding binding = new Binding();
            binding.Source = entitiesCollections;
            SetBinding(DataContextProperty, binding);

            entitiesCollections.PlantsCollectionScenario1.CollectionChanged += PlantsCollectionScenario1_CollectionChanged;

            CreateZonesListBoxes();
        }

        private void PlantsCollectionScenario1_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    string deletedPlant = item as string;
                    IEnumerable<ZonesListBox> currentZones = ZonesWrapPanel.ChildrenOfType<ZonesListBox>();
                    foreach (ZonesListBox zoneListBox in currentZones)
                    {
                        ZoneViewModel currentZone = zoneListBox.DataContext as ZoneViewModel;
                        if (currentZone.Plants.Contains(deletedPlant))
                            currentZone.Plants.Remove(deletedPlant);
                    }
                }
            }
        }

        private void CreateZonesListBoxes()
        {
            List<string> zoneNames = new List<string>();
            entitiesCollections.ZonesCollection.ItemEndEdit += ZonesCollection_ItemEndEdit; ;
            entitiesCollections.ZonesCollection.CollectionChanged += ZonesCollection_CollectionChanged;
            entitiesCollections.ZonesCollectionImported += EntititesCollections_ZonesCollectionImported;

            foreach (ZoneViewModel zone in entitiesCollections.ZonesCollection)
            {
                if (!zoneNames.Contains(zone.Name))
                {
                    AddZoneToPanel(zone);
                    zoneNames.Add(zone.Name);
                }
            }
        }

        private void AddZoneToPanel(ZoneViewModel zone)
        {
            UserControl zoneListBox = new ZonesListBox();
            Binding binding = new Binding();
            binding.Source = zone;
            zoneListBox.SetBinding(DataContextProperty, binding);

            ZonesWrapPanel.Children.Add(zoneListBox);
        }

        private void EntititesCollections_ZonesCollectionImported()
        {
            ZonesWrapPanel.Children.RemoveRange(0, ZonesWrapPanel.Children.Count);
            CreateZonesListBoxes();
        }

        private void ZonesCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    ZoneViewModel deletedZone = item as ZoneViewModel;
                    IEnumerable<ZonesListBox> currentZones = ZonesWrapPanel.ChildrenOfType<ZonesListBox>();
                    foreach (ZonesListBox zoneListBox in currentZones)
                    {
                        ZoneViewModel currentZone = zoneListBox.DataContext as ZoneViewModel;
                        if (currentZone.Equals(deletedZone))
                            ZonesWrapPanel.Children.Remove(zoneListBox);
                    }

                    ZonesDataAccess.DeleteAllPlantsFromZone(deletedZone.Name);
                }
            }
        }

        private void ZonesCollection_ItemEndEdit(System.ComponentModel.IEditableObject sender)
        {
            ZoneViewModel editedZone = sender as ZoneViewModel;
            IEnumerable<ZonesListBox> currentZones = ZonesWrapPanel.ChildrenOfType<ZonesListBox>();
            foreach (ZonesListBox zoneListBox in currentZones)
            {
                ZoneViewModel zone = zoneListBox.DataContext as ZoneViewModel;
                if (zone.Equals(editedZone))
                    return;
            }
            AddZoneToPanel(editedZone);
        }
    }
}
