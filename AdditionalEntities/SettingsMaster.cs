using Newtonsoft.Json;

namespace Razor_Test.AdditionalEntities
{
    public class SettingsMaster
    {
        private Dictionary<string, (string[] AvailableEngineVolume, string[] AvailableSeats, string[] AvailableLight, string[] AvailableGearBoxes)>? carSettingsStore;
        private string storagePath;

        public SettingsMaster(IConfiguration configuration)
        {
            storagePath = configuration
                .GetSection("CarSettingsStorage")
                .GetSection("JsonPath")
                .Value;

            if (!File.Exists(storagePath))
                File.Create(storagePath);

            var fileContent = File.ReadAllText(storagePath);
            carSettingsStore = JsonConvert.DeserializeObject<
                Dictionary<string,
                (string[], string[], string[], string[])>>
                (fileContent);

            if (carSettingsStore == null)
                carSettingsStore = new();
        }

        public void AddSettings(KeyValuePair<
            (string Manufacturer, string Model, int Year),
            (string[] MinMaxEngineVolume, string[] AvailableSeatNumbers, string[] AvailableLight, string[] AvailableGearBoxes)>
            carSettings)
        {
            carSettingsStore.Add(carSettings.Key.Manufacturer + "_" + carSettings.Key.Model + "_" + carSettings.Key.Year, carSettings.Value);
            Save();
        }

        public void RemoveSettings(string manufacturer, string model, int year)
        {
            carSettingsStore.Remove(manufacturer + "_" + model + "_" + year);
            Save();
        }

        public (string[] MinMaxEngineVolume, string[] AvailableSeatNumbers, string[] AvailableLight, string[] AvailableGearBoxes) GetSettings(string manufacturer, string model, int year)
        {
            carSettingsStore.TryGetValue(manufacturer + "_" + model + "_" + year, out var result);

            return result;
        }

        public bool TryUpdateSettings(string manufacturer, string model, int year, (string[], string[], string[], string[]) settings)
        {
            try
            {
                UpdateSettings(manufacturer, model, year, settings);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void UpdateSettings(string manufacturer, string model, int year, (string[] MinMaxEngineVolume, string[] AvailableSeatNumbers, string[] AvailableLight, string[] AvailableGearBoxes) settings)
        {
            var complexKey = manufacturer + "_" + model + "_" + year;
            carSettingsStore[complexKey] = settings;
            Save();
        }

        private void Save()
        {
            var serialized = JsonConvert.SerializeObject(carSettingsStore);
            File.WriteAllText(storagePath, serialized);
        }
    }
}