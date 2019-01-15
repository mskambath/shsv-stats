using System;
namespace shsv_stats
{
    public struct MeetInfo
    {
        public string Location;
        public string DateRange;
        public string Organizer;
        public int Year;
        public TimingType Timing;
        public CourseType CourseType;
        public DateTime FirstDate()
        {
            if (DateRange.Contains('-'))
            {
                return DateTime.Parse(DateRange.Split("-")[0]);
            }
            else
            {
                return DateTime.Parse(DateRange);
            }
        }
        public DateTime LastDate()
        {
            if (DateRange.Contains('-'))
            {
                return DateTime.Parse(DateRange.Split("-")[1]);
            }
            else
            {
                return DateTime.Parse(DateRange);
            }
        }
    }
}
