using DHOG_WPF.Models;
using System;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicLoadBlocksCollectionViewModel : PeriodicEntityCollectionViewModel
    {

    }

    public class PeriodicLoadBlockViewModel: PeriodicEntityViewModel
    {
        PeriodicLoadBlock periodicLoadBlock;

        public PeriodicLoadBlockViewModel(PeriodicLoadBlock periodicLoadBlock) : base(periodicLoadBlock)
        {
            this.periodicLoadBlock = periodicLoadBlock;
        }

        public PeriodicLoadBlock GetDataObject()
        {
            return periodicLoadBlock;
        }

        public int Block
        {
            get
            {
                return periodicLoadBlock.Block;
            }
            set
            {
                periodicLoadBlock.Block = value;
                RaisePropertyChanged("Block");
            }
        }

        public double Load
        {
            get
            {
                return periodicLoadBlock.Load;
            }
            set
            {
                periodicLoadBlock.Load = value;
                RaisePropertyChanged("Load");
            }
        }
    }
}
