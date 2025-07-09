using Spectre.Console;

namespace Dinners_Dilemma;

public static class Tooltips
{
    private static readonly string[] Quotes =
    [
        "They only have, like, what? 25 sanity?",
        "There is a correct option every time, you know?",
        "Maybe better graphics means more wins?",
        "\"Keep on keeping on.\" - You Already Know Who.",
        "There's technically a defense option.",
        "If I was a cow, I would hang around the middle of the field.",
        "If I was a chicken, I'm busting out of that factory.",
        "Do you think you can water bucket clutch with a Roman Cancel?",
        "Yeah, my OC's called The Fifth Angle, because a rectangle only has four of em.",
        "I'm still working on what agenda I'm pushing with this. Kinda like Rango?",
        "I swear, there is no resource for character backdashes. Like aside from basic frames there is nothing.",
        "Uh, don't look too hard at wild throw or dp, ok buddy?",
        "Bet that at Cornell they take tons of Cornell notes.",
        "Canonically, this takes place in Round 3 where both characters just used blue burst.",
        "So you know when you rack your brain but all you can think about is one thing? That's why dust becomes the only option.",
        "Ha... A certain subject has made me a loser...",
        "Did you know there's a secret way to exit to the main menu in the middle of a game?",
        "Next season will have hover-dash, run into Wild Throw, all three jump arcs, Burst, Deflect Shield, Dead Angle Attack...",
        "For the record, this is not accurate. You will die if you backdash and Sol is running at you.",
        "Did you know the reason you can't punish whiffed Volcanic Viper with 5[[D]] is because I can't?",
        "You have to admit that Sol 5D looks at least somewhat defensive, right?",
        "There are different endings, just so you know."
    ];

    private static readonly Random Random = new Random();

    public static string GetRandomQuote()
    {
        return Quotes[Random.Next(Quotes.Length)];
    }

    public static void PrintFirstHalf()
    {
        for (int i = 0; i < Quotes.Length / 2; i++)
        {
            AnsiConsole.MarkupLine("[green]" + Quotes[i] + "[/]\n"); 
        }
        AnsiConsole.WriteLine();

    }
    
    public static void PrintSecondHalf()
    {
        
        for (int i = Quotes.Length / 2; i < Quotes.Length; i++)
        {
            AnsiConsole.MarkupLine("[green]" + Quotes[i] + "[/]\n"); 
        }
        AnsiConsole.WriteLine();
    }
}