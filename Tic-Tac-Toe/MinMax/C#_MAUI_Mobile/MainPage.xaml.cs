namespace TicTacToeMinMax;

public partial class MainPage : ContentPage
{
    private const char AI_PLAYER = 'O';
    private const char HUMAN_PLAYER = 'X';
    private const char EMPTY = ' ';

    private readonly Button[] buttons = new Button[9];
    private readonly char[,] board = new char[3, 3];
    private readonly UnbeatableAIController aiController = new();
    private bool isHumanTurn = true;
    private bool isGameOver;

    public MainPage()
    {
        InitializeComponent();
        InitializeGame();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        double boardSize = Math.Min(width - 68, 340);
        if (boardSize > 0 && Math.Abs(BoardGrid.WidthRequest - boardSize) > 1)
        {
            BoardGrid.WidthRequest = boardSize;
            BoardGrid.HeightRequest = boardSize;
        }
    }

    private void InitializeGame()
    {
        for (int i = 0; i < 9; i++)
        {
            int index = i;
            var button = new Button
            {
                AutomationId = $"btnTicTacToe{index + 1}",
                BackgroundColor = Colors.White,
                BorderColor = Colors.Transparent,
                BorderWidth = 0,
                CornerRadius = 16,
                FontAttributes = FontAttributes.Bold,
                FontSize = 44,
                Padding = 0,
                TextColor = Color.FromArgb("#345DEE"),
                CommandParameter = index
            };

            SemanticProperties.SetDescription(button, $"Casilla {index + 1}, vacía");
            button.Clicked += OnCellClicked;
            buttons[index] = button;
            BoardGrid.Add(button, index % 3, index / 3);
        }

        ResetBoard();
    }

    private void ResetBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = EMPTY;
            }
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            button.Text = "";
            button.IsEnabled = true;
            SemanticProperties.SetDescription(button, $"Casilla {i + 1}, vacía");
        }

        isHumanTurn = true;
        isGameOver = false;
    }

    private async void OnCellClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int index)
        {
            return;
        }

        if (isGameOver || !isHumanTurn)
        {
            return;
        }

        int row = index / 3;
        int col = index % 3;

        if (board[row, col] == EMPTY)
        {
            MakeMove(index, HUMAN_PLAYER);

            if (await CheckGameEndAsync())
            {
                return;
            }

            isHumanTurn = false;
            await AIMoveAsync();
        }
    }

    private async Task AIMoveAsync()
    {
        if (isGameOver)
        {
            return;
        }

        int bestMove = aiController.GetBestMove(board);
        if (bestMove != -1)
        {
            MakeMove(bestMove, AI_PLAYER);
            await CheckGameEndAsync();
        }

        isHumanTurn = true;
    }

    private void MakeMove(int index, char player)
    {
        int row = index / 3;
        int col = index % 3;

        board[row, col] = player;
        buttons[index].Text = player.ToString();
        buttons[index].TextColor = Color.FromArgb(player == HUMAN_PLAYER ? "#345DEE" : "#BC571F");
        buttons[index].IsEnabled = false;
        SemanticProperties.SetDescription(buttons[index], $"Casilla {index + 1}, {player}");
    }

    private async Task<bool> CheckGameEndAsync()
    {
        if (CheckWin(HUMAN_PLAYER))
        {
            await DisplayAlertAsync("Tic Tac Toe", "You Win!", "OK");
            isGameOver = true;
            return true;
        }

        if (CheckWin(AI_PLAYER))
        {
            await DisplayAlertAsync("Tic Tac Toe", "AI Wins!", "OK");
            isGameOver = true;
            return true;
        }

        if (IsBoardFull())
        {
            await DisplayAlertAsync("Tic Tac Toe", "It's a Draw!", "OK");
            isGameOver = true;
            return true;
        }

        return false;
    }

    private bool CheckWin(char player)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
            if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
        }

        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;

        return false;
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == EMPTY) return false;
            }
        }

        return true;
    }

    private void OnNewClicked(object? sender, EventArgs e)
    {
        ResetBoard();
    }

    private void OnExitClicked(object? sender, EventArgs e)
    {
        Application.Current?.Quit();
    }

    private async void OnArchivoClicked(object? sender, EventArgs e)
    {
        await DisplayActionSheetAsync("Archivo", "Cancelar", null, "Nuevo", "Salir");
    }

    private async void OnVerClicked(object? sender, EventArgs e)
    {
        await DisplayActionSheetAsync("Ver", "Cancelar", null, "Puntajes", "Resetear puntajes");
    }

    private async void OnAyudaClicked(object? sender, EventArgs e)
    {
        await DisplayActionSheetAsync("Ayuda", "Cancelar", null, "Acerca de...", "Contenido");
    }
}
