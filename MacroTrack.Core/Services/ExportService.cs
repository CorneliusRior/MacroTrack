using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Repositories;

namespace MacroTrack.Core.Services
{
    public class ExportService : ServiceBase
    {
        private readonly ExportRepo _repo;

        public ExportService(ExportRepo repo, CoreContext ctx) : base(ctx)
        {
            _repo = repo;
        }

        public void ExportToExcel(string outputPath)
        {
            _repo.ExportDataBaseToExcel(outputPath);
        }
    }
}
