using UnityEngine;
using UnityEngine.InputSystem;

namespace Remnants
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        private CharacterController controller;
        public Animator animator;
        //이동
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float runSpeed = 9f;
        [SerializeField] private float acceleration = 9f;   //가속도

        //점프 높이
        [SerializeField] private float jumpHeight = 1f;

        //중력
        private float gravity = -9.81f;

        //입력 - 이동방향
        private Vector2 inputMove;
        [SerializeField] private Vector3 velocity;       //중력 계산에 의한 이동 속도
        private bool isRunning;             //shift키 눌렀는지 여부
        private float currentSpeed;

        //그라운드 체크
        public Transform groundCheck;   //발 바닥 위치
        [SerializeField] private float checkRange = 0.2f;    //체크 하는 구의 반경
        [SerializeField] private LayerMask groundMask;       //그라운드 레이어 판별

        #endregion

        #region Property
        //Object가 활성화될 때 InputSystem 켜기
        //private void OnEnable() => controls.Enable();
        //Object가 비활성화될 때 InputSystem 끄기
        //private void OnDisable() => controls.Disable();
        #endregion

        #region Unity Event Method
        private void Start()
        {
            animator.GetComponent<Animator>();
            controller = this.GetComponent<CharacterController>();
            currentSpeed = 0f;
        }

        private void Update()
        {
            bool isGrounded = GroundCheck();
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -10f;
            }

            HandleMovement();
            HandleAnimation();
        }

        #endregion

        #region custom Method
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isRunning = true;
            }
            else if (context.canceled)
            {
                isRunning = false;
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && GroundCheck())
            {
                //점프 높이만큼 뛰기 위한 속도 구하기
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }
        //그라운드 체크
        bool GroundCheck()
        {
            return Physics.CheckSphere(groundCheck.position, checkRange, groundMask);
        }
        private void HandleMovement()
        {
            //이동키(wasd) 구현
            Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

            // 입력이 없으면 속도 0, 이동도 하지 않음
            if (inputMove.magnitude < 0.01f)
            {
                currentSpeed = 0f;
                velocity.y += gravity * Time.deltaTime;
                controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
                return;
            }

            //걷기 / 뛰기 속도 판별
            float targetSpeed = isRunning ? runSpeed : walkSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

            //카메라 바라보는 방향으로 이동
            Vector3 move = transform.TransformDirection(moveDir) * currentSpeed;

            //이동
            controller.Move(moveDir * Time.deltaTime * currentSpeed);

            //중력에 따른 y축 이동
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        private void HandleAnimation()
        {
            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("MoveX", inputMove.x);
            animator.SetFloat("MoveY", inputMove.y);
        }
        #endregion
    }

}