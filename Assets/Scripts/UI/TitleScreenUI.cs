using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class TitleScreenUI : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}