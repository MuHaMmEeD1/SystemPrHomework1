using SchoolBusWpfProje.RealyCommands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SystemPrHomework1.DTOS;

namespace SystemPrHomework1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PRS> Processes { get; set; }
        private DispatcherTimer timer;
        private DispatcherTimer timer2;

        public List<string> BlackString { get; set; }

        public PRS rS { get; set; }
        public MyRealyCommand CreatCommand { get; set; }
        public MyRealyCommand DeleteCommand { get; set; }
        public MyRealyCommand BlackCommand { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            CreatCommand = new MyRealyCommand(CreatCommandFunction, CanCreatCommandFunction);
            DeleteCommand = new MyRealyCommand(DeleteCommandFunction, CanDeleteCommandFunction);
            BlackCommand = new MyRealyCommand(BlackCommandFunction, CanDeleteCommandFunction);

            rS = new PRS();
            BlackString = new();
            this.DataContext=this;

            Processes = new ObservableCollection<PRS>();
            ListViewEsas.ItemsSource = Processes;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += PRS_DOTS; 
            timer.Start();

            timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Tick += KillList;
            timer2.Start();

        }


        void PRS_DOTS(object? sender, EventArgs e)
        {
            
            var allProcesses = Process.GetProcesses().ToList();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Processes.Clear();
                BaxLabel.Content = allProcesses.Count;
                foreach (var process in allProcesses)
                {
                    Processes.Add(new PRS()
                    {
                        Name = process.ProcessName,
                        Id = process.Id,
                        HandlenCount = process.HandleCount,
                        ThreadCount = process.Threads.Count,
                        MachineName = process.MachineName
                    });
                }
            });
        }

        void KillList(object? sender, EventArgs e)
        {
            var allProcesses = Process.GetProcesses().ToList();

            foreach (var pro in allProcesses)
            {
                foreach (var name in BlackString)
                {
                    if(name == pro.ProcessName)
                    {
                        pro.Kill();
                    }
                }
            }
        }

        public void CreatCommandFunction(object? par)
        {
            Process.Start(ComboBoxName.Text);
        }
        public bool CanCreatCommandFunction(object? par)
        {
            if(ComboBoxName.Text == "") { return false; }




            return true;
        }

        public void DeleteCommandFunction(object? par)
        {

            var process = Process.GetProcessById(rS.Id);
            process.Kill();

        }

        public bool CanDeleteCommandFunction(object? par)
        {

            if(ListViewEsas.SelectedItem is null) { return false; }

            return true;
        }

        
        public void BlackCommandFunction(object? par)
        {

            BlackString.Add(rS.Name);


        }



    }
}