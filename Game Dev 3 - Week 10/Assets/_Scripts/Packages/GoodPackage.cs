using GameDevWithMarco.Interfaces;
using GameDevWithMarco.Managers;
using GameDevWithMarco.ObserverPattern;
using UnityEngine;

namespace GameDevWithMarco.Packages
{
    public class GoodPackage : MonoBehaviour, ICollidable
    {
        [SerializeField] GameEvent goodPackageCollected;
        public void CollidedLogic()
        {
            goodPackageCollected.Raise();
        }
    }
}
