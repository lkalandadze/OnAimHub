using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrationStateMachine.DbContext;

public class StateMachineDbContext
{
    public StateMachineDbContext()
    {
    }



    //protected override IEnumerable<ISagaClassMap> Configurations
    //{
    //    get { yield return new StateMachineMap(); }
    //}
}
