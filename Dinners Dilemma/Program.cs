// See https://aka.ms/new-console-template for more information
namespace Dinners_Dilemma;

using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Rendering;

public static class Program
{
    public static void Main(string[] args)
    {
        //AnsiConsole.Markup("[underline red]Hello[/] World!\n");
        //Load();
        Menu();
    }

    private static void Load()
    {
        var panel = new Panel("\"The Dunning-Kruger effect is a cognitive bias where people overestimate their knowledge or ability in a specific area. " +
                              "It occurs because a lack of self-awareness prevents them from accurately assessing their own skills. " +
                              "Essentially, the more incompetent someone is, the less aware they are of their own incompetence\" - Microsoft Copilot\n");
        panel.Border = BoxBorder.Rounded;
        panel.Expand = true;
        AnsiConsole.Write(panel);
    }
    
    public static void Menu()
    {
        var rule = new Rule("[red]Dinner's Dilemma[/]");
        AnsiConsole.Write(rule);
        var prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[white]\nMake The[/] [red]Move[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Start", "Language", "Extras", "Exit",
                }));

        if (prompt == "Extras")           
        {
            Extras();
        } else if (prompt == "Exit")
        {
            Quit();
        } else if (prompt == "Start")
        {
            Game.Start();
        }
    }

    private static void Extras()
    {
        var image = new CanvasImage("assets/elphet.png");
        image.Mutate(ctx => ctx.Resize(75, 20)); 
        var layout = new Layout("Root")
            .SplitRows(
                new Layout("Columns").Size(10)
                    .SplitColumns(
                        new Layout("Choices"),
                        new Layout("Image").Size(75)));
        
        layout["Image"].Update(new Panel(image).Expand());
        layout["Choices"].Update(
            new Panel(
                    Align.Left(
                        new Markup("All assets from Guilty Gear Strive and Dustloop.\n" +
                                   "References to Death Stranding, Limbus Company, Rango, Cornell University"), 
                        VerticalAlignment.Top))
                .Expand());
        var panel = new Panel(layout) { Height = 12 };
        panel.NoBorder();
        AnsiConsole.Write(panel);
        
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Can't stick around forever")
                .AddChoices("Back"));
        AnsiConsole.Clear();
        Menu();
        
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