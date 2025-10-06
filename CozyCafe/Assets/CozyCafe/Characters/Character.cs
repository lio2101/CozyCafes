using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    private int sympathy;

    private Image currentSprite;
    private CharacterData data;

    private bool isTalking;
    private bool isWaiting;

    public CharacterData Data { get { return data; } set { } }
    public bool IsTalking { get { return isTalking; } set { isTalking = value; } }

    void Awake()
    {
        currentSprite = GetComponent<Image>();
    }

    // Public

    public void InitCharacter(CharacterData givenData)
    {
        data = givenData;
        ChangeExpression(ExpressionType.Neutral);
        StartCoroutine(SpawnRoutine());
    }


    public List<string> GetConversation()
    {
        List<string> newConvo = new List<string>();

        newConvo.Add(GetLine(GreetingType.FirstGreeting));
        newConvo.Add(GetLine(OrderType.FirstOrder));

        return newConvo;
    }

    public void ChangeExpression(ExpressionType type)
    {
        Debug.Log("Change exp to " + type);
        currentSprite.sprite = data.Expressions.Where(i => i.Type == type).Select(i => i.Sprite).FirstOrDefault();
    }
    // Private

    private string GetLine(GreetingType type)
    {
        return data.Greetings.Where(i => i.Type == type).Select(i => i.Text).FirstOrDefault();
    }

    private string GetLine(OrderType type)
    {
        return data.Orders.Where(i => i.Type == type).Select(i => i.Text).FirstOrDefault();
    }

    private string GetLine(FeedbackType type)
    {
        return data.Feedbacks.Where(i => i.Type == type).Select(i => i.Text).FirstOrDefault();
    }

    private void SayStory()
    {

    }


    private IEnumerator SpawnRoutine()
    {
        CharacterManager.Instance.PlaySound();

        float elapsedTime = 0;
        Color startColor = currentSprite.color;
        startColor.a = 0f;
        Color endColor = startColor;
        endColor.a = 1f;
        float dur = CharacterManager.Instance.AppearAnimationDuration;

        currentSprite.color = startColor;
        yield return new WaitForSeconds(CharacterManager.Instance.TimeBeforeAppear);

        //Fade in
        while (elapsedTime < dur)
        {
            float t = elapsedTime / dur;
            currentSprite.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentSprite.color = endColor;

        yield return new WaitForSeconds(1);

        CharacterManager.Instance.ToggleConversationUI(false);
    }

}
