using TMPro;
using UnityEngine;

namespace Remnants
{
    public class StoryObjectScript : Interactive
    {
        #region Variables
        public GameObject storyUI;
        public TextMeshProUGUI story;

        [TextArea]
        public string storyText;
        #endregion

        #region Property
        #endregion

        #region Unity Event Method
        private void Update()
        {
            if (IsUIOpened && Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log($"{gameObject.name} ESC 눌림");
                StoryClose();
            }
        }
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            if (!IsUIOpened)
            {
                StoryOpen();
            }
            
        }

        private void StoryOpen()
        {
            story.text = storyText;

            storyUI.SetActive(true);
            IsUIOpened = true;
        }
        private void StoryClose()
        {
            story.text = "";
            storyUI.SetActive(false);
            IsUIOpened = false;
        }
        #endregion
    }

}
