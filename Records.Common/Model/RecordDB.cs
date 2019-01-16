using System;
using System.Collections;
using System.Collections.Generic;

namespace Records.Common.Model
{
	public struct RecordTable
	{
		public string Name;
        public  CourseType CourseType;
		public Sex Sex;
		public AgeGroup AgeGroup;
		public List<Discipline> Disciplines;
	}



	public class RecordDB
	{
		private List<RecordTable> mTables;
		private List<SingleRecordEntry> mSingleStorage;
		private List<TeamRecordEntry> mTeamStorage;

		public RecordDB()
		{
			RecordDBNew();
		}

		public List<RecordTable> Views
		{
			get
			{
				return this.mTables;
			}
		}

		public List<SingleRecordEntry> GetTable(RecordTable t)
		{
			var ret = new List<SingleRecordEntry>();
			foreach(var entry in mSingleStorage)
			{
				if (entry.sex == t.Sex && entry.Agegroup == t.AgeGroup)
					ret.Add(entry);
			}
			return ret;
		}

		private SingleRecordEntry CreateEmptySingleRecordEntry(Sex s, CourseType c, AgeGroup g, int length, SwimType swimType)
		{
			var entry = new SingleRecordEntry()
			{
				Club = "",
				Name = "",
				Surname = "",
				Agegroup = g,
				Date = "",
				sex = s,
				CourseType = c,
				Time = new TimeSpan(0, 0, 0, 0, 00),
				Discipline = new Discipline(length, swimType)
			};
			return entry;
	}

		public void RecordDBNew() {
			mSingleStorage = new List<SingleRecordEntry>();
			mTeamStorage = new List<TeamRecordEntry>();
			mTables = new List<RecordTable>();

			var groups = new AgeGroup[] {
			new AgeGroup(10,10),
			new AgeGroup(11,11),
			new AgeGroup(12,12),
			new AgeGroup(13,13),
			new AgeGroup(14,14), 
			new AgeGroup(15,15),
			new AgeGroup(16,16),
			new AgeGroup(0,99),
			};

			var sexsel = new Sex[] { Sex.Female, Sex.Male };
			foreach (AgeGroup g in groups)
			{
				foreach (CourseType c in Enum.GetValues(typeof(CourseType)))
				{
					foreach (Sex s in sexsel)
					{
						if ((s == Sex.Female && g == groups[6]) || (s == Sex.Male && g == groups[0]))
							continue;

						var table = new RecordTable() { AgeGroup = g, CourseType = c, Sex = s, Disciplines = new List<Discipline>() };
						if (g.IsOpen()) { 
							table.Name = "Landesrekorde"; 
						}
						else if (g.MinimumAge == g.MaximumAge)
						{
							table.Name = "Altersrekorde";
						}
						else
						{
							table.Name = "Altersklassenrekorde";
						}


						if (c != CourseType.OpenWater)
						{
							var type = SwimType.Butterfly;
							var lengths = new int[] { 50, 100, 200 };
							foreach (int l in lengths)
							{
								table.Disciplines.Add(new Discipline(l, type));
								mSingleStorage.Add(CreateEmptySingleRecordEntry(s, c,g, l, type));
							}

							type = SwimType.Backstroke;
							lengths = new int[] { 50, 100, 200 };
							foreach (int l in lengths)
							{
								table.Disciplines.Add(new Discipline(l, type));
								mSingleStorage.Add(CreateEmptySingleRecordEntry(s, c,g, l, type));
							}

							type = SwimType.Breaststroke;
							lengths = new int[] { 50, 100, 200 };
							foreach (int l in lengths)
							{
								table.Disciplines.Add(new Discipline(l, type));
								mSingleStorage.Add(CreateEmptySingleRecordEntry(s, c, g, l, type));
							}

							type = SwimType.Freestyle;
							lengths = new int[] { 50, 100, 200, 400, 800, 1500, 5000, 10000 };
							foreach (int l in lengths)
							{
								table.Disciplines.Add(new Discipline(l, type));
								mSingleStorage.Add(CreateEmptySingleRecordEntry(s, c,g, l, type));
							}

							type = SwimType.Medley;
							if(c != CourseType.Long)
								lengths = new int[] { 100, 200, 400 };
							else
								lengths = new int[] { 200, 400};
							foreach (int l in lengths)
								{
									table.Disciplines.Add(new Discipline(l, type));
									mSingleStorage.Add(CreateEmptySingleRecordEntry(s, c,g, l, type));
								}

							mTables.Add(table);
						}
						else
						{
							//mSingleStorage.Add(CreateEmptySingleRecordEntry(s, c, 1500, SwimType.Freestyle));
						}
					}
				}
			}

			//mSingleStorage = new List<SingleRecordEntry>();
            mTeamStorage = new List<TeamRecordEntry>();

            mSingleStorage.Add(new SingleRecordEntry() {
                Club = "XYZ", Name="HANS",Surname="XYE",
                Agegroup = new AgeGroup(0, 99),
                Date = "",
                sex = Sex.Male,
                CourseType = CourseType.Short,
                Time = new TimeSpan(0, 0, 0, 25, 00),
                Discipline = Discipline.Parse("50S")
			});
            mSingleStorage.Add(new SingleRecordEntry()
            {
                Club = "50FRECORD",
                Name = "HOLDER",
                Surname = "XYE",
                Agegroup = new AgeGroup(14, 14),
                Date = "",
                sex = Sex.Male,
                CourseType = CourseType.Short,
                Time = new TimeSpan(0, 0, 0, 27, 00),
                Discipline = Discipline.Parse("50S")
			});
            mSingleStorage.Add(new SingleRecordEntry()
            {
                Club = "XYZ",
                Name = "SIE",
                Surname = "SIE",
                CourseType = CourseType.Short,
                Agegroup = new AgeGroup(0, 99),
                Date = "",
                sex = Sex.Female,
                Time = new TimeSpan(0, 0, 0, 27, 00),
                Discipline = Discipline.Parse("50S")
            });
            //mTeamStorage.Add(new TeamRecordEntry() { Club = "XYZ", Date = new DateTime(2000, 01, 01), sex = Sex.Male, Time = new TimeSpan(0, 0, 3, 36, 00), Discipline = "4x50 F" })
            mTeamStorage.Add(new TeamRecordEntry() { 
            Club = "XYZ", 
            CourseType = CourseType.Short,
            Date = "", 
            sex = Sex.Male,
            Time = new TimeSpan(0, 0, 1, 36, 00),
             Discipline = new Discipline(4,50,SwimType.Freestyle) });
        }

        public IList<SingleRecordEntry> TestSingleRecords(string discipline, Sex sex, CourseType course, uint birthyear, DateTime date, TimeSpan time)
        {
            var ret = new List<SingleRecordEntry>();
            foreach(SingleRecordEntry entry in mSingleStorage) { 
                if(entry.Discipline.Equals(discipline) 
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
                if (entry.Discipline.Equals(discipline)
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

