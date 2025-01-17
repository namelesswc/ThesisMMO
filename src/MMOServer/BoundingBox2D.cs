﻿using System;
using System.Diagnostics;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer {

    using Common.Types;

    /// <summary>
    /// The 3D floating point bounding box.
    /// </summary>
    internal struct BoundingBox2D {

        public Vector Max { get; set; }
        public Vector Min { get; set; }

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public BoundingBox2D(Vector min, Vector max) : this() {
            Min = min;
            Max = max;
            Debug.Assert(IsValid());
        }

        public static BoundingBox2D CreateFromPoints(params Vector[] points) {
            if (points == null) {
                throw new ArgumentNullException("points");
            }

            if (points.Length != 2) {
                throw new ArgumentException("points");
            }

            Vector min = points[0];
            Vector max = points[1];
            for (int i = 1; i < points.Length; i++) {
                min = Vector.Min(min, points[i]);
                max = Vector.Max(max, points[i]);
            }

            return new BoundingBox2D { Min = min, Max = max };
        }

        public Vector Size { get { return this.Max - this.Min; } }

        public bool Contains(Vector point) {
            // not outside of box?
            //if((point.X < this.Min.X)) {
            //    log.InfoFormat("MinxTestFailed !({0}<{1})", point.X, Min.X);
            //    return false;
            //}
            //if ((point.X > this.Max.X)) {
            //    log.InfoFormat("MaxxTestFailed");
            //    return false;
            //}
            //if ((point.Z < this.Min.Z)) {
            //    log.InfoFormat("MinzTestFailed");
            //    return false;
            //}
            //if ((point.Z > this.Max.Z)) {
            //    log.InfoFormat("MaxzTestFailed");
            //    return false;
            //}
            //return true;

            return (point.X < this.Min.X || point.X > this.Max.X || point.Z < this.Min.Z || point.Z > this.Max.Z) ==
                   false;
        }

        public BoundingBox2D IntersectWith(BoundingBox2D other) {
            return new BoundingBox2D { Min = Vector.Max(this.Min, other.Min), Max = Vector.Min(this.Max, other.Max) };
        }
        
        //public bool OverlapPoint(Vector point) {
        //    return Min.X <= point.X && Min.Z <= point.Z && Max.X > point.X && Max.Z > point.Z;
        //}

        public BoundingBox2D UnionWith(BoundingBox2D other) {
            return new BoundingBox2D { Min = Vector.Min(this.Min, other.Min), Max = Vector.Max(this.Max, other.Max) };
        }

        public bool IsValid() {
            return (this.Max.X < this.Min.X || this.Max.Z < this.Min.Z) == false;
        }

        public override string ToString() {
            return string.Format("Bounds Min:({0},{1}) Max:({2},{3})", Min.X, Min.Z, Max.X, Max.Z);
        }
    }
}