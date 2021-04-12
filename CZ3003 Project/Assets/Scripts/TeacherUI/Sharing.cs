using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharing : MonoBehaviour
{
    string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

    string TWEET_LANGUAGE = "en";

    string textToDisplay = "I have made an assignment! Room ID: ";//change to room id and password later

    string AppID = "457206565578095";

    string Link = "https://google.com";//change to smthing else later

    string Caption = "Stuff";

    string Description = "Thank you Jezoz";//change to room id and password later

    public void shareTwitter()
    {
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(textToDisplay) + "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
    }
    public void shareFacebook()
    {
        //Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + AppID + "&link=" + Link + "&caption=" + Caption + "&description=" + Description);
        Application.OpenURL("https://www.facebook.com");
    }
}