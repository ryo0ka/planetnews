/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal
{
    using UnityEngine;
    using System;

    public class NRInternalUpdater : MonoBehaviour
    {
        private static NRInternalUpdater m_Instance;

        public static NRInternalUpdater Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject updateObj = new GameObject("NRInternalUpdater");
                    GameObject.DontDestroyOnLoad(updateObj);
                    m_Instance = updateObj.AddComponent<NRInternalUpdater>();
                }
                return m_Instance;
            }
        }

        public Action OnUpdate;

        private void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }
        }
    }
}
