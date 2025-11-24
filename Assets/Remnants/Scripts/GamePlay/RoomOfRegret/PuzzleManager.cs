using UnityEngine;
using TMPro;
using System.Collections;

namespace Remnants
{
    public class PuzzleManager : MonoBehaviour
    {
        #region Variables
        public static PuzzleManager Instance;

        public GameObject puzzlePanel;              // 퍼즐 UI
        public TextMeshProUGUI regretText;          // 후회 텍스트

        public GameObject completeMirror;           // 완성된 거울
        public GameObject brokenMirrorRoot;         // 깨진 거울 부모
        public GameObject[] mirrorPieces;           // 드래그할 조각들

        public string[] regretLines;                // 후회 대사들
        public float regretDisplayDuration = 3f;

        private Coroutine hideTextCoroutine;
        private int[] collectedFlags;
        private int collectedCount = 0;
       
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            collectedFlags = new int[mirrorPieces.Length];
          
        }
        #endregion

        #region Custom Method
        public void CollectPiece(int index)
        {
            if (index < 0 || index >= collectedFlags.Length) return;
            if (collectedFlags[index] == 1) return;

            collectedFlags[index] = 1;
            collectedCount++;
            ShowRegretLine(index);

            if (collectedCount >= mirrorPieces.Length)
            {

                StartCoroutine(DelayedStartPuzzle());
            }
        }

        private IEnumerator DelayedStartPuzzle()
        {

            yield return new WaitForSeconds(1f);
            StartCoroutine(AnimatePuzzleAssembly());
        }

        private IEnumerator AnimatePuzzleAssembly()
        {
            puzzlePanel.SetActive(true);
            brokenMirrorRoot.SetActive(false);
            completeMirror.SetActive(false);


            for (int i = 0; i < mirrorPieces.Length; i++)
            {
                if (collectedFlags[i] == 1 && mirrorPieces[i] != null)
                {
                    mirrorPieces[i].SetActive(true);

                    Animator anim = mirrorPieces[i].GetComponent<Animator>();
                    if (anim != null)
                        anim.SetTrigger("Snap");

                    ShowRegretLine(i);

                    yield return new WaitForSeconds(2f); // 다음 조각까지 텀
                }
            }

            yield return new WaitForSeconds(1f);
            CompletePuzzle();
        }

        public void ShowRegretLine(int index)
        {
            if (regretText == null || index < 0 || index >= regretLines.Length)
                return;

            regretText.text = regretLines[index];

            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);

            hideTextCoroutine = StartCoroutine(HideTextAfterDelay());
        }

        private IEnumerator HideTextAfterDelay()
        {
            yield return new WaitForSeconds(regretDisplayDuration);
            regretText.text = "";
        }

        private void CompletePuzzle()
        {
            completeMirror.SetActive(true);
            brokenMirrorRoot.SetActive(false);
            puzzlePanel.SetActive(false);
        }
        #endregion
    }
}