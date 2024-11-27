
using UnityEngine;

namespace GameDevWithMarco.Managers
{
    public class VariablesSetter : MonoBehaviour
    {
        [SerializeField] Animator transitionAnim;

        private void Awake()
        {
            //Sets the fade variable
            MyScenemanager.Instance.transitionAnim = transitionAnim;
        }
    }
}
