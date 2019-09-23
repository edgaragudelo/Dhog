using DHOG_WPF.DataAccess;
using DHOG_WPF.Models;
using System;
using System.Collections.Generic;

namespace DHOG_WPF.ViewModels
{
    public class RutasDhogParametersCollectionViewModel : CollectionBaseViewModel
    {
        public RutasDhogParametersCollectionViewModel()
        {
            List<RutasDhogParameter> dataObjects = RutasDhogParametersDataAccess.GetObjects();
            foreach (RutasDhogParameter dataObject in dataObjects)
                Add(new RutasDhogParameterViewModel(dataObject));
        }

    }

    public class RutasDhogParameterViewModel : BaseViewModel
    {
        RutasDhogParameter RutasDhogParameter;

        public RutasDhogParameterViewModel(RutasDhogParameter RutasDhogParameter)
        {
            this.RutasDhogParameter = RutasDhogParameter;
        }

        public RutasDhogParameter GetDataObject()
        {
            return RutasDhogParameter;
        }

        public int ID
        {
            get
            {
                return RutasDhogParameter.Id;
            }
            set
            {
                RutasDhogParameter.Id = value;
                RutasDhogParametersDataAccess.UpdateObject(RutasDhogParameter);
                //RutasDhogParametersDataAccess.UpdateObject()

                RaisePropertyChanged("Id");
            }
        }

        public string RutaModelo
        {
            get
            {
                return RutasDhogParameter.RutaModelo;
            }
            set
            {
                    if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    RutasDhogParameter.RutaModelo = value;
                    RutasDhogParametersDataAccess.UpdateObject(RutasDhogParameter);
                    RaisePropertyChanged("RutaModelo");
                }
            }
        }

        public string RutaEjecutable
            {
            get
            {
                return RutasDhogParameter.RutaEjecutable;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    RutasDhogParameter.RutaEjecutable = value;
                    RutasDhogParametersDataAccess.UpdateObject(RutasDhogParameter);
                    RaisePropertyChanged("RutaEjecutable");
                }
            }
        }

        public string RutaBD
        {
            get
            {
                return RutasDhogParameter.RutaBD;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    RutasDhogParameter.RutaBD = value;
                    RutasDhogParametersDataAccess.UpdateObject(RutasDhogParameter);
                    RaisePropertyChanged("RutaBD");
                }
            }
        }

        public string RutaSalida
        {
            get
            {
                return RutasDhogParameter.RutaSalida;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    RutasDhogParameter.RutaSalida = value;
                    RutasDhogParametersDataAccess.UpdateObject(RutasDhogParameter);
                    RaisePropertyChanged("RutaSalida");
                }
            }

        }

        public string RutaSolver
        {
            get
            {
                return RutasDhogParameter.RutaSolver;
            }
            set
            {
                if (value == null || value.Equals(""))
                    throw new ArgumentException("No puede estar vacío");
                else
                {
                    RutasDhogParameter.RutaSolver = value;
                    RutasDhogParametersDataAccess.UpdateObject(RutasDhogParameter);
                    RaisePropertyChanged("RutaSolver");
                }
            }
        }

       

       
    }
}
