#if NEWTONSOFT_JSON
namespace Inui.Core.Providers.SaveState.Migrations
{
    public interface IMigrator
    {
        public bool TryMigrateIfNecessary<T>(string loadedData, out T upToDateData) where T : SaveStateBase;
    }
}
#endif