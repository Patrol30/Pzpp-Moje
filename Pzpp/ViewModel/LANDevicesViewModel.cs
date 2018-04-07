using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pzpp
{
    class LANDevicesViewModel : INotifyPropertyChanged
    {
        #region property changed func
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        public LANDevicesViewModel()
        {
            AddCommand = new RelayCommand(Add);
            AddAllCommand = new RelayCommand(AddAll);
            //string sHostName = Dns.GetHostName();
            //IPHostEntry ipE = Dns.GetHostEntry(sHostName);
            //IPAddress[] IpA = ipE.AddressList;
            //for (int i = 0; i < IpA.Length; i++)
            //{
            //    IPHostEntry hostEntry = Dns.GetHostEntry(IpA[i].ToString());
            //    Computers.Add(new Devices()
            //    {
            //        IP = IpA[i].ToString(),
            //        Name = hostEntry.HostName

            //    });
            //}
            //foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
            //    {
            //        if (!ip.IsDnsEligible)
            //        {
            //            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //            {
            //                _Computers.Add(new Devices()
            //                {
            //                    IP = ip.ToString(),
            //                    Name = Dns.GetHostEntry(ip.ToString()).HostName

            //                });
            //            }
            //        }
            //    }
            //}

           var a = NetworkInterface.GetAllNetworkInterfaces()
    .SelectMany(adapter => adapter.GetIPProperties().UnicastAddresses)
    .Where(adr => adr.Address.AddressFamily == AddressFamily.InterNetwork && adr.IsDnsEligible)
    .Select(adr => adr.Address.ToString());
            foreach(var cos in a)
            {
                _Computers.Add(new Devices()
                {
                    IP = cos,
                    Name = Dns.GetHostEntry(cos).HostName

                });
            }
        }

        private ObservableCollection<Devices> _Computers = new ObservableCollection<Devices>(); //tabelka komputerów
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


        private int _SelectedDevice;//zaznaczony wiersz tabelki
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
        public ICommand AddCommand { get; private set; }
        private void Add()//funkcja usuwania rekordów z bazy (usuwa też powiązane kaskadowo)
        {
            using (var db = new PingDataContext())
            {
                db.Devices.Add(new Devices
                {
                    IP = _Computers[_SelectedDevice].IP,
                    Name = _Computers[_SelectedDevice].Name
                });
                db.SaveChanges();
                
            }
        }

        public ICommand AddAllCommand { get; private set; }
        private void AddAll()//funkcja usuwania rekordów z bazy (usuwa też powiązane kaskadowo)
        {
            using (var db = new PingDataContext())
            {
                for (int i = 0; i < _Computers.Count; i++)
                {
                    db.Devices.Add(new Devices
                    {
                        IP = _Computers[i].IP,
                        Name = _Computers[i].Name
                    });
                }
                db.SaveChanges();
            }
        }


    }
}

