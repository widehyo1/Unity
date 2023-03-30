using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System.IO;
using System.Text;
using System.Linq;
using System;
using CoordinateSystem;
using RangeSpace;
using RangeUtilSpace;
using Range = RangeSpace.Range;
using CellSpace;
using GameSpace;
using GameUtilSpace;

public class GameStateController : MonoBehaviour {

    public int displayMineNumber = 10;

    public MetaInfo metaInfo;

    public Sprite smile;
    public Sprite smileSunglass;
    public Sprite smileFrown;

    public Sprite timer0;
    public Sprite timer1;
    public Sprite timer2;
    public Sprite timer3;
    public Sprite timer4;
    public Sprite timer5;
    public Sprite timer6;
    public Sprite timer7;
    public Sprite timer8;
    public Sprite timer9;

    public Timer gameTimer;
    public string startTime;
    public string endTime;
    private StringBuilder currentGameRecord;
    private StringBuilder wholeGameRecord;
    private const string minesweeperDir = "Assets/Records";
    private const string difficultyFilePath = "Assets/Materials/difficulty.csv";
    private string difficultyName = ""; // = "";
    private string difficultyDir = ""; // = String.Format("{0}/{1}", minesweeperDir, difficultyName);
    private string recordFileName = ""; // = String.Format("{0}/{1}/{2}.txt", minesweeperDir, difficultyName, "record_" + difficultyName);

    private void DebugLog(object obj, string name = "") {
        if (!String.IsNullOrEmpty(name)) {
            Debug.Log(String.Format("{0}: ", name));
        }
        Debug.Log(String.Format("{0}", obj.ToString()));
    }

