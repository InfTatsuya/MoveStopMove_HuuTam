using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CachedObjects 
{
    private static Dictionary<Collider, Character> characterDict = new Dictionary<Collider, Character>();

    public static bool TryGetCharacterByCollider(Collider collider, out Character character)
    {
        if(characterDict.ContainsKey(collider))
        {
            character = characterDict[collider];
            return true;    
        }
        else
        {
            if(collider.TryGetComponent<Character>(out character))
            {
                characterDict.Add(collider, character);
                return true;    
            }
            else
            {
                character = null;
                return false;
            }
        }
    }

    private static Dictionary<Collider, IDamageable> iDamageableDict = new Dictionary<Collider, IDamageable>();

    public static bool TryGetIDamagebleByCollider(Collider collider, out IDamageable damagedObject)
    {
        if (iDamageableDict.ContainsKey(collider))
        {
            damagedObject = characterDict[collider];
            return true;
        }
        else
        {
            if (collider.TryGetComponent<IDamageable>(out damagedObject))
            {
                iDamageableDict.Add(collider, damagedObject);
                return true;
            }
            else
            {
                damagedObject = null;
                return false;
            }
        }
    }
}
