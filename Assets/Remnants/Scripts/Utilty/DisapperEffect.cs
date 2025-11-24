using System.Collections;
using UnityEngine;

namespace Remnants
{
    public class DisapperEffect : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float fadeDuration = 2f;

        private Renderer objRenderer;
        private Material fadeMat;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            objRenderer = this.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                fadeMat = objRenderer.material;
                SetMaterialToTransparent(fadeMat);
            }
        }
        #endregion

        #region Custom Method
        public void StartDisapper()
        {
            if (fadeMat != null)
            {
                StartCoroutine(FadeOutAndDisable());
            }
        }

        IEnumerator FadeOutAndDisable()
        {
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                Color color = fadeMat.color;
                color.a = alpha;
                fadeMat.color = color;
                yield return null;
            }

            Destroy(this.gameObject);
        }

        private void SetMaterialToTransparent(Material mat)
        {
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
        #endregion
    }

}