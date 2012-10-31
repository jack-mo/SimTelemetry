﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimTelemetry.Data.Logger;
using SimTelemetry.Objects;
using Triton.Database;
using Triton.Maths;
using Timer = System.Timers.Timer;

namespace SimTelemetry.Data
{
    public class Splits
    {
        private static Filter _Split = new Filter(5);
        public static double Split { get { return _Split.Average; }}
        private bool PreviousRecords = false;
        private TelemetryLogReader _mReferenceLapReader;
        private Dictionary<double, double> MetersToTime_Table = new Dictionary<double, double>();
        private Timer UpdateSplitTime;
        private Timer _mUpdateBestLap;
        private double Time_Start = 0;

        public Splits()
        {
            Telemetry.m.Driving_Start += m_Driving_Start;
            Telemetry.m.Driving_Stop += m_Driving_Stop;
            Telemetry.m.Lap += m_Lap;
            Telemetry.m.Track_Loaded += m_Track_Loaded;

            UpdateSplitTime = new Timer {Interval = 50};
            UpdateSplitTime.Elapsed += (s, e) =>
                                           {
                                               double MyMeters = Telemetry.m.Sim.Drivers.Player.MetersDriven;
                                               double MyDt = Telemetry.m.Sim.Session.Time - Time_Start;
                                               double OldTime = GetTimeFromTable(MyMeters);
                                               if (OldTime > 0)
                                               {
                                                   _Split.Add(MyDt - OldTime/1000);
                                               }


                                           };

            // Trigger-once timer
            _mUpdateBestLap = new Timer{Interval=100};
            _mUpdateBestLap.AutoReset = false;
            _mUpdateBestLap.Elapsed += (s,e) =>
                                              {
                                                  if (Telemetry.m.Sim == null) return;
                                                  if (Telemetry.m.Track == null) return;
                                                  OleDbConnection con =
                                                      DatabaseOleDbConnectionPool.GetOleDbConnection();
                                                  using (
                                                      OleDbCommand times =
                                                          new OleDbCommand("SELECT Filepath FROM Laptimes WHERE " +
                                                                           "Car = '" +
                                                                           Telemetry.m.Sim.Drivers.Player.CarModel.
                                                                               Replace("'", "\\'") + "' AND " +
                                                                           "Series = '" +
                                                                           Telemetry.m.Sim.Drivers.Player.CarClass.
                                                                               Replace("'", "\\'") + "' AND " +
                                                                           "Circuit = '" +
                                                                           Telemetry.m.Track.Name.Replace("'", "\\'") +
                                                                           "' AND " +
                                                                           "Simulator = '" + Telemetry.m.Sim.Name + "'" +
                                                                           "AND Laptime > 1 " +
                                                                           "ORDER BY Laptime ASC",
                                                                           con))
                                                  using (OleDbDataReader times_Reader = times.ExecuteReader())
                                                  {
                                                      if (times_Reader.HasRows)
                                                      {
                                                          while (PreviousRecords == false && times_Reader.Read())
                                                          {
                                                              try
                                                              {
                                                                  _mReferenceLapReader =
                                                                      new TelemetryLogReader(times_Reader.GetString(0));
                                                                  _mReferenceLapReader.ReadPolling();
                                                                  PreviousRecords = true;
                                                              }
                                                              catch (Exception ex)
                                                              {
                                                                  _mReferenceLapReader = null;
                                                              }

                                                          }
                                                      }
                                                  }
                                                  DatabaseOleDbConnectionPool.Freeup();
                                                  Load_ExtractMetersTable();
                                              };
        }

        private void m_Lap(object sender)
        {
            Time_Start = Telemetry.m.Sim.Session.Time; // TODO: add local timing if not supported.
            // Get the last lap from the game.
            PreviousRecords = false;
            _mUpdateBestLap.Start();
        }

        private double GetTimeFromTable(double meters)
        {
            double previous_m = -1;
            lock (MetersToTime_Table)
            {
                foreach (double m in MetersToTime_Table.Keys)
                {
                    if (previous_m < meters && meters <= m)
                    {
                        return MetersToTime_Table[m];
                    }
                    else
                    {
                        previous_m = m;
                    }
                }
            }
            return -1;
        }

        private void Load_ExtractMetersTable()
        {
            if (PreviousRecords)
            {
                lock (MetersToTime_Table)
                {
                    MetersToTime_Table.Clear();
                    lock (_mReferenceLapReader)
                    {
                        foreach (KeyValuePair<double, TelemetrySample> kvp in _mReferenceLapReader.Samples)
                        {
                            double meter = _mReferenceLapReader.GetDouble(kvp.Value, "Driver.MetersDriven");
                            double time = kvp.Key;

                            if (MetersToTime_Table.ContainsKey(meter) == false)
                                MetersToTime_Table.Add(meter, time);

                        }
                    }
                }
            }
        }

        private void m_Track_Loaded(object sender)
        {
            PreviousRecords = false;
            // Get the best lap from the telemetry archive.

            _mUpdateBestLap.Start();
        }

        private void m_Driving_Start(object sender)
        {
            UpdateSplitTime.Start();
        }

        private void m_Driving_Stop(object sender)
        {
            UpdateSplitTime.Stop();
        }
    }

}