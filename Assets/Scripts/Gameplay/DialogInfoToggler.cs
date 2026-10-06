using UnityEngine;

namespace Recalled.Gameplay
{
    public class DialogInfoToggler : MonoBehaviour, IApproachReactor
    {
        public void ReactToApproach(IApproachable approachable)
        {
            gameObject.SetActive(true);
        }

        public void ReactToRetreat(IApproachable approachable)
        {
            gameObject.SetActive(false);
        }
    }
}
