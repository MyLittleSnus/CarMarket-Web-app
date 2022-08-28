namespace Razor_Test.AdditionalEntities
{
	public static class Helper
	{
		public static string ResourcesFolderName { get; set; } = "ProfileResources";

		public static string SavePhoto(IFormFile file, IWebHostEnvironment env, int id)
        {
            if (file == null)
                return null;

            Func<string> pathCalculator = () =>
            {
                var part = Path.Combine(Path.Combine(ResourcesFolderName, "Pictures"));

                return Path.Combine(part, $"pimg_{id}.jpg");
            };


            var path = pathCalculator();

            if (File.Exists(path))
                File.Delete(path);
            var pathTOCreate = Path.Combine(env.WebRootPath, path);

            using (FileStream stream = new FileStream(pathTOCreate, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(stream);
            }

            return pathTOCreate;
        }

        public static string GetEntityPhotoRelativePath(int id)
        {
            var part = Path.Combine(Path.Combine(ResourcesFolderName, "Pictures"));

            return Path.Combine(part, $"pimg_{id}.jpg");
        }
	}
}