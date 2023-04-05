using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CoordinateSystem;
using CellSpace;
using Range = RangeSpace.Range;
using RangeSpace;
using RangeUtilSpace;
using UnityEngine;


namespace GameSpace {

    public enum Difficulty { EASY, NORMAL, HARD };
    public enum GameState { BEFORE_START, ON_GOING, GAME_OVER, WIN };

    public static class DictionaryExtension {
        public static string GetString<K, V>(this IDictionary<K, V> dict) {
            var items = dict.Select(item => item.ToString());
            return string.Join(", ", items);
        }
    }

    [Serializable]
    public class MetaInfo {

        public Difficulty difficulty;
        public int rowNumber { get; set; }
        public int columnNumber { get; set; }
        public int mineNumber { get; set; }

        public MetaInfo(Difficulty difficulty, int rowNumber, int columnNumber, int mineNumber) {
            this.difficulty = difficulty;
            this.rowNumber = rowNumber;
            this.columnNumber = columnNumber;
            this.mineNumber = mineNumber;
        }

        public override string ToString() {
            return String.Format("metaInfo::ToString() difficulty: {0}, rowNumber: {1}, columnNumber: {2}, mineNumber: {3}",
                    difficulty.ToString(),
                    rowNumber,
                    columnNumber,
                    mineNumber);
        }
    }

    [Serializable]
    public class GameBoard {

        public Dictionary<TwoDPoint, Cell> cellBoard;
        public Board board = null;

        public void SetCellBoard(Dictionary<TwoDPoint, Cell> cellBoard) {
            this.cellBoard = cellBoard;
        }

        public Dictionary<TwoDPoint, Cell> GetCellBoard() {
            return cellBoard;
        }

        public Board GetBoard() {
            return board;
        }

        public GameBoard(List<TwoDPoint> area) {

            cellBoard = new Dictionary<TwoDPoint, Cell>();

            foreach(TwoDPoint position in area) {
                Cell cell = new Cell(position);
                cellBoard.Add(position, cell);
            }

        }

        public GameBoard(Board board)
            : this(RangeUtil.GetBoardArea(board)) {
            this.board = board;
        }

        public GameBoard(MetaInfo metaInfo)
            : this(RangeUtil.Product(
                        new Range(metaInfo.rowNumber - 1), 
                        new Range(metaInfo.columnNumber - 1)
                        )
                  ) {
            board = new Board(metaInfo.rowNumber - 1, metaInfo.columnNumber - 1);
        }

        public Cell GetCell(TwoDPoint position) {
            Cell cell = null;
            if (cellBoard.TryGetValue(position, out cell)) {
                return cell;
            } else {
                return null;
            }
        }

        public override string ToString() {
            return String.Format("GameBoard::cellBoard {0}", cellBoard.GetString());
        }

    }

}
