using UnityEngine;
using GameDevWithMarco.Managers;
using GameDevWithMarco.DesignPattern;
using System.Collections;

namespace GameDevWithMarco.Packages
{
    public class FallenObjectsSpawner : MonoBehaviour
    {
        [Header("Packages Spawn Position")]
        [SerializeField] GameObject[] spawners;

        [Header("Package Delay Variables")]
        [SerializeField] float initialDelay = 2.0f;
        [SerializeField] float minDelay = 0.5f;
        [SerializeField] float delayIncreaseRate = 0.1f;
        private float currentDelay;

        [Header("Packages Drop Chance Percentages")]
        [SerializeField] float goodPackageDropPercentage = 70f;
        [SerializeField] float badPackageDropPercentage = 25f;
        [SerializeField] float lifePackageDropPercentage = 5f;
        [SerializeField] float minimum_goodPackagePercentage = 50f;
        [SerializeField] float maximum_badPackagePercentage = 50f;
        [SerializeField] float percentageChangeRatio = 0.1f;

        private float lastGoodPercentage;
        private float lastBadPercentage;

        void Start()
        {
            currentDelay = initialDelay;
            lastGoodPercentage = goodPackageDropPercentage;
            lastBadPercentage = badPackageDropPercentage;
            StartCoroutine(SpawningLoop());
        }

        private void SpawnPackageAtARandomLocation(ObjectPoolingPattern.TypeOfPool poolType)
        {
            GameObject spawnedPackage = ObjectPoolingPattern.Instance.GetPoolItem(poolType);

            int randomInteger = Random.Range(0, spawners.Length);
            Vector2 spawnPosition = spawners[randomInteger].transform.position;

            spawnedPackage.transform.position = spawnPosition;
        }

        private IEnumerator SpawningLoop()
        {
            AdjustPercentagesBasedOnSuccessRate();
            SpawnPackageAtARandomLocation(GetPackageTypeBasedOnPercentage());

            yield return new WaitForSeconds(currentDelay);

            currentDelay -= delayIncreaseRate;
            if (currentDelay < minDelay) currentDelay = minDelay;

            StartCoroutine(SpawningLoop());
        }

        private void AdjustPercentagesBasedOnSuccessRate()
        {
            int successRate = GameManager.Instance.successRate;

            // Adjust percentages based on success rate
            if (successRate >= 50)
            {
                badPackageDropPercentage += percentageChangeRatio;
                goodPackageDropPercentage -= percentageChangeRatio;
            }
            else
            {
                badPackageDropPercentage -= percentageChangeRatio;
                goodPackageDropPercentage += percentageChangeRatio;
            }

            CapThePercentages();
            LogPercentageChanges();
        }

        private void CapThePercentages()
        {
            goodPackageDropPercentage = Mathf.Clamp(goodPackageDropPercentage, minimum_goodPackagePercentage, 100f);
            badPackageDropPercentage = Mathf.Clamp(badPackageDropPercentage, 0f, maximum_badPackagePercentage);

            float total = goodPackageDropPercentage + badPackageDropPercentage + lifePackageDropPercentage;
            if (total > 100f)
            {
                lifePackageDropPercentage -= (total - 100f);
                lifePackageDropPercentage = Mathf.Max(lifePackageDropPercentage, 0f);
            }
        }

        private void LogPercentageChanges()
        {
            if (goodPackageDropPercentage != lastGoodPercentage || badPackageDropPercentage != lastBadPercentage)
            {
                Debug.Log($"Percentages Updated: Good = {goodPackageDropPercentage}%, Bad = {badPackageDropPercentage}%");
                lastGoodPercentage = goodPackageDropPercentage;
                lastBadPercentage = badPackageDropPercentage;
            }
        }

        private ObjectPoolingPattern.TypeOfPool GetPackageTypeBasedOnPercentage()
        {
            float randomValue = Random.Range(0f, 100.1f);

            if (randomValue <= goodPackageDropPercentage)
            {
                return ObjectPoolingPattern.TypeOfPool.Good;
            }
            else if (randomValue <= goodPackageDropPercentage + badPackageDropPercentage)
            {
                return ObjectPoolingPattern.TypeOfPool.Bad;
            }
            else
            {
                return ObjectPoolingPattern.TypeOfPool.Life;
            }
        }
    }
}
