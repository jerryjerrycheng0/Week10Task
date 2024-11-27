
using UnityEngine;
using GameDevWithMarco.Managers;
using GameDevWithMarco.DesignPattern;
using System.Collections;
using UnityEditor.Experimental.GraphView;

namespace GameDevWithMarco.Packages
{
    public class FallenObjectsSpawner : MonoBehaviour
    {
        //Referencing Gameobjects
        [Header("Packages Spawn Position")]
        [SerializeField] GameObject[] spawners;
        [Header("package Delay Variables")]
        [SerializeField] float initialDelay = 2.0f;
        [SerializeField] float minDelay = 0.5f;
        [SerializeField] float delayIncreaseRate = 0.1f;
        float currentDelay;
        [Header("Packages Drop Chance Percentages")]
        [SerializeField] float goodPackageDropPercentage = 70f;
        [SerializeField] float badPackageDropPercentage = 25f;
        [SerializeField] float lifePackageDropPercentage = 5f;
        [SerializeField] float minimum_goodPackagePercentage;
        [SerializeField] float maximum_badPackagePercentage;
        [SerializeField] float percentageChangeRatio = 0.1f;


        void Start()
        {
            StartCoroutine(SpawningLoop());
        }

        private void SpawnPackageAtARandomLocation(ObjectPoolingPattern.TypeOfPool poolType)
        {
            GameObject spawnedPackage = ObjectPoolingPattern.Instance.GetPoolItem(poolType);

            int randomInteger = Random.Range(0, spawners.Length - 1);

            Vector2 spawnPosition = spawners[randomInteger].transform.position;

            spawnedPackage.transform.position = spawnPosition;
        }

        private IEnumerator SpawningLoop()
        {
            SpawnPackageAtARandomLocation(GetPackageTypeBasedOnPercentage());

            yield return new WaitForSeconds(currentDelay);

            currentDelay = delayIncreaseRate;

            if(currentDelay < minDelay) currentDelay = minDelay;

            StartCoroutine(SpawningLoop());
        }

        private void CapThePercentages()
        {
            if(goodPackageDropPercentage <= minimum_goodPackagePercentage && badPackageDropPercentage >= maximum_badPackagePercentage)
            {
                goodPackageDropPercentage = minimum_goodPackagePercentage;
                badPackageDropPercentage = maximum_badPackagePercentage;
            }
        }

        public void GrowBadPercentage()
        {
            goodPackageDropPercentage -= percentageChangeRatio;
            badPackageDropPercentage += percentageChangeRatio;
            CapThePercentages();
        }

        public void GrowGoodPercentage()
        {
            goodPackageDropPercentage += percentageChangeRatio;
            badPackageDropPercentage -= percentageChangeRatio;
            CapThePercentages();
        }

        private ObjectPoolingPattern.TypeOfPool GetPackageTypeBasedOnPercentage()
        {
            float randomValue = Random.Range(0f, 100.1f);
            if (randomValue <= goodPackageDropPercentage)
            {
                return ObjectPoolingPattern.TypeOfPool.Good;
            }
            else if (randomValue > goodPackageDropPercentage && randomValue <= (goodPackageDropPercentage + badPackageDropPercentage)) {

                return ObjectPoolingPattern.TypeOfPool.Bad;
            }
            else
            {
                return ObjectPoolingPattern.TypeOfPool.Life;
            }
        }
        
    }
}
