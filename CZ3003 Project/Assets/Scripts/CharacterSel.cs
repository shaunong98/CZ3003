using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterSel : MonoBehaviour
{
    private int selectedCharacterIndex;
    private Color desiredColor;
    [Header("List of characters")]
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();
    
    [Header("IO Reference")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterSplash;
    [SerializeField] private Image backgroundColor;

    [Header("Sounds")]
    [SerializeField] private AudioClip arrowClickSFX;
    [SerializeField] private AudioClip characterSelectMusic;


    [Header("Tweaks")]
    [SerializeField] private float backgroundColorTransitionSpeed = 1f;

    [System.Serializable]
    public class CharacterSelectObject
    {
        public Sprite splash;
        public string characterName;
        public Color characterColor;
    }

    public void LeftArrow()
    {
        selectedCharacterIndex--;
        if(selectedCharacterIndex<0)
            selectedCharacterIndex = characterList.Count - 1;
        UpdateCharacterSelectionUI();
    }

    public void RightArrow()
    {
        selectedCharacterIndex++;
        if(selectedCharacterIndex == characterList.Count)
            selectedCharacterIndex = 0;
        UpdateCharacterSelectionUI();
    }
    private void UpdateCharacterSelectionUI()
    {
        characterSplash.sprite = characterList[selectedCharacterIndex].splash;
        characterName.text = characterList[selectedCharacterIndex].characterName;
        desiredColor = characterList[selectedCharacterIndex].characterColor;
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
