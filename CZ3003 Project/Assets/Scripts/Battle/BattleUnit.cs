// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    // Reference to MonsterBase
    [SerializeField] MonsterBase _base;

    // Boolean variable to check if it is a player or trainer.
    [SerializeField] bool isPlayerUnit;

    // Reference variable to BattleHud.
    [SerializeField] BattleHud hud;

    // Reference object to image of the player in the battlescene.
    Image image;

    // Reference to original position of the player image in the battlescene.
    Vector3 originalPos;

    // Reference to original color of the player image in the battlescene.
    Color originalColor;

    // Monster variable.
    public Monster Monster { get; set; }

    // Getter method of isPlayerUnit.
    public bool IsPlayerUnit { 
        get { return isPlayerUnit; }
    }

    // Getter method of isPlayerUnit.
    public BattleHud Hud {
        get { return hud; }
    }
    
    // Awake function that is called whenever this class is instantiated.
    public void Awake() {
        image = GetComponent<Image>();
        image.enabled = false;
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    // Method that sets up the player and trainer battlescene 
    public void SetUp(bool isPlayerUnit) {
        image.enabled = false;
        image.enabled = true;
        Monster = new Monster(_base);
        if (isPlayerUnit) {
            Debug.Log("back sprite");
            image.sprite = Monster.Base.BackSprite;
        }
        else

            image.sprite = Monster.Base.FrontSprite;
        image.color = originalColor;

        hud.SetData(Monster, isPlayerUnit);
        
        PlayerEnterAnimation();
    }

    // Method that causes the player/trainer image to be disabled.
    public void disableImage() {
        image.enabled = false;
    }

    // Method that shows animation of the player/trainer entering the battlescene.
    public void PlayerEnterAnimation() {
        if (isPlayerUnit) {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    // Method that shows animation of the player/trainer attacking
    public void PlayerAttackAnimation() {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit) {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    // Method that shows animation of player/trainer getting hit.
    public void PlayerHitAnimation() {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    // Method that shows animation of player/trainer leaving the battlescene as they faint.
    public void PlayerFaintAnimation() {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
