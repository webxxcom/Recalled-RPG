using UnityEngine;

namespace Recalled.UI
{
    sealed class QuitGame : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit(0);
        }
    }
}
