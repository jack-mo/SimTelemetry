﻿using System.Collections.Generic;
using SimTelemetry.Core.Aggregates;
using SimTelemetry.Core.ValueObjects;

namespace SimTelemetry.Core.Entities
{
    public class Season
    {
        public string Name { get; private set; }
        public IEnumerable<string> Classes { get; private set; }
        public Range Opponents { get; private set; }
        public IEnumerable<Track> Tracks { get; private set; }

        public Season(string name, IEnumerable<string> classes, Range opponents, IEnumerable<Track> tracks)
        {
            Name = name;
            Classes = classes;
            Opponents = opponents;
            Tracks = tracks;
        }
    }
}