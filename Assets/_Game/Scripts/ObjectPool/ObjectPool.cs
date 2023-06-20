using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected uint initPoolSize;
    [SerializeField] protected T objectToPool;
    [SerializeField] protected Transform parent;
    // store the pooled objects in a collection
    protected Stack<T> stack;

    protected virtual void Start()
    {
        SetupPool();
    }


    // creates the pool (invoke when the lag is not noticeable)
    protected abstract void SetupPool();

    public abstract T GetPooledObject();

    public abstract void ReturnToPool(T pooledObject);

    private void OnDestroy()
    {
        while (stack.Count > 0)
        {
            Destroy(stack.Pop());
        }

        stack.Clear();
    }
}
