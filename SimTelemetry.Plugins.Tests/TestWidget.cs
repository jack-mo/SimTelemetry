﻿using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SimTelemetry.Domain;
using SimTelemetry.Objects;
using SimTelemetry.Objects.Plugins;
using SimTelemetry.Tests.Events;

namespace SimTelemetry.Game.Tests
{
    [Export(typeof(IWidget))]
    public class TestWidget : IWidget
    {
        public ITelemetry Host { get; set; }

        public TestWidget()
        {
            GlobalEvents.Fire(new PluginTestWidgetConstructor(), false);
        }

        public string PluginId
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return "Test Widget"; }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
        }

        public string Author
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public void Initialize()
        {
            Debug.WriteLine("TestWidget::Initialize()");
        }

        public void Deinitialize()
        {
            Debug.WriteLine("TestWidget::Deinitialize()");
        }

        public Control Control
        {
            get { throw new NotImplementedException(); }
        }

        public Size MinimumSize
        {
            get { throw new NotImplementedException(); }
        }

        public Size MaximumSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool Resizable
        {
            get { throw new NotImplementedException(); }
        }
    }
}