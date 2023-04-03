using System;
using CoordinateSystem;
using System.Diagnostics;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace RangeSpace {

    [Serializable]
    public class Range : IEquatable<Range> {

        public int min { get; set; }
        public int max { get; set; }

        public Range(int min, int max) {
            min = min < 0 ? 0 : min;
            max = max < 0 ? 0 : max;
            Debug.Assert(min <= max, "max must be large or equal to min");

            this.min = min;
            this.max = max;
        }

        public Range(int max) {
            Debug.Assert(0 <= max, "max must be large or equal to 0");
            this.max = max;
            this.min = 0;
        }

        public bool Contains(int index) {
            return min <= index && index <= max;
        }

        public int Backward(int index) {
            return this.min < index ? index - 1 : index;
        }

        public int Forward(int index) {
            return index < this.max ? index + 1 : index;
        }

        public override string ToString() {
            return String.Format("Range::min {0}, Range::max {1})", min, max);
        }

        public override bool Equals(object obj) => this.Equals(obj as Range);

        public bool Equals(Range range) {
            if (range is null) {
                return false;
            }

            if (Object.ReferenceEquals(this, range)) {
                return true;
            }

            if (this.GetType() != range.GetType()) {
                return false;
            }

            return (min == range.min) && (max == range.max);
        }

        public override int GetHashCode() => (min, max).GetHashCode();

        public static bool operator ==(Range r1, Range r2) {
            if (r1 is null) {
                if (r2 is null) {
                    return true;
                }
                return false;
            }
            return r1.Equals(r2);
        }

        public static bool operator !=(Range r1, Range r2) => !(r1 == r2);
    }

    [Serializable]
    public class RangeX : Range {

        public RangeX(int min, int max) : base(min, max)  {
            // empty
        }

        public RangeX(int max) : base(max)  {
            // empty
        }

        public override string ToString() {
            return String.Format("RangeX::min {0}, Range::max {1})", this.min, this.max);
        }
    }

    [Serializable]
    public class RangeY : Range {

        public RangeY(int min, int max) : base(min, max) {
            // empty
        }

        public RangeY(int max) : base(max) {
            // empty
        }

        public override string ToString() {
            return String.Format("RangeY::min {0}, Range::max {1})", this.min, this.max);
        }
    }

    [Serializable]
    public class Board {

        public int boardX { get; set; }
        public int boardY { get; set; }

        public Board(int boardX, int boardY) {
            boardX = boardX < 0 ? 0 : boardX;
            boardY = boardY < 0 ? 0 : boardY;

            this.boardX = boardX;
            this.boardY = boardY;

        }

        public bool Contains(TwoDPoint position) {
            return this.GetRangeX().Contains(position.x) && this.GetRangeY().Contains(position.y);
        }

        public bool Contains(List<TwoDPoint> area) {
            foreach(TwoDPoint position in area) {
                if (!Contains(position)) {
                    return false;
                }
            }
            return true;
        }

        public Range GetRangeX() {
            return new RangeX(this.boardX);
        }

        public Range GetRangeY() {
            return new RangeY(this.boardY);
        }

        public override string ToString() {
            return String.Format("Board::boardX {0}, Board::boardY {1})", boardX, boardY);
        }

    }


}
