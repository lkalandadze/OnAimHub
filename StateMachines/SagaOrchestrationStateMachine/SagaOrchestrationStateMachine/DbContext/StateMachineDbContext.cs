using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachine.StateMaps;

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
