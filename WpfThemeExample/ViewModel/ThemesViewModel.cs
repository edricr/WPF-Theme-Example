using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfThemeExample.Model;

namespace WpfThemeExample.ViewModel
{
    public class ThemesViewModel : ViewModelBase
    {
        private ObservableCollection<Theme> themes_;
        private Theme selectedTheme_;
        private IThemesDataService data_;

        public ThemesViewModel(IThemesDataService dataService)
        {
            data_ = dataService;

            data_.GetAllThemes((item, error) =>
            {
                themes_ = item;
            });
        }

#region Properties

        public ObservableCollection<Theme> Themes
        {
            get { return themes_; }
            set
            {
                if (themes_ != value)
                {
                    themes_ = value;
                    RaisePropertyChanged(() => Themes);
                }
            }
        }

        public Theme SelectedTheme
        {
            get { return selectedTheme_; }
            set
            {
                if (selectedTheme_ != value)
                {
                    data_.SetTheme(value);
                    selectedTheme_ = value;
                    RaisePropertyChanged(() => SelectedTheme);
                }
            }
        }

#endregion
    }
}
