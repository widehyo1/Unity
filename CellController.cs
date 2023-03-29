using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour {

    // [Header("Component Reperences")]
    /*
    public GameStateController gameStateController;
    */

    public enum DisplayType { Discovered, Undiscovered, Flag, QuestionMark };
    public enum CellType { Number, Mine };

    public Button interactiveCell;
    public Dictionary<string, bool> interaction;
    public CellType cellType;
    public DisplayType displayType;
    // cellValue range: 0 ~ 8 (cellType number) || -1 (cellType mine)
    public int cellValue;

    public Sprite cell0;
    public Sprite cell1;
    public Sprite cell2;
    public Sprite cell3;
    public Sprite cell4;
    public Sprite cell5;
    public Sprite cell6;
    public Sprite cell7;
    public Sprite cell8;

    public Sprite cellFlag;
    public Sprite cellMine;
    public Sprite cellUndiscovered;
    public Sprite cellQuestionmark;

    /*
     * GetDisplayType() = DisplayType.Undiscovered;
     * cellType = Number;
     *
     * interaction = {
     *     left: bool, // uncover cell
     *     right: bool, // countdown displayMineNumber
     *     both: bool // ProbeMine
     * }
     *
     * DisplayType {
     *     Undiscovered, // left, right
     *     Discovered, // double(celltype number && adjacent uncovered cell)
     *     Flag, // right
     *     QuestionMark // right
     * }
     *
     * CellType {
     *    Number,
     *    Mine // gameover
     * }
     *
     */

    public void ClickLeft() {

        // 1. displayType Discovered
        // 2. change Image
        // 3. interaction.left = false, interaction.right = false
        // 4. biz logic
        // if cellType == number
        //     GameDicision()
        //     << if gameOnGoing == false
        //         StartTimer
        // else if cellType == mine
        //     GameOver()
        if (!GetInteraction("left")) {
            return;
        }

        DiscoverCellImage();
        SetInteraction();

        /*
        if (GetCellType == CellType.Number) {
            gameStateController.GameDicision();
        } else if (GetCellType == CellType.Mine) {
            gameStateController.GameOver();
        }
        */

    }

    public void ClickRight() {
        // 1. displaytype toggle (uncovered -> flag -> questionmark)
        // 2. biz logic (ToggleDisplay)
        // if uncovered -> flag
        //     countdown displayMineNumber
        //     interaction.left = false, interaction.right = true, interactive.both = false
        // else if flag -> questionmark
        //     countup displayMineNumber
        //     interaction.left = false, interaction.right = true, interaction.both = false
        // else if questionmark -> uncovered
        //     interaction.left = true, interaction.right = true, interaction.both = false

        if (!GetInteraction("right")) {
            return;
        }

        ToggleDisplay();
        SetInteraction();

    }

    public void ClickDouble() {

        // call ProbeMine()
        if (!GetInteraction("double")) {
            return;
        }

        // gameStateController.ProbeMine();

    }

    public void ToggleDisplay() {
        /*
        if (GetDisplayType() == DisplayType.Undiscovered) {
            displayType = DisplayType.Flag;
            return;
        }
        if (GetDisplayType() == DisplayType.Flag) {
            displayType = DisplayType.Questionmark;
            return;
        }
        if (GetDisplayType() == DisplayType.Questionmark) {
            displayType = DisplayType.Undiscovered;
            return;
        }
        */
    }

    public void SetInteraction() {
        if (GetDisplayType() == DisplayType.Undiscovered) {
            /*
            interaction.left = true;
            interaction.right = true;
            interaction.both = false;
            */
            interaction = new Dictionary<string, bool>();
            interaction.Add("left", true);
            interaction.Add("right", true);
            interaction.Add("both", false);
            return;
        }
        if (GetDisplayType() == DisplayType.Discovered) {
            /*
            interaction.left = false;
            interaction.right = false;
            interaction.both = true;
            */
            interaction = new Dictionary<string, bool>();
            interaction.Add("left", false);
            interaction.Add("right", false);
            interaction.Add("both", true);
            return;
        }
        if (GetDisplayType() == DisplayType.Flag) {
            /*
            interaction.left = false;
            interaction.right = true;
            interaction.both = false;
            */
            interaction = new Dictionary<string, bool>();
            interaction.Add("left", false);
            interaction.Add("right", true);
            interaction.Add("both", false);
            return;
        }
        if (GetDisplayType() == DisplayType.QuestionMark) {
            /*
            interaction.left = false;
            interaction.right = true;
            interaction.both = false;
            */
            interaction = new Dictionary<string, bool>();
            interaction.Add("left", false);
            interaction.Add("right", true);
            interaction.Add("both", false);
            return;
        }
    }

    public bool GetInteraction(string action) {
        if ("left".Equals(action)) {
            // return interactiveCell.interaction.left;
            return interaction["left"];
        }
        if ("right".Equals(action)) {
            // return interactiveCell.interaction.right;
            return interaction["right"];
        }
        if ("both".Equals(action)) {
            // return interactiveCell.interaction.both;
            return interaction["both"];
        }
        return false;
    }

    private void DiscoverCellImage() {
        // interactiveCell.DisplayType = DisplayType.Discovered;
        displayType = DisplayType.Discovered;
        interactiveCell.image.sprite = GetCellSprite();
    }

    private DisplayType GetDisplayType() {
        // return interactiveCell.DisplayType;
        return displayType;
    }

    public void SetCellValue(int _cellValue) {
        cellValue = _cellValue;
        if (_cellValue != -1) {
            // interactiveCell.cellType = CellType.Number;
            cellType = CellType.Number;
        } else if (_cellValue == -1) {
            // interactiveCell.cellType = CellType.Mine;
            cellType = CellType.Mine;
        }
    }

    public CellType GetCellType() {
        // return interactiveCell.cellType;
        return cellType;
    }


    private Sprite GetCellSprite() {
        if (GetDisplayType() == DisplayType.Undiscovered) return cellUndiscovered;
        if (GetDisplayType() == DisplayType.QuestionMark) return cellQuestionmark;
        if (GetDisplayType() == DisplayType.Flag) return cellFlag;

        // displayType == Discovered
        if (cellValue == 0) return cell0;
        if (cellValue == 1) return cell1;
        if (cellValue == 2) return cell2;
        if (cellValue == 3) return cell3;
        if (cellValue == 4) return cell4;
        if (cellValue == 5) return cell5;
        if (cellValue == 6) return cell6;
        if (cellValue == 7) return cell7;
        if (cellValue == 8) return cell8;
        if (cellValue == -1) return cellMine;

        return cellUndiscovered;
    }

}
