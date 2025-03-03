using System.Collections.Generic;

namespace UiCommon
{
    internal static class UiModelManager
    {
        private static Dictionary<int, UiBaseModel> m_UiModels;
        
        internal static T GetModel<T>() where T : UiBaseModel, new()
        {
            if (m_UiModels == null)
            {
                m_UiModels = new Dictionary<int, UiBaseModel>();
            }

            int hash = typeof(T).GetHashCode();
            UiBaseModel model = null;
            if (m_UiModels.TryGetValue(hash, out model) == false)
            {
                model = new T();
                model.Init();
                m_UiModels.Add(hash, model);
            }
            return (T)model;
        }
        
    }
}