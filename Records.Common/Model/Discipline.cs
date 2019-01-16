//
// Discipline.cs
//
// Author:
//       malte <>
//
// Copyright (c) 2019 ${CopyrightHolder}
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
namespace Records.Common.Model
{
	public struct Discipline
	{
		int Factor;
		int BaseLength;
		Records.Common.Model.SwimType SwimType;

		bool IsSingleDiscipline()
		{
			return Factor == 1;
		}

		bool IsTeamDiscipline() {
			return Factor > 1;
		}

		private static Records.Common.Model.SwimType ParseSwimTypeCode(char code)
		{
			switch(code)
			{
				case 'X': return Model.SwimType.None;
				case 'S': return Model.SwimType.Butterfly;
				case 'R': return Model.SwimType.Backstroke;
				case 'B': return Model.SwimType.Breaststroke;
				case 'F': return Model.SwimType.Freestyle;
				case 'L': return Model.SwimType.Medley;
			}
			return Model.SwimType.None;
		}

		private static string SwimTypeCode(Records.Common.Model.SwimType type)
		{
			switch(type)
			{
				case Model.SwimType.None: return "R"; break;
				case Model.SwimType.Butterfly: return "S"; break;
				case Model.SwimType.Backstroke: return "R"; break;
				case Model.SwimType.Breaststroke: return "B"; break;
				case Model.SwimType.Freestyle: return "F"; break;
				case Model.SwimType.Medley: return "L"; break;
			}
			return "";
		}
		public static Discipline Parse(string str)
		{
			var disc = new Discipline();
			if(str.Contains("x"))
			{
				disc.Factor = int.Parse( str.Split('x')[0]);
				disc.BaseLength = int.Parse(str.Split('x')[1]);
			}
			else
			{
				disc.Factor = 1;
				disc.BaseLength = int.Parse(str);
			}
			disc.SwimType = ParseSwimTypeCode(str[str.Length - 1]);

		}

		public override string ToString()
		{
			if (Factor > 1)
				return Factor + "x" + BaseLength + SwimTypeCode(SwimType);
			return BaseLength + SwimTypeCode(SwimType);

		}
	}
}
