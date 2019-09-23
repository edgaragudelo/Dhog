using DHOG_WPF.Models;

namespace DHOG_WPF.ViewModels
{
    public class BlocksCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class BlockViewModel : BaseViewModel
    {
        Block block;

        /* Constructor needed to add rows in the RadGridView control */
        public BlockViewModel()
        {
            block = new Block();
        }

        public BlockViewModel(Block block)
        {
            this.block = block;
        }

        public Block GetDataObject()
        {
            return block;
        }

        public int Id
        {
            get
            {
                return block.Id;
            }
            set
            {
                block.Id = value;
            }
        }

        public int Name
        {
            get
            {
                return block.Name;
            }
            set
            {
                block.Name = value;
            }
        }

        public double DurationFactor
        {
            get
            {
                return block.DurationFactor;
            }
            set
            {
                block.DurationFactor = value;
                RaisePropertyChanged("DurationFactor");
            }
        }

        public double LoadFactor
        {
            get
            {
                return block.LoadFactor;
            }
            set
            {
                block.LoadFactor = value;
                RaisePropertyChanged("LoadFactor");
            }
        }
    }
}
