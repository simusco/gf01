//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.DataTable;
using GameFramework.Event;

using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace App
{
    public class ProcedureSplash : ProcedureBase
    {
        private const float SplashPlaySeconds = 5;

        private SplashForm m_SplashForm = null;
        //是否跳过闪屏
        private bool m_SkipSplash = false;
        private float m_SplashPlaySeconds = 0f;

        public void SkipSplash()
        {
            m_SkipSplash = true;
        }

        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.UI.OpenUIForm(UIFormId.SplashForm, this);
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            m_SplashPlaySeconds += elapseSeconds;
            if (m_SkipSplash || m_SplashPlaySeconds >= SplashPlaySeconds)
            {
                // 编辑器模式下，直接进入预加载流程；否则，检查一下版本
                if (GameEntry.Base.EditorResourceMode)
                {
                    procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Login"));
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                }
                else
                {
                    Debug.Log("检查是否有新版");
                    ChangeState(procedureOwner, typeof(ProcedureCheckVersion));
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_SplashForm != null)
            {
                m_SplashForm.Close(isShutdown);
                m_SplashForm = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_SplashForm = (SplashForm)ne.UIForm.Logic;
        }
    }
}
