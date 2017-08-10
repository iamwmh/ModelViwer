using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Windows.Ink;
using Leap;

namespace ModelViewer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    
    public partial class Window1 : Window, ILeapEventDelegate
    {
        //leap vars
        private Controller controller = new Controller();
        private LeapEventListener listener;
        private Boolean isClosing = false;
        public string Info { get; set; }
       

        public double zoomOffset;
        public double zoomOutCounter = 0;
        public double zoomInCounter = 0;
        public Window1()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(new FileDialogService(), view1,view2);
            zoomOffset = 1.0;
            //Leap init
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        delegate void LeapEventDelegate(string EventName);

        public void LeapEventNotification(string EventName)
        {
            if (this.CheckAccess())
            {
                switch (EventName)
                {
                    case "onInit":
                        Debug.WriteLine("Init");
                        break;
                    case "onConnect":
                        this.connectHandler();
                        break;
                    case "onFrame":
                        if (!this.isClosing)
                            this.newFrameHandler(this.controller.Frame());
                        break;
                }
            }
            else
            {
                Dispatcher.Invoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }

        void connectHandler()
        {
            this.controller.SetPolicy(Controller.PolicyFlag.POLICY_IMAGES);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            //this.controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            this.controller.Config.SetFloat("Gesture.Swipe.MinLength", 100.0f);
        }

        void newFrameHandler(Leap.Frame frame)
        {
            //this.displayID.Content = frame.Id.ToString();
            //this.displayTimestamp.Content = frame.Timestamp.ToString();
            //this.displayFPS.Content = frame.CurrentFramesPerSecond.ToString();
            //this.displayIsValid.Content = frame.IsValid.ToString();
            //this.displayGestureCount.Content = frame.Gestures().Count.ToString();
            //this.displayImageCount.Content = frame.Images.Count.ToString();
            //Parse the gestures here
            for (int i = 0; i < frame.Gestures().Count; i++)
            {
                Gesture gesture = frame.Gestures()[i];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPE_CIRCLE:

                        CircleGesture circle = new CircleGesture(gesture);

                        // Calculate clock direction using the angle between circle normal and pointable
                        String clockwiseness;
                        if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 2)
                        {
                            //Clockwise if angle is less than 90 degrees
                            clockwiseness = "clockwise";
                            this.Info = "gesture circle-"+clockwiseness;
                            //MessageBox.Show(Info);
                            //View1 and View2 zoom out
                            zoomInCounter = zoomInCounter + 1.0;
                           
                            view1.IsZoomEnabled = true;
                            view2.IsZoomEnabled = true;
                            if (zoomInCounter > 0)
                            {
                                view1.ZoomExtents(new Rect3D(0, 0, 0, zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1));
                                view2.ZoomExtents(new Rect3D(0, 0, 0, zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1));
                            }
                            else
                            {
                                zoomInCounter = 0;
                            }                                  
                        }
                        else
                        {
                            clockwiseness = "counterclockwise";
                            this.Info = "gesture circle-"+clockwiseness;
                            zoomInCounter = zoomInCounter -1.0;
                            if (zoomInCounter > 0)
                            {
                                view1.ZoomExtents(new Rect3D(0, 0, 0,zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1));
                                view2.ZoomExtents(new Rect3D(0, 0, 0,zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1, zoomInCounter * zoomOffset+1));
                            }

        }

                        float sweptAngle = 0;

                        // Calculate angle swept since last frame
                        if (circle.State != Gesture.GestureState.STATE_START)
                        {
                            CircleGesture previousUpdate = new CircleGesture(controller.Frame(1).Gesture(circle.Id));
                            sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                        }

                        break;
                    case Gesture.GestureType.TYPE_SWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        this.Info = "gesture swipe" + swipe.Pointable.ToString() + swipe.Direction.ToString();
                      
                        //Enable the Rotation of view1 and view2
                        view1.IsRotationEnabled = true;
                        view1.RotationSensitivity = 5.0;
                        view2.IsRotationEnabled = true;
                        view2.RotationSensitivity = 5.0;
                        //Set the camera rotation mode here
                        //view1.CameraRotationMode = HelixToolkit.Wpf.CameraRotationMode.Trackball;
                        //Get teh original camera position vector 3D 
                        Leap.Vector startPosition = swipe.StartPosition;
                        Leap.Vector currentPosition = swipe.Position;

                        //Swipe Down direction
                        if ((currentPosition.y-startPosition.y)>0.0)
                        {
                            //Rotate view1
                            ModelVisual3D device3D_0 = root1;
                            Vector3D axis_0 = new Vector3D(0, 1, 0);
                            var angle_0 = 10;                          
                            var matrix_0 = device3D_0.Transform.Value;
                            matrix_0.Rotate(new Quaternion(axis_0, angle_0));                           
                            device3D_0.Transform = new MatrixTransform3D(matrix_0);

                            //Rotate view2
                            device3D_0 = root2;
                            matrix_0 = device3D_0.Transform.Value;
                            matrix_0.Rotate(new Quaternion(axis_0, angle_0));
                            device3D_0.Transform = new MatrixTransform3D(matrix_0);
                        }

                        //Swipe Up direction
                        if ((currentPosition.y - startPosition.y) < 0.0)
                        {
                            ModelVisual3D device3D_1 = root1;
                            var axis_1 = new Vector3D(0, -1, 0);
                            var angle_1 = -10;                       
                            var matrix_1 = device3D_1.Transform.Value;
                            matrix_1.Rotate(new Quaternion(axis_1, angle_1));
                            device3D_1.Transform = new MatrixTransform3D(matrix_1);

                            //Rotate view2
                            device3D_1 = root2;
                            matrix_1 = device3D_1.Transform.Value;
                            matrix_1.Rotate(new Quaternion(axis_1, angle_1));
                            device3D_1.Transform = new MatrixTransform3D(matrix_1);
                        }

                        //swipe Right direction
                        if((currentPosition.x - startPosition.x) > 0.0)
                        {
                            ModelVisual3D device3D_2 = root1;
                            var axis_2 = new Vector3D(1, 0, 0);
                            var angle_2 = 10;
                            var matrix_2 = device3D_2.Transform.Value;
                            matrix_2.Rotate(new Quaternion(axis_2, angle_2));
                            device3D_2.Transform = new MatrixTransform3D(matrix_2);

                            //Rotate view2
                            device3D_2 = root2;
                            matrix_2 = device3D_2.Transform.Value;
                            matrix_2.Rotate(new Quaternion(axis_2, angle_2));
                            device3D_2.Transform = new MatrixTransform3D(matrix_2);
                        }

                        //Swipe left direction
                        if((currentPosition.x - startPosition.x) < 0.0)
                        {
                            ModelVisual3D device3D_3 = root1;
                            var axis_3 = new Vector3D(-1, 0, 0);
                            var angle_3 = -10;
                            var matrix_3 = device3D_3.Transform.Value;
                            matrix_3.Rotate(new Quaternion(axis_3, angle_3));
                            device3D_3.Transform = new MatrixTransform3D(matrix_3);

                            //Rotate view2
                            device3D_3 = root2;
                            matrix_3 = device3D_3.Transform.Value;
                            matrix_3.Rotate(new Quaternion(axis_3, angle_3));
                            device3D_3.Transform = new MatrixTransform3D(matrix_3);
                        }
                        break;
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        this.Info = "gesture key tape" + keytap.Position.ToString() + keytap.Direction.ToString();
                       
                        break;
                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        this.Info = "gesture screen tap" + screentap.Position.ToString() + screentap.Direction.ToString();

                        //Translate the objects to where finger screen taps
                        ModelVisual3D device3D = root1;
                        //var axis = new Vector3D(-1, 0, 0);
                        //var angle = -10;
                        var matrix = device3D.Transform.Value;
                        //matrix.Rotate(new Quaternion(axis, angle));
                        matrix.Transform(new Point3D(screentap.Position.x, screentap.Position.y, screentap.Position.z));
                        device3D.Transform = new MatrixTransform3D(matrix);

                        //Rotate view2
                        device3D = root2;
                        matrix = device3D.Transform.Value;
                        //matrix.Rotate(new Quaternion(axis, angle));
                        matrix.Transform(new Point3D(screentap.Position.x, screentap.Position.y, screentap.Position.z));
                        device3D.Transform = new MatrixTransform3D(matrix);
                        break;
                    default:
                        this.Info = "Unknown gesture!";
                        break;
                }
            }
        }

        void MainWindow_Closing(object sender, EventArgs e)
        {
            this.isClosing = true;
            this.controller.RemoveListener(this.listener);
            this.controller.Dispose();
        }


        private void view1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            view2.ZoomExtents(500);
            MessageBox.Show("view2");
        }

        private void view2_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MessageBox.Show("view2");
        }
    }

    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            this.eventDelegate = delegateObject;
        }
        public override void OnInit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onInit");
        }
        public override void OnConnect(Controller controller)
        {
            controller.SetPolicy(Controller.PolicyFlag.POLICY_IMAGES);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.eventDelegate.LeapEventNotification("onConnect");
        }

        public override void OnFrame(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onFrame");
        }
        public override void OnExit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onExit");
        }
        public override void OnDisconnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDisconnect");
        }

    }
}
