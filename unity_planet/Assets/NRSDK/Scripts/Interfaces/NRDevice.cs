/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

using UnityEngine;

namespace NRKernal
{
    /// @cond EXCLUDE_FROM_DOXYGEN
    public class NRDevice : SingleTon<NRDevice>
    {
        internal NativeHMD NativeHMD { get; set; }
        public delegate void GlassEvent(string msg);
        public GlassEvent OnGlassPutOn = (msg) => { };
        public GlassEvent OnGlassPutOff = (msg) => { };
        public GlassEvent OnGlassModeSwitch = (msg) => { };
        public GlassEvent OnGlassConnect = (msg) => { };
        public GlassEvent OnGlassDisConnect = (msg) => { };
        public GlassEvent OnGlassIDResponse = (msg) => { };

        public const int TYPE_EVENT = 1000;
        public const int TYPE_CONNECT = 1001;
        public const int TYPE_DISCONNECT = 1002;
        public const int TYPE_MDOE = 1003;
        public const int TYPE_GLASSES_ID = 1004;

        internal AndroidJavaObject m_GlassService;
        internal AndroidJavaObject m_UnityActivity;

        public NRDevice()
        {
            NativeHMD = new NativeHMD();
            NativeHMD.Create();

            var listener = new GlassesCmdProxy(this);
            AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            m_UnityActivity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            m_GlassService = new AndroidJavaObject("com.nreal.tools.Util", m_UnityActivity, listener);
        }

        public void RequestGlassID()
        {
            m_GlassService.Call("getGlassesId");
        }

        public void OprateGlassResponse(int eventid, string msg)
        {
            if (eventid == TYPE_EVENT)
            {
                if (msg.Equals("Pnear"))
                {
                    OnGlassPutOn(msg);
                }
                else if (msg.Equals("Paway"))
                {
                    OnGlassPutOff(msg);
                }
            }
            else if (eventid == TYPE_CONNECT)
            {
                OnGlassConnect(null);
            }
            else if (eventid == TYPE_DISCONNECT)
            {
                OnGlassDisConnect(null);
            }
            else if (eventid == TYPE_MDOE)
            {
                OnGlassModeSwitch(msg);
            }
            else if (eventid == TYPE_GLASSES_ID)
            {
                OnGlassIDResponse(msg);
            }
        }

        public void Destroy()
        {
            if (NativeHMD != null)
            {
                NativeHMD.Destroy();
                NativeHMD = null;
            }
        }
    }

    public class GlassesCmdProxy : AndroidJavaProxy
    {
        private NRDevice m_GlassEventListener;
        public GlassesCmdProxy(NRDevice manager) : base("com.nreal.tools.GlassesCmdCallback")
        {
            m_GlassEventListener = manager;
        }

        public void onReceive(int type, string value)
        {
            //Debug.LogFormat("onReceive type:{0} value:{1}", type, value);
            m_GlassEventListener.OprateGlassResponse(type, value);
        }
    }
    /// @endcond
}
