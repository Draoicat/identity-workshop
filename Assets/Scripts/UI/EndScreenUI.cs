using Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        public TMP_Text choicesText;
        
        private void Start()
        {
            EventManager.Instance.OnGameFinished += ActivateScreen;
            gameObject.SetActive(false);
        }

        private void ActivateScreen()
        {
            gameObject.SetActive(true);
            choicesText.text = "Choix : " + '\n';
            foreach (Solution solution in Results.Instance.chosenSolutions)
                choicesText.text += "- " + solution.solution + '\n';
        }
        

        public void ToTitleScreen()
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
