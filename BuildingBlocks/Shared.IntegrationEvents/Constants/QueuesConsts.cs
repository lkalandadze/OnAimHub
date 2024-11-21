namespace Shared.IntegrationEvents.Constants;

public class QueuesConsts
{
    // events
    public const string PromotionCreatedEventQueueName = "promotion-created-queue";
    public const string PromotionCompletedEventQueueName = "promotion-completed-queue";
    public const string PromotionFailedEventQueueName = "promotion-failed-queue";

    // messages
    public const string CreatePromotionMessageQueueName = "create-order-message-queue";
    public const string CompletePaymentMessageQueueName = "complete-payment-message-queue";
    public const string StockRollBackMessageQueueName = "stock-rollback-message-queue";
}
