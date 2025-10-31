#if NEWTONSOFT_JSON
namespace DevKit.Saves.Migrations
{
    public interface IMigrator
    {
        public bool TryMigrateIfNecessary<T>(string loadedData, out T upToDateData) where T : SaveStateBase;
    }
}
#endif