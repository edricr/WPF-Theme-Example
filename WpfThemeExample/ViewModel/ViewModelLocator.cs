using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using WpfThemeExample.Model;

namespace WpfThemeExample.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IThemesDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IThemesDataService, ThemesDataService>();
            }
            
            SimpleIoc.Default.Register<ThemesViewModel>();
        }

        public ThemesViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ThemesViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}
