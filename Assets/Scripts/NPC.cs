using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact();
}

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    [SerializeField] private float interactionRange = 1.5f;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private float previousTimeScale = 1f;
    private Transform playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("NPC could not find an object tagged Player.", this);
            return;
        }

        playerTransform = player.transform;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && CanInteract())
        {
            Interact();
        }
    }

    public bool CanInteract()
    {
        return isDialogueActive || (Time.timeScale > 0f && IsPlayerInRange());
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

        if (dialogueData == null)
        {
            Debug.LogError("NPC requires dialogue data.", this);
            return;
        }

        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else
        {
            NextLine();
        }
    }

    private void StartDialogue()
    {
        if (dialoguePanel == null || dialogueText == null || nameText == null)
        {
            Debug.LogError("NPC dialogue panel, text, and name references must be assigned.", this);
            return;
        }

        if (dialogueData.dialogueLines == null || dialogueData.dialogueLines.Length == 0)
        {
            Debug.LogWarning("NPC dialogue data contains no dialogue lines.", this);
            return;
        }

        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.text = dialogueData.npcName;

        dialoguePanel.SetActive(true);
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueData.dialogueLines[dialogueIndex];
            isTyping = false;
            return;
        }

        dialogueIndex++;
        if (dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        string line = dialogueData.dialogueLines[dialogueIndex];

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, dialogueData.typingSpeed));
        }

        isTyping = false;

        if (dialogueData.autoProgressLines != null
            && dialogueData.autoProgressLines.Length > dialogueIndex
            && dialogueData.autoProgressLines[dialogueIndex])
        {
            int currentLine = dialogueIndex;
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, dialogueData.autoProgressDelay));
            if (isDialogueActive && !isTyping && dialogueIndex == currentLine)
            {
                NextLine();
            }
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        if (isDialogueActive)
        {
            Time.timeScale = previousTimeScale;
        }

        isDialogueActive = false;
        isTyping = false;

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private bool IsPlayerInRange()
    {
        return playerTransform != null
            && Vector2.Distance(transform.position, playerTransform.position) <= interactionRange;
    }

    private void OnDisable()
    {
        if (isDialogueActive)
        {
            EndDialogue();
        }
    }
}
