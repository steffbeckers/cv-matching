using RJM.API.Framework;
using RJM.API.Models;

namespace RJM.API.DAL.Repositories
{
    public class DocumentContentRepository : Repository<DocumentContent>
    {
        private new readonly RJMContext context;

        public DocumentContentRepository(RJMContext context) : base(context)
        {
            this.context = context;
        }

        // Additional functionality and overrides
    }
}
