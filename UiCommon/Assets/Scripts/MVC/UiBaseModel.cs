using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiCommon
{
    public class UiBaseModel
    {
        private MessageHandler _messageHandler;
        
        class MessageHandler
        {
            private Dictionary<int, Delegate> _eventDictionary = new Dictionary<int, Delegate>();
            
            public void AddListener(int eventId, Action listener)
            {
                if (_eventDictionary.ContainsKey(eventId))
                {
                    _eventDictionary[eventId] = Delegate.Combine(_eventDictionary[eventId], listener);
                }
                else
                {
                    _eventDictionary[eventId] = listener;
                }
            }
            
            public void RemoveListener(int eventId, Action listener)
            {
                if (_eventDictionary.ContainsKey(eventId))
                {
                    var currentDelegate = _eventDictionary[eventId];
                    currentDelegate = Delegate.Remove(currentDelegate, listener);

                    if (currentDelegate == null)
                    {
                        _eventDictionary.Remove(eventId);
                    }
                    else
                    {
                        _eventDictionary[eventId] = currentDelegate;
                    }
                }
            }
            
            public void TriggerEvent(int eventId)
            {
                if (_eventDictionary.ContainsKey(eventId))
                {
                    if (_eventDictionary[eventId] is Action callback)
                    {
                        callback.Invoke();
                    }
                }
            }
            
        }
        
        public virtual void Init()
        {
            _messageHandler = new MessageHandler();
        }
        
        public void AddListener(int eventId, Action listener)
        {
            _messageHandler.AddListener(eventId, listener);
        }
        
        public void RemoveListener(int eventId, Action listener)
        {
            _messageHandler.RemoveListener(eventId, listener);
        }

        
        
    }

  
}