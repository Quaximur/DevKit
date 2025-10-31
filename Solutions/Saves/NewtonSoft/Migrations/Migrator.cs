#if NEWTONSOFT_JSON
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inui.Core.Providers.SaveState.Migrations
{
    /// <summary>
    /// Класс для миграции сохраненных данных. 
    /// Необходим для преобразования данных, сохранённых на устройстве, 
    /// в актуальную версию в случае обновления приложения.
    /// </summary>
    public class Migrator
    {
        private readonly IOrderedEnumerable<IMigration> _migrations;

        public Migrator()
        {
            _migrations = new IMigration[]
            {
                // new MigrationV1ToV2(),
            }.OrderBy(x => x.ToVersion);
        }

        public bool TryMigrateIfNecessary<T>(string loadedData, out T upToDateData) where T : SaveStateBase
        {
            var dataObject = JObject.Parse(loadedData);
            int version = (int)dataObject[nameof(IVersioned.Version)];

            if (_migrations.Count() == 0 || version.CompareTo(_migrations.Last().ToVersion) >= 0)
            {
                upToDateData = JsonConvert.DeserializeObject<T>(loadedData);
                return false;
            }

            foreach (var migration in _migrations)
            {
                if (version.CompareTo(migration.ToVersion) < 0)
                {
                    dataObject = migration.Migrate(dataObject);
                    dataObject[nameof(IVersioned.Version)] = migration.ToVersion.ToString();
                    version = migration.ToVersion;
                }
            }

            upToDateData = dataObject.ToObject<T>();
            return true;
        }
    }
}
#endif