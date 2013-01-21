using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;

namespace PhotoBeamerClone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string _guid;
        private PhotoChooserTask _photoChooserTask;
        private IHubProxy _mainHub;
        private HubConnection _connection;
        private readonly DispatcherTimer _timer;
        private PhotoCameraLuminanceSource _luminance;
        private QRCodeReader _reader;
        private PhotoCamera _photoCamera;
        private Stream _imgStream;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _photoCamera = new PhotoCamera();
            _photoCamera.Initialized += OnPhotoCameraInitialized;
            _previewVideo.SetSource(_photoCamera);
            CameraButtons.ShutterKeyHalfPressed += (sender, args) => _photoCamera.Focus();

            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.Completed += photoChooserTask_Completed;
            _photoChooserTask.Show();

            _timer = new DispatcherTimer
                         {
                             Interval = TimeSpan.FromMilliseconds(250)
                         };

            _timer.Tick += (o, arg) => ScanPreviewBuffer();
        }

        private async Task SetupSignalRConnection()
        {
            _connection = new HubConnection("http://pbclone.azurewebsites.net/");
            _connection.StateChanged += ConnectionOnStateChanged;
            _mainHub = _connection.CreateHubProxy("imghub");

            await _connection.Start();

            _mainHub.Invoke("Create", _guid);
        }

        private void ConnectionOnStateChanged(StateChange stateChange)
        {
            switch (stateChange.NewState)
            {
                case ConnectionState.Connecting:
                    StatusText.Text = "Connecting...";
                    break;
                case ConnectionState.Connected:
                    Dispatcher.BeginInvoke(() => StatusText.Text = "Connected");
                    break;
                case ConnectionState.Reconnecting:
                    Dispatcher.BeginInvoke(() => StatusText.Text = "Reconnecting...");
                    break;
                case ConnectionState.Disconnected:
                    Dispatcher.BeginInvoke(() => StatusText.Text = "Disconnected");
                    break;
            }
        }

        private void OnPhotoCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            int width = Convert.ToInt32(_photoCamera.PreviewResolution.Width);
            int height = Convert.ToInt32(_photoCamera.PreviewResolution.Height);
            _photoCamera.FlashMode = FlashMode.Off;

            _luminance = new PhotoCameraLuminanceSource(width, height);
            _reader = new QRCodeReader();

            Dispatcher.BeginInvoke(() =>
            {
                _previewTransform.Rotation = _photoCamera.Orientation;
            });
        }

        private void ScanPreviewBuffer()
        {
            if (_guid != null)
            {
                _timer.Stop();

                SendImage();
            }

            try
            {
                _photoCamera.GetPreviewBufferY(_luminance.PreviewBufferY);
                var binarizer = new HybridBinarizer(_luminance);
                var binBitmap = new BinaryBitmap(binarizer);
                var result = _reader.decode(binBitmap);

                Dispatcher.BeginInvoke(() =>
                                           {
                                               _guid = result.Text;
                                           });
            }
            catch
            {

            }
        }

        private async void SendImage()
        {
            if (_connection == null || _connection.State != ConnectionState.Connected)
            {
                await SetupSignalRConnection();
            }

            if (_connection.State == ConnectionState.Connected || _connection.State == ConnectionState.Reconnecting)
            {
                MemoryStream s = new MemoryStream();
                _imgStream.CopyTo(s);
                _mainHub.Invoke("ShareImage", new object[]{s.ToArray(), _guid}); 
            }
            else
            {
                MessageBox.Show("not connected");
            }
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                _imgStream = e.ChosenPhoto;

                _timer.Start();
            }
        }
    }
}