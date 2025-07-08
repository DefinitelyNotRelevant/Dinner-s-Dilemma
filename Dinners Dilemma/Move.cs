namespace Dinners_Dilemma;

public enum Move
{
    Dust,
    TwoS,
    Stroke,
    ChemicalLove,
    BackDash,
    Dp,
    WildThrow,
    Run,
    Block,
    Left,
    Right
}

public static class MoveLogic
{
    
    private static readonly Dictionary<Move, HashSet<Move>> BeatenBy = new()
    {
        { Move.Dust, [Move.Dp, Move.WildThrow] },
        { Move.TwoS, [Move.Dp] },
        { Move.Stroke, [Move.Dp, Move.WildThrow] },
        { Move.ChemicalLove, [Move.Dp] },
        { Move.BackDash, [Move.Run] },
    };  
    
    private static readonly Dictionary<Move, string> Display = new()
    {
        { Move.Dust, "Charged Dust" },
        { Move.TwoS, "2S" },
        { Move.Stroke, "Stroke" },
        { Move.ChemicalLove, "Chemical Love" },
        { Move.BackDash, "Backdash" },
        { Move.Dp, "Volcanic Viper" },
        { Move.WildThrow, "Wild Throw" },
        { Move.Run, "Run" },
        { Move.Block, "Block" },
        { Move.Left, "Idle" },
        { Move.Right, "Idle" },
    };
    
    // Holy that's kind of weird. Jetbrains AI Assistant just became free and I tried it. Also a terrible implementation.
    private static readonly Dictionary<string, Move> Data = new()
    {
        { "Charged Dust", Move.Dust },
        { "2S", Move.TwoS },
        { "Stroke", Move.Stroke },
        { "Chemical Love", Move.ChemicalLove },
        { "Backdash", Move.BackDash },
    };

    private static readonly Dictionary<Move, string> Image = new()
    {
        { Move.Dust, "assets/ino_5d" },
        { Move.TwoS, "assets/ino_2s" },
        { Move.Stroke, "assets/ino_stroke" },
        { Move.ChemicalLove, "assets/ino_chemical_love" },
        { Move.BackDash, "assets/ino_fake_backdash" },
        { Move.Dp, "assets/sol_dp" },
        { Move.WildThrow, "assets/sol_wild_throw" },
        { Move.Run, "assets/sol_run" },
        { Move.Block, "assets/sol_5d" },
        { Move.Left, "assets/ino_idle" },
        { Move.Right, "assets/sol_idle" },
    };
    
    public static bool Compare(Move move1, Move move2)
    {
        if (BeatenBy[move1].Contains(move2))
        {
            return true;
        }
        return false;
    }

    public static Move GetCounter(Move move)
    {
        List<Move> counters = new List<Move>(BeatenBy[move]);
        Random random = new Random();
        
        return counters[random.Next(0, counters.Count)];
    }

    public static string GetDisplay(Move move)
    {
        return Display[move];
    }
    
    public static Move GetData(string name)
    {
        return Data[name];
    }

    public static string GetImage(Move move)
    {
        return Image[move];
    }
    
}