using System;
namespace Records.Common.Model
{
    public class AgeGroup
    {
        public uint MinimumAge{get;set;}
        public uint MaximumAge { get; set; }
        public AgeGroup(uint MinimumAge,  uint MaximumAge)
        {
            this.MaximumAge = MaximumAge;
            this.MinimumAge = MinimumAge;
        }

        public AgeGroup(string agkey) {
            // AK (1[0-9]|!1) | 
            var ak = agkey.Replace("AK","").Trim();
            var aka = ak.ToCharArray();
            this.MinimumAge = uint.Parse(ak);
            if (ak[0] <= 1)
            {
                this.MaximumAge = this.MinimumAge;
            }
            else 
            {

                this.MaximumAge = this.MaximumAge + 4;
            }        
        }

		public bool IsOpen()
		{
			return MinimumAge == 0 && MaximumAge >= 99 ;
		}

		public bool Covers(uint age) {
            return (age <= this.MaximumAge) && (age >= this.MinimumAge);
        }

        /// <summary>
        /// Tests if a person born in the specified birthyear is in 
        /// this AgeRange in a specified year.
        /// </summary>
        /// <returns>True, if and only if a person born in the specified year is in this AgeGroup in the specfied year. </returns>
        /// <param name="birthyear">Birthyear.</param>
        /// <param name="year">Year.</param>
        public bool Covers(uint birthyear, uint year)
        {
            return Covers(year - birthyear);
        }

        public bool Covers(uint birthyear, DateTime date)
        {
            return Covers(birthyear, (uint) date.Year);
        }

        public override string ToString()
        {
            if (this.MinimumAge == this.MaximumAge)
                return this.MinimumAge.ToString();
            else
                return this.MinimumAge.ToString() + "-" + this.MaximumAge.ToString();
        }

		public override bool Equals(object obj)
		{
			if (!(obj is AgeGroup)) return false;
			return this.MinimumAge == ((AgeGroup)obj).MinimumAge && this.MaximumAge == ((AgeGroup)obj).MaximumAge; 
		}
	}
}
