using DHOG_WPF.Models;
using System;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.ViewModels
{
    public class PFEquationsCollectionViewModel : CollectionBaseViewModel
    {

    }

    public class PFEquationViewModel: BaseViewModel
    {
        PFEquation pfEquation;

        public PFEquationViewModel()
        {
            pfEquation = new PFEquation();
            Case = 1;
        }

        public PFEquationViewModel(PFEquation pfEquation)
        {
            this.pfEquation = pfEquation;
        }

        public PFEquation GetDataObject()
        {
            return pfEquation;
        }

        public int Id
        {
            get
            {
                return pfEquation.Id;
            }
            set
            {
                pfEquation.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return pfEquation.Name;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    pfEquation.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public double Intercept
        {
            get
            {
                return pfEquation.Intercept;
            }
            set
            {
                pfEquation.Intercept = value;
                RaisePropertyChanged("Intercept");
            }
        }

        public double LinearCoefficient
        {
            get
            {
                return pfEquation.LinearCoefficient;
            }
            set
            {
                pfEquation.LinearCoefficient = value;
                RaisePropertyChanged("LinearCoefficient");
            }
        }

        public double CuadraticCoefficient
        {
            get
            {
                return pfEquation.CuadraticCoefficient;
            }
            set
            {
                pfEquation.CuadraticCoefficient = value;
                RaisePropertyChanged("CuadraticCoefficient");
            }
        }

        public string Reservoir
        {
            get
            {
                return pfEquation.Reservoir;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    pfEquation.Reservoir = value;
                    RaisePropertyChanged("Reservoir");
                }
            }
        }

        public int Case
        {
            get
            {
                return pfEquation.Case;
            }
            set
            {
                pfEquation.Case = value;
                RaisePropertyChanged("Case");
            }
        }
    }
}
