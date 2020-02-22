using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ETLAppInternal.Models.Jobs
{
    public class PlmInstructions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string _name { get; set; }
        public bool _selected { get; set; }
        public string Name { get { return _name; } set { OnPropertyChanged(); _name = value; } }

        public bool Selected { get { return _selected; } set { OnPropertyChanged(); _selected = value; } }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
