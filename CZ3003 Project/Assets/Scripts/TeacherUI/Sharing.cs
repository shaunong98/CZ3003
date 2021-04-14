using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Done by Jun Hao and Zhi Fah
public class Sharing : MonoBehaviour
{
    //This is the twitter address
    string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    //This is the twitter language
    string TWEET_LANGUAGE = "en";
    //This is the text to display for the Tweet
    string textToDisplay = "I have made an assignment! Room ID: ";//change to room id and password later
    //The method opens twitter in a browser for the user to tweet
    public void shareTwitter()
    {
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(textToDisplay) + "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
    }
    //The method opens facebook in a browser for the user to post
    public void shareFacebook()
    {
        Application.OpenURL("https://www.facebook.com");
    }
    //This method opens the forum
    public void Quora()
    {
        Application.OpenURL("https://softwaremon.quora.com/");
    }
}