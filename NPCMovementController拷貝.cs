using UnityEngine;
using TMPro;

public class NPCMovementController : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float moveSpeed = 2.0f;

    [SerializeField, Header("對話文字顯示欄位")]
    private TMP_Text dialogueText;

    private Animator ani; // 動畫控制器
    private Vector3 movement;

    private string walkMessage = "正在行走..."; // 行走文字
    private string jumpMessage = "跳起來了！";  // 跳躍文字

    private void Awake()
    {
        ani = GetComponent<Animator>(); // 獲取 Animator
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    /// <summary>
    /// 處理方向鍵輸入，控制 NPC 移動
    /// </summary>
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // 左右
        float vertical = Input.GetAxisRaw("Vertical");     // 上下

        movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude > 0)
        {
            // 移動 NPC 位置
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            // 讓 NPC 面向移動方向
            transform.rotation = Quaternion.LookRotation(movement);

            // 顯示行走文字
            if (dialogueText != null)
            {
                dialogueText.text = walkMessage;
            }
        }
        else
        {
            // 停止移動時清空對話文字
            if (dialogueText != null)
            {
                dialogueText.text = "";
            }
        }
    }

    /// <summary>
    /// 檢測空白鍵並觸發跳躍動畫
    /// </summary>
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 播放跳躍動畫
            ani.SetTrigger("跳");

            // 顯示跳躍文字
            if (dialogueText != null)
            {
                dialogueText.text = jumpMessage;
            }
        }
    }
}