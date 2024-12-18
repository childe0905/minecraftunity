using UnityEngine;

namespace childe
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField, Header("移動速度")]
        private float moveSpeed = 5f; // 控制移動速度

        [SerializeField, Header("動畫參數")]
        private string[] parameters =
        {
            "向前走", "講話", "跳"
        };

        [SerializeField, Header("攝像機跟隨偏移")]
        private Vector3 cameraOffset = new Vector3(0, 10, -8); // 攝像機初始位置偏移

        private Animator ani; // 動畫控制器
        private Vector3 moveDirection; // 移動方向
        private Camera mainCamera; // 攝像機引用

        private void Awake()
        {
            ani = GetComponent<Animator>();

            // 獲取主攝像機
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("未找到主攝像機！請確認場景中有攝像機設置為 Main Camera。");
            }
        }

        private void Start()
        {
            // 重設角色位置為原點，排除位置問題
            transform.position = Vector3.zero;

            // 初始化攝像機位置
            if (mainCamera != null)
            {
                Vector3 desiredPosition = transform.position + cameraOffset;
                mainCamera.transform.position = desiredPosition;
                mainCamera.transform.LookAt(transform.position);
            }
        }

        private void Update()
        {
            HandleArrowKeyMovement();
            HandleTalkAction();
            HandleCameraMoveUp(); // 攝像機高度控制
        }

        private void LateUpdate()
        {
            HandleCameraFollow(); // 攝像機跟隨角色
        }

        private void HandleArrowKeyMovement()
        {
            moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow)) moveDirection += Vector3.forward;
            if (Input.GetKey(KeyCode.DownArrow)) moveDirection += Vector3.back;
            if (Input.GetKey(KeyCode.LeftArrow)) moveDirection += Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow)) moveDirection += Vector3.right;

            if (moveDirection != Vector3.zero)
            {
                // 移動角色
                transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);

                // 設定「向前走」動畫為連續播放
                ani.SetBool(parameters[0], true);

                // 角色朝向移動方向
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            else
            {
                // 停止播放「向前走」動畫
                ani.SetBool(parameters[0], false);
            }

            // 按空白鍵播放跳躍動畫
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ani.SetTrigger(parameters[2]);
            }
        }

        private void HandleTalkAction()
        {
            // 按下 Enter 鍵播放講話動畫
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ani.SetTrigger(parameters[1]);
            }
        }

        private void HandleCameraFollow()
        {
            if (mainCamera != null)
            {
                // 計算攝像機位置
                Vector3 desiredPosition = transform.position + cameraOffset;

                // 平滑過渡攝像機位置
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * 3f);

                // 設定攝像機看向角色
                mainCamera.transform.LookAt(transform.position);
            }
        }

        private void HandleCameraMoveUp()
        {
            // 按 Tab 鍵時調整攝像機高度
            if (Input.GetKey(KeyCode.Tab))
            {
                cameraOffset.y += Time.deltaTime * 2f; // 緩慢增加高度
                cameraOffset.y = Mathf.Clamp(cameraOffset.y, 10f, 30f); // 限制高度範圍
            }
        }
    }
}