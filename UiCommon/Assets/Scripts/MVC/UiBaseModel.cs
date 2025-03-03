using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiCommon
{
    public class UiBaseModel
    {
        private MessageHandler m_MessageHandler;
        
        class MessageHandler
        {
            private Dictionary<int, Delegate> m_EventDictionary = new Dictionary<int, Delegate>();
            
            public void AddListener(int eventId, Action listener)
            {
                if (m_EventDictionary.ContainsKey(eventId))
                {
                    m_EventDictionary[eventId] = Delegate.Combine(m_EventDictionary[eventId], listener);
                }
                else
                {
                    m_EventDictionary[eventId] = listener;
                }
            }
            
            public void RemoveListener(int eventId, Action listener)
            {
                if (m_EventDictionary.ContainsKey(eventId))
                {
                    var currentDelegate = m_EventDictionary[eventId];
                    currentDelegate = Delegate.Remove(currentDelegate, listener);

                    if (currentDelegate == null)
                    {
                        m_EventDictionary.Remove(eventId);
                    }
                    else
                    {
                        m_EventDictionary[eventId] = currentDelegate;
                    }
                }
            }
            
            public void TriggerEvent(int eventId)
            {
                if (m_EventDictionary.ContainsKey(eventId))
                {
                    if (m_EventDictionary[eventId] is Action callback)
                    {
                        callback.Invoke();
                    }
                }
            }
            
        }
        
        public virtual void Init()
        {
            m_MessageHandler = new MessageHandler();
        }
        
        public void AddListener(int eventId, Action listener)
        {
            m_MessageHandler.AddListener(eventId, listener);
        }
        
        public void RemoveListener(int eventId, Action listener)
        {
            m_MessageHandler.RemoveListener(eventId, listener);
        }

        
        
    }

  
}