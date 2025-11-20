using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.OnGameFinished += ActivateScreen;
        gameObject.SetActive(false);
    }

    private void ActivateScreen()
    {
        gameObject.SetActive(true);
    }

    public void ToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
