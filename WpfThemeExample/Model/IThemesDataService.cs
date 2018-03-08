using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfThemeExample.Model
{
    public interface IThemesDataService
    {
        void GetAllThemes(Action<ObservableCollection<Theme>, Exception> callback);
        void SetTheme(Theme theme);
    }
}
