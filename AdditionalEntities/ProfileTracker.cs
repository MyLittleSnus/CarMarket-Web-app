using Razor_Test.Models;

namespace Razor_Test.AdditionalEntities
{
	public static class ProfileTracker
	{
		public static Profile CurrentProfile { get; set; } = new Profile()
		{
			Username = "Account"
		};

		public static void TrackProfile (Profile profile)
        {
			CurrentProfile = profile;
        }
	}
}

