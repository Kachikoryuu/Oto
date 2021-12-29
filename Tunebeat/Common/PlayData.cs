﻿using System.IO;
using Amaoto;
using static DxLibDLL.DX;

namespace Tunebeat.Common
{
    class PlayData
    {
        public static void Init()
        {
            if (!File.Exists("Config.json"))
                ConfigManager.SaveConfig(Data, "Config.json");

            Data = ConfigManager.GetConfig<Data>(@"Config.json");
        }
        public static void End()
        {
            ConfigManager.SaveConfig(Data, "Config.json");
        }
        public static Data Data { get; set; }
    }
    class Data
    {
        public string SkinName = "Default";
        public string SoundName = "Default";
        public string BGMName = "Default";
        public string PlayFile = @"Songs/水天神術・時雨.tja";
        public int[] PlayCourse = new int[2] { 3, 3 };
        public bool IsPlay2P = false;

        public bool[] Auto = new bool[2] { false, false };
        public int AutoRoll = 15;

        public int[] GaugeType = new int[2] { 0, 0 };
        public int[] GaugeAutoShift = new int[2] { 0, 0 };
        public int[] GaugeAutoShiftMin = new int[2] { 0, 0 };
        public int[] Hazard = new int[2] { 0, 0 };

        public int JudgeType = 0;
        public int JudgePerfect = 10;
        public int JudgeGreat = 25;
        public int JudgeGood = 75;
        public int JudgeBad = 100;
        public int JudgePoor = 120;
        public bool Just = false;

        public int LEFTDON = KEY_INPUT_F;
        public int RIGHTDON = KEY_INPUT_J;
        public int LEFTKA = KEY_INPUT_D;
        public int RIGHTKA = KEY_INPUT_K;

        public int LEFTDON2P = KEY_INPUT_V;
        public int RIGHTDON2P = KEY_INPUT_M;
        public int LEFTKA2P = KEY_INPUT_C;
        public int RIGHTKA2P = KEY_INPUT_COMMA;
    }
}