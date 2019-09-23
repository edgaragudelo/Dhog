using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Controls;
using System;
using System.Collections.ObjectModel;

namespace DHOG_WPF.Views
{
    public class ZonesListBoxDragDropBehavior : ListBoxDragDropBehavior
    {
        public override void Drop(DragDropState state)
        {
            ObservableCollection<string> zonePlants = state.DestinationItemsSource as ObservableCollection<string>;
            List<string> newPlants = state.DraggedItems.OfType<string>().ToList();

            List<string> existingPlants = new List<string>();

            foreach (string newPlant in newPlants)
            {
                if (newPlant != null)
                {
                    foreach (string zonePlant in zonePlants)
                    {
                        if (zonePlant.Equals(newPlant))
                        {
                            existingPlants.Add(zonePlant);
                            break;
                        }
                    }
                }
            }

            if (newPlants.Count > 1 && existingPlants.Any())
            {
                string existingPlantsNames = "";
                foreach (string plantName in existingPlants)
                    existingPlantsNames += plantName + Environment.NewLine;
                
                RadWindow.Alert(new DialogParameters
                {
                    Content = "No se copiaron los recursos, dado que los siguientes recursos " + Environment.NewLine +
                               "ya exitían en la zona: " + Environment.NewLine +
                               existingPlantsNames
                });

            } 
            else if(!existingPlants.Any())
                base.Drop(state);
        }
    }
}
