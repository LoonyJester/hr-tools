using System.Threading.Tasks;
using NLoad;

namespace HRTools.Presentation.Tests
{
    public abstract class Test: ITest
    {
        public virtual void Initialize()
        {
        }

        public TestResult Execute()
        {
            return ExecuteAsync().Result;
        }

        protected abstract Task<TestResult> ExecuteAsync();
    }
}
