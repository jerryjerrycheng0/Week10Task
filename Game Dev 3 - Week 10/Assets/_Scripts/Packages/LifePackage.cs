using GameDevWithMarco.Interfaces;
using GameDevWithMarco.Managers;
using GameDevWithMarco.ObserverPattern;
using UnityEngine;

namespace GameDevWithMarco.Packages
{
    public class LifePackage : MonoBehaviour, ICollidable
    {
        [SerializeField] GameEvent lifePackageCollected;
        public void CollidedLogic()
        {
            lifePackageCollected.Raise();
        }
    }
}
