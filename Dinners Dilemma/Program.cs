// See https://aka.ms/new-console-template for more information

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
        AnsiConsole.Render(panel);
    }
    
    private static void Menu()
    {
        var rule = new Rule("[red]Dinner's Dilemma[/]");
        AnsiConsole.Render(rule);
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
                    new Markup("All assets from Guilty Gear Strive and Dustloop"), 
                    VerticalAlignment.Top))
                .Expand());
        var panel = new Panel(layout) { Height = 12 };
        panel.NoBorder();
        AnsiConsole.Write(panel);
        
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Can't stick around forever")
                .AddChoices("Back"));
        AnsiConsole.Clear();
        Menu();
        
    }
}