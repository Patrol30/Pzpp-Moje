using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            DeleteCommand = new RelayCommand(Delete);
            using (var db = new PingDataContext())
            {
                Computers = new ObservableCollection<Devices>(db.Devices.ToList());
            }
        }

        private ObservableCollection<Devices> _Computers = new ObservableCollection<Devices>();
        public ObservableCollection<Devices> Computers
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


        private int _SelectedDevice;
        public int SelectedDevice
        {
            get
            {
                return _SelectedDevice;
            }
            set
            {
                if (_SelectedDevice == value)
                    return;
                _SelectedDevice = value;
                OnPropertyChanged();
            }
        }
        public ICommand DeleteCommand { get; private set; }
        private void Delete()
        {
            using (var db = new PingDataContext())
            {
                Devices device = new Devices() { Id = _Computers[_SelectedDevice].Id };               
                db.Devices.Attach(device);
                db.Devices.Remove(device);
                db.SaveChanges();
                Computers = new ObservableCollection<Devices>(db.Devices.ToList());
            }
        }




    }    
}
