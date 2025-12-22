using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex,uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint SetWindowPos(IntPtr hWnd, IntPtr hwndInsertAfter, int x ,int y ,int cx , int cy , uint uFlags);

    private struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopHeight;
        public int cxBottomHeight;
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    IntPtr hWnd;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    [SerializeField] LayerMask gameObjectMask;

    void Start()
    {
#if !UNITY_EDITOR
        hWnd = GetActiveWindow();

        Margins margins = new Margins() {cxLeftWidth = -1};

        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
#endif
        Application.runInBackground = true;
    }

    void Update() 
    {
        Debug.Log(Physics2D.OverlapPoint(GetWorldMouse()));
        SetClickThrough(!IsUIHit || !IsWorldHit);
    }

    bool IsWorldHit
    { 
        get => Physics2D.OverlapPoint(GetWorldMouse(),gameObjectMask) != null;
    }

    bool IsUIHit
    {
        get
        {
            if (EventSystem.current == null)
                return false;

            var data = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);

            return results.Count > 0;
        }
    }


    Vector3 GetWorldMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        return mouseWorldPos;
    }

    void SetClickThrough(bool canClickThrough)
    {
        if (canClickThrough)
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        else
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
    }
}
