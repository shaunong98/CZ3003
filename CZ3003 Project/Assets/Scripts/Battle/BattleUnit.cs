using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] MonsterBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public Monster Monster { get; set; }

    public int Level {
        get { return level; }
    }

    public bool IsPlayerUnit { 
        get { return isPlayerUnit; }
    }

    public BattleHud Hud {
        get { return hud; }
    }

    Image image;
    Vector3 originalPos;
    Color originalColor;
        
    public void Awake() {
        image = GetComponent<Image>();
        image.enabled = false;
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void SetUp() {
        image.enabled = true;
        Monster = new Monster(_base, level);
        if (isPlayerUnit) {
            Debug.Log("back sprite");
            image.sprite = Monster.Base.BackSprite;
        }
        else
            
            image.sprite = Monster.Base.FrontSprite;
        image.color = originalColor;

        hud.SetData(Monster);
        
        PlayerEnterAnimation();
    }

    public void PlayerEnterAnimation() {
        if (isPlayerUnit) {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayerAttackAnimation() {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit) {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayerHitAnimation() {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayerFaintAnimation() {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
