using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using HelixToolkit.Wpf;


namespace ModelViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //public partial class MainWindow : Window
    //{
    //    //public MainWindow()
    //    //{
    //    //    InitializeComponent();
    //    //}
    //}
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;


    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class MainViewModel : Observable
    {
      
        private const string OpenFileFilter = "3D model files (*.3ds;*.obj;*.lwo;*.stl)|*.3ds;*.obj;*.objz;*.lwo;*.stl";

        private const string TitleFormatString = "Medical 3D model viewer - {0}";

        private readonly IFileDialogService fileDialogService;

        private readonly IHelixViewport3D viewport;
        private readonly IHelixViewport3D viewport2;

        private readonly Dispatcher dispatcher;

        private string currentModelPath;

        private string applicationTitle;

        private double expansion;

        private Model3D currentModel;

        public ICommand FileOpenCommand { get; set; }

        public ICommand FileExportCommand { get; set; }

        public ICommand FileExitCommand { get; set; }

        public ICommand HelpAboutCommand { get; set; }

        public ICommand ViewZoomExtentsCommand { get; set; }

        public ICommand EditCopyXamlCommand { get; set; }

        public MainViewModel(IFileDialogService fds, HelixViewport3D viewport, HelixViewport3D viewport2)
        {
            if (viewport == null||viewport2==null)
            {
                throw new ArgumentNullException("Neither viewport nor viewport2 is NULL!");
            }
          
            this.dispatcher = Dispatcher.CurrentDispatcher;
            this.Expansion = 1;
            this.fileDialogService = fds;
            this.viewport = viewport;
            this.viewport2 = viewport2;
            this.FileOpenCommand = new DelegateCommand(this.FileOpen);
            this.FileExportCommand = new DelegateCommand(this.FileExport);
            this.FileExitCommand = new DelegateCommand(FileExit);
            this.ViewZoomExtentsCommand = new DelegateCommand(this.ViewZoomExtents);
            this.EditCopyXamlCommand = new DelegateCommand(this.CopyXaml);
            this.ApplicationTitle = "Medical 3D Model viewer";
            this.Elements = new List<VisualViewModel>();
            foreach (var c in viewport.Children)
            {
                this.Elements.Add(new VisualViewModel(c));
            }
            foreach (var c in viewport2.Children)
            {
                this.Elements.Add(new VisualViewModel(c));
            }
            this.Info1 = "This is a test";
        }

        public string CurrentModelPath
        {
            get
            {
                return this.currentModelPath;
            }

            set
            {
                this.currentModelPath = value;
                this.RaisePropertyChanged("CurrentModelPath");
            }
        }

        public string ApplicationTitle
        {
            get
            {
                return this.applicationTitle;
            }

            set
            {
                this.applicationTitle = value;
                this.RaisePropertyChanged("ApplicationTitle");
            }
        }

        public List<VisualViewModel> Elements { get; set; }
        public string Info1 { get; set; }
        public double Expansion
        {
            get
            {
                return this.expansion;
            }

            set
            {
                if (!this.expansion.Equals(value))
                {
                    this.expansion = value;
                    this.RaisePropertyChanged("Expansion");
                }
            }
        }

        public Model3D CurrentModel
        {
            get
            {
                return this.currentModel;
            }

            set
            {
                this.currentModel = value;
                this.RaisePropertyChanged("CurrentModel");
            }
        }

        

        private static void FileExit()
        {
            Application.Current.Shutdown();
        }

        private void FileExport()
        {
            var path = this.fileDialogService.SaveFileDialog(null, null, Exporters.Filter, ".png");
            if (path == null)
            {
                return;
            }

            this.viewport.Export(path);
        }

        private void CopyXaml()
        {
            var rd = XamlExporter.WrapInResourceDictionary(this.CurrentModel);
            Clipboard.SetText(XamlHelper.GetXaml(rd));
        }

        private void ViewZoomExtents()
        {
            this.viewport.ZoomExtents(500);
            this.viewport2.ZoomExtents(500);
            MessageBox.Show("ViewZoomExtentCMD");
        }

        private async void FileOpen()
        {
            this.CurrentModelPath = this.fileDialogService.OpenFileDialog("models", null, OpenFileFilter, ".3ds");
            this.CurrentModel = await this.LoadAsync(this.CurrentModelPath, false);
            this.ApplicationTitle = string.Format(TitleFormatString, this.CurrentModelPath);
            this.viewport.ZoomExtents(500);
            this.viewport2.ZoomExtents(500);
        }

        private async Task<Model3DGroup> LoadAsync(string model3DPath, bool freeze)
        {
            return await Task.Factory.StartNew(() =>
            {
                var mi = new ModelImporter();

                if (freeze)
                {
                    // Alt 1. - freeze the model 
                    return mi.Load(model3DPath, null, true);
                }

                // Alt. 2 - create the model on the UI dispatcher
                return mi.Load(model3DPath, this.dispatcher);
            });
        }
    }

    
}

