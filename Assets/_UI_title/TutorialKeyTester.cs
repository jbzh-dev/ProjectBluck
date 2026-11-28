using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialKeyTester : MonoBehaviour
{
    [System.Serializable]
    public class KeyIcon
    {
        public Key key;
        public Image icon;
    }

    [Header("Keyboard")]
    public KeyIcon[] keys;

    [Header("Mouse")]
    public Image leftClickIcon;    // พื้นหลังของไอคอนคลิกซ้าย
    public Image rightClickIcon;   // ไม่ใช้ก็ปล่อยว่างได้

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color pressedColor = Color.yellow;

    void Update()
    {
        if (Keyboard.current != null)
        {
            foreach (var k in keys)
            {
                if (k.icon == null) continue;
                k.icon.color = Keyboard.current[k.key].isPressed ? pressedColor : normalColor;
            }
        }

        if (Mouse.current != null)
        {
            if (leftClickIcon != null)
                leftClickIcon.color = Mouse.current.leftButton.isPressed ? pressedColor : normalColor;

            if (rightClickIcon != null)
                rightClickIcon.color = Mouse.current.rightButton.isPressed ? pressedColor : normalColor;
        }
    }
}