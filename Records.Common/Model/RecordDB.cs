using System;
using System.Collections;
using System.Collections.Generic;

namespace Records.Common.Model
{
    public class RecordDB
    {


        private List<SingleRecordEntry> mSingleStorage;
        private List<TeamRecordEntry> mTeamStorage;

        public RecordDB()
        {
            mSingleStorage = new List<SingleRecordEntry>();
            mTeamStorage = new List<TeamRecordEntry>();
        }

        public RecordDB(string Filename) {


            mSingleStorage = new List<SingleRecordEntry>();
            mTeamStorage = new List<TeamRecordEntry>();

            mSingleStorage.Add(new SingleRecordEntry() {
                Club = "XYZ", Name="HANS",Surname="XYE",
                Agegroup = new AgeGroup(0, 99),
                Date = new DateTime(2000, 02, 01),
                sex = Sex.Male,
                CourseType = CourseType.Short,
                Time = new TimeSpan(0, 0, 0, 25, 00),
                Discipline = "50S" });
            mSingleStorage.Add(new SingleRecordEntry()
            {
                Club = "50FRECORD",
                Name = "HOLDER",
                Surname = "XYE",
                Agegroup = new AgeGroup(14, 14),
                Date = new DateTime(2000, 02, 01),
                sex = Sex.Male,
                CourseType = CourseType.Short,
                Time = new TimeSpan(0, 0, 0, 27, 00),
                Discipline = "50S"
            });
            mSingleStorage.Add(new SingleRecordEntry()
            {
                Club = "XYZ",
                Name = "SIE",
                Surname = "SIE",
                CourseType = CourseType.Short,
                Agegroup = new AgeGroup(0, 99),
                Date = new DateTime(2000, 02, 01),
                sex = Sex.Female,
                Time = new TimeSpan(0, 0, 0, 27, 00),
                Discipline = "50S"
            });
            //mTeamStorage.Add(new TeamRecordEntry() { Club = "XYZ", Date = new DateTime(2000, 01, 01), sex = Sex.Male, Time = new TimeSpan(0, 0, 3, 36, 00), Discipline = "4x50 F" })
            mTeamStorage.Add(new TeamRecordEntry() { 
            Club = "XYZ", 
            CourseType = CourseType.Short,
            Date = new DateTime(2000, 01, 01), 
            sex = Sex.Male,
            Time = new TimeSpan(0, 0, 1, 36, 00),
             Discipline = "4x50F" });
        }

        public IList<SingleRecordEntry> TestSingleRecords(string discipline, Sex sex, CourseType course, uint birthyear, DateTime date, TimeSpan time)
        {
            var ret = new List<SingleRecordEntry>();
            foreach(SingleRecordEntry entry in mSingleStorage) { 
                if(entry.Discipline == discipline 
                && (entry.sex==sex || entry.sex==Sex.Mixed)
                && entry.Agegroup.Covers(birthyear,date)
                && (entry.CourseType == course)
                && time<entry.Time) {
                    ret.Add(entry);
                }
            }
            return ret;
        }

        public IList<TeamRecordEntry> TestTeamRecords(string discipline, Sex sex, CourseType course, DateTime date, TimeSpan time)
        {
            var ret = new List<TeamRecordEntry>();
            foreach (TeamRecordEntry entry in mTeamStorage)
            {
                if (entry.Discipline == discipline
                && (entry.sex == sex)
                && (entry.CourseType == course)
                && time < entry.Time)
                {
                    ret.Add(entry);
                }
            }
            return ret;
        }
    }
}
