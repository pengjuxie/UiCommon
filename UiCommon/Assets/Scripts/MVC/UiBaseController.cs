using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiCommon
{
    public abstract class UiBaseController<TView> : UiBaseController where TView : UiBaseView
    {
        protected TView m_View;
        
        protected List<UiBaseController> m_ChildControllers = null;
        private List<RegisterItem> m_ListenedItems = new List<RegisterItem>();
        
        public bool IsOpened { get; private set; }
        public bool IsShown { get; private set; }

        class RegisterItem
        {
            public UiBaseModel Model;
            public int EventID;
            public Action Callback;
        }

        protected void RegisterToModel<T>(int eventId, Action callback) where T : UiBaseModel, new()
        {
            var model = UiModelManager.GetModel<T>();
            model.AddListener(eventId, callback);
            
            RegisterItem item = new RegisterItem()
            {
                Model = model,
                EventID = eventId,
                Callback = callback
            };
            m_ListenedItems.Add(item);
        }
        
        protected void OpenChildController<T>() where T : UiBaseController
        {
            if (m_ChildControllers == null)
            {
                m_ChildControllers = new List<UiBaseController>();
            }
            var childCtrl = UiControllerManager.OpenUi<T>(this.transform);
            if (childCtrl != null)
            {
                m_ChildControllers.Add(childCtrl);
            }
        }

        private void Awake()
        {
            OnUiInit();
        }

        internal sealed override void Open()
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
                
                if (m_ChildControllers.Count != 0)
                {
                    for (int i = 0; i < m_ChildControllers.Count; i++)
                    {
                        m_ChildControllers[i].Close();
                    }
                    
                    m_ChildControllers.Clear();
                }
                
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
                gameObject.SetActive(true);
            }
        }

        public sealed override void Hide()
        {
            if (IsShown)
            {
                IsShown = false;
                OnUiHide();
                gameObject.SetActive(false);
            }
        }

        public sealed override void Destroy()
        {
            if (m_ListenedItems.Count > 0)
            {
                foreach (var item in m_ListenedItems)
                {
                    item.Model.RemoveListener(item.EventID, item.Callback);
                }
                m_ListenedItems.Clear();
            }
            
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
        internal abstract void Open();
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