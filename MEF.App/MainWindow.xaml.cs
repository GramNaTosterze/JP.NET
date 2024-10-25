using System.Collections.ObjectModel;
using MEF.Ext;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MEF.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _pluginDir =
            ConfigurationManager.AppSettings["PluginDir"] ?? ".\\Plugins"; 
        private CompositionContainer _container;
        [ImportMany(typeof(ICalculator), AllowRecomposition = true)] 
        private Lazy<ICalculator>[] Plugins { get; set; }
        
        private FileSystemWatcher _pluginWatcher;
        private DirectoryCatalog cat;
        private ObservableCollection<TabItem> _pluginTabs = [];

        public MainWindow()
        {
            InitializeComponent();
            LoadContainer();
            StartWatcher();
            LoadPlugins();
        }

        private void LoadContainer()
        {
            cat = new DirectoryCatalog(
                _pluginDir);

            _container = new CompositionContainer(cat);
            _container.ComposeParts(this);
        }

        private void StartWatcher()
        {
            _pluginWatcher = new FileSystemWatcher();
            _pluginWatcher.Path = _pluginDir;
            _pluginWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes;
            _pluginWatcher.Filter = "*.*";
            _pluginWatcher.Changed += (_, _) => Dispatcher.Invoke(UpdatePlugins);
            _pluginWatcher.Created += (_, _) => Dispatcher.Invoke(UpdatePlugins);
            _pluginWatcher.Deleted += (_, _) => Dispatcher.Invoke(UpdatePlugins);
            _pluginWatcher.EnableRaisingEvents = true;
        }

        private void LoadPlugins()
        {
            try
            {
                foreach (var plugin in Plugins)
                {
                    _pluginTabs.Add(new TabItem
                    {
                        Header = plugin.Value.Name,
                        Content = plugin.Value.GetUserControl()
                    });
                }
                PluginTabControl.DataContext = _pluginTabs;
                PluginTabControl.ItemsSource = _pluginTabs;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error loading plugins: {e}");
            }
        }

        private void UpdatePlugins()
        {
            cat.Refresh();

            _pluginTabs.Clear();
            foreach (var plugin in Plugins)
            {
                _pluginTabs.Add(new TabItem
                {
                    Header = plugin.Value.Name,
                    Content = plugin.Value.GetUserControl()
                });
            }
        }

    }
}