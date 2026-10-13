using UnityEngine;

public class ExternalLinkOpener : MonoBehaviour
{
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}
