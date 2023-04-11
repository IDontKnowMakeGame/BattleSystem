using UnityEngine;

namespace Tool.Resource
{
    #if UNITY_EDITOR
    public static class ResourceLoader
    {
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
    }
    #endif
}