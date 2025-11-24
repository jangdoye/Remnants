using UnityEngine;

namespace Remnants
{
    // 대사 후 해피 엔딩 문에 다가갔을 때 발동되는 트리거
    public class HappyTrigger : FadeTriggerBase
    {
        // 흰색 페이드
        protected override Color FadeColor => Color.white;

        // BGM
        protected override string EndingBgmName => "HappyEndingBgm";
    }
}

