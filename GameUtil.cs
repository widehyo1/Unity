using System;
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
using System.Runtime.Serialization.Formatters.Binary;

namespace GameUtilSpace {

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

    public static class GameUtil {

        public static void InitiateMine(GameBoard gameBoard, int mineNumber) {
            Dictionary<TwoDPoint, Cell> mineDictionary = PlantMine(gameBoard, mineNumber);
            ComputeCell(gameBoard, mineDictionary);
        }

        private static void ComputeCell(GameBoard gameBoard, Dictionary<TwoDPoint, Cell> mineDictionary) {

            Dictionary<TwoDPoint, Cell> oldCellBoard = gameBoard.GetCellBoard();
            Dictionary<TwoDPoint, Cell> cellBoard = oldCellBoard.DeepClone();

            Board board = gameBoard.GetBoard();

            List<TwoDPoint> minePositions = new List<TwoDPoint>(mineDictionary.Keys);
            foreach(TwoDPoint minePosition in minePositions) {

                List<TwoDPoint> mineDetectionArea = RangeUtil.AdjacentArea(minePosition, board);

                foreach(TwoDPoint position in mineDetectionArea) {

                    Cell cell;
                    if (mineDictionary.TryGetValue(position, out cell)) {
                        continue;
                    } else {
                        cellBoard[position].SetCellValueForward();
                    }
                }
            }
            gameBoard.SetCellBoard(cellBoard);
        }

        private static Dictionary<TwoDPoint, Cell> PlantMine(GameBoard gameBoard, int mineNumber) {
            Debug.Assert(gameBoard.GetBoard() != null, "game board is not 0,0 board");

            int counter = 0;
            Dictionary<TwoDPoint, Cell> mineDictionary = new Dictionary<TwoDPoint, Cell>();

            Dictionary<TwoDPoint, Cell> oldCellBoard = gameBoard.GetCellBoard();
            Dictionary<TwoDPoint, Cell> cellBoard = oldCellBoard.DeepClone();

            Board board = gameBoard.GetBoard();
            if (board is null) {
                return mineDictionary;
            }

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

                cellBoard[position] = mineCell;
                ++counter;
            }

            gameBoard.SetCellBoard(cellBoard);
            return mineDictionary;
        }
    }

}