    public void SetDifficulty(string inputDifficulty) {
        // 난이도를 선택한다.
        // 세가지 버튼을 UI에서 생성하여 각 버튼에 binding
        // 난이도는 파일을 읽어서 비교한다.
        // DebugLog("=== SetDifficulty start ===");
        try {
            using (StreamReader sr = new StreamReader(difficultyFilePath)) {
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    if (values[0].Equals(inputDifficulty)) {
                        Difficulty difficulty = (Difficulty) Enum.Parse(typeof(Difficulty), inputDifficulty);
                        int rowNumber = Int32.Parse(values[1]);
                        int columnNumber = Int32.Parse(values[2]);
                        int mineNumber = Int32.Parse(values[3]);
                        metaInfo = new MetaInfo(difficulty, rowNumber, columnNumber, mineNumber);
                        DebugLog(metaInfo);
                        difficultyName = values[0];
                        break;
                    }
                }
            }
        } catch (Exception error) {
            Debug.Log(error.Message);
        }
        difficultyDir = String.Format("{0}/{1}", minesweeperDir, difficultyName);
        recordFileName = String.Format("{0}/{1}/{2}.txt", minesweeperDir, difficultyName, "record_" + difficultyName);
        // DebugLog(difficultyDir, "difficultyDir");
        // DebugLog(recordFileName, "recordFileName");
    }

    public void InitCellBoard() {
        // 1. initialize cellBoard of size rowNumber * columnNumber with 0
        // 2. 무작위 위치에 지뢰를 설정한다.
        //     while()
    }

    public void StartTimer() {
        // 타이머를 생성하고 초기화한다, 타이머를 실행한다.
        // 시작시간을 설정한다.
        startTime = DateTime.Now.ToString("yyyy-MM-dd_hh:mm");

        /*
        gameTimer = new System.Timers.Timer();
        timer.Interval = 1;
        timer.Elapsed += new ElapsedHandler(RenderTimer);
        timer.Start();
        */

    }

    public void RenderTimer() {
        // 타이머를 렌더링한다.
    }

    void Start() {
        DebugLog("=== GameStateController start ===");

        // SetDifficulty("NORMAL");
        SetDifficulty("EASY");
        DebugLog(difficultyName);
        /*
        int rowNumber = metaInfo.rowNumber;
        int columnNumber = metaInfo.columnNumber;

        Board board = new Board(rowNumber - 1, columnNumber - 1);
        List<TwoDPoint> boardArea = RangeUtil.GetBoardArea(board);
        DebugLog(string.Join(", ", boardArea));

        // GameBoard gameBoard = new GameBoard(boardArea);
        // GameBoard gameBoard = new GameBoard(board);
        */
        GameBoard gameBoard = new GameBoard(metaInfo);
        // DebugLog(gameBoard, "gameBoard");

        TwoDPoint point1 = new TwoDPoint(1, 2);
        DebugLog(gameBoard.GetCell(point1), "gameBoard.GetCell(point1)");

        GameUtil.InitiateMine(gameBoard, metaInfo.mineNumber);
        // DebugLog(string.Join(", ", mineDictionary.Keys));

        Dictionary<TwoDPoint, Cell> cellBoard = gameBoard.GetCellBoard();
        StringBuilder sb = new StringBuilder();
        foreach(TwoDPoint position in gameBoard.GetCellBoard().Keys) {
            sb.Append(position.ToString() + ":" + cellBoard[position].cellValue);
            // DebugLog(position, "position");
            // DebugLog(cellBoard[position].cellValue, "cellBoard[position].cellValue");
        }
        Debug.Log(sb.ToString());

        /*
        TwoDPoint point1 = new TwoDPoint(1, 2);
        TwoDPoint point2 = new TwoDPoint(1, 2);
        TwoDPoint point3 = new TwoDPoint(3, 4);
        DebugLog(point1);
        DebugLog(point2);
        DebugLog(point3);

        DebugLog(point1 == point2);
        DebugLog(point1 == point3);
        */

        /*
        Range rx1 = new RangeX(1, 1);
        Range ry1 = new RangeY(1, 3);
        Range rx2 = new RangeX(3, 1);
        Range ry2 = new RangeY(-1, 3);

        DebugLog(rx1);
        DebugLog(ry1);
        DebugLog(rx2);
        DebugLog(ry2);
        */

        /*
        Board board = new Board(9, 9);
        DebugLog(board);

        TwoDPoint point1 = new TwoDPoint(0, 0);
        DebugLog(point1);
        TwoDPoint point2 = new TwoDPoint(0, 4);
        DebugLog(point2);
        TwoDPoint point3 = new TwoDPoint(0, 9);
        DebugLog(point3);
        TwoDPoint point4 = new TwoDPoint(4, 0);
        DebugLog(point4);
        TwoDPoint point5 = new TwoDPoint(4, 4);
        DebugLog(point5);
        TwoDPoint point6 = new TwoDPoint(4, 9);
        DebugLog(point6);
        TwoDPoint point7 = new TwoDPoint(9, 0);
        DebugLog(point7);
        TwoDPoint point8 = new TwoDPoint(9, 4);
        DebugLog(point8);
        TwoDPoint point9 = new TwoDPoint(9, 9);
        DebugLog(point9);

        Range boardRangeX = board.GetRangeX();
        Range boardRangeY = board.GetRangeY();
        DebugLog(boardRangeX);
        DebugLog(boardRangeY);

        List<TwoDPoint> area = RangeUtil.Product(boardRangeX, boardRangeY);
        DebugLog(string.Join(", ", area));

        List<TwoDPoint> area1 = RangeUtil.AdjacentArea(point1, board);
        DebugLog(string.Join(", ", area1));
        List<TwoDPoint> area2 = RangeUtil.AdjacentArea(point2, board);
        DebugLog(string.Join(", ", area2));
        List<TwoDPoint> area3 = RangeUtil.AdjacentArea(point3, board);
        DebugLog(string.Join(", ", area3));
        List<TwoDPoint> area4 = RangeUtil.AdjacentArea(point4, board);
        DebugLog(string.Join(", ", area4));
        List<TwoDPoint> area5 = RangeUtil.AdjacentArea(point5, board);
        DebugLog(string.Join(", ", area5));
        List<TwoDPoint> area6 = RangeUtil.AdjacentArea(point6, board);
        DebugLog(string.Join(", ", area6));
        List<TwoDPoint> area7 = RangeUtil.AdjacentArea(point7, board);
        DebugLog(string.Join(", ", area7));
        List<TwoDPoint> area8 = RangeUtil.AdjacentArea(point8, board);
        DebugLog(string.Join(", ", area8));
        List<TwoDPoint> area9 = RangeUtil.AdjacentArea(point9, board);
        DebugLog(string.Join(", ", area9));
        */

    }

}
