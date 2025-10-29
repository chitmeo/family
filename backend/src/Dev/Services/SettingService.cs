using Dev.Common.Caching;
using Dev.Domain.Interfaces;

namespace Dev.Services;

internal class SettingService : ISettingService
{
    #region Fields

    private readonly ICoreDbContext _context;
    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor
    public SettingService(ICoreDbContext context,
                          IStaticCacheManager staticCacheManager)
    {
        _context = context;
        _staticCacheManager = staticCacheManager;
    }
    #endregion
}
