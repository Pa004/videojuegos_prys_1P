namespace WinAppTicTacToe;

public partial class MainPage : ContentPage
{
    private readonly UnbeatableAIController unbeatableAI = new();

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

        if (!unbeatableAI.PlayUser(index))
        {
            return;
        }

        if (unbeatableAI.GameControl == GameState.Active)
        {
            unbeatableAI.PlayNPC();
        }

        RenderBoard();
        await ShowGameResultAsync();
    }

    private void OnNewClicked(object? sender, EventArgs e)
    {
        unbeatableAI.InitializeGameData();
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
            var player = unbeatableAI.ArrayMoves[index];
            child.Text = player == Player.User ? "X" : player == Player.NPC ? "O" : string.Empty;
            child.IsEnabled = player == Player.None && unbeatableAI.GameControl == GameState.Active;
        }

    }

    private Task ShowGameResultAsync()
    {
        if (unbeatableAI.GameControl != GameState.GameOver)
        {
            return Task.CompletedTask;
        }

        var result = unbeatableAI.Winner == Player.None ? "¡Empate!" : $"¡Ganó {FlagForPlayer(unbeatableAI.Winner)}!";
        return DisplayAlertAsync("Resultado", result, "Aceptar");
    }

    private static string FlagForPlayer(Player player) => player == Player.User ? "X" : "O";
}
