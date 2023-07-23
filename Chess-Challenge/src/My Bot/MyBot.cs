using ChessChallenge.API;
using System.Collections.Generic;
using System.Linq;

public class MyBot : IChessBot {
	/*
     Piece types:
        0: None
        1: Pawn
        2: Knight
        3: Bishop
        4: Rook
        5: Queen
        6: King
     */
	readonly int[] pieceCaptureScores = { 0, 5, 50, 35, 40, 300, 100000 };

	public Move Think(Board board, Timer timer) {
		List<Move> moves = board.GetLegalMoves().ToList();
		List<KeyValuePair<Move, int>> moveScores = new List<KeyValuePair<Move, int>>();
		foreach (var move in moves) {
			board.MakeMove(move);
			moveScores.Add(new KeyValuePair<Move, int>(move, GetAdvantageScore(move, board)));
			board.UndoMove(move);
		}
		moveScores = moveScores.OrderBy(x => x.Value).ToList();
		return moveScores.Last().Key;
	}

	public int GetAdvantageScore(Move move, Board board) {
		Square targettingSquare = move.TargetSquare;
		int danger = pieceCaptureScores[(int)move.MovePieceType] / 5;
		int runningTotal = 0;
		if (board.IsInCheck()) runningTotal+=50;
		if (move.IsCapture)
			runningTotal += pieceCaptureScores[((int)move.CapturePieceType)];
		if (move.IsPromotion)
			runningTotal += pieceCaptureScores[(int)move.PromotionPieceType];

		return runningTotal;
	}
}