namespace WinAppTicTacToe;

public partial class MainPage : ContentPage
{
    private readonly MediumAIController mediumAI = new();

    public MainPage()
    {
        InitializeComponent();
        RenderBoard();
    }

    private async void OnCellClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || !int.TryParse(button.ClassId, out var index))
        {
            return;
        }

        if (!mediumAI.PlayUser(index))
        {
            return;
        }

        if (mediumAI.GameControl == GameState.Active)
        {
            mediumAI.PlayNPC();
        }

        RenderBoard();
        await ShowGameResultAsync();
    }

    private void OnNewClicked(object? sender, EventArgs e)
    {
        mediumAI.InitializeGameData();
        RenderBoard();
    }

    private void OnExitClicked(object? sender, EventArgs e)
    {
        if (OperatingSystem.IsWindows())
        {
            Application.Current?.Quit();
        }
    }

    private void RenderBoard()
    {
        foreach (var child in BoardGrid.Children.OfType<Button>())
        {
            var index = int.Parse(child.ClassId);
            var player = mediumAI.ArrayMoves[index];
            child.Text = player == Player.User ? "X" : player == Player.NPC ? "O" : string.Empty;
            child.IsEnabled = player == Player.None && mediumAI.GameControl == GameState.Active;
            SemanticProperties.SetDescription(child, $"Casilla {index + 1}, {(child.Text == string.Empty ? "vacía" : child.Text)}");
        }

    }

    private Task ShowGameResultAsync()
    {
        if (mediumAI.GameControl != GameState.GameOver)
        {
            return Task.CompletedTask;
        }

        var result = mediumAI.Winner == Player.None ? "¡Empate!" : $"¡Ganó {FlagForPlayer(mediumAI.Winner)}!";
        return DisplayAlertAsync("Resultado", result, "Aceptar");
    }

    private static string FlagForPlayer(Player player) => player == Player.User ? "X" : "O";
}
