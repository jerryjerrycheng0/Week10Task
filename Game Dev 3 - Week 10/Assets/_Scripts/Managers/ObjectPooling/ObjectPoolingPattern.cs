using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.DesignPattern
{
    public class ObjectPoolingPattern : Singleton<ObjectPoolingPattern>
    {
        [SerializeField] PoolData bulletPool;
        [SerializeField] PoolData muzzleFlashPool;

        public enum TypeOfPool
        {
            BulletPool,
            MuzzleFlash
        }

        // Start is called before the first frame update
        private void Start()
        {
            FillThePool(bulletPool);
            FillThePool(muzzleFlashPool);
        }       
     

        private void FillThePool(PoolData poolData)
        {
            //Clears the pool
            poolData.ResetThePool();

            //Goes as many time as we want the pool amount to be
            for (int i = 0; i < poolData.poolAmount; i++)
            {
                //Instantiates on item in the pool
                GameObject thingToAddToThePool = Instantiate(poolData.poolItem);
                //Sets the patent to be what this script is attached to
                thingToAddToThePool.transform.parent = transform;
                //Deactivates it 
                thingToAddToThePool.SetActive(false);
                //Adds it to the pool container list
                poolData.pooledObjectContainer.Add(thingToAddToThePool);
            }
        }

        public GameObject GetPoolItem(TypeOfPool poolToUse)
        {
            //To store the local pool
            PoolData pool = ScriptableObject.CreateInstance<PoolData>();

            switch (poolToUse)
            {
                case TypeOfPool.BulletPool:
                    pool = bulletPool;
                    break;
                case TypeOfPool.MuzzleFlash: 
                    pool = muzzleFlashPool;
                    break;
            }

            //Goes through the pool
            for (int i = 0; i < pool.pooledObjectContainer.Count; i++)
            {
                //Looks for the first item that is not active
                if (!pool.pooledObjectContainer[i].activeInHierarchy)
                {
                    //activates it
                    pool.pooledObjectContainer[i].SetActive(true);
                    //Returns it
                    return pool.pooledObjectContainer[i];
                }
            }
            //Gives us a warning that the pool might be too small
            Debug.LogWarning("No Availeble Items Found, Pool Too Small!");
            //If there are none returns null
            return null;
        }
    }
}
