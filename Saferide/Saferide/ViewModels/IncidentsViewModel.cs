﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Saferide.GPS;
using Saferide.Helpers;
using Saferide.Interfaces;
using Saferide.Models;
using Xamarin.Forms;

namespace Saferide.ViewModels
{
    public class IncidentsViewModel : BaseViewModel
    {
        public List<Incident> ListContent { get; set; }
        public ICommand RefreshCommand { get; set; }

        public IncidentsViewModel()
        {
            GetIncidents();
            RefreshCommand = new Command(GetIncidents);
        }

        private async void GetIncidents()
        {
            IsBusy = true;
            await GetIncident.GetIncidents();
            ListContent = Constants.NearestIncidents;
            IsBusy = false;
        }
    }
}
