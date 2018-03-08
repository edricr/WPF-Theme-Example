using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfThemeExample.Model
{
    public class Theme : ObservableObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
