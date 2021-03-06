﻿using System.Collections.Generic;
using SimTelemetry.Domain.ValueObjects;

namespace SimTelemetry.Domain.Entities
{

    public class Engine
    {
        public string Name { get; private set; }
        public string Manufacturer { get; private set; }

        public int Cilinders { get; private set; }
        public int Displacement { get; private set; }

        public Range IdleRpm { get; private set; }
        public Range MaximumRpm { get; private set; }

        public IEnumerable<EngineMode> Modes { get; private set; }
        public IEnumerable<EngineTorque> TorqueCurve { get; private set; }

        public EngineLifetime Lifetime { get; private set; }

        public Engine(string name, string manufacturer, int cilinders, int displacement, Range idleRpm, Range maximumRpm, IEnumerable<EngineMode> modes, IEnumerable<EngineTorque> torqueCurve, EngineLifetime lifetime)
        {
            Lifetime = lifetime;
            Name = name;
            Manufacturer = manufacturer;
            Cilinders = cilinders;
            Displacement = displacement;
            IdleRpm = idleRpm;
            MaximumRpm = maximumRpm;
            Modes = modes;
            TorqueCurve = torqueCurve;
        }
    }
}
