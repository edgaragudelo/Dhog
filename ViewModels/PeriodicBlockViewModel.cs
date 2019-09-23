using DHOG_WPF.Models;
using System;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class PeriodicBlocksCollectionViewModel : PeriodicEntityCollectionViewModel
    {

    }

    public class PeriodicBlockViewModel: PeriodicEntityViewModel
    {
        PeriodicBlock periodicBlock;

        public PeriodicBlockViewModel(PeriodicBlock periodicBlock) : base(periodicBlock)
        {
            this.periodicBlock = periodicBlock;
        }

        public PeriodicBlock GetDataObject()
        {
            return periodicBlock;
        }

        public int Block
        {
            get
            {
                return periodicBlock.Block;
            }
            set
            {
                periodicBlock.Block = value;
                RaisePropertyChanged("Block");
            }
        }

        public double DurationFactor
        {
            get
            {
                return periodicBlock.DurationFactor;
            }
            set
            {
                periodicBlock.DurationFactor = value;
                RaisePropertyChanged("DurationFactor");
            }
        }

        public double LoadFactor
        {
            get
            {
                return periodicBlock.LoadFactor;
            }
            set
            {
                periodicBlock.LoadFactor = value;
                RaisePropertyChanged("LoadFactor");
            }
        }
    }
}
