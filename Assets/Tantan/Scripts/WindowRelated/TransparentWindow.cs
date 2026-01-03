using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint SetWindowPos(IntPtr hWnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    private struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopHeight;
        public int cxBottomHeight;
    }

    struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [DllImport("user32.dll")]
    private static extern bool SystemParametersInfo(
        int uiAction,
        int uiParam,
        ref RECT pvParam,
        int fWinIni
    );

    [DllImport("dwmapi.dll")]
    private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

    #region Transparent Window Constants
    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    #endregion

    #region WWorking Area Constants
    const int SPI_GETWORKAREA = 0x0030;
    const uint SWP_NOACTIVATE = 0x0010;
    const uint SWP_SHOWWINDOW = 0x0040;
    #endregion

    #region Borderless Window Constants
    const int GWL_STYLE = -16;

    const uint WS_CAPTION = 0x00C00000;
    const uint WS_THICKFRAME = 0x00040000;
    const uint WS_BORDER = 0x00800000;
    const uint WS_SYSMENU = 0x00080000;

    const uint SWP_FRAMECHANGED = 0x0020;
    #endregion


    IntPtr hWnd;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    [SerializeField] LayerMask gameObjectMask;

    IEnumerator Start()
    {
#if !UNITY_EDITOR
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;

        yield return null;

        Rect workArea = GetWorkArea();

        Screen.SetResolution(
            (int)workArea.width,
            (int)workArea.height,
            false
        );

        yield return null;

        hWnd = GetActiveWindow();

        uint style = GetWindowLong(hWnd, GWL_STYLE);

        // Remove borders
        style &= ~WS_CAPTION;
        style &= ~WS_THICKFRAME;
        style &= ~WS_BORDER;
        style &= ~WS_SYSMENU;

        SetWindowLong(hWnd, GWL_STYLE, style);

        SetWindowPos(
            hWnd,
            HWND_TOPMOST,
            (int)workArea.x,
            (int)workArea.y,
            (int)workArea.width,
            (int)workArea.height,
            SWP_NOACTIVATE | SWP_SHOWWINDOW
        );

        Margins margins = new Margins { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
#endif
        Application.runInBackground = true;
        yield break;
    }


    void Update()
    {
        Debug.Log(Physics2D.OverlapPoint(GetWorldMouse()));
        SetClickThrough(!(IsUIHit || IsWorldHit));
    }

    bool IsWorldHit
    {
        get => Physics2D.OverlapPoint(GetWorldMouse(), gameObjectMask) != null;
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

    Rect GetWorkArea()
    {
        RECT r = new RECT();
        SystemParametersInfo(SPI_GETWORKAREA, 0, ref r, 0);

        return new Rect(
            r.left,
            r.top,
            r.right - r.left,
            r.bottom - r.top
        );
    }
}