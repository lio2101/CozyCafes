using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enums
public enum ExpressionType
{
    Neutral,
    Talking,
    Happy
}
public enum GreetingType
{
    FirstGreeting,
    GreetingLowSym,
    Greeting,MedSym,
    GreetingHighSym,
}

public enum OrderType
{
    FirstOrder,
    OrderWithHint,
    LastCorrect,
    LastTwoCorrect
}

public enum FeedbackType
{
    CorrectDrink,
    CorrectAgain,
    WrongRoast,
    WrongMilk,
    WrongFlavor
}

// Structs
[Serializable]
public struct Expression
{
    [SerializeField] private ExpressionType type;
    [SerializeField] private Sprite sprite;
    public ExpressionType Type => type;
    public Sprite Sprite => sprite;
}
[Serializable]
public struct Greeting
{
    [SerializeField] private GreetingType type; [SerializeField] private string text;
    [SerializeField] public GreetingType Type => type;
    public string Text => text;
}
[Serializable]
public struct Order
{
    [SerializeField] private OrderType type;
    [SerializeField] private string text;
    public OrderType Type => type;
    public string Text => text;
}
[Serializable]
public struct Feedback
{
    [SerializeField] private FeedbackType type;
    [SerializeField] private string text;
    public FeedbackType Type => type;
    public string Text => text;
}


[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    // Fields

    [SerializeField] private string charName;
    [SerializeField] private BeverageData favoriteDrink;

    [Header("Sprites")]
    [SerializeField] private List<Expression> expressions = new();

    [Header("Dialogue")]
    [SerializeField] private List<Greeting> greetings = new();
    [SerializeField] private List<Order> orders = new();
    [SerializeField] private List<Feedback> feedbacks = new();
    [SerializeField] string[] story;

    private int storyprogress = 0;


    // Properties

    public string Name => charName;
    public BeverageData FavoriteDrink => favoriteDrink;
    public List<Expression> Expressions => expressions;
    public List<Greeting> Greetings => greetings;
    public List<Order> Orders => orders;
    public List<Feedback> Feedbacks => feedbacks;
    public string[] Story => story;

    public int StoryProgress {  get { return storyprogress; } set {  storyprogress = value; } }

}
