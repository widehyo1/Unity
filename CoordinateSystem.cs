using System;

namespace CoordinateSystem {

    [Serializable]
    public readonly struct TwoDPoint : IEquatable<TwoDPoint> {

        public int x { get; }
        public int y { get; }

        public TwoDPoint(int paramX, int paramY) : this() {
            x = paramX;
            y = paramY;
        }

        public override string ToString() {
            return String.Format("({0}, {1})", x, y);
        }

#nullable enable
        public override bool Equals(object? obj) => obj is TwoDPoint other && this.Equals(other);

        public bool Equals(TwoDPoint point) => x == point.x && y == point.y;

        public override int GetHashCode() => (x, y).GetHashCode();

        public static bool operator ==(TwoDPoint p1, TwoDPoint p2) => p1.Equals(p2);

        public static bool operator !=(TwoDPoint p1, TwoDPoint p2) => !(p1 == p2);
    }
}
