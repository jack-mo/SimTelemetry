﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimTelemetry.Objects
{

    public interface ILap
    {
        int Lap { get; }
        float Sector1 { get; }
        float Sector2 { get; }
        float Sector3 { get; }

        float LapTime { get; }
    }
}