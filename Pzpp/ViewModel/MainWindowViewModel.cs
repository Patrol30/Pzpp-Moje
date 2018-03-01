using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Pzpp
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region property changed func
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion



        public MainWindowViewModel()
        {
            AddDeviceCommand = new RelayCommand(AddDevice);
            PingCommand = new RelayCommand(PingDevice);
            CanPing = false;
            CanAdd = false;
            using (var db = new PingDataContext())
            {
                var ipAll = db.Devices.ToList();
                foreach (var item in ipAll)
                {
                    Devices.Add(item);
                    DevicesIP.Add(item.IP);
                }

            }
        }
        #region properties

        #region pinging properties
        private ObservableCollection<string> _DevicesIP = new ObservableCollection<string>();
        public ObservableCollection<string> DevicesIP
        {
            get
            {
                return _DevicesIP;
            }
            set
            {
                _DevicesIP = value;
                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(DevicesIP)));  alternatywne by nie rozpisywać się tak za każdym razem jest funkcja OnPropertyChanged (użyta poniżej)
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
                CanPing = true;
                _SelectedDevice = value;
                OnPropertyChanged();
            }
        }


        private bool _CanPing;
        public bool CanPing
        {
            get
            {
                return _CanPing;
            }
            set
            {
                if (_CanPing == value)
                    return;
                _CanPing = value;
                OnPropertyChanged();
            }
        }

        private string _PingStatusColor;
        public string PingStatusColor
        {
            get
            {
                return _PingStatusColor;
            }
            set
            {
                if (_PingStatusColor == value)
                    return;
                _PingStatusColor = value;
                OnPropertyChanged();
            }
        }

        private string _PingStatusText;
        public string PingStatusText
        {
            get
            {
                return _PingStatusText;
            }
            set
            {
                if (_PingStatusText == value)
                    return;
                _PingStatusText = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Adding Properties
        private bool _CanAdd;
        public bool CanAdd
        {
            get
            {
                return _CanAdd;
            }
            set
            {
                if (_CanAdd == value)
                    return;
                _CanAdd = value;
                OnPropertyChanged();
            }
        }

        private string _IPAddress;
        public string IPAddress
        {
            get
            {
                return _IPAddress;
            }

            set
            {
                if (_IPAddress == value)
                    return;
                bool correct;
                string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
                Regex check = new Regex(Pattern);

                if (string.IsNullOrEmpty(value))
                    correct= false;
                else
                    correct =  check.IsMatch(value);
                if (correct)
                {
                    if (!_DevicesIP.Contains(value))
                    {
                        CanAdd = true;
                    }
                    else CanAdd = false;

                }
                else CanAdd = false;
                _IPAddress = value;
                OnPropertyChanged();

            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (Description == value)
                    return;
                _Description = value;
                OnPropertyChanged();
            }
        }




        #endregion

        #region tables properties
        private ObservableCollection<Responses> _PingResponds;
        public ObservableCollection<Responses> PingResponds
        {
            get
            {
                return _PingResponds;
            }

            set
            {
                if (_PingResponds == value)
                    return;
                _PingResponds = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Devices> _Devices;
        public ObservableCollection<Devices> Devices
        {
            get
            {
                return _Devices;
            }

            set
            {
                if (_Devices == value)
                    return;
                _Devices = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion
        #region Commands

        public ICommand PingCommand { get; private set; }
        private void PingDevice()
        {           
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(DevicesIP[SelectedDevice]);
                using (var db = new PingDataContext())
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        PingStatusColor = "Green";
                        PingStatusText = "TRUE";
                        db.Responses.Add(new Responses()
                        {
                            ID = System.Guid.NewGuid(),
                            Device = Devices[SelectedDevice],
                            Success = true,
                            Time = reply.RoundtripTime
                        });
                    }
                    else
                    {
                        PingStatusColor = "Red";
                        PingStatusText = "False";
                        db.Responses.Add(new Responses()
                        {
                            ID = System.Guid.NewGuid(),
                            Device = Devices[SelectedDevice],
                            Success = false,
                            
                        });
                    }
                    var responses = db.Responses.ToList();

                    foreach (var item in responses)                    
                        PingResponds.Add(item);                        
                    
                }
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }  
                      
        }

        public ICommand AddDeviceCommand { get; private set; }

        private void AddDevice()
        {
            using (var db = new PingDataContext())
            {
                db.Devices.Add(new Devices()
                {
                    IP = _IPAddress,
                    Description = _Description
                });
                
                var ipAll = db.Devices.ToList();
                
                foreach (var item in ipAll)
                {
                    Devices.Add(item);
                    DevicesIP.Add(item.IP);
                }
            }
        }


        #endregion


    }
}
