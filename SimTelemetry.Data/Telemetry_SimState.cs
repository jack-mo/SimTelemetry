﻿namespace SimTelemetry.Data
{
    /// <summary>
    /// Class containing whether a simulator is running and has a session active.
    /// </summary>
    public class Telemetry_SimState
    {
        public bool Active;
        public bool Session;

        public Telemetry_SimState()
        {
            Active = false;
            Session = false;
        }
    }
}