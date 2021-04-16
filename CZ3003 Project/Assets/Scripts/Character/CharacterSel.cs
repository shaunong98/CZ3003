// Authors: Daryl Neo
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

    // Method to execute when click Confirm Button
    public void Confirm()
    {
        // Store Player's selection into PlayerPref
        PlayerPrefs.SetInt("CharacterSelected",selectedCharacterIndex);

        // Plays sfx
        AudioManager.Instance.PlaySFX(cfmClickSFX);

        // Transition Scene to Main Menu
        levelLoader.LoadMain();
    }
        
    // Method to execute when click Left Button
    public void LeftArrow()
    {
        // Cycles through character list
        selectedCharacterIndex--;
        if(selectedCharacterIndex<0)
            selectedCharacterIndex = characterList.Count - 1;

        // Switch to next character on list
        UpdateCharacterSelectionUI();

        // Plays sfx
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }

    // Method to execute when click Right Button
    public void RightArrow()
    {
        // Cycles through character list
        selectedCharacterIndex++;
        if(selectedCharacterIndex == characterList.Count)
            selectedCharacterIndex = 0;
        
        // Switch to next character on list
        UpdateCharacterSelectionUI();

        // Plays sfx
        AudioManager.Instance.PlaySFX(arrowClickSFX);
    }

    // UpdateCharacterSelection method
    private void UpdateCharacterSelectionUI()
    {
        // Changes color to unique color tied to character
        desiredColor = characterList[selectedCharacterIndex].characterColor;

        // Changes character image
        characterSplash.sprite = characterList[selectedCharacterIndex].splash;

        // Changes character name
        characterName.text = characterList[selectedCharacterIndex].characterName;
    }

    private void start()
    {
        UpdateCharacterSelectionUI();
    }
    
    private void Update() 
    {
        // Creates a animation to change background colors between each character
        backgroundColor.color = Color.Lerp(backgroundColor.color,desiredColor,Time.deltaTime*backgroundColorTransitionSpeed);
    }
}
