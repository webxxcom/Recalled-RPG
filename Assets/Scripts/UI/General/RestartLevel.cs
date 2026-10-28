using UnityEngine;
using UnityEngine.SceneManagement;

namespace Recalled.UI
{
    class RestartLevel : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
