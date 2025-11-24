using TMPro;
using UnityEngine;

namespace Remnants
{
    public class BreakSphere : Interactive
    {
        #region Variables
        private KeyCode[] keyCodes = new KeyCode[]
        {
             KeyCode.Q,KeyCode.E
        };

        [SerializeField]
        private bool requireMultiplePress = false;
        [SerializeField]
        private int requiredPressCount = 5;
        private int currentPressCount = 0;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            RandomKeyCode();

            globalCooldown = 1.5f;
        }
        #endregion

        #region Custom Method
        private void RandomKeyCode()
        {
            int randomKey = Random.Range(0, keyCodes.Length);

            CurrentKey = new KeyCode[] { keyCodes[randomKey] };            
        }

        protected override void DoAction()
        {
            if (theDistance <= 2f)
                return;
            if (requireMultiplePress)
            {
                currentPressCount++;

                //requiredPressCount만큼 눌러야 파괴
                if (currentPressCount >= requiredPressCount)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                //한번만 눌러도 파괴
                Destroy(this.gameObject);
            }        
        }
        #endregion
    }

}
