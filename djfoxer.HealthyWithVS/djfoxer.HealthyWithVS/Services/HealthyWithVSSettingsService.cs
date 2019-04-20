﻿using djfoxer.HealthyWithVS.Options;
using djfoxer.HealthyWithVS.StatusBar;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.HealthyWithVS.Services
{
    public class HealthyWithVSSettingsService
    {

        private HealthyWithVSSettingsService()
        {
        }

        private static HealthyWithVSSettingsService _Instance { get; set; }

        public static HealthyWithVSSettingsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new HealthyWithVSSettingsService();
                }
                return _Instance;

            }
        }

        public bool AutostartPomodoroStatusBar { get; set; }

        private int _WorkTimerSeconds;
        public int WorkTimerSeconds
        {
            get
            {
                return _WorkTimerSeconds;
            }
            set
            {
                _WorkTimerSeconds = value;

                TomatoStatusBarInstance?.StopClick();

            }
        }

        public bool WorkoutActive { get; set; }

        public TomatoStatusBar TomatoStatusBarInstance { get; set; }
    }
}
