using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class CharacterSel : MonoBehaviour
{
    public LevelLoader levelLoader;
    private int selectedCharacterIndex;
    private Color desiredColor = new Color(1, 0, 0, 0.63f);
    [Header("List of characters")]
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();
    
    [Header("IO Reference")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterSplash;
    [SerializeField] private Image backgroundColor;

    [Header("Sounds")]
    [SerializeField] private AudioClip arrowClickSFX;
    [SerializeField] private AudioClip cfmClickSFX;

    [Header("Tweaks")]
    [SerializeField] private float backgroundColorTransitionSpeed = 1f;

    [System.Serializable]
    public class CharacterSelectObject
    {
        public Sprite splash;
        public string characterName;
        public Color characterColor;
    }

    public void Confirm()
    {
        PlayerPrefs.SetInt("CharacterSelected",selectedCharacterIndex);
        if(PlayerPrefs.HasKey("CharacterSelected"))

        Debug.Log(string.Format("Character {0}:{1} has been chosen", selectedCharacterIndex, characterList[selectedCharacterIndex].characterName));
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        levelLoader.LoadMain();
    }
    public void LeftArrow()
    {
        selectedCharacterIndex--;
        if(selectedCharacterIndex<0)
            selectedCharacterIndex = characterList.Count - 1;
        UpdateCharacterSelectionUI();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }

    public void RightArrow()
    {
        selectedCharacterIndex++;
        if(selectedCharacterIndex == characterList.Count)
            selectedCharacterIndex = 0;
        UpdateCharacterSelectionUI();
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }
    private void UpdateCharacterSelectionUI()
    {
        desiredColor = characterList[selectedCharacterIndex].characterColor;
        characterSplash.sprite = characterList[selectedCharacterIndex].splash;
        characterName.text = characterList[selectedCharacterIndex].characterName;
    }

    private void start()
    {
        UpdateCharacterSelectionUI();
    }
    
    private void Update() 
    {
        backgroundColor.color = Color.Lerp(backgroundColor.color,desiredColor,Time.deltaTime*backgroundColorTransitionSpeed);
    }
}
