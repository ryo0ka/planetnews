using NRKernal;
using UnityEngine;

public class GlassEventTest : MonoBehaviour
{
    void Start()
    {
        NRDevice.Instance.OnGlassPutOn += OnGlassPutOn;
        NRDevice.Instance.OnGlassPutOff += OnGlassPutOff;
        NRDevice.Instance.OnGlassIDResponse += OnGlassIDResponse;
    }

    private void Update()
    {
        if (NRInput.GetButtonDown(ControllerButton.TRIGGER))
        {
            NRDevice.Instance.RequestGlassID();
        }
    }

    private void OnGlassIDResponse(string msg)
    {
        Debug.LogError("..............OnGlassIDResponse............... :" + msg);
    }

    private void OnGlassPutOff(string msg)
    {
        Debug.LogError("..............OnGlassPutOff...............");
    }

    private void OnGlassPutOn(string msg)
    {
        Debug.LogError("..............OnGlassPutOn...............");
    }
}
