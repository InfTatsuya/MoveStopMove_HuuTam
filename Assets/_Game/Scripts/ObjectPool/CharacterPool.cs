using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool : ObjectPool<Character>
{
    public override Character GetPooledObject()
    {
        // if the pool is not large enough, instantiate a new PooledObjects
        if (stack.Count == 0)
        {
            Character newInstance = Instantiate(objectToPool);
            newInstance.Pool = this;
            return newInstance;
        }
        // otherwise, just grab the next one from the list
        Character nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public override void ReturnToPool(Character pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.transform.SetParent(parent);
        pooledObject.gameObject.SetActive(false);
    }

    protected override void SetupPool()
    {
        stack = new Stack<Character>();
        Character instance = null;
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
