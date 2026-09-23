using UnityEngine;

public class ExternalLinkOpener : MonoBehaviour
{
    const string url = "https://docs.google.com/forms/d/e/1FAIpQLSd1WPxyDiPz-9F7pL27UYW8njabYy21QbdUnlACoCopadp3Fg/viewform";
    const string playTimeEntry = "entry.1032684454";

    public void OpenLink()
    {
        float playTime = Time.realtimeSinceStartup;

        int playHours = (int)playTime / 3600;
        int playMin = ((int)playTime % 3600) / 60;
        int playSec = (int)playTime % 60;

        string playTimeString = $"{playHours:D2}:{playMin:D2}:{playSec:D2}";

        string formURL = $"{url}?usp=pp_url&{playTimeEntry}={UnityEngine.Networking.UnityWebRequest.EscapeURL(playTimeString)}";

        Application.OpenURL(formURL);
    }
}
