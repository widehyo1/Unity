using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using CoordinateSystem;
using GameSpace;
using RangeSpace;
using RangeUtilSpace;
using CellSpace;
using UnityEngine;
using Random = System.Random;
using Debug = UnityEngine.Debug;
using Range = RangeSpace.Range;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameUtilSpace {

    public enum GameState {
        BeforeStart,
        OnGoing,
        Win,
        GameOver
    }

    public static class GameUtil {

        public static GameState gameState = GameState.BeforeStart;
        public static GameBoard gameBoard;
        private static Board board;
        private static Dictionary<TwoDPoint, Cell> cellBoard;

        public static int mineNumber = 0;
        public static int flagNumber = 0;
        public static int correctFlagNumber = 0;

        public static GameBoard GetGameBoard() {
            return gameBoard;
        }

        public static Board GetBoard() {
            return board;
        }

        public static Dictionary<TwoDPoint, Cell> GetCellBoard() {
            return cellBoard;
        }

        public static void SetGameBoard(GameBoard paramGameBoard) {
            gameBoard = paramGameBoard;
            board = paramGameBoard.GetBoard();
            cellBoard = paramGameBoard.GetCellBoard();
            gameState = GameState.BeforeStart;
        }

        public static void StartGame() {
            gameState = GameState.OnGoing;
            foreach (TwoDPoint position in RangeUtil.GetBoardArea(board)) {
                cellBoard[position].displayType = DisplayType.Undiscovered;
            }
            // timer start
        }

        static GameUtil() {
            gameBoard = new GameBoard(new Board(0, 0));
            board = gameBoard.GetBoard();
            cellBoard = gameBoard.GetCellBoard();
            gameState = GameState.BeforeStart;
        }

        public static void PrintCellCorrect() {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (TwoDPoint position in cellBoard.Keys) {
                counter++;
                sb.Append(position.ToString() + ":" + cellBoard[position].GetCorrectness() + "\t");
                if (counter % (board.boardY + 1) == 0) {
                    sb.Append("\n");
                }
            }
            Debug.Log(sb.ToString());
        }

        public static void PrintCellDisplay() {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (TwoDPoint position in cellBoard.Keys) {
                counter++;
                sb.Append(position.ToString() + ":" + cellBoard[position].displayType + "\t");
                if (counter % (board.boardY + 1) == 0) {
                    sb.Append("\n");
                }
            }
            Debug.Log(sb.ToString());
        }

        public static void PrintCellValue(Dictionary<TwoDPoint, Cell> cellBoard) {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (TwoDPoint position in cellBoard.Keys) {
                counter++;
                sb.Append(position.ToString() + ":" + cellBoard[position].GetCellValue() + "\t");
                if (counter % (board.boardY + 1) == 0) {
                    sb.Append("\n");
                }
            }
            Debug.Log(sb.ToString());
        }

        public static void PrintCellValue() {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (TwoDPoint position in cellBoard.Keys) {
                counter++;
                sb.Append(position.ToString() + ": " + cellBoard[position].GetCellValue() + "\t");
                if (counter % (board.boardY + 1) == 0) {
                    sb.Append("\n");
                }
            }
            Debug.Log(sb.ToString());
        }

        public static void GameOver() {

            gameState = GameState.GameOver;

        }

        public static void InitiateMine(int paramMineNumber) {

            ResetCellBoard();

            mineNumber = paramMineNumber;
            flagNumber = 0;
            correctFlagNumber = 0;

            gameState = GameState.BeforeStart;

            Dictionary<TwoDPoint, Cell> mineDictionary = PlantMine(mineNumber);
            ComputeCell(mineDictionary);
            EvaluateBoard();

        }

        public static void EvaluateBoard() {

            foreach (TwoDPoint position in RangeUtil.GetBoardArea(board)) {
                Evaluate(cellBoard[position]);
            }

        }

        public static void ResetCellBoard() {
            // fill cellBoard with cell of displayType default, cellValue 0, isCorrect false
            
            foreach (TwoDPoint position in RangeUtil.GetBoardArea(board)) {
                cellBoard[position].displayType = DisplayType.Default;
                cellBoard[position].SetCellValue(0);
                cellBoard[position].SetCorrectness(false);
            }
        }

        public static bool Evaluate(Cell cell) {
            Debug.Assert(board.Contains(cell.position), "gameBoard must contain the cell");

            List<TwoDPoint> adjacentArea = RangeUtil.AdjacentArea(cell.position, board);
            
            int cellValue = cell.GetCellValue();
            if (cellValue == -1) {
                cell.SetCorrectness(cell.displayType == DisplayType.Flag);
                return cell.displayType == DisplayType.Flag;
            }

            int counter = 0;
            foreach (TwoDPoint position in adjacentArea) {
                // Cause I know position is in cellBoard.Keys [See Debug.Assert] 
                // access value simple cellBoard[position].
                // if I don't know whether dictionary[key] exists, i would use TryGetValue
                if (CorrectFlag(cellBoard[position])) {
                    ++counter;
                }
            }
            cell.SetCorrectness(cellValue == counter);
            return cellValue == counter;
        }

        private static bool CorrectFlag(Cell cell) {
            return cell.displayType == DisplayType.Flag && cell.GetCellValue() == -1;
        }

        /*
         * 1. Mine셀의 경우 Done, onGoing = false
         *
         * 2. 인접영역 중 Discovered가 아닌 셀들을 두개로 분류
         * : Flag, (Undiscovered || QuestionMark) [해당하는 유형의 셀에 대해 loop]
         * 2-1. 인접영역이 모두 Discovered라면 Done, onGoing = false
         *
         * 3. EvaluateCell -> correct라면 인접 영역 중 Undiscovered셀에 대하여 반복
         * 3-1. incorrect라면 인접 영역에 Flag셀이 있는지 판단
         * 3-1-1. Flag중 CorrectFlag가 아니라면 GameOver
         *
         *
         */
        public static void Expand(Cell cell) {
            int cellValue = cell.GetCellValue();
            if (cellValue == -1) {
                Debug.Log("cellValue == -1");
                return;
            }
            List<TwoDPoint> targetArea = GetTargetArea(cell);
            if (targetArea.Count == 0) {
                Debug.Log("targetArea.Count == 0");
                return;
            }
            Debug.Log("=== flag ===");
            List<TwoDPoint> nextStep = new List<TwoDPoint>();
            if (Evaluate(cell)) {
                Debug.Log("Evaluate(cell)");
                foreach (TwoDPoint position in RangeUtil.AdjacentArea(cell.position, board)) {
                    if (cellBoard[position].displayType != DisplayType.Flag) {
                        cellBoard[position].displayType = DisplayType.Discovered;
                        nextStep.Add(cellBoard[position].position);
                    }
                }
            } else {
                Debug.Log("else");
                foreach (TwoDPoint position in RangeUtil.AdjacentArea(cell.position, board)) {
                    // incorrect cell의 인접영역에 Flag가 있다면 잘못 설정한 Flag
                    // gameOver
                    if (cellBoard[position].displayType == DisplayType.Flag) {
                        Debug.Log(String.Format("position: {0}is incorrect Flag, gameOver", position.ToString()));
                        GameOver();
                        return;
                    }
                }
            }
            foreach (TwoDPoint nextStepCell in nextStep) {
                Debug.Log("foreach (TwoDPoint nextStepCell in nextStep)");
                Debug.Log(nextStepCell.ToString());
                Expand(cellBoard[nextStepCell]);
            }
        }

        private static List<TwoDPoint> GetTargetArea(Cell cell) {
            List<TwoDPoint> targetArea = new List<TwoDPoint>();
            foreach (TwoDPoint position in RangeUtil.AdjacentArea(cell.position, board)) {
                if (cellBoard[position].displayType != DisplayType.Discovered) {
                    targetArea.Add(position);
                }
            }
            return targetArea;
        }

        private static void ComputeCell(Dictionary<TwoDPoint, Cell> mineDictionary) {

            Dictionary<TwoDPoint, Cell> cloneBoard = cellBoard.DeepClone();

            List<TwoDPoint> minePositions = new List<TwoDPoint>(mineDictionary.Keys);
            foreach (TwoDPoint minePosition in minePositions) {

                List<TwoDPoint> mineDetectionArea = RangeUtil.AdjacentArea(minePosition, board);

                foreach (TwoDPoint position in mineDetectionArea) {

                    if (mineDictionary.TryGetValue(position, out Cell cell)) {
                        continue;
                    } else {
                        cloneBoard[position].SetCellValueForward();
                    }
                }
            }
            gameBoard.SetCellBoard(cloneBoard);
            cellBoard = gameBoard.GetCellBoard();
        }

        private static Dictionary<TwoDPoint, Cell> PlantMine(int mineNumber) {

            int counter = 0;
            Dictionary<TwoDPoint, Cell> mineDictionary = new Dictionary<TwoDPoint, Cell>();

            Debug.Assert(board != null, "game board is not 0,0 board");
            Debug.Assert(mineNumber >= 0, "mineNumber is not negative");

            if (board is null) {
                return mineDictionary;
            }

            Dictionary<TwoDPoint, Cell> cloneBoard = cellBoard.DeepClone();

            // generate random TwoDPoint position
            // position is to be added to mineDictionary only if position is not in

            Random rand = new Random();
            while (true) {
                if (counter >= mineNumber) {
                    break;
                }

                int randX = rand.Next(board.boardX);
                int randY = rand.Next(board.boardY);

                TwoDPoint position = new TwoDPoint(randX, randY);
                Cell mineCell;
                if (mineDictionary.TryGetValue(position, out mineCell)) {
                    continue;
                }
                mineCell = new Cell(position);
                mineCell.SetCellValue(-1);
                mineDictionary.Add(position, mineCell);

                cloneBoard[position] = mineCell;
                ++counter;
            }

            gameBoard.SetCellBoard(cloneBoard);
            cellBoard = gameBoard.GetCellBoard();
            return mineDictionary;
        }
    }

    public static class DictionaryExtensions {

        public static Dictionary<TKey, TValue> DeepClone<TKey, TValue>(this Dictionary<TKey, TValue> original) where TValue : class {

            Dictionary<TKey, TValue> clone = new Dictionary<TKey, TValue>(original.Count);
            foreach (var keyValuePair in original) {
                clone.Add(keyValuePair.Key, keyValuePair.Value.DeepClone());
            }
            return clone;
        }

        private static TValue DeepClone<TValue>(this TValue original) where TValue : class {
            if (original == null) {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream()) {
                formatter.Serialize(stream, original);
                stream.Seek(0, SeekOrigin.Begin);
                return (TValue)formatter.Deserialize(stream);
            }
        }
    }

}
