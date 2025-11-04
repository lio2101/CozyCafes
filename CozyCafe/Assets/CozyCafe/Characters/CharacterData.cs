using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
    NormalGreeting,
    FriendGreeting
}

public enum OrderType
{
    FirstOrder,
    OrderWithHint,
    LastCorrect,
    Regular
}

public enum FeedbackType
{
    CorrectDrink,
    CorrectAgain,
    TooLightRoast,
    TooDarkRoast,
    TooLittleMilk,
    TooMuchMilk,
    WrongFlavor,
    NoFlavor
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
    [Header("Info")]
    [TextArea(2, 10)][SerializeField] private string notes;

    [SerializeField] private string charName;
    [SerializeField] private BeverageData favoriteDrink;

    [Header("Stats (READ ONLY)")]
    [SerializeField] private int storyprogress = 0;
    [SerializeField] private int visitAmount = 0;
    [SerializeField] private int correctAmount = 0;

    [Header("Sprites")]
    [SerializeField] private List<Expression> expressions = new();

    [Header("Dialogue")]
    [SerializeField] private List<Greeting> greetings = new();
    [SerializeField] private List<Order> orders = new();
    [SerializeField] private List<Feedback> feedbacks = new();
    [SerializeField] private string goodbye;
    [TextArea(5, 10)][SerializeField] private string[] story = new string[5];


    private bool lastCorrect;


    // Properties

    public string Name => charName;
    public BeverageData FavoriteDrink => favoriteDrink;
    public List<Expression> Expressions => expressions;
    public List<Greeting> Greetings => greetings;
    public List<Order> Orders => orders;
    public List<Feedback> Feedbacks => feedbacks;
    public string Goodbye => goodbye;
    public string[] Story => story;

    public int StoryProgress {  get { return storyprogress; } set {  storyprogress = value; } }
    public int VisitAmount { get { return visitAmount; } set { visitAmount = value; } }
    public int CorrectAmount { get { return correctAmount; } set { correctAmount = value; } }
    public bool LastCorrect { get { return lastCorrect; } set { lastCorrect = value; } }

    // First story part on the third visit
    // Second story part on the fifth visit > greeting goes to mid sym
    // Third story part when player gets the drink right , or right after fifth visit
    // Fourth story part when player got drink right 3 times
    // greeting goes to high sym, player gets item

}
