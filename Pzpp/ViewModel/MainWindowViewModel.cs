using System;
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
            OpenComputersTableCommand = new RelayCommand(OpenComputersTable);
            OpenResponsesTableCommand = new RelayCommand(OpenResponsesTable);
            AddDeviceCommand = new RelayCommand(AddDevice);
            PingCommand = new RelayCommand(PingDevice);            
            CanPing = false;
            CanAdd = false;
            using (var db = new PingDataContext())
            {
                db.Database.CreateIfNotExists();
                var devicesAll = db.Devices.ToList();
                foreach (var item in devicesAll)
                {
                    DevicesIP.Add(item.IP);
                    IdControl.Add(item.Id);
                    DevicesList.Add(item.Name+" "+item.IP);
                }

            }
            
        }
        #region properties

        #region pinging properties
        private ObservableCollection<string> _DevicesList = new ObservableCollection<string>();
        public ObservableCollection<string> DevicesList
        {
            get
            {
                return _DevicesList;
            }
            set
            {
                _DevicesList = value;
                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(DevicesIP)));  alternatywne by nie rozpisywać się tak za każdym razem jest funkcja OnPropertyChanged (użyta poniżej)
                OnPropertyChanged();
            }

        }
        private ObservableCollection<string> DevicesIP = new ObservableCollection<string>();

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
                if (value != -1)
                    CanPing = true;
                else
                    CanPing = false;
                _SelectedDevice = value;
                OnPropertyChanged();
            }
        }
        private List<int> IdControl = new List<int>();
        

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

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                IPAddress = _IPAddress;
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
               
                bool correct;
                string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
                Regex check = new Regex(Pattern);

                if (string.IsNullOrEmpty(value))
                    correct= false;
                else
                    correct =  check.IsMatch(value);
                if (correct)
                {

                    if (_Name == null||_Name=="")
                        CanAdd = false;
                    
                    else CanAdd = true;

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


        #endregion
        #region Commands

        public ICommand PingCommand { get; private set; }
        private void PingDevice()
        {           
            Ping pinger = new Ping();
            try
            {
                if (_SelectedDevice != -1)
                {
                    var id = IdControl[_SelectedDevice];
                    PingReply reply = pinger.Send(DevicesIP[SelectedDevice]);
                    using (var db = new PingDataContext())
                    {
                        if (reply.Status == IPStatus.Success)
                        {
                            PingStatusColor = "Green";
                            PingStatusText = "TRUE";
                            db.Responses.Add(new Responses()
                            {
                                Device_Id = id,
                                Success = true,
                                PingTime = reply.RoundtripTime,
                                Time = DateTime.Now                                
                            });
                        }
                        else
                        {
                            PingStatusColor = "Red";
                            PingStatusText = "False";
                            db.Responses.Add(new Responses()
                            {

                                Device_Id = id,
                                Success = false,
                                Time = DateTime.Now


                            });
                        }
                        db.SaveChanges();
                    }
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
                    Name = _Name,
                    Description = _Description
                });
                
                db.SaveChanges();
                DevicesList.Add(_Name + " " + _IPAddress);
                DevicesIP.Add(_IPAddress);
                IdControl.Add(db.Devices.ToList().Last().Id);
                Name = "";
                IPAddress = "";
                Description = "";
            }
        }

        public ICommand OpenComputersTableCommand { get;private set; }
        private void OpenComputersTable()
        {            
            Computers computers = new Computers();
            computers.ShowDialog();
            using (var db = new PingDataContext())
            {
                DevicesIP.Clear();
                IdControl.Clear();
                DevicesList.Clear();             
                var devicesAll = db.Devices.ToList();
                foreach (var item in devicesAll)
                {
                    DevicesIP.Add(item.IP);
                    IdControl.Add(item.Id);
                    DevicesList.Add(item.Name + " " + item.IP);
                }

            }
        }
        public ICommand OpenResponsesTableCommand { get; private set; }
        private void OpenResponsesTable()
        {            
            ResponsesTable responses = new ResponsesTable();
            responses.ShowDialog();
        }
        #endregion


    }
}
