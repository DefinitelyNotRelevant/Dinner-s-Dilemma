using SixLabors.ImageSharp.Processing;
using Spectre.Console;

namespace Dinners_Dilemma;

public static class Game
{
    public static void Start()
    {
        AnsiConsole.Clear();
        while (Round())
        {
            AnsiConsole.Clear();
        }
        AnsiConsole.Clear();
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
        Move player = Move.Left;
        Move response = Move.Right;
        while (!end)
        {
            // Print the board
            PrintBoard(player, response, playerCommitted, opponentStagger);
            
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
                        .Title("[bold darkred on white]  Turn " + turn + "  [/]")
                        .AddChoices(names));
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
                        .Title("[bold darkred on white]  Turn " + turn + "  [/]")
                        .AddChoices("Commit"));
            }

            // Get opponent move
            if (!opponentStagger && reactable)
            {
                if (response == Move.Block && random.Next(1,4) == 1)
                {
                    win = true;
                }
                else
                {
                    response = Move.WildThrow;
                }
                end = true;
            }
            else if (!opponentStagger)
            {
                // Determine result
                switch (random.Next(0, 101))
                {
                    case < 6:
                        response = Move.Right;
                        if (!playerCommitted && player != Move.BackDash)
                        {
                            end = true;
                            win = true;
                        }
                        break;
                    
                    case < 16:
                        response = Move.Block;
                        break;
                    
                    case < 26:
                        response = (Move) random.Next(5, 7);
                        if (MoveLogic.Compare(player, response) || playerCommitted)
                        {
                            end = true;
                        }
                        else if (player != Move.BackDash)
                        {
                            end = true;
                            win = true;
                        }
                        else
                        {
                            opponentStagger = true;
                        }
                        break;
                    
                    default:
                        response = MoveLogic.GetCounter(player);
                        if (response != Move.Run)
                        {
                            end = true;
                        }
                        break;
                }
            }
            else
            {
                response = Move.Right;
                if (!playerCommitted && player != Move.BackDash)
                {
                    end = true;
                    win = true;
                }
                opponentStagger = false;
            }
            
            
            
            
            turn++;
            AnsiConsole.Clear();
        }
        PrintBoard(player, response, playerCommitted, opponentStagger, win);
        return Ending(win, player);
    }
    
    private static void PrintBoard(Move ino, Move sol, bool prep, bool whiff)
    {
        // Get images
        string inoAddress = MoveLogic.GetImage(ino);
        string solAddress = MoveLogic.GetImage(sol);
        if ((ino == Move.Dust || ino == Move.Stroke) && prep)
        {
            inoAddress += "_prep";
        }
        inoAddress += ".png";

        if ((sol == Move.Dp || sol == Move.WildThrow) && whiff)
        {
            solAddress += "_whiff";
        }
        solAddress += ".png";
        
        // Create grid
        Grid stage = new Grid();
        Panel inoImage = new Panel(
            new CanvasImage(inoAddress).Mutate(ctx => ctx.Resize(80, 60)))
        {
            Width = 80,
            Height = 20
        }.Expand();
        Panel solImage = new Panel(
            new CanvasImage(solAddress).Mutate(ctx => ctx.Resize(80, 60)))
        {
            Width = 80,
            Height = 20
        }.Expand();
        stage.AddColumn();
        stage.AddColumn();
        stage.AddRow(inoImage, solImage).Centered();
        
        // Print grid
        AnsiConsole.Write(stage);
        AnsiConsole.Write(new Align(
            new Markup("Ino : [bold]" + MoveLogic.GetDisplay(ino) + "[/]    Sol : [bold]" + MoveLogic.GetDisplay(sol) + "[/]\n"),
            HorizontalAlignment.Center,
            VerticalAlignment.Top
            )
        );
    }

    private static void PrintBoard(Move ino, Move sol, bool prep, bool whiff, bool win)
    {
        // Get images
        string inoAddress = MoveLogic.GetImage(ino);
        string solAddress = MoveLogic.GetImage(sol);
        if ((ino == Move.Dust || ino == Move.Stroke) && prep)
        {
            inoAddress += "_prep";
        }
        inoAddress += ".png";

        if ((sol == Move.Dp || sol == Move.WildThrow) && whiff)
        {
            solAddress += "_whiff";
        }
        solAddress += ".png";
        
        // Create grid
        Grid stage = new Grid();
        Panel inoImage = new Panel(
            new CanvasImage(inoAddress).Mutate(ctx => ctx.Resize(80, 60)))
        {
            Width = 80,
            Height = 20
        }.Expand();
        Panel solImage = new Panel(
            new CanvasImage(solAddress).Mutate(ctx => ctx.Resize(80, 60)))
        {
            Width = 80,
            Height = 20
        }.Expand();
        stage.AddColumn();
        stage.AddColumn();
        stage.AddRow(inoImage, solImage).Centered();
        
        // Print grid and ending
        if (!win || ino != Move.Dust)
        {
            FlashAnimation(stage, win, solImage, inoImage, ino, sol);
        }
        else
        {
            DustAnimation();
        }
        
        
    }

    private static bool Ending(bool win, Move move)
    {
        switch (win, move) 
        {
            case (true, Move.Dust):
                AnsiConsole.Write(new Align(
                    new Markup("[underline bold]Truest Win[/]\n"), 
                    HorizontalAlignment.Center
                    )
                );
                break;
            case (true, _):
                AnsiConsole.Write(new Align(
                        new Markup("[underline bold]Fakest Win[/]\n"), 
                        HorizontalAlignment.Center
                    )
                );
                break;
            default:
                AnsiConsole.Write(new Align(
                        new Markup("[underline bold]Fairest Loss[/]\n"), 
                        HorizontalAlignment.Center
                    )
                );
                break;
        } 
        var prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What are we supposed to do now?")
                .AddChoices("Rematch", "Back"));
        if (prompt == "Rematch")
        {
            return true;
        }
        return false;
    }

    private static void FlashAnimation(Grid stage, bool win, Panel solImage, Panel inoImage, Move ino, Move sol)
    {
        AnsiConsole.Write(stage);
        AnsiConsole.Write(new Align(
                new Markup("Ino : [bold]" + MoveLogic.GetDisplay(ino) + "[/]    Sol : [bold]" + MoveLogic.GetDisplay(sol) + "[/]\n"),
                HorizontalAlignment.Center,
                VerticalAlignment.Top
            )
        );
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold darkred on white]  You already see it  [/]")
                .AddChoices("Commit"));
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Align(new FigletText("\n\nCOUNTER").Color(Color.Red),
                HorizontalAlignment.Center,
                VerticalAlignment.Middle
                ));
        Thread.Sleep(400);
        AnsiConsole.Clear();
        Panel ending;
        if (win)
        {
            ending = new Panel(
                new CanvasImage("assets/ino_win.png").Mutate(ctx => ctx.Resize(160, 60)))
            {
                Width = 160,
                Height = 20
            }.Expand();
            solImage = new Panel(
                new CanvasImage("assets/sol_death.png").Mutate(ctx => ctx.Resize(80, 60)))
            {
                Width = 80,
                Height = 20
            }.Expand();
        }
        else
        {
            ending = new Panel(
                new CanvasImage("assets/sol_win.png").Mutate(ctx => ctx.Resize(160, 60)))
            {
                Width = 160,
                Height = 20
            }.Expand();
            inoImage = new Panel(
                new CanvasImage("assets/ino_death.png").Mutate(ctx => ctx.Resize(80, 60)))
            {
                Width = 80,
                Height = 20
            }.Expand();
        }
        stage = new Grid();
        stage.AddColumn();
        stage.AddColumn();
        stage.AddRow(inoImage, solImage).Centered();
        AnsiConsole.Write(stage);
        AnsiConsole.Write(new Align(
                new Markup("Ino : [bold]" + MoveLogic.GetDisplay(ino) + "[/]    Sol : [bold]" + MoveLogic.GetDisplay(sol) + "[/]\n"),
                HorizontalAlignment.Center,
                VerticalAlignment.Top
            )
        );
        Thread.Sleep(400);
        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(new Panel(ending).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
    }

    private static void DustAnimation()
    {
        Panel move = new Panel(
            new CanvasImage("assets/ino_5d.png").Mutate(ctx => ctx.Resize(160, 80)))
        {
            Width = 100,
            Height = 20
        }.Expand();
        AnsiConsole.Write(new Align(new Panel(move).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(600);
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("DUST ATTACK")
                .LeftJustified()
                .Color(Color.Orange1));
        Thread.Sleep(600);
        AnsiConsole.Write(
            new FigletText("5D")
                .Centered()
                .Color(Color.Orange1));
        Thread.Sleep(600);
        AnsiConsole.Write(
            new FigletText("HOMING JUMP")
                .RightJustified()
                .Color(Color.Orange1));
        Thread.Sleep(600);
        
        
        AnsiConsole.Clear();
        move = new Panel(new CanvasImage("assets/ino_jh.png").Mutate(ctx => ctx.Resize(160, 50))).Expand();
        AnsiConsole.Write(new Align(new Panel(move), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(200);
        AnsiConsole.Clear();
        CanvasImage death = new CanvasImage("assets/sol_death.png").MaxWidth(20);
        AnsiConsole.Write(new Align(new Panel(death), HorizontalAlignment.Right, VerticalAlignment.Middle));
        Thread.Sleep(200);
        AnsiConsole.Clear();
        move = new Panel(new CanvasImage("assets/ino_j2.png").Mutate(ctx => ctx.Resize(160, 50))).Expand();
        AnsiConsole.Write(new Align(new Panel(move).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(200); 
        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(new Panel(death), HorizontalAlignment.Right, VerticalAlignment.Middle));        Thread.Sleep(200);
        AnsiConsole.Clear();
        move = new Panel(new CanvasImage("assets/ino_jh.png").Mutate(ctx => ctx.Resize(160, 50))).Expand();
        AnsiConsole.Write(new Align(new Panel(move).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(200); 
        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(new Panel(death), HorizontalAlignment.Right, VerticalAlignment.Middle));        Thread.Sleep(200);
        AnsiConsole.Clear();
        move = new Panel(new CanvasImage("assets/ino_jp.png").Mutate(ctx => ctx.Resize(160, 50))).Expand();
        AnsiConsole.Write(new Align(new Panel(move).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(200); 
        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(new Panel(death), HorizontalAlignment.Right, VerticalAlignment.Middle));        Thread.Sleep(200);
        AnsiConsole.Clear();
        move = new Panel(new CanvasImage("assets/ino_jh.png").Mutate(ctx => ctx.Resize(160, 50))).Expand();
        AnsiConsole.Write(new Align(new Panel(move).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(200); 
        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(new Panel(death), HorizontalAlignment.Right, VerticalAlignment.Middle));        Thread.Sleep(200);
        AnsiConsole.Clear();
        move = new Panel(new CanvasImage("assets/ino_dive_kick.png").Mutate(ctx => ctx.Resize(160, 50))).Expand();
        AnsiConsole.Write(new Align(new Panel(move).Expand(), HorizontalAlignment.Center, VerticalAlignment.Top));
        Thread.Sleep(500); 
        AnsiConsole.Clear();
        death = new CanvasImage("assets/sol_death.png").MaxWidth(60).Mutate(ctx => ctx.Rotate(90).Resize(60, 20));
        AnsiConsole.Write(new Align(new Panel(death), HorizontalAlignment.Center, VerticalAlignment.Middle));
        Thread.Sleep(100); 
        AnsiConsole.Clear();
        Panel ending = new Panel(
            new CanvasImage("assets/ino_win.png").Mutate(ctx => ctx.Resize(160, 60)))
        {
            Width = 160,
            Height = 20
        }.Expand();
        AnsiConsole.Write(new Align(new Panel(ending).Expand(), HorizontalAlignment.Center, VerticalAlignment.Bottom));
    }
    
}