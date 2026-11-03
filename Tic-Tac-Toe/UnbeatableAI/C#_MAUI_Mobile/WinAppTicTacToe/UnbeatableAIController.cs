using System;

namespace WinAppTicTacToe
{
    #region Project Enumerations

    public enum Player
    {
        None = 0,
        User = 1,
        NPC = 2
    }

    public enum GameState
    {
        GameOver = 0,
        Active = 1
    }

    #endregion

    public class UnbeatableAIController
    {
        #region Class Members

        private Player mCurrentPlayerFlag;
        private int mMoveCount;
        private GameState mCurrentGameState;
        private Player[] mBoardState = null!;
        private int mHeuristicSum;
        private int[] mArrayMoves = null!;

        private const int CENTER_INDEX = 4;
        private static readonly int[] CORNER_INDICES = { 0, 2, 6, 8 };
        private static readonly int[] SIDE_INDICES = { 1, 3, 5, 7 };

        private static readonly int[][] WinningLines = new int[][]
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },
            new int[] { 0, 3, 6 },
            new int[] { 1, 4, 7 },
            new int[] { 2, 5, 8 },
            new int[] { 0, 4, 8 },
            new int[] { 2, 4, 6 }
        };

        #endregion

        #region Properties

        public string Flag
        {
            get => mCurrentPlayerFlag == Player.User ? "X" : "O";
            set => mCurrentPlayerFlag = (value == "X") ? Player.User : Player.NPC;
        }

        public int MoveNumber
        {
            get => mMoveCount;
            set => mMoveCount = value;
        }

        public GameState GameControl
        {
            get => mCurrentGameState;
            set => mCurrentGameState = value;
        }

        public Player[] ArrayMoves
        {
            get => mBoardState;
            set => mBoardState = value;
        }

        // Replaces the result that WinForms previously showed with MessageBox.
        public Player Winner { get; private set; }

        #endregion

        #region Constructors of the class

        public UnbeatableAIController()
        {
            InitializeGameData();
        }

        #endregion

        #region Game Control Methods

        public void InitializeGameData()
        {
            mArrayMoves = new int[9];
            for (int i = 0; i < mArrayMoves.Length; i++)
                mArrayMoves[i] = 0;
            mBoardState = new Player[9];
            mCurrentPlayerFlag = Player.User;
            mMoveCount = 0;
            mCurrentGameState = GameState.Active;
            mHeuristicSum = 0;
            Winner = Player.None;
        }

        private GameState CheckWinner()
        {
            foreach (var line in WinningLines)
            {
                Player cell1 = mBoardState[line[0]];
                Player cell2 = mBoardState[line[1]];
                Player cell3 = mBoardState[line[2]];

                if (cell1 != Player.None && cell1 == cell2 && cell2 == cell3)
                {
                    Winner = cell1;
                    return GameState.GameOver;
                }
            }

            if (mMoveCount == 9)
            {
                return GameState.GameOver;
            }

            return GameState.Active;
        }

        private string FlagForPlayer(Player player)
        {
            return player == Player.User ? "X" : "O";
        }

        #endregion

        #region Move Handling Methods

        private void ShowUserMove(int index)
        {
            mBoardState[index] = Player.User;
            mMoveCount++;
            mCurrentPlayerFlag = Player.NPC;
            mArrayMoves[index] = 1;
        }

        public bool PlayUser(int index)
        {
            if (mBoardState[index] == Player.None && mCurrentGameState == GameState.Active)
            {
                if (mCurrentPlayerFlag == Player.User)
                {
                    ShowUserMove(index);
                    mCurrentGameState = CheckWinner();
                    return true;
                }
            }
            return false;
        }

        private int GenerateRandomNPCMove()
        {
            int index = 0;
            Random RandomNumber = new Random();

            do
            {
                index = RandomNumber.Next(0, 9);
                if (mArrayMoves[index] == 0 && index < mArrayMoves.Length)
                    return index;
            }
            while (true);
        }

        private void ShowNPCMove(int index)
        {
            mBoardState[index] = Player.NPC;
            mMoveCount++;
            mCurrentPlayerFlag = Player.User;
            mArrayMoves[index] = 3;
        }

        private int HeuristicNumericalMethod()
        {
            int index = 0;
            if (mArrayMoves[0] + mArrayMoves[1] + mArrayMoves[2] == 6)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[1] == 0) index = 1;
                else if (mArrayMoves[2] == 0) index = 2;
                mHeuristicSum = mArrayMoves[0] + mArrayMoves[1] + mArrayMoves[2];
                return index;
            }
            else if (mArrayMoves[3] + mArrayMoves[4] + mArrayMoves[5] == 6)
            {
                if (mArrayMoves[3] == 0) index = 3;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[5] == 0) index = 5;
                mHeuristicSum = mArrayMoves[3] + mArrayMoves[4] + mArrayMoves[5];
                return index;
            }
            else if (mArrayMoves[6] + mArrayMoves[7] + mArrayMoves[8] == 6)
            {
                if (mArrayMoves[6] == 0) index = 6;
                else if (mArrayMoves[7] == 0) index = 7;
                else if (mArrayMoves[8] == 0) index = 8;
                mHeuristicSum = mArrayMoves[6] + mArrayMoves[7] + mArrayMoves[8];
                return index;
            }
            else if (mArrayMoves[0] + mArrayMoves[3] + mArrayMoves[6] == 6)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[3] == 0) index = 3;
                else if (mArrayMoves[6] == 0) index = 6;
                mHeuristicSum = mArrayMoves[0] + mArrayMoves[3] + mArrayMoves[6];
                return index;
            }
            else if (mArrayMoves[1] + mArrayMoves[4] + mArrayMoves[7] == 6)
            {
                if (mArrayMoves[1] == 0) index = 1;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[7] == 0) index = 7;
                mHeuristicSum = mArrayMoves[1] + mArrayMoves[4] + mArrayMoves[7];
                return index;
            }
            else if (mArrayMoves[2] + mArrayMoves[5] + mArrayMoves[8] == 6)
            {
                if (mArrayMoves[2] == 0) index = 2;
                else if (mArrayMoves[5] == 0) index = 5;
                else if (mArrayMoves[8] == 0) index = 8;
                mHeuristicSum = mArrayMoves[2] + mArrayMoves[5] + mArrayMoves[8];
                return index;
            }
            else if (mArrayMoves[0] + mArrayMoves[4] + mArrayMoves[8] == 6)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[8] == 0) index = 8;
                mHeuristicSum = mArrayMoves[0] + mArrayMoves[4] + mArrayMoves[8];
                return index;
            }
            else if (mArrayMoves[2] + mArrayMoves[4] + mArrayMoves[6] == 6)
            {
                if (mArrayMoves[2] == 0) index = 2;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[6] == 0) index = 6;
                mHeuristicSum = mArrayMoves[2] + mArrayMoves[4] + mArrayMoves[6];
                return index;
            }
            else if (mArrayMoves[0] + mArrayMoves[1] + mArrayMoves[2] == 2)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[1] == 0) index = 1;
                else if (mArrayMoves[2] == 0) index = 2;
                mHeuristicSum = mArrayMoves[0] + mArrayMoves[1] + mArrayMoves[2];
                return index;
            }
            else if (mArrayMoves[3] + mArrayMoves[4] + mArrayMoves[5] == 2)
            {
                if (mArrayMoves[3] == 0) index = 3;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[5] == 0) index = 5;
                mHeuristicSum = mArrayMoves[3] + mArrayMoves[4] + mArrayMoves[5];
                return index;
            }
            else if (mArrayMoves[6] + mArrayMoves[7] + mArrayMoves[8] == 2)
            {
                if (mArrayMoves[6] == 0) index = 6;
                else if (mArrayMoves[7] == 0) index = 7;
                else if (mArrayMoves[8] == 0) index = 8;
                mHeuristicSum = mArrayMoves[6] + mArrayMoves[7] + mArrayMoves[8];
                return index;
            }
            else if (mArrayMoves[0] + mArrayMoves[3] + mArrayMoves[6] == 2)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[3] == 0) index = 3;
                else if (mArrayMoves[6] == 0) index = 6;
                mHeuristicSum = mArrayMoves[0] + mArrayMoves[3] + mArrayMoves[6];
                return index;
            }
            else if (mArrayMoves[1] + mArrayMoves[4] + mArrayMoves[7] == 2)
            {
                if (mArrayMoves[1] == 0) index = 1;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[7] == 0) index = 7;
                mHeuristicSum = mArrayMoves[1] + mArrayMoves[4] + mArrayMoves[7];
                return index;
            }
            else if (mArrayMoves[2] + mArrayMoves[5] + mArrayMoves[8] == 2)
            {
                if (mArrayMoves[2] == 0) index = 2;
                else if (mArrayMoves[5] == 0) index = 5;
                else if (mArrayMoves[8] == 0) index = 8;
                mHeuristicSum = mArrayMoves[2] + mArrayMoves[5] + mArrayMoves[8];
                return index;
            }
            else if (mArrayMoves[0] + mArrayMoves[4] + mArrayMoves[8] == 2)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[8] == 0) index = 8;
                mHeuristicSum = mArrayMoves[0] + mArrayMoves[4] + mArrayMoves[8];
                return index;
            }
            else if (mArrayMoves[2] + mArrayMoves[4] + mArrayMoves[6] == 2)
            {
                if (mArrayMoves[2] == 0) index = 2;
                else if (mArrayMoves[4] == 0) index = 4;
                else if (mArrayMoves[6] == 0) index = 6;
                mHeuristicSum = mArrayMoves[2] + mArrayMoves[4] + mArrayMoves[6];
                return index;
            }
            else if (mArrayMoves[0] + mArrayMoves[4] + mArrayMoves[8] == 5)
            {
                if (mArrayMoves[2] == 0) index = 2;
                else if (mArrayMoves[6] == 0) index = 6;
                else
                    index = GenerateRandomNPCMove();
                mHeuristicSum = 5;
                return index;
            }
            else if (mArrayMoves[2] + mArrayMoves[4] + mArrayMoves[6] == 5)
            {
                if (mArrayMoves[0] == 0) index = 0;
                else if (mArrayMoves[8] == 0) index = 8;
                else
                    index = GenerateRandomNPCMove();
                mHeuristicSum = 5;
                return index;
            }
            else if ((mArrayMoves[1] + mArrayMoves[3] == 2) && (mArrayMoves[0] == 0))
            {
                mHeuristicSum = 2;
                index = CORNER_INDICES[0];
                return index;
            }
            else if ((mArrayMoves[1] + mArrayMoves[5] == 2) && (mArrayMoves[2] == 0))
            {
                mHeuristicSum = 2;
                index = CORNER_INDICES[1];
                return index;
            }
            else if ((mArrayMoves[3] + mArrayMoves[7] == 2) && (mArrayMoves[6] == 0))
            {
                mHeuristicSum = 2;
                index = CORNER_INDICES[2];
                return index;
            }
            else if ((mArrayMoves[5] + mArrayMoves[7] == 2) && (mArrayMoves[8] == 0))
            {
                mHeuristicSum = 2;
                index = CORNER_INDICES[3];
                return index;
            }
            else
            {
                index = GenerateRandomNPCMove();
                return index;
            }
        }

        public void PlayNPC()
        {
            int index;
            Random RandomNumber = new Random();

            if (mCurrentGameState == GameState.Active)
            {
                if (MoveNumber == 1)
                {
                    if (mArrayMoves[4] == 0)
                    {
                        index = CENTER_INDEX;
                        ShowNPCMove(index);
                        mCurrentGameState = CheckWinner();
                        return;
                    }
                    do
                    {
                        index = RandomNumber.Next(0, 9);
                        if (mArrayMoves[index] == 0 &&
                           (index == CORNER_INDICES[0] || index == CORNER_INDICES[1] ||
                            index == CORNER_INDICES[2] || index == CORNER_INDICES[3]))
                        {
                            ShowNPCMove(index);
                            mCurrentGameState = CheckWinner();
                            return;
                        }
                    } while (true);
                }
                else
                {
                    if (mArrayMoves[4] == 0)
                    {
                        index = CENTER_INDEX;
                        ShowNPCMove(index);
                        mCurrentGameState = CheckWinner();
                        return;
                    }
                    else
                    {
                        index = HeuristicNumericalMethod();
                        if (mArrayMoves[4] == 3 && mHeuristicSum == 5)
                        {
                            do
                            {
                                index = RandomNumber.Next(0, 9);
                                if (mArrayMoves[index] == 0 &&
                                   (index == SIDE_INDICES[0] || index == SIDE_INDICES[0] ||
                                    index == SIDE_INDICES[2] || index == SIDE_INDICES[3]))
                                {
                                    ShowNPCMove(index);
                                    mCurrentGameState = CheckWinner();
                                    return;
                                }
                            } while (true);
                        }
                        ShowNPCMove(index);
                        mCurrentGameState = CheckWinner();
                        return;
                    }
                }
            }
            else
                return;
        }

        #endregion
    }
}
