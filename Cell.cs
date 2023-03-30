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
        public int cellValue { get; set; }

        public void SetDisplayType (DisplayType displayType) {
            this.displayType = displayType;
        }

        public void SetCellValue (int cellValue) {
            this.cellValue = cellValue;
        }

        public void SetCellValueForward() {
            this.cellValue = this.cellValue + 1;
        }

        public Cell(TwoDPoint position) {
            this.position = position;
            this.displayType = DisplayType.Undiscovered;
            this.cellValue = 0;
        }

        public override string ToString() {
            return String.Format("position: {0}, displayType: {1}, cellValue: {2}",
                    position.ToString(),
                    displayType.ToString(),
                    cellValue);
        }

    }

}
