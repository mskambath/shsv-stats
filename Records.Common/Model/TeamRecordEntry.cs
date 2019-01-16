﻿using System;
using Records.Common;
namespace Model
{
    public struct TeamRecordEntry
    {
        public string Club;
        public DateTime Date;
        public TimeSpan Time;
        public string Discipline;
        public CourseType CourseType;
        public Sex sex;
    }
}
