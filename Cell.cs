using System;
using CoordinateSystem;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace CellSpace {

    public enum DisplayType {
        Default,
        Discovered,
        Undiscovered,
        Flag,
        QuestionMark
    }

    [Serializable]
    public class Cell {

        public TwoDPoint position { get; set; }
        public DisplayType displayType = DisplayType.Default;
        private int cellValue { get; set; }
        private bool isCorrect = false;

        public void RigthClick() {

            switch (displayType) {
                case DisplayType.Undiscovered:
                    displayType = DisplayType.Flag;
                    break;
                case DisplayType.Flag:
                    displayType = DisplayType.QuestionMark;
                    break;
                case DisplayType.QuestionMark:
                    displayType = DisplayType.Undiscovered;
                    break;
                case DisplayType.Discovered:
                    break;
                case DisplayType.Default:
                    break;
            }

        }

        public bool IsUndiscovered() {
            return displayType == DisplayType.Undiscovered
                || displayType == DisplayType.QuestionMark
                || displayType == DisplayType.Default;
        }

        public bool IsFlag() {
            return displayType == DisplayType.Flag;
        }

        public bool IsDiscovered() {
            return displayType == DisplayType.Discovered;
        }

        public bool GetCorrectness() {
            return isCorrect;
        }

        public void SetCorrectness(bool isCorrect) {
            this.isCorrect = isCorrect;
        }

        public int GetCellValue() {
            return cellValue;
        }

        public void SetCellValue (int cellValue) {
            this.cellValue = cellValue;
        }

        public void SetCellValueForward() {
            this.cellValue = this.cellValue + 1;
        }

        public Cell(TwoDPoint position) {
            this.position = position;
            this.displayType = DisplayType.Default;
            this.cellValue = 0;
            this.isCorrect = false;
        }

        public override string ToString() {
            return String.Format("position: {0}, displayType: {1}, cellValue: {2}, isCorrect: {3}",
                    position.ToString(),
                    displayType.ToString(),
                    cellValue,
                    isCorrect);
        }

    }

}
