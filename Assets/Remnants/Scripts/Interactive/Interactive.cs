using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Remnants
{
    public class Interactive : MonoBehaviour
    {
        #region Variables
        //theDistance를 protected 로 중복 가능하게
        protected float theDistance;

        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionKey;
        public TextMeshProUGUI actionText;

        public GameObject extraCross;       //커서 올렸을 때 그 오브젝트에 콜라이더가 붙어있다면

        //인터렉티브 기능 사용 여부
        [SerializeField]
        protected bool unInteractive = false;

        [SerializeField]
        protected string action = "Do Interactive Action";

        [SerializeField]
        private float actionDistance = 2f;

        private KeyCode[] currentKey = new KeyCode[] { KeyCode.E };

        protected static float globalCooldown = 0f; // 기본값 0초
        protected static float lastGlobalActionTime = -Mathf.Infinity;

        public static bool IsUIOpened = false;
        #endregion

        #region Property
        public virtual KeyCode[] CurrentKey
        {
            get
            {
                return currentKey;
            }
            set
            {
                currentKey = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void Update()       //Update로 구하는 이유는 실시간으로 구해야 하기 때문
        {
            //오브젝트와 플레이어 사이간 거리 구하기
            theDistance = PlayerCasting.distanceFromTarget;
        }
        private void OnMouseOver()
        {
            if (IsUIOpened)
                return;

            extraCross.SetActive(true);

            if (theDistance <= actionDistance)
            {
                ShowActionUI();

            }
            else
            {
                //UI 숨기기
                HideActionUI();
            }
            //키입력 체크
            foreach (KeyCode key in CurrentKey)
            {
                if (Input.GetKeyDown(key))
                {
                    if (Time.time - lastGlobalActionTime < globalCooldown)
                        return; // 전역 쿨타임 중이면 무시

                    lastGlobalActionTime = Time.time;

                    extraCross.SetActive(false);
                    HideActionUI();
                    DoAction();
                    break;
                }
            }


        }
        private void OnMouseExit()
        {
            extraCross.SetActive(false);
            HideActionUI();
        }
        #endregion

        #region Custom Method
        public void SetInteracted(bool value)
        {
            unInteractive = value;
        }
        //Action UI 보여주기
        protected void ShowActionUI()
        {
            actionUI.SetActive(true);
            actionText.text = action;

            if (CurrentKey != null && CurrentKey.Length > 0)
            {
                actionKey.text = "[" + CurrentKey[0] + "]";               
            }
        }

        //Action UI 숨기기
        protected void HideActionUI()
        {
            actionUI.SetActive(false);
            actionText.text = "";
        }

        protected virtual void DoAction()
        {

        }
        #endregion
    }

}
