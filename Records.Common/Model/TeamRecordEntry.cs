using System;
using Records.Common;
namespace Records.Common.Model
{
    public struct TeamRecordEntry
    {
        public string Club;
		public string ClubId;
		public string Date;
        public TimeSpan Time;
        public Discipline Discipline;
        public CourseType CourseType;
        public Sex sex;
    }
}
