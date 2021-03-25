using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Button RegisterButton;

    // Start is called before the first frame update
    void Start()
    {
        
        LoginButton.onClick.AddListener(()=>
        {
            StartCoroutine(Main.Instance.Web.Login(UsernameInput.text,PasswordInput.text));
        });
        
        RegisterButton.onClick.AddListener(()=>
        {
            StartCoroutine(Main.Instance.Web.Register(UsernameInput.text,PasswordInput.text));
        });
    }
}
