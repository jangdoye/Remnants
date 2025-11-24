using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Remnants
{
    // 엔딩 신 연출
    public class Ending : MonoBehaviour
    {
        #region Variables
        // 플레이어 오브젝트
        public GameObject thePlayer;
        // 페이더 객체
        public SceneFader fader;
        // 시나리오 대사 처리
        public TextMeshProUGUI sequenceText;
        // 펫 오브젝트
        public GameObject thePet;
        // 시퀀스 UI
        public GameObject sequenceUI;
        // 선택지 UI
        public GameObject selectUI;
        // 문 열림 애니메이션
        public Animator happyEndingAnimator;
        public Animator badEndingAnimator;
        // 문 앞에 빛 애니메이션
        public Animator happyLightAnimator;
        public Animator badLightAnimator;
        // 트리거 오브젝트
        public GameObject happyEndingTrigger;
        public GameObject badEndingTrigger;
        // 안개 효과
        public GameObject fog;
        // Bgm, Sfx
        private AudioManager audioManager;

        // 누가 말하고 있는지 (0 : 없음, 1 : 플레이어, 2 : 펫)
        private int whoIsSaying;

        // 대사 여부
        private bool isSequencePlaying = true;

        // 분기 결정
        private bool isHappy = false;
        private bool isBad = false;

        // 타이핑 효과
        private TypewriterEffect typeWritter;

        private bool isSkip = false;
        private bool isSkippable = false;

        // 대사 정보를 담는 구조체
        private struct Dialogue
        {
            public int speaker;
            public string line;
            public float waitTime;

            public Dialogue(int speaker, string line, float waitTime)
            {
                this.speaker = speaker;
                this.line = line;
                this.waitTime = waitTime;
            }
        }

        // 실제 재생할 대사 목록
        private List<Dialogue> sequence = new List<Dialogue>();
        private List<Dialogue> happySequence = new List<Dialogue>();
        private List<Dialogue> badSequence = new List<Dialogue>();

        #endregion

        #region Unity Event Method
        // 연출 시작
        private void Start()
        {
            // 초기화
            isHappy = false;
            isBad = false;

            fog.SetActive(false);

            happyEndingTrigger.SetActive(false);
            badEndingTrigger.SetActive(false);

            // 마우스 보이게
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 스킵 가능 초기화
            isSkippable = false;

            //참조
            audioManager = AudioManager.Instance;
            typeWritter = sequenceText.GetComponent<TypewriterEffect>();

            //오프닝 연출 시작
            StartCoroutine(SequencePlay());
        }

        // 대사 순서 등록
        private void Awake()
        {
            sequence = new List<Dialogue>
            {
                new Dialogue(1, "이제 모든 방을 지나왔어.", 4f),
                new Dialogue(1, "그 안엔, 내가 외면했던 감정들이 가득했지.", 4f),
                new Dialogue(1, "하지만...", 4f),
                new Dialogue(1, "더 이상 피할 수 없어.", 4f),

                new Dialogue(0, "", 4f),

                new Dialogue(1, "이건 또... 뭐야...", 4f),
                new Dialogue(1, "네가 왜 거기 있어...?", 4f),

                new Dialogue(2, "기억나? 그날의 너.", 4f),
                new Dialogue(2, "내 마음이 무너져 내리고 있을 때, 넌 비웃었지.", 4f),
                new Dialogue(2, "아무 말도 하지 않고, 그냥... 웃었어.", 4f),
                new Dialogue(2, "그게 아무 일 아니라는 듯이.", 4f),
                new Dialogue(2, "그 순간, 난...", 4f),
                new Dialogue(2, "모든 것이 무너졌어.", 4f),

                new Dialogue(1, "설마... 너....", 4f),

                new Dialogue(2, "그래, 맞아. 난...", 4f),
                new Dialogue(2, "언제나 네 곁에 있었던...", 4f),
                new Dialogue(2, "네 소꿉친구야.", 4f),
                new Dialogue(2, "네가 외면했던, 그 아이의 슬픔, 외로움, 그리고 분노가...", 4f),
                new Dialogue(2, "결국, 나를 만들었어.", 4f),

                new Dialogue(1, "헉...", 4f),

                new Dialogue(2, "넌 날 해치지 않았다고 생각했겠지.", 4f),
                new Dialogue(2, "하지만 그 침묵이...", 4f),
                new Dialogue(2, "나를 여기까지 끌고 왔어.", 4f),

                new Dialogue(1, "미안해... 난 그저 장난이었을 뿐인데...", 4f),
                new Dialogue(1, "그런데 넌... 얼마나 무서웠을까...", 4f),
                new Dialogue(1, "다 내 잘못이야. 정말 미안해...", 4f),

                new Dialogue(2, "알아, 원망하지는 않아.", 4f),
                new Dialogue(2, "왜냐하면...", 4f),
                new Dialogue(2, "그저 네가, 진심으로 나를 바라봐 주길 바랐을 뿐이니까.", 4f),
                new Dialogue(2, "하지만 이제는, 선택해야 해.", 4f),
                new Dialogue(2, "다시 돌아가서 죄책감을 짊어지고 현실을 살아갈 건지,", 4f),
                new Dialogue(2, "아니면 여기 남아...", 4f),
                new Dialogue(2, "모든 걸 잊고 평생 죄책감을 짊어지고 살아갈 건지.", 4f),
                new Dialogue(2, "결정은 너에게 달렸어.", 4f),
            };

            happySequence = new List<Dialogue>
            {
                new Dialogue(1, "결심했어, 난...", 4f),
                new Dialogue(1, "현실로 돌아가겠어.", 4f),

                new Dialogue(2, "결국... 기억났구나.", 4f),
                new Dialogue(2, "그날, 내가 얼마나 무서웠는지.", 4f),

                new Dialogue(1, "그래... 처음엔 그냥 장난이라고만 생각했어.", 4f),
                new Dialogue(1, "하지만, 네 표정을 떠올리면서...", 4f),
                new Dialogue(1, "정말 많이 아팠겠구나 싶었어.", 4f),

                new Dialogue(2, "그럼, 이젠 도망치지 않을 수 있겠어?", 4f),

                new Dialogue(1, "응. 이제야 진심으로 마주할 수 있을 것 같아...", 4f),
                new Dialogue(1, "고맙고, 정말 미안해...", 4f),
            };

            badSequence = new List<Dialogue>
            {
                new Dialogue(1, "...난 그냥 여기에 남을래.", 4f),

                new Dialogue(2, "여기에... 남겠다고?", 4f),
                new Dialogue(2, "정말 그게 네 선택이야?", 4f),
                new Dialogue(2, "마지막으로 날 바라보는 것조차 어려운 거야?", 4f),

                new Dialogue(1, "미안해. 하지만 난...", 4f),
                new Dialogue(1, "돌아갈 용기가 없어...", 4f),
                new Dialogue(1, "널 보면...", 4f),
                new Dialogue(1, "내가 얼마나 비겁했는지 다시 떠올라...", 4f),

                new Dialogue(2, "괜찮아, 난 널 미워하지 않아, 정말로.", 4f),

                new Dialogue(1, "그래서 더 힘든 것 같아...", 4f),
                new Dialogue(1, "그 따뜻함조차, 나한텐 벌 같이 느껴져...", 4f),
                new Dialogue(1, "그냥 여기 남을게.", 4f),

                new Dialogue(2, "...", 4f),
                new Dialogue(2, "그렇게라도 네 마음이 편해진다면, 말리지 않을게.", 4f),

                new Dialogue(1, "정말... 미안해...", 4f),
            };
        }

        private void Update()
        {
            if (!isSequencePlaying)
                return;

            // 누가 말하고 있는지 체크
            switch (whoIsSaying)
            {
                case 0:
                    break;
                case 1:
                    sequenceText.color = new Color32(255, 230, 0, 255);
                    break;
                case 2:
                    sequenceText.color = Color.cyan;
                    break;
            }

            // 스킵 허용 가능하면, Shift + S 누르면 스킵 요청
            if (isSkippable && Keyboard.current.leftShiftKey.isPressed && Keyboard.current.sKey.wasPressedThisFrame)
            {
                isSkip = true;
            }
        }
        #endregion

        #region Custom Method
        // 돌아가기 선택했을 때
        public void OnHappyButtonClick()
        {
            // 선택지 숨기고 대사 처리
            whoIsSaying = 0;
            selectUI.SetActive(false);
            StartCoroutine(PlaySelectedSequence(happySequence, 1.5f));
            isHappy = true;
        }

        // 남기 선택했을 때
        public void OnBadButtonClick()
        {
            // 선택지 숨기고 대사 처리
            whoIsSaying = 0;
            selectUI.SetActive(false);
            StartCoroutine(PlaySelectedSequence(badSequence, 1.5f));
            isBad = true;
        }

        // 오프닝 연출 코루틴 함수
        IEnumerator SequencePlay()
        {
            // 플레이 캐릭터 비활성화
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            // 페이드 인, 안개 연출, Bgm 재생
            fader.FadeStart(17f);
            fog.SetActive(true);
            audioManager.PlayBgm("EndingRoomBgm");

            // 페이드 아웃 되기 전까지는 스킵 불가능하게 대기
            isSkippable = false;
            StartCoroutine(EnableSkipAfterDelay(17f));

            // 대사 순서대로 출력
            foreach (var line in sequence)
            {
                if (isSkip)
                    break;

                whoIsSaying = line.speaker;
                typeWritter.StartTyping(line.line);
                yield return new WaitForSeconds(line.waitTime);
            }

            isSequencePlaying = false;
            typeWritter.ClearText();
            isSkip = false;

            // 대사 텍스트 및 누가 말하는지 숨기기
            sequenceText.text = "";

            // 선택지 보이기
            selectUI.SetActive(true);
        }

        // 스킵 가능 여부 제어
        IEnumerator EnableSkipAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            isSkippable = true;
        }

        // 선택지 연출
        IEnumerator PlaySelectedSequence(List<Dialogue> selectedSequence, float delay)
        {
            isSequencePlaying = true;

            // 선택 직후 잠시 기다림
            yield return new WaitForSeconds(delay);

            // 분기별 대사 출력
            foreach (var line in selectedSequence)
            {
                if (isSkip)
                    break;

                whoIsSaying = line.speaker;
                typeWritter.StartTyping(line.line);
                yield return new WaitForSeconds(line.waitTime);
            }

            // 대사 숨김
            isSequencePlaying = false;
            sequenceText.text = "";
            isSkip = false;

            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = true;

            if (isHappy)
            {
                happyEndingAnimator.SetTrigger("Happy");
                happyLightAnimator.SetTrigger("HLight");
                happyEndingTrigger.SetActive(true);

                audioManager.Play("EndingOpenDoor");
            }
            else if (isBad)
            {
                badEndingAnimator.SetTrigger("Bad");
                badLightAnimator.SetTrigger("BLight");
                badEndingTrigger.SetActive(true);

                audioManager.Play("EndingOpenDoor");
            }
        }

        // Main Menu 버튼을 눌렀을 때 호출되는 함수
        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        #endregion
    }
}