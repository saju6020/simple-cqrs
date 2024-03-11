namespace TestSse
{
    public interface ISseHolder
    {
        Task AddAsync(HttpContext context);
        Task SendMessageAsync(SseMessage message);
    }
}
