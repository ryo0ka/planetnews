using NRToolkit.Record;
using System.Linq;
using UnityEngine;

namespace NRKernal.NRExamples
{
    public class PhotoCaptureExample : MonoBehaviour
    {
        NRPhotoCapture photoCaptureObject = null;
        Texture2D targetTexture = null;

        Resolution cameraResolution;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) || NRInput.GetButtonDown(ControllerButton.TRIGGER))
            {
                TakeAPhoto();
            }
        }

        // Use this for initialization
        void Start()
        {
            // Create a PhotoCapture object
            NRPhotoCapture.CreateAsync(false, delegate (NRPhotoCapture captureObject)
            {
                cameraResolution = NRPhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
                targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

                if (captureObject != null)
                {
                    photoCaptureObject = captureObject;
                }
                else
                {
                    Debug.LogError("Can not get a captureObject.");
                }
            });
        }

        void TakeAPhoto()
        {
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (NRPhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        }

        void OnCapturedPhotoToMemory(NRPhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);

            // Create a gameobject that we can apply our texture to
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
            quadRenderer.material = new Material(Shader.Find("Unlit/Texture"));

            quad.transform.parent = this.transform;
            quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

            quadRenderer.material.SetTexture("_MainTex", targetTexture);

            // Deactivate our camera
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }

        void OnStoppedPhotoMode(NRPhotoCapture.PhotoCaptureResult result)
        {
            // Shutdown our photo capture resource
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }
    }
}
