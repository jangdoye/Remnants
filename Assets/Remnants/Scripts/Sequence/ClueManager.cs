using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Remnants
{
    public class ClueManager : TypewriterEffect
    {
        #region Variables
        public GameObject exitFlower;

        private FindingClues[] allClues;
        [SerializeField]
        private string sequence = "sequence Text";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            allClues = FindObjectsByType<FindingClues>(FindObjectsSortMode.None);
            StartCoroutine(ActiveFlower());
        }
        #endregion

        #region Custom Method
        IEnumerator ActiveFlower()
        {
            while (true)
            {
                bool hasAnyRealClue = false;

                foreach (var clue in allClues)
                {
                    if (clue != null && clue.IsClue)
                    {
                        hasAnyRealClue = true;
                        break;
                    }
                }

                if (!hasAnyRealClue && exitFlower != null)
                {
                    exitFlower.SetActive(true);

                    yield return new WaitForSeconds(1f);
                    StartTyping(sequence);

                    yield return new WaitForSeconds(sequence.Length * typingSpeed + 2f);
                    ClearText();

                    yield break; // 조건 만족했으면 코루틴 종료
                }

                yield return new WaitForSeconds(1f); // 매초마다 검사
            }
            #endregion
        }
    }
}
