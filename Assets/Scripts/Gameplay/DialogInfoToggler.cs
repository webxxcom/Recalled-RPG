using UnityEngine;

namespace Recalled.Gameplay
{
    public class DialogInfoToggler : MonoBehaviour, IApproachReactor
    {
        public void ReactToApproach(Approachable approachable)
        {
            gameObject.SetActive(true);
        }

        public void ReactToRetreat(Approachable approachable)
        {
            gameObject.SetActive(false);
        }
    }
}
