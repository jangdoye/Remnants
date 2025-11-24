using UnityEngine;

namespace Remnants
{
    // 대사 후 배드 엔딩 문에 다가갔을 때 발동되는 트리거
    public class BadTrigger : FadeTriggerBase
{
        // 검은색 페이드
        protected override Color FadeColor => Color.gray;

        // BGM
        protected override string EndingBgmName => "BadEndingBgm";
    }
}

