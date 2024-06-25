using CoreDashboard.Models.Extras;

namespace CoreDashboard.Services.DataUploadingSevice
{
	public static class DataGetters
	{
		public static List<bool> GetControlPoints(IEnumerable<EducationalRecord> educationalRecords)
		{
			List<bool> controlPoints = [];
			var groupedControlPointsDict = educationalRecords
				.GroupBy(er => new { er.GroupName, er.PairThemeName })
				.ToDictionary(group => group.Key, group => group.Any(er => er.ControlPoint is not null));

			foreach (var er in educationalRecords)
				controlPoints.Add(groupedControlPointsDict[new { er.GroupName, er.PairThemeName }]);

			return controlPoints;
		}

		public static int GetPairType(string groupName)
		{
			var groupNameTag = GetGroupNameTag(groupName);

			return groupNameTag[0] switch
			{
				'л' => 1,
				'п' => 2,
				'к' => 3,
				'а' => 4,
				_ => 2
			};
		}

		public static string GetGroupNameTag(string groupName)
		{
			groupName = groupName.Trim().ToLower();

			int startIndex = groupName.IndexOf(' ') + 1;
			int endIndex = groupName.IndexOf('-');

			if (startIndex >= 0 && endIndex > startIndex)
				return groupName[startIndex..endIndex];

			return string.Empty;
		}

		public static long GetStudentIdFromEmail(string? email)
		{
			if (string.IsNullOrEmpty(email))
				return -1;

			string startString = "stud", endString = "@";
			int startIndex = email.IndexOf(startString);
			int endIndex = email.IndexOf(endString);

			if (startIndex != -1 && endIndex != -1)
			{
				bool result = long.TryParse(email.Substring(startIndex + startString.Length, endIndex - startString.Length), out long id);
				if (result)
					return id;
				else
					return -1;
			}
			else
				return -1;
		}
	}
}
