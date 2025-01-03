using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UiCommon
{
    internal static class UiControllerManager
    {
        private static Dictionary<Type, Queue<UiBaseController>> m_UiPoolDic =
            new Dictionary<Type, Queue<UiBaseController>>();

        internal static T OpenUi<T>(Transform parent) where T : UiBaseController
        {
            T controller = GetUiControllerFormPool<T>();
            if (controller == null)
            {
                controller.transform.parent = parent;
                return controller;
            }

            MethodInfo getPathMethod = typeof(T).GetMethod("GetPath");
            if (getPathMethod == null)
            {
                return null;
            }

            string path = (string)getPathMethod.Invoke(null, null);

            GameObject go = Resources.Load<GameObject>(path);
            if (go != null)
            {
                GameObject instance = UnityEngine.Object.Instantiate(go, parent);
                instance.transform.position = Vector3.zero; // 设置位置
            }
            else
            {
                Debug.LogError($"无法加载路径为 {path} 的对象！");
            }

            controller = go.GetComponent<T>();
            if (controller == null)
            {
                controller = go.AddComponent<T>();
            }

            return controller;
        }

        internal static void RecycleUiController<T>(T uiController) where T : UiBaseController
        {
            if (uiController != null)
            {
                if (!m_UiPoolDic.ContainsKey(uiController.GetType()))
                {
                    m_UiPoolDic[uiController.GetType()] = new Queue<UiBaseController>();
                }

                m_UiPoolDic[uiController.GetType()].Enqueue(uiController);
            }
        }

        internal static T GetUiControllerFormPool<T>() where T : UiBaseController
        {
            if (!m_UiPoolDic.ContainsKey(typeof(T)) || m_UiPoolDic[typeof(T)].Count == 0)
            {
                return null;
            }

            var q = m_UiPoolDic[typeof(T)];
            return (T)q.Dequeue();
        }
    }
}