//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;

namespace App
{
    public class SplashForm : UGuiForm
    {
        private ProcedureSplash m_ProcedureSplash = null;

        public void OnSkipSplashClick()
        {
            m_ProcedureSplash.SkipSplash();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureSplash = (ProcedureSplash)userData;
            if (m_ProcedureSplash == null)
            {
                Log.Warning("ProcedureSplash is invalid when open SplashForm.");
                return;
            }

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            m_ProcedureSplash = null;

            base.OnClose(userData);
        }
    }
}
