using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pzpp
{
    class ComputersViewModel: INotifyPropertyChanged
    {
        #region property changed func
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        public ComputersViewModel()
        {
            using (var db = new PingDataContext())
            {
                Computers = db.Devices.ToList();
            }
        }

        private List<Devices> _Computers;
        public List<Devices> Computers
        {
            get
            {
                return _Computers;
            }
            set
            {
                if (_Computers == value)
                    return;
                _Computers = value;
                OnPropertyChanged();
            }
        }




    }    
}
