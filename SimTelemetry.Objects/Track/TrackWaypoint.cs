﻿namespace SimTelemetry.Objects
{
    public struct TrackWaypoint
    {
        public double X;
        public double Y;
        public double Z;

        public TrackRoute Route;
        public int Sector;
        public double Meters;
        public double Width;
        public double[] PerpVector;
    }
}