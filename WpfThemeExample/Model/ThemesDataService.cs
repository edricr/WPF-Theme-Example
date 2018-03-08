using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfThemeExample.Model
{
    public class ThemesDataService : IThemesDataService
    {
        private ObservableCollection<Theme> themes_ = new ObservableCollection<Theme>();

        public ThemesDataService()
        {
            // Manually add the default theme since it's not a discoverable resource asset
            themes_.Add(new Theme() { Name = "Default", Path="Themes/DefaultTheme.xaml" });

            scanResources();
            scanDisk("Themes");
        }

        private void scanResources(string fileEnding = "Theme.xaml")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                ResourceSet set = new ResourceSet(assembly.GetManifestResourceStream(resourceName));
                foreach (DictionaryEntry item in set)
                {
                    string fileName = item.Key.ToString();
                    if (fileName.ToLower().EndsWith(fileEnding.ToLower()))
                    {
                        themes_.Add(new Theme() { Name = getNameFromPath(fileName), Path = "pack://application:,,,/WpfThemeExample;component/" + fileName });
                    }
                }
            }
        }

        private void scanDisk(string relativePath, string fileEnding = "Theme.xaml")
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + relativePath))
            {
                var themeFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + relativePath, "*Theme.xaml", SearchOption.AllDirectories);
                foreach (var fileName in themeFiles)
                {
                    themes_.Add(new Theme() { Name = getNameFromPath(fileName, '\\'), Path = fileName });
                }
            }
        }

        // Trim leading path elements and trailing file extensions
        private string getNameFromPath(string path, char pathChar = '/', string fileEnding = "Theme.xaml")
        {
            string name = path.Substring(path.LastIndexOf(pathChar) + 1);
            name = name.Substring(0, name.Length - fileEnding.Length);
            name = char.ToUpper(name[0]) + (name.Length > 1 ? name.Substring(1) : "");

            return name;
        }

#region IThemesDataService

        public void GetAllThemes(Action<ObservableCollection<Theme>, Exception> callback)
        {
            callback(themes_, null);
        }

        public void SetTheme(Theme theme)
        {
            if (theme == null)
            {
                Trace.WriteLine("Error setting theme: Attempting to set theme to null.");
                return;
            }

            try
            {
                // Theme is stored in the second entry of the merged dictionaries as listed in App.xaml
                Application.Current.Resources.MergedDictionaries[1].Source = new Uri(theme.Path, UriKind.RelativeOrAbsolute);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error setting theme: " + ex.Message);
            }
        }

        #endregion
    }
}
