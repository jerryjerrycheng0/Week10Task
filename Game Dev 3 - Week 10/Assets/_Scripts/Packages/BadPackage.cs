using GameDevWithMarco.Interfaces;
using GameDevWithMarco.ObserverPattern;
using UnityEngine;

namespace GameDevWithMarco.Packages
{
    public class BadPackage : MonoBehaviour, ICollidable
    {
        [SerializeField] GameEvent badPackageCollected;

        public void CollidedLogic()
        {
            badPackageCollected.Raise();
            //GameManager.Instance.RedPackLogic();
        }
    }
}
