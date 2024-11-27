
using UnityEngine;
using GameDevWithMarco.Managers;
using System.Collections.Generic;

namespace GameDevWithMarco.DesignPattern
{
    public class ObjectPoolingPattern : Singleton<ObjectPoolingPattern>
    {
        [SerializeField] PoolData goodPackagePoolData;
        [SerializeField] PoolData badPackagePoolData;
        [SerializeField] PoolData lifePackagePoolData;

        public List<GameObject> goodPool = new List<GameObject>();
        public List<GameObject> badPool = new List<GameObject>();
        public List<GameObject> lifePool = new List<GameObject>();

        public enum TypeOfPool
        {
            Good,
            Bad,
            Life
        }

        // Start is called before the first frame update
        protected override void Awake()
        {
            FillThePool(goodPackagePoolData, goodPool);
            FillThePool(badPackagePoolData, badPool);
            FillThePool(lifePackagePoolData, lifePool);
        }
     

        private void FillThePool(PoolData poolData, List<GameObject> targetPoolContainer)
        {
            GameObject container = CreateAContainerForThePool(poolData);

            for (int i = 0; i < poolData.poolAmount; i++)
            {
                GameObject thingToAddToPool = Instantiate(poolData.poolItem, container.transform);

                thingToAddToPool.SetActive(false);

                targetPoolContainer.Add(thingToAddToPool);
            }
        }

        private GameObject CreateAContainerForThePool(PoolData poolData)
        {
            GameObject container = new GameObject();

            container.transform.SetParent(this.transform);

            container.name = poolData.name;

            return container;
        }

        public GameObject GetPoolItem(TypeOfPool typeOfPoolToUse)
        {
            List<GameObject> poolToUse = new List<GameObject>();

            switch (typeOfPoolToUse)
            {
                case TypeOfPool.Good:
                    poolToUse = goodPool;
                    break;
                case TypeOfPool.Bad:
                    poolToUse = badPool;
                    break;
                case TypeOfPool.Life:
                    poolToUse = lifePool;
                    break;
                
            }

            int itemInPoolCount = poolToUse.Count;
            //Goes through the pool
            for (int i = 0; itemInPoolCount > 0; i++)
            {
                //Looks for the first item that is not active
                if (!poolToUse[i].activeSelf)
                {
                    poolToUse[i].SetActive(true);
                    return poolToUse[i];  
                }
            }
            //Gives us a warning that the pool might be too small
            Debug.LogWarning("No Availeble Items Found, Pool Too Small!");
            //If there are none returns null
            return null;
        }
    }
}
