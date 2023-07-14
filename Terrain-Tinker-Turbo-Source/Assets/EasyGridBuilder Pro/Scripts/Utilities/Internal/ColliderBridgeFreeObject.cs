using UnityEngine;
using SoulGames.EasyGridBuilderPro;

namespace SoulGames.Utilities
{
    public class ColliderBridgeFreeObject : MonoBehaviour
    {
        FreeObjectGhost listener;

        public void Awake()
        {
            this.listener = MultiGridManager.Instance.transform.GetComponentInChildren<FreeObjectGhost>();
        }

        private void OnTriggerEnter(Collider other)
        {
            listener.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            listener.OnTriggerExit(other);
        }

        private void OnTriggerStay(Collider other)
        {
            listener.OnTriggerStay(other);
        }
    }
}