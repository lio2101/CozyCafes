using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class Character : MonoBehaviour
{
    private int sympathy;

    private Image currentSprite;
    private CharacterData data;

    private bool isTalking;
    private bool drinkReceived;

    public CharacterData Data { get { return data; } set { } }
    public bool IsTalking { get { return isTalking; } set { isTalking = value; } }
    public bool DrinkReceived => drinkReceived;

    void Awake()
    {
        currentSprite = GetComponent<Image>();
    }

    // Public

    public void InitCharacter(CharacterData givenData)
    {
        drinkReceived = false;
        data = givenData;
        ChangeExpression(ExpressionType.Neutral);
        StartCoroutine(SpawnRoutine());
    }

    public void DestroyCharacter(bool isLast)
    {
        StartCoroutine(DestroyRoutine(isLast));
    }


    public List<string> GetConversation(bool hasOrdered)
    {
        List<string> newConvo = new List<string>();

        if (!hasOrdered)
        {
            //Greeting and Order
            data.IsReturning = false;
            if (data.IsReturning)
            {
                //Not first Visit

                //calc sympathy
                newConvo.Add(GetLine(GreetingType.GreetingLowSym));
                newConvo.Add(GetLine(OrderType.OrderWithHint));
            }
            else
            {
                //First Visit
                newConvo.Add(GetLine(GreetingType.FirstGreeting));
                newConvo.Add(GetLine(OrderType.FirstOrder));
                data.IsReturning = true;
            }
        }
        else
        {
            //Feedback
            BeverageData charDrink = data.FavoriteDrink;
            BeverageData givenDrink = Beverage.ActiveDrink.BeverageData;

            List<string> feedbackRoul = new List<string>();

            if (charDrink.Roast < givenDrink.Roast)
            {
                feedbackRoul.Add(GetLine(FeedbackType.TooDarkRoast));
            }
            if (charDrink.Roast > givenDrink.Roast)
            {
                feedbackRoul.Add(GetLine(FeedbackType.TooLightRoast));
            }
            if (charDrink.Milk < givenDrink.Milk)
            {
                feedbackRoul.Add(GetLine(FeedbackType.TooLittleMilk));
            }
            if (charDrink.Milk > givenDrink.Milk)
            {
                feedbackRoul.Add(GetLine(FeedbackType.TooMuchMilk));
            }

            if (charDrink.Flavor != givenDrink.Flavor)
            {
                string str;
                if (givenDrink.Flavor == 0)
                    str = GetLine(FeedbackType.NoFlavor);

                else
                    str = GetLine(FeedbackType.WrongFlavor).Replace("[flavor]", charDrink.Flavor.ToString());

                feedbackRoul.Add(str);
            }

            if (feedbackRoul.Count != 0)
            {
                string feedback = feedbackRoul[UnityEngine.Random.Range(0, feedbackRoul.Count)];
                newConvo.Add(feedback);
            }

            if (charDrink.Equals(givenDrink))
            {
                //AllCorrect
                newConvo.Add(GetLine(FeedbackType.CorrectDrink));
            }
            //End
            newConvo.Add(GetGoodbye());
            drinkReceived = true;

        }
        return newConvo;
    }

    public void ChangeExpression(ExpressionType type)
    {
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

    private string GetGoodbye()
    {
        return data.Goodbye;
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
        CharacterManager.Instance.ToggleButton(true);
    }

    private IEnumerator DestroyRoutine(bool isLast)
    {
        float elapsedTime = 0;
        Color startColor = currentSprite.color;
        startColor.a = 1f;
        Color endColor = startColor;
        endColor.a = 0f;
        float dur = CharacterManager.Instance.AppearAnimationDuration;

        currentSprite.color = startColor;

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

        if (!isLast) { CharacterManager.Instance.NewCharacter(); }
        Destroy(this.gameObject);
    }

}
