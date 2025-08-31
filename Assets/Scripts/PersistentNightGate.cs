using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersistentNightGate : Health
{
    [Header("Gate Settings")]
    public bool isOpen = false;
    public bool scheduledToOpen = false;
    public bool hasBeenDestroyed = false;

    [Header("UI References")]
    public static GameObject messageUI;
    public static TextMeshProUGUI messageText;

    private int dayWhenDestroyed = -1;
    private Collider gateCollider;
    private Renderer gateRenderer;
    private Color originalColor;
    private bool playerInRange = false;
    private PlayerAttack playerAttackScript;
    private TorchManager torchManager;
    public BrambleScript bramble;
    void Start()
    {
        InfoStart();

        gateCollider = GetComponent<Collider>();
        gateRenderer = GetComponent<Renderer>();
        originalColor = gateRenderer.material.color;

        if (messageUI == null)
        {
            CreateMessageUI();
        }
    }

    void Update()
    {
        InfoUpdate();

        // Check for attack while player is in range
        if (playerInRange && playerAttackScript != null && torchManager != null)
        {
            CheckForAttack();
        }

        // Check if gate should open
        if (scheduledToOpen && TimeControl.Instance != null)
        {
            int currentDay = TimeControl.Instance.CurDay;
            if (currentDay > dayWhenDestroyed)
            {
                OpenGate();
            }
        }
    }

    void CheckForAttack()
    {
        if (isOpen || scheduledToOpen || isDead) return;

        // Check if player is attacking and has torch power
        if (IsPlayerAttacking() && torchManager.Cur_Power > 0)
        {
            Take_Dmg(playerAttackScript.Damage);
        }
    }

    bool IsPlayerAttacking()
    {
        // Check if player is currently in attack state
        // You can modify this based on your attack system

        // Method 1: Check for mouse click/attack input
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            return true;
        }

        // Method 2: Check animator state (if you have attack animations)
        Animator playerAnimator = playerAttackScript.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") || stateInfo.IsName("Attacking"))
            {
                return true;
            }
        }

        // Method 3: Check for attack key input
        if (Input.GetKeyDown(KeyCode.Space)) // Space bar attack
        {
            return true;
        }

        return false;
    }

    // Called when player enters attack range (from child trigger)
    public void OnPlayerEnterAttackRange(Collider player)
    {
        if (isOpen || scheduledToOpen || isDead) return;

        playerAttackScript = player.GetComponentInParent<PlayerAttack>();
        torchManager = player.GetComponentInParent<TorchManager>();

        if (playerAttackScript != null && torchManager != null)
        {
            playerInRange = true;
            Debug.Log($"Player in range of gate {name}");
        }
    }

    // Called when player leaves attack range (from child trigger)
    public void OnPlayerExitAttackRange(Collider player)
    {
        playerInRange = false;
        playerAttackScript = null;
        torchManager = null;
        Debug.Log($"Player left range of gate {name}");
    }

    public override void Take_Dmg(float dmg)
    {
        if (IsInvul || isDead || isOpen || scheduledToOpen) return;

        Cur_Health = Mathf.Clamp(Cur_Health - dmg, 0, Max_Health);
        MakeInv(PostDmgInvulDur);

        Debug.Log($"Gate {name} took {dmg} damage! Health: {Cur_Health}/{Max_Health}");

        StartCoroutine(FlashDamage());

        if (Cur_Health <= 0)
        {
            Death();
        }
    }

    public override void Death()
    {
        if (isDead) return;

        base.Death();

        Debug.Log($"Gate {name} has been destroyed!");

        hasBeenDestroyed = true;
        scheduledToOpen = true;
        dayWhenDestroyed = TimeControl.Instance != null ? TimeControl.Instance.CurDay : 0;

        gateRenderer.material.color = Color.yellow;

        ShowMessage("The bramble gate resists the flame. I should come back tomorrow");
        bramble.LightUp();

        StartCoroutine(BurnEffect());
    }

    IEnumerator FlashDamage()
    {
        Color flashColor = Color.red;
        gateRenderer.material.color = flashColor;

        yield return new WaitForSeconds(0.1f);

        if (!isDead && !scheduledToOpen)
        {
            gateRenderer.material.color = originalColor;
        }
    }

    IEnumerator BurnEffect()
    {
        Vector3 originalScale = transform.localScale;

        float burnTime = 2f;
        for (float t = 0; t < burnTime; t += Time.deltaTime)
        {
            float progress = t / burnTime;
            float scaleMultiplier = 1f + Mathf.Sin(progress * 10f) * 0.05f;
            transform.localScale = originalScale * scaleMultiplier;
            yield return null;
        }

        transform.localScale = originalScale;
        gateRenderer.material.color = Color.yellow;
    }
    public bool IsFinal;
    void OpenGate()
    {
        if (IsFinal) { FindObjectOfType<Ending>().BurntDownFinalGate(); }
        isOpen = true;
        scheduledToOpen = false;

        ShowMessage("The gate burnt out");
        bramble.gameObject.SetActive(false);

        if (gateCollider != null)
            gateCollider.enabled = false;

        StartCoroutine(OpenAnimation());
    }

    IEnumerator OpenAnimation()
    {
        float openTime = 1.5f;
        Vector3 originalScale = transform.localScale;

        for (float t = 0; t < openTime; t += Time.deltaTime)
        {
            float progress = t / openTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            transform.Rotate(0, 180f * Time.deltaTime, 0);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    void CreateMessageUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        messageUI = new GameObject("GateMessagePanel");
        messageUI.transform.SetParent(canvas.transform, false);

        Image bgImage = messageUI.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);

        RectTransform panelRect = messageUI.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.85f);
        panelRect.anchorMax = new Vector2(0.5f, 0.85f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(500, 80);

        GameObject textGO = new GameObject("MessageText");
        textGO.transform.SetParent(messageUI.transform, false);

        messageText = textGO.AddComponent<TextMeshProUGUI>();
        messageText.text = "";
        messageText.fontSize = 28;
        messageText.color = Color.white;
        messageText.alignment = TextAlignmentOptions.Center;
        messageText.fontStyle = FontStyles.Bold;

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);

        messageUI.SetActive(false);
    }

    public static void ShowMessage(string message)
    {
        if (messageUI == null || messageText == null) return;

        messageText.text = message;
        messageUI.SetActive(true);

        PersistentNightGate anyGate = FindObjectOfType<PersistentNightGate>();
        if (anyGate != null)
        {
            anyGate.StartCoroutine(HideMessageAfterDelay());
        }
    }

    static IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(6f);

        if (messageUI != null)
            messageUI.SetActive(false);
    }

    public void ResetGate()
    {
        isOpen = false;
        scheduledToOpen = false;
        hasBeenDestroyed = false;
        isDead = false;
        dayWhenDestroyed = -1;
        playerInRange = false;

        Cur_Health = Max_Health;
        IsInvul = false;

        gameObject.SetActive(true);
        transform.localScale = Vector3.one;

        if (gateCollider != null)
            gateCollider.enabled = true;

        if (gateRenderer != null)
            gateRenderer.material.color = originalColor;
    }

    void OnDrawGizmos()
    {
        if (Max_Health > 0)
        {
            Vector3 healthBarPos = transform.position + Vector3.up * 4f;
            float healthPercent = Cur_Health / Max_Health;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(healthBarPos, new Vector3(3f, 0.3f, 0.1f));

            Gizmos.color = Color.green;
            Vector3 healthFillSize = new Vector3(3f * healthPercent, 0.3f, 0.1f);
            Gizmos.DrawCube(healthBarPos, healthFillSize);
        }

        Color stateColor = isOpen ? Color.green : (scheduledToOpen ? Color.yellow : (isDead ? Color.red : Color.gray));
        Gizmos.color = stateColor;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 3f, new Vector3(4f, 6f, 0.5f));
    }
}
