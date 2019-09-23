using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.DragDrop.Behaviors;

namespace DHOG_WPF.Views
{
    public class PlantsListBoxDragDropBehavior : ListBoxDragDropBehavior
    {
        protected override bool IsMovingItems(DragDropState state)
        {
            return false;
        }

        public override void Drop(DragDropState state)
        {
            
        }
    }
}
