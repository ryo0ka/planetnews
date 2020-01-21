using NRToolkit.Record;
using UnityEngine;
using UnityEngine.UI;
#if !UNITY_EDITOR
using System.IO;
#endif

namespace NRKernal.NRExamples
{
    public class CombineTextureTest : MonoBehaviour
    {
        public Camera RecordCamera;
        public RawImage ScreenImage;
        public Texture2D SourceTex;

        private BlendCamera BlendCamera { get; set; }

        public BlendMode BlendMode;

        private RGBTextureFrame RGBTextureFrame;

        public class TestEncoder : IEncoder
        {
            public void Commit(RenderTexture rt, ulong timestamp)
            {
                Debug.Log("Commit a frame:" + timestamp);
            }
        }

        //public NRRecordConfig RecordConfig;

        IEncoder Encoder;

        // Use this for initialization
        void Start()
        {
            RecordCamera.targetTexture = new RenderTexture(1280, 720, 24, RenderTextureFormat.ARGB32);


#if UNITY_EDITOR
            Encoder = new TestEncoder();
#else
        //var config = RecordConfig.ToNativeConfig();
        //if (!Directory.Exists(config.outPutPath))
        //{
        //    Directory.CreateDirectory(config.outPutPath);
        //}
        //Encoder = new VideoEncoder(config);
#endif

            BlendCamera = new BlendCamera(Encoder, RecordCamera, BlendMode, 1280, 720);

            RGBTextureFrame = new RGBTextureFrame();
            RGBTextureFrame.texture = SourceTex;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RGBTextureFrame.timeStamp = NRTools.GetTimeStamp();
                BlendCamera.OnFrame(RGBTextureFrame);
                ScreenImage.texture = BlendCamera.BlendTexture;
            }

            if (Input.touchCount == 2)
            {
                ((VideoEncoder)Encoder).Start();
            }

            if (Input.touchCount == 2)
            {
                ((VideoEncoder)Encoder).Stop();
            }
        }
    }
}
