using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class UITriggerAnimation : MonoBehaviour
    {
        [Space]
        [SerializeField]private Animator animator;
        [SerializeField]private string firstTriggerParameter;
        [SerializeField]private string secondTriggerParameter;
        [Space]

        [SerializeField]private GameObject[] firstObject;
        [SerializeField]private GameObject[] secondObject;

        [Space]
        [SerializeField]private GameObject[] toggleObject;

        private bool triggered = false;

        public void TriggerAnimation()
        {
            if (!triggered)
            {
                triggered = true;
                animator.SetTrigger(firstTriggerParameter);
                
                foreach (var item in firstObject)
                {
                    item.SetActive(false);
                }
                foreach (var item in secondObject)
                {
                    item.SetActive(true);
                }
            }
            else
            {
                triggered = false;
                animator.SetTrigger(secondTriggerParameter);

                foreach (var item in firstObject)
                {
                    item.SetActive(true);
                }
                foreach (var item in secondObject)
                {
                    item.SetActive(false);
                }
            }

            foreach (var item in toggleObject)
            {
                if (item.activeSelf == true) item.SetActive(false);
                else item.SetActive(true); 
            }
        }
    }
}