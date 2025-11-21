using System.Linq;
using UnityEngine;

namespace Core
{
    public class Results : MonoBehaviour
    {       
        public static Results Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }
        public SemiCircleProportional semiCircleProportional;

        public Solution[] chosenSolutions;
        public Solution ChosenSolutionForCurrentStep => chosenSolutions[EventManager.Instance.CurrentEventIndex];

        private void Start()
        {
            chosenSolutions = new Solution[EventManager.Instance.EventCount];
            EventManager.Instance.OnVoteEnded += GetResults;
        }

        public void GetResults(Event gameEvent)
        {
            float[] percents = semiCircleProportional.GetSlicePercentages();
            float[] results = new float[gameEvent.solutions.Length]; // total vote percentage per solution

            for (int i = 0; i < gameEvent.solutions.Length; ++i)
            {
                for (int j = 0; j < percents.Length; ++j)
                {
                    if (gameEvent.solutions[i].supportingParties.Contains((Parties)j))
                        results[i] += percents[j];
                }
            }

            int chosenSolutionIndex = 0;
            for (int i = 0; i < results.Length; ++i)
            {
                if (results[i] > results[chosenSolutionIndex])
                    chosenSolutionIndex = i;
            }
            
            chosenSolutions[EventManager.Instance.CurrentEventIndex] = gameEvent.solutions[chosenSolutionIndex];
            Debug.Log(chosenSolutions[EventManager.Instance.CurrentEventIndex]);
        }
    }
}
