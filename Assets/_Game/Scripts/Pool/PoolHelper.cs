using UnityEngine;
using System.Collections;

namespace Tools.Pool
{
    public class PoolHelper : MonoBehaviour
    {
        public void DisableComponentAfter(Component component, float seconds)
        {
            StartCoroutine(DisableAfterRoutine(component,seconds));
        }
        private IEnumerator DisableAfterRoutine(Component component, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            component.gameObject.SetActive(false);
        }
    }
}
