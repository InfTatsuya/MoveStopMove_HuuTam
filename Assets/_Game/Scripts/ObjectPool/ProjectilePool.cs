using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool<Projectile>
{
    public override Projectile GetPooledObject()
    {
        // if the pool is not large enough, instantiate a new PooledObjects
        if (stack.Count == 0)
        {
            Projectile newInstance = Instantiate(objectToPool);
            newInstance.Pool = this;
            return newInstance;
        }
        // otherwise, just grab the next one from the list
        Projectile nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public override void ReturnToPool(Projectile pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.transform.SetParent(parent);
        pooledObject.gameObject.SetActive(false);
    }

    protected override void SetupPool()
    {
        stack = new Stack<Projectile>();
        Projectile instance = null;
        for (int i = 0; i < initPoolSize; i++)
        {
            instance = Instantiate(objectToPool, parent);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }

    public int Count => stack.Count;
}
