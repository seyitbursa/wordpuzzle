using UnityEngine;

namespace Core.Pooling
{ 
    public class PoolObject : MonoBehaviour
    {
        protected ObjectType objectType;

        public ObjectType GetObjectType()
        {
            return objectType;
        }
    }
}