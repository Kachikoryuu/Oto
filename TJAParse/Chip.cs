﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJAParse
{
    public class Chip
    {
        public ENote ENote;
        public EChip EChip;
        public EScroll EScroll;
        public double Time;
        public double Bpm;
        public double Scroll;
        public double Measure;
        public int RollCount;
        public int Balloon;
        public bool IsGogo;
        public bool IsHit;
        public bool IsMiss;
        public bool IsRoll;
        public bool IsShow;
        public bool CanShow;
        public Chip RollEnd;
        public string Lyric;
    }

    public enum EChip
    {
        Note,
        Measure,
        GoGoStart,
        GoGoEnd
    }

    public enum ENote
    {
        Space,
        Don,
        Ka,
        DON,
        KA,
        RollStart,
        ROLLStart,
        Balloon,
        RollEnd,
        Kusudama
    }
}
