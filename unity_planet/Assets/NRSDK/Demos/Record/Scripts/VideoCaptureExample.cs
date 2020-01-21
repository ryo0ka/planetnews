using NRKernal;
using NRToolkit.Record;
using System.Linq;
using UnityEngine;

namespace NRKernal.NRExamples
{
    public class VideoCaptureExample : MonoBehaviour
    {
        public NRPreviewer Previewer;

        public bool IsLive;
        public string RTMPPath
        {
            get
            {
                return @"rtmp://192.168.69.47:1935/live/movie";
            }
        }

        public string SdcardSavePath
        {
            get
            {
                string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
                string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
                string filepath = System.IO.Path.Combine(NRTools.GetSdcardPath() + "RecordVideos/", filename);
                filepath = filepath.Replace("file://", "");
                return filepath;
            }
        }

        NRVideoCapture m_VideoCapture = null;

        void Start()
        {
            CreateVideoCaptureTest();
        }

        void Update()
        {
            if (m_VideoCapture == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.R) || NRInput.GetButtonDown(ControllerButton.TRIGGER))
            {
                StartVideoCapture();

                Previewer.SetData(m_VideoCapture.RecordBehaviour.PreviewTexture, true);
            }

            if (Input.GetKeyDown(KeyCode.T) || NRInput.GetButtonDown(ControllerButton.HOME))
            {
                StopVideoCapture();

                Previewer.SetData(m_VideoCapture.RecordBehaviour.PreviewTexture, false);
            }
        }

        void CreateVideoCaptureTest()
        {
            NRVideoCapture.CreateAsync(false, delegate (NRVideoCapture videoCapture)
            {
                if (videoCapture != null)
                {
                    m_VideoCapture = videoCapture;
                }
                else
                {
                    Debug.LogError("Failed to create VideoCapture Instance!");
                }
            });
        }

        private void StartVideoCapture()
        {
            Resolution cameraResolution = NRVideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Debug.Log(cameraResolution);

            int cameraFramerate = NRVideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
            Debug.Log(cameraFramerate);

            if (m_VideoCapture != null)
            {
                Debug.Log("Created VideoCapture Instance!");
                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
                cameraParameters.blendMode = BlendMode.Blend;

                m_VideoCapture.StartVideoModeAsync(cameraParameters,
                    NRVideoCapture.AudioState.ApplicationAndMicAudio,
                    OnStartedVideoCaptureMode);
            }

        }

        private void StopVideoCapture()
        {
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
        }

        void OnStartedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Started Video Capture Mode!");
            m_VideoCapture.StartRecordingAsync(IsLive ? RTMPPath : SdcardSavePath, OnStartedRecordingVideo);
        }

        void OnStoppedVideoCaptureMode(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Stopped Video Capture Mode!");
        }

        void OnStartedRecordingVideo(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Started Recording Video!");
        }

        void OnStoppedRecordingVideo(NRVideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Stopped Recording Video!");
            m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
        }
    }
}
