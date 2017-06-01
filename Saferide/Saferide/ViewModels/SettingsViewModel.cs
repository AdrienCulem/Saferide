using Saferide.Ressources;

namespace Saferide.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool _metricToggle;
        public bool MetricToggle
        {
            get => _metricToggle;
            set
            {
                if (_metricToggle == value) return;
                _metricToggle = value;
                Constants.MetricSystem = _metricToggle ? Constants.MetricSystems.Kmh : Constants.MetricSystems.Mph;
                RaisePropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            if (Constants.MetricSystem == Constants.MetricSystems.Kmh)
            {
                MetricToggle = true;
            }
        }
    }
}
