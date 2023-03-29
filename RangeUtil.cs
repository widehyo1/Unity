using System;
using CoordinateSystem;
using RangeSpace;
using System.Diagnostics;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Range = RangeSpace.Range;

namespace RangeUtilSpace {

    public static class RangeUtil {

        public static Range GetAdjacentRange(int index, Range range) {
            Debug.Assert(range.Contains(index), "invalid index for given range");
            return new Range(range.Backward(index), range.Forward(index));
        }

        public static List<TwoDPoint> GetBoardArea(Board board) {

            Range boardRangeX = board.GetRangeX();
            Range boardRangeY = board.GetRangeY();

            return Product(boardRangeX, boardRangeY);
        }

        public static List<TwoDPoint> Product(Range rangeX, Range rangeY) {

            List<TwoDPoint> area = new List<TwoDPoint>();

            for (int x = rangeX.min; x <= rangeX.max; x++) {
                for (int y = rangeY.min; y <= rangeY.max; y++) {
                    area.Add(new TwoDPoint(x, y));
                }
            }

            return area;
        }

        public static List<TwoDPoint> AdjacentArea(TwoDPoint point, Board board) {

            List<TwoDPoint> area = new List<TwoDPoint>();
            Range boardRangeX = board.GetRangeX();
            Range boardRangeY = board.GetRangeY();

            Range rangeX = GetAdjacentRange(point.x, boardRangeX);
            Range rangeY = GetAdjacentRange(point.y, boardRangeY);

            return Product(rangeX, rangeY);

        }
    }
}
