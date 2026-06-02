using System.Threading;
using UnityEngine;

public class GameSingleInstance : MonoBehaviour
{
    private static Mutex mutex;

    void Awake()
    {
        bool createdNew;

        mutex = new Mutex(true, "Ger-Le-Boi_Flooded-Soul", out createdNew);

        if (!createdNew)
        {
            Application.Quit();
        }
    }

    void OnApplicationQuit()
    {
        mutex?.ReleaseMutex();
    }
}
