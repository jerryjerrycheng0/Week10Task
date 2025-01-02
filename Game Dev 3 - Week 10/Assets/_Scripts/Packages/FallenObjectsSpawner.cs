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
        [SerializeField] float minimum_goodPackagePercentage = 25f;
        [SerializeField] float maximum_badPackagePercentage = 70f;
        [SerializeField] float percentageChangeRatio = 0.1f;

        void Start()
        {
            currentDelay = initialDelay;
            StartCoroutine(SpawningLoop());
        }

        /// <summary>
        /// Adjust percentages after collecting a package.
        /// </summary>
        /// <param name="isGoodPackage">True if a good package is collected, false otherwise.</param>
        public void AdjustPercentagesAfterPackageCollection(bool isGoodPackage)
        {
            if (isGoodPackage)
            {
                AdjustGoodAndBadPercentages(-percentageChangeRatio);
            }
            else
            {
                AdjustGoodAndBadPercentages(percentageChangeRatio);
            }
        }

        /// <summary>
        /// Adjust percentages when the difficulty increases.
        /// </summary>
        public void IncreaseDifficulty()
        {
            AdjustGoodAndBadPercentages(percentageChangeRatio * 0.5f); // Smaller adjustment for difficulty increases
        }

        /// <summary>
        /// Adjusts good and bad package percentages in opposite directions.
        /// </summary>
        /// <param name="change">The amount to increase/decrease bad package percentage.</param>
        private void AdjustGoodAndBadPercentages(float change)
        {
            badPackageDropPercentage += change;
            goodPackageDropPercentage -= change;

            CapThePercentages();
        }

        /// <summary>
        /// Ensures percentages remain within specified bounds.
        /// </summary>
        private void CapThePercentages()
        {
            // Cap the good and bad percentages within their allowed ranges
            goodPackageDropPercentage = Mathf.Clamp(goodPackageDropPercentage, minimum_goodPackagePercentage, 100f - lifePackageDropPercentage);
            badPackageDropPercentage = Mathf.Clamp(badPackageDropPercentage, 0f, maximum_badPackagePercentage);

            // Ensure total percentages (good + bad + life) do not exceed 100%
            float total = goodPackageDropPercentage + badPackageDropPercentage + lifePackageDropPercentage;
            if (total > 100f)
            {
                lifePackageDropPercentage -= (total - 100f);
                lifePackageDropPercentage = Mathf.Max(lifePackageDropPercentage, 0f);
            }
        }

        private IEnumerator SpawningLoop()
        {
            SpawnPackageAtARandomLocation(GetPackageTypeBasedOnPercentage());

            yield return new WaitForSeconds(currentDelay);

            currentDelay -= delayIncreaseRate;
            if (currentDelay < minDelay) currentDelay = minDelay;

            StartCoroutine(SpawningLoop());
        }

        private void SpawnPackageAtARandomLocation(ObjectPoolingPattern.TypeOfPool poolType)
        {
            GameObject spawnedPackage = ObjectPoolingPattern.Instance.GetPoolItem(poolType);

            int randomIndex = Random.Range(0, spawners.Length);
            Vector2 spawnPosition = spawners[randomIndex].transform.position;

            spawnedPackage.transform.position = spawnPosition;
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

        public void GrowGoodPercentage()
        {
            AdjustGoodAndBadPercentages(-percentageChangeRatio);
        }

        public void GrowBadPercentage()
        {
            AdjustGoodAndBadPercentages(percentageChangeRatio);
        }
    }
}
