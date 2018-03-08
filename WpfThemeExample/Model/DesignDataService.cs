using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfThemeExample.Model
{
    /// <summary>
    /// Design-time data service containing data to be shown in the designer.
    /// </summary>
    public class DesignDataService : IThemesDataService
    {
        private ObservableCollection<Theme> themes_ = new ObservableCollection<Theme>();

        public DesignDataService()
        {
            themes_.Add(new Theme() { Name = "Default", Path = "Themes/DefaultTheme.xaml" });
            themes_.Add(new Theme() { Name = "Dark", Path = "Themes/DarkTheme.xaml" });
            themes_.Add(new Theme() { Name = "Summertime", Path = "Themes/SummerTimeTheme.xaml" });
        }

        public void GetAllThemes(Action<ObservableCollection<Theme>, Exception> callback)
        {
            callback(themes_, null);
        }

        public void SetTheme(Theme theme)
        {
            throw new NotImplementedException();
        }
    }
}
