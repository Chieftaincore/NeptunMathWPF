using System.Data.Entity.Core.EntityClient;

namespace NeptunMathWPF
{
    public partial class NeptunDB
{
    public NeptunDB(string entityConnectionString)
        : base(entityConnectionString)
    {
    }
}
}
