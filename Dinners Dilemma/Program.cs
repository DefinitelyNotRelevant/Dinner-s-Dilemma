namespace Dinners_Dilemma;

using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Rendering;

public enum MenuAction
{
    Menu,
    Extras,
    Quotes,
    Quit
}

public static class Program
{
    public static void Main()
    {
        Load();
        MenuAction next = Menu();
        while (next != MenuAction.Quit)
        {
            switch (next)
            {
                case MenuAction.Menu:
                    next = Menu();
                    break;
                case MenuAction.Extras:
                    next = Extras();
                    break;
                case MenuAction.Quotes:
                    next = Quotes();
                    break;
            }
        }

        Quit();
    }

    private static void Load()
    {
        AnsiConsole.MarkupLine("[green]Tooltip : " + Tooltips.GetRandomQuote() + "[/]");
        AnsiConsole.Progress()
            .AutoRefresh(true)
            .AutoClear(false)
            .HideCompleted(false)
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn()
            })
            .Start(ctx =>
            {
                var load = ctx.AddTask("Initializing system and prepping assets...");
                while (!load.IsFinished)
                {
                    load.Increment(1.0);
                    Thread.Sleep(5);
                }
                var connect = ctx.AddTask("Connecting to server to pad time...");
                while (!connect.IsFinished)
                {
                    connect.Increment(1.0);
                    Thread.Sleep(25);
                }
            });
        AnsiConsole.MarkupLine("[bold green]Ready![/]");
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("")
                .PageSize(10)
                .AddChoices(new[] {
                    "Continue",
                }));
        AnsiConsole.Clear();
    }
    
    public static MenuAction Menu()
    {
        var rule = new Rule("[red]Dinner's Dilemma[/]");
        AnsiConsole.Write(rule);
        var prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[white]\nMake The[/] [red]Move[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Start", "Extras", "Exit",
                }));

        if (prompt == "Extras")           
        {
            return MenuAction.Extras;
        }
        if (prompt == "Exit")
        {
            return MenuAction.Quit;
        }
        Game.Start();
        return MenuAction.Menu;
    }

    private static MenuAction Extras()
    {
        var image = new CanvasImage("assets/elphet.png");
        image.Mutate(ctx => ctx.Resize(60, 20)); 
        var layout = new Layout("Root")
            .SplitRows(
                new Layout("Columns").Size(10)
                    .SplitColumns(
                        new Layout("Choices"),
                        new Layout("Image").Size(60)));
        
        layout["Image"].Update(new Panel(image).Expand());
        layout["Choices"].Update(
            new Panel(
                    Align.Left(
                        new Markup("By DefinitelyNotRelevant with .NET 8 and Spectre.Console.\n" + 
                                   "All assets from Guilty Gear Strive and sourced from Dustloop Wiki.\n" +
                                   "([link]https://www.dustloop.com/w/Main_Page[/])\n" +
                                   "References Death Stranding, Limbus Company, Rango, and Cornell University."), 
                        VerticalAlignment.Top))
                .Expand());
        var panel = new Panel(layout) { Height = 12 };
        panel.NoBorder();
        AnsiConsole.Write(panel);
        
        var prompt= AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[hotpink]Rub-a-dub-dub (Rub, rub-a-dub) Yeah yeah[/]")
                .AddChoices("Tooltips", "Back"));
        AnsiConsole.Clear();
        if (prompt == "Tooltips")
        {
            return MenuAction.Quotes;
        }
        else
        {
            return MenuAction.Menu;       
        }
    }

    private static MenuAction Quotes()
    {
        bool finished = false; 
        bool firstPage = true;
        string prompt;
        while (!finished)
        {
            if (firstPage)
            {
                Tooltips.PrintFirstHalf();
                prompt = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Can't stick around forever")
                        .AddChoices("Next Page", "Leave"));
            }
            else
            {
                Tooltips.PrintSecondHalf();
                prompt = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Can't stick around forever")
                        .AddChoices("Previous Page", "Back"));
            }

            if (prompt == "Next Page")
            {
                firstPage = false;
            }
            else if (prompt == "Previous Page")
            {
                firstPage = true;
            } else
            {
                finished = true;
            }
            AnsiConsole.Clear();
        }
        return MenuAction.Extras;
    }

    private static void Quit()
    {
        var table = new Table().Centered().Border(TableBorder.Double).ShowRowSeparators();
        table.AddColumn(new TableColumn("Cursor").Centered());
        table.AddColumn(new TableColumn("Action").Centered());
        table.AddRow("", "\n[bold]Rematch[/]\n[grey]Rematch with the same opponent.[/]\n");
        table.AddRow("", "\n[bold]Quit[/]\n[grey]Quit the match.[/]\n");
        table.HideHeaders();
        AnsiConsole.Live(table)
            .Start(ctx => 
            {
                table.Rows.RemoveAt(0);
                table.Rows.Insert(0, new TableRow(new IRenderable[]
                {
                    new Panel("[bold][blue]X[/][/]")
                    {
                        Border = BoxBorder.Rounded,
                    },
                    new Markup("\n[bold]Rematch[/]\n[grey]Rematch with the same opponent.[/]\n")
                }));
                ctx.Refresh();
                Thread.Sleep(1500);
                table.Rows.RemoveAt(0);
                table.Rows.Insert(0, new TableRow(new IRenderable[]
                {
                    new Markup(""),
                    new Markup("\n[bold]Rematch[/]\n[grey]Rematch with the same opponent.[/]\n")
                }));
                table.Rows.RemoveAt(1);
                table.Rows.Insert(1, new TableRow(new IRenderable[]
                {
                    new Panel("[bold][blue]X[/][/]")
                    {
                        Border = BoxBorder.Rounded,
                    },
                    new Markup("\n[bold]Quit[/]\n[grey]Quit the match.[/]\n")
                }));
                ctx.Refresh();
                Thread.Sleep(500);
            });
        Environment.Exit(0);
    }
    
    // They only have, like, what? 25 sanity?
    
    // There is a correct option every time, you know?
    
    // Maybe better graphics means more wins?
    
    // "Keep on keeping on." - You Already Know Who.
    
    // There's technically a defense option.
    
    // If I was a cow, I would hang around the middle of the field.
    
    // If I was a chicken, I'm busting out of that factory.
    
    // Do you think you can water bucket clutch with a Roman Cancel?
    
    // Yeah, my OC's called The Fifth Angle, because a rectangle only has four of em.
    
    // I'm still working on what agenda I'm pushing with this. Kinda like Rango?
    
    // I swear, there is no resource for character backdashes. Like aside from basic frames there is nothing.
    
    // Uh, don't look too hard at wild throw or dp, ok buddy?
    
    // Bet that at Cornell they take tons of Cornell notes.
    
    // Canonically, this takes place in Round 3 where both characters just used blue burst.
    
    // So you know when you rack your brain but all you can think about is one thing? That's why dust becomes the only option.
    
    // Ha... A certain subject has made me a loser...
    
    // Did you know there's a secret way to exit to the main menu in the middle of a game?
    
    // Next season will have hover-dash, run into wild throw, all three jump arcs, burst, deflect shield, dead angle attack...
    
    // For the record, this is not accurate. You can definitely whiff punish dp with 5[D] after backdash. I just can't do it.
    
    // You have to admit that Sol 5D at least somewhat defensive, right?
    
    // There are different endings, just so you know.
}