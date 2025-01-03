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
        public float goodPackageDropPercentage = 70f;
        public float badPackageDropPercentage = 25f;
        [SerializeField] float lifePackageDropPercentage = 5f;
        public float minimum_goodPackagePercentage = 10f;
        public float maximum_badPackagePercentage = 85f;
        [SerializeField] float percentageChangeRatio = 0.1f;

        public static FallenObjectsSpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        void Start()
        {
            currentDelay = initialDelay;
            StartCoroutine(SpawningLoop());
        }

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

        public void IncreaseDifficulty()
        {
            AdjustGoodAndBadPercentages(percentageChangeRatio * 0.5f); // Smaller adjustment for difficulty increases
        }

        private void AdjustGoodAndBadPercentages(float change)
        {
            badPackageDropPercentage += change;
            goodPackageDropPercentage -= change;

            CapThePercentages();

            // If badPackageDropPercentage hits the maximum, stop increasing difficulty
            if (badPackageDropPercentage >= maximum_badPackagePercentage)
            {
                GameManager.Instance.difficulty = Mathf.Clamp(GameManager.Instance.difficulty, 1.0f, GameManager.Instance.difficulty);
            }
        }

        private void CapThePercentages()
        {
            goodPackageDropPercentage = Mathf.Clamp(goodPackageDropPercentage, minimum_goodPackagePercentage, 100f - lifePackageDropPercentage);
            badPackageDropPercentage = Mathf.Clamp(badPackageDropPercentage, 0f, maximum_badPackagePercentage);

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
