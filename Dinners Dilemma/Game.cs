using Spectre.Console;

namespace Dinners_Dilemma;

public static class Game
{
    public static void Start()
    {
        while (Round())
        {
            AnsiConsole.Clear();
        }
        AnsiConsole.Clear();
        Program.Menu();
    }
    
    private static bool Round()
    {
        bool end = false;
        bool win = false;
        bool opponentStagger = false;
        bool playerCommitted = false;
        bool reactable = false;
        Random random = new Random();
        int turn = 1;
        Move player = Move.Idle;
        Move response = Move.Idle;
        while (!end)
        {
            // Get player move
            if (!playerCommitted)
            {
                HashSet<String> names = new HashSet<String>();
                names.Add("Charged Dust");
                for (int i = 0; i < 3; i++)
                {
                    names.Add(MoveLogic.GetDisplay((Move)random.Next(0, 5)));
                }
                string playerName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Turn " + turn)
                        .AddChoices(names));
                AnsiConsole.WriteLine(playerName);
                player = MoveLogic.GetData(playerName);
                if (player == Move.Stroke || player == Move.Dust)
                {
                    playerCommitted = true;
                }
            }
            else
            {
                reactable = true;
                playerCommitted = false;
                AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Turn " + turn)
                        .AddChoices("Commit"));
            }

            // Get opponent move
            if (!opponentStagger && reactable)
            {
                if (response == Move.Block && random.Next(1,4) == 1)
                {
                    AnsiConsole.WriteLine("OVERHEAD");
                    win = true;
                }
                else
                {
                    response = Move.Dp;
                    AnsiConsole.WriteLine("PUNISHED");
                }
                end = true;
            }
            else if (!opponentStagger)
            {
                var roll = random.Next(0, 101); // Generates a random number between 1 and 100 (inclusive of 1, exclusive of 101)
                AnsiConsole.WriteLine(roll);
                
                // Determine result
                switch (roll) // Remove roll later
                {
                    case < 6:
                        if (!playerCommitted && player != Move.BackDash)
                        {
                            end = true;
                            win = true;
                            AnsiConsole.WriteLine("WIN");
                        }
                        else
                        {
                            AnsiConsole.WriteLine("WASTED");
                        }
                        break;
                    
                    case < 16:
                        response = Move.Block;
                        AnsiConsole.WriteLine("BLOCK");
                        break;
                    
                    case < 26:
                        response = (Move) random.Next(5, 7);
                        AnsiConsole.WriteLine(MoveLogic.GetDisplay(response));
                        if (MoveLogic.Compare(player, response) || playerCommitted)
                        {
                            end = true;
                            AnsiConsole.WriteLine("LOST CLASH");
                        }
                        else if (player != Move.BackDash)
                        {
                            end = true;
                            win = true;
                            AnsiConsole.WriteLine("WON CLASH");
                        }
                        else
                        {
                            opponentStagger = true;
                            AnsiConsole.WriteLine("OPPONENT WHIFFED");
                        }
                        break;
                    
                    default:
                        response = MoveLogic.GetCounter(player);
                        AnsiConsole.WriteLine(MoveLogic.GetDisplay(response));
                        if (response != Move.Run)
                        {
                            end = true;
                            AnsiConsole.WriteLine("ATTACK CALLOUT");
                        }
                        else
                        {
                            AnsiConsole.WriteLine("BACKDASH CALLOUT");

                        }
                        break;
                }
            }
            else
            {
                if (!playerCommitted && player != Move.BackDash)
                {
                    end = true;
                    win = true;
                    AnsiConsole.WriteLine("WIN");
                }
                else
                {
                    AnsiConsole.WriteLine("WASTED");
                }
                opponentStagger = false;
            }
            
            
            
            
            turn++;
        }
        return Ending(win, player);
    }

    private static bool Ending(bool win, Move move)
    {
        switch (win, move) 
        {
            case (true, Move.Dust):
                AnsiConsole.WriteLine("Truest Win");
                break;
            case (true, _):
                AnsiConsole.WriteLine("Fakest Win");
                break;
            default:
                AnsiConsole.WriteLine("Fairest Loss");
                break;
        } 
        var prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Can't stick around forever")
                .AddChoices("Rematch", "Back"));
        if (prompt == "Rematch")
        {
            return true;
        }
        return false;
    }
}