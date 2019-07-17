using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Xml.Schema;

namespace Euler1
{
    public enum Piece
    {
        Empty = 0,
        Horizontal,
        Vertical,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }

    public class PieceMove
    {
        public PieceMove(int x, int y, Piece piece)
        {
            Piece = piece;
            X = x;
            Y = y;
        }

        public Piece Piece { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Board
    {
        public int XLength;
        public int YLength;
        
        public PieceMove[] Moves { get; set; }
        public bool[,] Occupied { get; set; }

        public Board(int length, int width)
        {
            XLength = length;
            YLength = width;

            Occupied = new bool[length, width];
            Moves = new PieceMove[] {};
        }

        public void InitializeBoard(int length, int width)
        {
            for (int i = 0; i < length; i++)
                for (int j = 0; j < width; j++)
                    Occupied[i, j] = false;
        }

        public void DrawBoard()
        {
            for (int i = YLength - 1; i >= 0; i--)
            {
                for (int j = 0; j < XLength; j++)
                    Console.Write(Occupied[j, i] ? "X" : "0");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool PlacePieceOnBoard(PieceMove pieceMove)
        {
            var x = pieceMove.X;
            var y = pieceMove.Y;

            if (Occupied[x, y])
                return false;

            Occupied[x, y] = true;

            switch (pieceMove.Piece)
            {
                case Piece.Vertical:
                    Occupied[x, y - 1] = true;
                    Occupied[x, y + 1] = true;
                    break;
                case Piece.Horizontal:
                    Occupied[x - 1, y] = true;
                    Occupied[x + 1, y] = true;
                    break;
                case Piece.DownLeft:
                    Occupied[x, y - 1] = true;
                    Occupied[x - 1, y] = true;
                    break;
                case Piece.DownRight:
                    Occupied[x, y - 1] = true;
                    Occupied[x + 1, y] = true;
                    break;
                case Piece.UpLeft:
                    Occupied[x, y + 1] = true;
                    Occupied[x - 1, y] = true;
                    break;
                case Piece.UpRight:
                    Occupied[x, y + 1] = true;
                    Occupied[x + 1, y] = true;
                    break;
            }

            // log result
          //  Console.WriteLine("Placed piece {2} at ({0},{1}):", x, y, pieceMove.Piece);

            return true;
        }

        public List<PieceMove> GenerateAvailableMovesAtSquare(int x, int y)
        {
            var pieces = new List<PieceMove>();

            if (Occupied[x, y] == true)
                return pieces;

            var left = x > 0 && Occupied[x - 1, y] == false;
            var leftUp = x > 0 && y < YLength - 1 && Occupied[x - 1, y + 1] == false;
            var leftDown = x > 0 && y > 0 && Occupied[x - 1, y - 1] == false;
            var right = x < XLength - 1 && Occupied[x + 1, y] == false;
            var rightUp = x < XLength - 1 && y < YLength - 1 && Occupied[x + 1, y + 1] == false;
            var rightDown = x < XLength - 1 && y > 0 && Occupied[x + 1, y - 1] == false;

            var up = y < YLength - 1 && Occupied[x, y + 1] == false;
            var down = y > 0 && Occupied[x, y - 1] == false;

            var right2Spaces = x < XLength - 2 && Occupied[x + 2, y] == false;
            var left2Spaces = x > 1 && Occupied[x - 2, y] == false;
            var up2Spaces = y < YLength - 2 && Occupied[x, y + 2] == false;
            var down2Spaces = y > 1 && Occupied[x, y - 2] == false;


            if (left && right) pieces.Add(new PieceMove(x, y, Piece.Horizontal));
            if (left && left2Spaces) pieces.Add(new PieceMove(x - 1, y, Piece.Horizontal));
            if (right && right2Spaces) pieces.Add(new PieceMove(x + 1, y, Piece.Horizontal));

            if (up && down) pieces.Add(new PieceMove(x, y, Piece.Vertical));
            if (up && up2Spaces) pieces.Add(new PieceMove(x, y + 1, Piece.Vertical));
            if (down && down2Spaces) pieces.Add(new PieceMove(x, y - 1, Piece.Vertical));

            //  _| UpLeft shape
            if (left && up) pieces.Add(new PieceMove(x, y, Piece.UpLeft));
            if (right && rightUp) pieces.Add(new PieceMove(x + 1, y, Piece.UpLeft));
            if (down && leftDown) pieces.Add(new PieceMove(x, y - 1, Piece.UpLeft));

            //  |_ UpRight shape
            if (right && up) pieces.Add(new PieceMove(x, y, Piece.UpRight));
            if (left && leftUp) pieces.Add(new PieceMove(x - 1, y, Piece.UpRight));
            if (down && rightDown) pieces.Add(new PieceMove(x, y - 1, Piece.UpRight));

            //  _
            //   | DownLeft shape
            if (left && down) pieces.Add(new PieceMove(x, y, Piece.DownLeft));
            if (up && leftUp) pieces.Add(new PieceMove(x, y + 1, Piece.DownLeft));
            if (right && rightDown) pieces.Add(new PieceMove(x + 1, y, Piece.DownLeft));

            //  _ 
            // |   Down Right shape
            if (right && down) pieces.Add(new PieceMove(x, y, Piece.DownRight));
            if (up && rightUp) pieces.Add(new PieceMove(x, y + 1, Piece.DownRight));
            if (left && leftDown) pieces.Add(new PieceMove(x - 1, y, Piece.DownRight));

            // log result
            //Console.Write("Available moves for ({0},{1}):", x, y);
            //foreach (var piece in pieces)
            //{
            //    Console.Write("{0} at ({1},{2}); ", piece.Piece.ToString(), piece.X, piece.Y);
            //}
            //Console.WriteLine();

            return pieces;
        }

        public bool RemovePieceFromBoard(PieceMove piece)
        {
            var x = piece.X;
            var y = piece.Y;

            int x1, x2, y1, y2;

            switch (piece.Piece)
            {
                case Piece.Horizontal:
                    x1 = x - 1;
                    x2 = x + 1;
                    y1 = y;
                    y2 = y;
                    break;

                case Piece.Vertical:
                    x1 = x;
                    x2 = x;
                    y1 = y - 1;
                    y2 = y + 1;
                    break;

                case Piece.UpRight:
                    x1 = x;
                    x2 = x + 1;
                    y1 = y + 1;
                    y2 = y;
                    break;

                case Piece.UpLeft:
                    x1 = x;
                    x2 = x - 1;
                    y1 = y + 1;
                    y2 = y;
                    break;

                case Piece.DownRight:
                    x1 = x;
                    x2 = x + 1;
                    y1 = y - 1;
                    y2 = y;
                    break;

                case Piece.DownLeft:
                    x1 = x;
                    x2 = x - 1;
                    y1 = y - 1;
                    y2 = y;
                    break;
                default:
                    return true;
            }

            Occupied[x, y] = false;
            Occupied[x1, y1] = false;
            Occupied[x2, y2] = false;

            // log result
          //  Console.WriteLine("Removed piece {2} at ({0},{1}):", x, y, piece.Piece);

            return true;
        }

        public bool GetNextEmptySquareToFill(ref int curX, ref int curY)
        {
            bool found = false;
            for (var x = 0; x < XLength; ++x)
                for (var y = 0; y < YLength; ++y)
                    if (!Occupied[x, y])
                    {
                        if (!CanPlacePieceAtSquare(x, y))
                            return false;
                        if (!found)
                        {
                            curX = x;
                            curY = y;
                            found = true;
                        }
                    }

            return found;
        }

        private bool CanPlacePieceAtSquare(int x, int y
            //out bool left, out bool right, out bool up, out bool down,
            //out bool leftDown, out bool rightDown, out bool leftUp,out bool rightUp,
            //out bool left2spaces, out bool right2spaces, out bool up2Spaces, out bool down2Spaces
            )
        {
            if (Occupied[x, y])
                return true;

            var left = x > 0 && Occupied[x - 1, y] == false;
            var leftUp = x > 0 && y < YLength - 1 && Occupied[x - 1, y + 1] == false;
            var leftDown = x > 0 && y > 0 && Occupied[x - 1, y - 1] == false;
            var right = x < XLength - 1 && Occupied[x + 1, y] == false;
            var rightUp = x < XLength - 1 && y < YLength - 1 && Occupied[x + 1, y + 1] == false;
            var rightDown = x < XLength - 1 && y > 0 && Occupied[x + 1, y - 1] == false;

            var up = y < YLength - 1 && Occupied[x, y + 1] == false;
            var down = y > 0 && Occupied[x, y - 1] == false;

            var right2Spaces = x < XLength - 2 && Occupied[x + 2, y] == false;
            var left2Spaces = x > 1 && Occupied[x - 2, y] == false;
            var up2Spaces = y < YLength - 2 && Occupied[x, y + 2] == false;
            var down2Spaces = y > 1 && Occupied[x, y - 2] == false;

            return (left && (right || left2Spaces || leftUp || leftDown || up || down)) ||
                   (right && (left || right2Spaces || rightUp || rightDown || up || down)) ||
                   (up && (down || up2Spaces || leftUp || rightUp)) ||
                   (down && (up || down2Spaces || leftDown || rightDown));
        }

        public bool FitInBlock(int XLength)
        {
            if (XLength < 1)
                return false;

            for(var x = Math.Max(0,XLength-2); x < XLength; x++)
            for (var y = 0; y < YLength; ++y)
                if (!Occupied[x, y])
                    return false;
            return true;
        }

        public bool IsSolved()
        {
            for (var y = 0; y < YLength; ++y)
                if (!Occupied[XLength - 1, y])
                    return false;
            return true;
        }

        public int CountTriominoesToSolve()
        {
            int free = 0;

            for( var x = 0; x < XLength; ++x )
                for (var y = 0; y < YLength; ++ y)
                {
                    free += Occupied[x, y] ? 0 : 1;
                    if (!CanPlacePieceAtSquare(x, y))
                        return -1;
                }

            if (free%3 != 0)
                return -1;

            return free/3;
        }

        public int GetSpillOverConfigNumber(int XStart)
        {
            if (XStart < 1)
                return -1;

            int binary = 0;
            int ind = 1;
            for (var x = XStart; x < XLength; ++x)
                for (var y = 0; y < YLength; ++y)
                {
                    binary += ind * (Occupied[x,y] ? 1 : 0);
                    ind *= 2;
                }
     
            return binary;
        }
    }
}