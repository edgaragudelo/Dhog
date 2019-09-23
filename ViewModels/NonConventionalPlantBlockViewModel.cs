using DHOG_WPF.Models;
using System;

namespace DHOG_WPF.ViewModels
{
    public class NonConventionalPlantBlocksCollectionViewModel: CollectionBaseViewModel
    {

    }

    public class NonConventionalPlantBlockViewModel : BaseViewModel
    {
        NonConventionalPlantBlock nonConventionalPlantBlock;

        /* Constructor needed to add rows in the RadGridView control */
        public NonConventionalPlantBlockViewModel()
        {
            nonConventionalPlantBlock = new NonConventionalPlantBlock();
            nonConventionalPlantBlock.Block = 1;
        }

        public NonConventionalPlantBlockViewModel(NonConventionalPlantBlock nonConventionalPlantBlock)
        {
            this.nonConventionalPlantBlock = nonConventionalPlantBlock;
        }

        public NonConventionalPlantBlock GetDataObject()
        {
            return nonConventionalPlantBlock;
        }

        public int Id
        {
            get
            {
                return nonConventionalPlantBlock.Id;
            }
            set
            {
                nonConventionalPlantBlock.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return nonConventionalPlantBlock.Name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    nonConventionalPlantBlock.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public int Block
        {
            get
            {
                return nonConventionalPlantBlock.Block;
            }
            set
            {
                nonConventionalPlantBlock.Block = value;
                RaisePropertyChanged("Block");
            }
        }

        public double ReductionFactor
        {
            get
            {
                return nonConventionalPlantBlock.ReductionFactor;
            }
            set
            {
                nonConventionalPlantBlock.ReductionFactor = value;
                RaisePropertyChanged("ReductionFactor");
            }
        }

        public int Case
        {
            get
            {
                return nonConventionalPlantBlock.Case;
            }
            set
            {
                nonConventionalPlantBlock.Case =  value;
                RaisePropertyChanged("Case");
            }
        }

    }
}
