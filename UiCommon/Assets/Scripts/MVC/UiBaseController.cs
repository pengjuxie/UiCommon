using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiCommon
{
    public abstract class UiBaseController<TView> : UiBaseController where TView : UiBaseView
    {
        protected TView m_View;

        public UiBaseController ParentController { get; private set; }
        protected List<UiBaseController> m_ChildControllers = null;

        public bool IsInited { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsShown { get; private set; }

        protected sealed override void Init()
        {
            OnUiInit();
            IsInited = true;
        }

        protected sealed override void Open()
        {
            if (!IsOpened)
            {
                IsOpened = true;
                OnUiOpen();
            }
        }

        public sealed override void Close()
        {
            if (IsOpened)
            {
                IsOpened = false;
                OnUiClose();
                if (NeedReturn2Pool())
                {
                    UiControllerManager.RecycleUiController(this);
                }
                else
                {
                    Destroy();
                }
               
            }
        }

        public sealed override void Show()
        {
            if (!IsShown)
            {
                IsShown = true;
                OnUiShow();
            }
        }

        public sealed override void Hide()
        {
            if (IsShown)
            {
                IsShown = false;
                OnUiHide();
            }
        }

        public sealed override void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (IsShown)
                OnUiHide();
            if (IsOpened)
                OnUiClose();

            OnUiDestroy();
        }
    }

    public abstract class UiBaseController : MonoBehaviour
    {
        protected abstract void Init();
        protected abstract void Open();
        public abstract void Close();
        public abstract void Show();
        public abstract void Hide();
        public abstract void Destroy();

        protected virtual bool NeedReturn2Pool()
        {
            return true;
        }

        protected virtual void OnUiInit()
        {
        }

        protected virtual void OnUiOpen()
        {
        }

        protected virtual void OnUiShow()
        {
        }

        protected virtual void OnUiHide()
        {
        }

        protected virtual void OnUiClose()
        {
        }

        protected virtual void OnUiDestroy()
        {
        }
    }
}