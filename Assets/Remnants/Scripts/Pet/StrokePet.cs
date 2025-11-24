using System.Collections;
using UnityEngine;

namespace Remnants
{
    public class StrokePet : Interactive
{
        #region Variables
        public Renderer targetRenderer;

        public GameObject angelRing;
        public GameObject blade;

        public Material smileMaterial;
        public Material scaryMaterial;

        [SerializeField]
        private float revertDelay = 10f;

        private Coroutine revertCoroutine;
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            if (targetRenderer == null)
                return;

            if (revertCoroutine != null)
            {
                StopCoroutine(RevertToScary());
            }

            targetRenderer.material = smileMaterial;
            angelRing.SetActive(true);
            blade.SetActive(false);

            revertCoroutine = StartCoroutine(RevertToScary());
        }

        IEnumerator RevertToScary()
        {
            yield return new WaitForSeconds(revertDelay);

            targetRenderer.material = scaryMaterial;
            angelRing.SetActive(false);
            blade.SetActive(true);

            revertCoroutine = null;
        }
        #endregion
    }

}
