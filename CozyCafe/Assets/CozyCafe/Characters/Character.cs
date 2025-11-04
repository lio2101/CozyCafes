using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
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
            Debug.Log("Visit: " + data.VisitAmount);
            //Greeting and Order
            if (data.VisitAmount > 0)
            {
                //Not first Visit > calc friends
                if (data.VisitAmount > 3 & data.CorrectAmount > 1)
                    newConvo.Add(GetLine(GreetingType.FriendGreeting));
                else
                    newConvo.Add(GetLine(GreetingType.NormalGreeting));

                //Order calc
                if (data.LastCorrect)
                    if (data.CorrectAmount > 3)
                        newConvo.Add(GetLine(OrderType.Regular));
                    else
                        newConvo.Add(GetLine(OrderType.LastCorrect));

                else
                    newConvo.Add(GetLine(OrderType.OrderWithHint));
            }
            else
            {
                //First Visit
                newConvo.Add(GetLine(GreetingType.FirstGreeting));
                newConvo.Add(GetLine(OrderType.FirstOrder));
            }
            data.VisitAmount++;
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
                feedbackRoul.Add(GetLine(FeedbackType.TooMuchMilk));
            }
            if (charDrink.Milk > givenDrink.Milk)
            {
                feedbackRoul.Add(GetLine(FeedbackType.TooLittleMilk));
            }

            if (charDrink.Flavor != givenDrink.Flavor)
            {
                Debug.Log(charDrink.Flavor + " " + givenDrink.Flavor);
                string str;
                if (givenDrink.Flavor == 0)
                    str = GetLine(FeedbackType.NoFlavor);

                else
                    str = GetLine(FeedbackType.WrongFlavor).Replace("[flavor]", givenDrink.Flavor.ToString());

                feedbackRoul.Add(str);
            }

            if (feedbackRoul.Count != 0)
            {
                string feedback = feedbackRoul[UnityEngine.Random.Range(0, feedbackRoul.Count)];
                newConvo.Add(feedback);
                data.LastCorrect = false;

                //foreach (string str in feedbackRoul)
                //{
                //    Debug.Log(str);
                //}
            }

            else
            {
                //AllCorrect
                if (data.LastCorrect)
                    newConvo.Add(GetLine(FeedbackType.CorrectAgain));
                else
                    newConvo.Add(GetLine(FeedbackType.CorrectDrink));

                data.LastCorrect = true;
                data.CorrectAmount++;
            }

            //Story
            if (
                (data.VisitAmount == 3) ||
                (data.VisitAmount > 3 && data.CorrectAmount > 0) ||
                (data.VisitAmount > 4 && data.CorrectAmount > 3) ||
                (data.VisitAmount > 3 && data.CorrectAmount > 4)
                )
            {
                newConvo.Add(GetStory());
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

    public void Jump()
    {
        StartCoroutine(SpriteJumpRoutine());
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

    private string GetStory()
    {
        Debug.Log("adding story part " + data.StoryProgress);
        string s = data.Story[data.StoryProgress];
        data.StoryProgress++;
        return s;
    }

    private string GetGoodbye()
    {
        return data.Goodbye;
    }

    private IEnumerator SpriteJumpRoutine()
    {
        float jumpDuration = 0.3f;
        int jumpHeight = 15;

        RectTransform rect = GetComponent<RectTransform>();
        Vector3 startPos = rect.localPosition;

        float elapsedTime = 0;
        while (elapsedTime < jumpDuration / 2f)
        {
            float t = elapsedTime / (jumpDuration / 2f);
            float newY = Mathf.Lerp(startPos.y, startPos.y + jumpHeight, t);
            rect.localPosition = new Vector3(startPos.x, newY, startPos.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // Fall down
        while (elapsedTime < jumpDuration / 2f)
        {
            float t = elapsedTime / (jumpDuration / 2f);
            float newY = Mathf.Lerp(startPos.y + jumpHeight, startPos.y, t);
            rect.localPosition = new Vector3(startPos.x, newY, startPos.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rect.localPosition = startPos; // ensure it resets exactly
    }


    private IEnumerator SpawnRoutine()
    {
        float elapsedTime = 0;
        Color startColor = currentSprite.color;
        startColor.a = 0f;
        Color endColor = startColor;
        endColor.a = 1f;
        currentSprite.color = startColor;

        yield return new WaitForSeconds(1f);

        CharacterManager.Instance.PlaySound();

        yield return new WaitForSeconds(3f);

        float dur = CharacterManager.Instance.AppearAnimationDuration;
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

        if (!isLast)
        {
            GameManager.Instance.NewTimeWindow();

            CharacterManager.Instance.NewCharacter();
            Destroy(this.gameObject);
        }

    }
}
