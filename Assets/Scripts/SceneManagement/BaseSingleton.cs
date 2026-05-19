using UnityEngine;

public class BaseSingleton : Singleton<BaseSingleton>
{
    // literally einfach nur damit es von singleton class das DontDestroyOnLoad inherited weils kein Parent hat xD
}
