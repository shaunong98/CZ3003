using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Login("testuser1","123456"));
        //StartCoroutine(Register("testuser2","123456"));

    }

    public IEnumerator Login(string username,string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest webRequest  = UnityWebRequest.Post("http://localhost/CZ3003%20Project/Login.php", form))
        {
            yield return webRequest .SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest .error);
            }
            else
            {
                Debug.Log(webRequest .downloadHandler.text);
            }
        }
    }

    public IEnumerator Register(string username,string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest webRequest  = UnityWebRequest.Post("http://localhost/CZ3003%20Project/Register.php", form))
        {
            yield return webRequest .SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest .error);
            }
            else
            {
                Debug.Log(webRequest .downloadHandler.text);
            }
        }
    }
}
