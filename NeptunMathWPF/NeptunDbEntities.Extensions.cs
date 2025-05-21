using System.Data.Entity.Core.EntityClient;

namespace NeptunMathWPF
{
    public partial class Neptun_DBEntities
    {
        public Neptun_DBEntities(string entityConnectionString)
            : base(entityConnectionString)
        {
        }
    }
}