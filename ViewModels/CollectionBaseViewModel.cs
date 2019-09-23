using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.ViewModels
{
    public class CollectionBaseViewModel : ObservableCollection<BaseViewModel>
    {
        
        protected override void InsertItem(int index, BaseViewModel item)
        {
            base.InsertItem(index, item);

            // handle any EndEdit events relating to this item
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }

        void ItemEndEditHandler(IEditableObject sender)
        {
            // simply forward any EndEdit events
            ItemEndEdit?.Invoke(sender);
        }
        
        public event ItemEndEditEventHandler ItemEndEdit;
        
    }

    public delegate void ItemEndEditEventHandler(IEditableObject sender);
}
