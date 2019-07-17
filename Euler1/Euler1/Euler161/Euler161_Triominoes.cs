using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Euler1
{
    public class Euler161_Triominoes
    {
        //y-length 9
        //public int YLength = 3;
        //public const int NumOfPiecesInBlock = 2;

        public int[] _configurations;
        public Board[] _boards;
        public List<int> _validConfigs;

        public long SolveTriominoes( int numOfBlocks, int YLength, ref long counter )
        {
            int NumOfPiecesInBlock = 2*YLength/3;
            var movesAll = new PieceMove[100000000];
            var movesCurrent = new PieceMove[NumOfPiecesInBlock * numOfBlocks];

            var movesAllInd = -1;
            var movesCurrentInd = -1;

            var board = new Board(numOfBlocks*2,YLength);
            board.InitializeBoard(2*numOfBlocks, YLength);

            long retNumber = 0;
            bool done = false;
            int curX = 0;
            int curY = 0;

            counter = 0;
            var availableMoves = new List<PieceMove>();

            while (!done)
            {
                ++counter;
     //           board.DrawBoard();

                var fitInBlock = false;
                if (movesCurrentInd >= NumOfPiecesInBlock * numOfBlocks - 2)
                    fitInBlock = board.FitInBlock((movesCurrentInd + 1) / NumOfPiecesInBlock);

                if (!fitInBlock && board.GetNextEmptySquareToFill(ref curX, ref curY))
                    availableMoves = board.GenerateAvailableMovesAtSquare(curX, curY);
                else
                    availableMoves.Clear();

                if (availableMoves.Any())
                {
                    foreach (PieceMove move in availableMoves)
                        movesAll[++movesAllInd] = move;

                    // place piece on the board
                    PieceMove lastPiece = movesAll[movesAllInd];
                    if (!board.PlacePieceOnBoard(lastPiece))
                        return -1;

                    movesCurrent[++movesCurrentInd] = lastPiece;

                    if (movesCurrentInd + 1 == NumOfPiecesInBlock * numOfBlocks)
                    {
                        ++retNumber;
                        //Console.WriteLine("*** Solution number {0} ****", retNumber);
                        //for (int i = 0; i < movesCurrentInd; i++)
                        //    Console.WriteLine("Piece {0} at ({1},{2})", movesCurrent[i].Piece, movesCurrent[i].X,
                        //        movesCurrent[i].Y);
                        //Console.WriteLine();
                    }
                }
                else
                {
                    while (movesAllInd >= 0 && movesCurrentInd >= 0 && movesAll[movesAllInd] == movesCurrent[movesCurrentInd])
                    {
                        board.RemovePieceFromBoard(movesCurrent[movesCurrentInd]);
                        movesAll[movesAllInd--] = null;
                        movesCurrent[movesCurrentInd--] = null;
                    } 

                    if (movesAllInd < 0)
                        done = true;
                    else
                    {
                        PieceMove lastPiece = movesAll[movesAllInd];
                        if (!board.PlacePieceOnBoard(lastPiece))
                            return -1;
                        movesCurrent[++movesCurrentInd] = lastPiece;
                    }

                }
            }

            return retNumber;
        }

        public long SolveTriominoesWithBoard(Board board)
        {
            int NumberOfMoves = board.CountTriominoesToSolve();
            if (NumberOfMoves == -1)
                return 0; 

            var movesAll = new PieceMove[100000000];
            var movesCurrent = new PieceMove[NumberOfMoves];

            var movesAllInd = -1;
            var movesCurrentInd = -1;

            long retNumber = 0;
            bool done = false;
            int curX = 0;
            int curY = 0;

            var availableMoves = new List<PieceMove>();

            while (!done)
            {

                var solved = false;
                if (movesCurrentInd + 1 ==  NumberOfMoves)
                    solved = board.IsSolved();

                if (solved)
                    ++retNumber;

                if (!solved && board.GetNextEmptySquareToFill(ref curX, ref curY))
                    availableMoves = board.GenerateAvailableMovesAtSquare(curX, curY);
                else
                    availableMoves.Clear();

                if (availableMoves.Any())
                {
                    foreach (PieceMove move in availableMoves)
                        movesAll[++movesAllInd] = move;

                    PieceMove lastPiece = movesAll[movesAllInd];
                    if (!board.PlacePieceOnBoard(lastPiece))
                        return -1;

                    movesCurrent[++movesCurrentInd] = lastPiece;
                }
                else
                {
                    while (movesAllInd >= 0 && movesCurrentInd >= 0 && movesAll[movesAllInd] == movesCurrent[movesCurrentInd])
                    {
                        board.RemovePieceFromBoard(movesCurrent[movesCurrentInd]);
                        movesAll[movesAllInd--] = null;
                        movesCurrent[movesCurrentInd--] = null;
                    }

                    if (movesAllInd < 0)
                        done = true;
                    else
                    {
                        PieceMove lastPiece = movesAll[movesAllInd];
                        if (!board.PlacePieceOnBoard(lastPiece))
                            return -1;
                        movesCurrent[++movesCurrentInd] = lastPiece;
                    }

                }
            }

            return retNumber;
        }

        public bool SolveTriominoesWithSpillOver(int XLength, int YLength)
        {
           // int NumOfPiecesInBlock = 2 * YLength / 3;
            int MinMovesToFinish = XLength*YLength/3;
            var movesAll = new PieceMove[100000000];
            var movesCurrent = new PieceMove[MinMovesToFinish+YLength*2/3];

            var movesAllInd = -1;
            var movesCurrentInd = -1;

            var board = new Board(XLength + 2, YLength);
            board.InitializeBoard(XLength + 2, YLength);

            bool done = false;
            int curX = 0;
            int curY = 0;

            var availableMoves = new List<PieceMove>();

            while (!done)
            {
                var fitInBlock = false;
                //if (movesCurrentInd >= MinMovesToFinish - YLength/3)
                    fitInBlock = board.FitInBlock(XLength);
                if (board.GetNextEmptySquareToFill(ref curX, ref curY) && !fitInBlock)
                    availableMoves = board.GenerateAvailableMovesAtSquare(curX, curY);
                else
                {
                   if (fitInBlock)
                    {
                        long configInd = board.GetSpillOverConfigNumber(XLength);
                        ++_configurations[configInd];
                        if (_boards[configInd] == null)
                        {
                            var brd = new Board(4, YLength);
                            for (int x = XLength; x < board.XLength; x++)
                                for (int y = 0; y < board.YLength; y++)
                                    brd.Occupied[x - XLength, y] = board.Occupied[x, y];

                            _boards[configInd] = brd;
                           // brd.DrawBoard();
                        }
                    }

                    availableMoves.Clear();
                }
                 
               if (availableMoves.Any())
                {
                    foreach (PieceMove move in availableMoves)
                        movesAll[++movesAllInd] = move;

                    // place piece on the board
                    PieceMove lastPiece = movesAll[movesAllInd];
                    //Console.WriteLine("New piece {0} at ({1},{2}", lastPiece.Piece, lastPiece.X, lastPiece.Y);
                    if (!board.PlacePieceOnBoard(lastPiece))
                        return false;

                    movesCurrent[++movesCurrentInd] = lastPiece;
                }
                else
                {
                    while (movesAllInd >= 0 && movesCurrentInd >= 0 &&
                           movesAll[movesAllInd] == movesCurrent[movesCurrentInd])
                    {
                        board.RemovePieceFromBoard(movesCurrent[movesCurrentInd]);
                        movesAll[movesAllInd--] = null;
                        movesCurrent[movesCurrentInd--] = null;

                    }

                    if (movesAllInd < 0)
                        done = true;
                    else
                    {
                        PieceMove lastPiece = movesAll[movesAllInd];
                        if (!board.PlacePieceOnBoard(lastPiece))
                            return false;
                        movesCurrent[++movesCurrentInd] = lastPiece;
                    }
                }
            }

            return true;
        }

        public long SolveEuler161( int XLength, int YLength)
        {
            if (XLength % 2 != 0)
                return -1;

            _configurations = new int[262144];
            _boards = new Board[262144];
            _validConfigs = new List<int>();
            
            long answer = 0;
            
            SolveTriominoesWithSpillOver((XLength-4)/2,YLength);

             for(int i = 0; i < _configurations.Length; i++)
                 if (_configurations[i] > 0)
                 {
                   //  _boards[i].DrawBoard();
                     _validConfigs.Add(i);
                 }
            
            for (var left = 0; left < _validConfigs.Count; left++ )
            {
                var answerLeft = _configurations[_validConfigs[left]];

                var brd = _boards[_validConfigs[left]];

                for (var right = 0; right <=left ; right++)
                {
                    // _boards[configRight].DrawBoard();
                    var answerRight = _configurations[_validConfigs[right]];

                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < YLength; j++)
                            brd.Occupied[3 - i, j] = _boards[_validConfigs[right]].Occupied[i, j];

                    
                    var middle = SolveTriominoesWithBoard(brd);
                    
                   // Console.WriteLine("Solved in {0} moves", middle);
                   // brd.DrawBoard();

                   if( right == left )
                        answer += (answerLeft * middle * answerRight);
                   else
                       answer += (2 * answerLeft * middle * answerRight);
                }
            }
            
            //18,449,424,764,853,004
            //18,609,659,091,919,254

            return answer;
        }
    }
}
