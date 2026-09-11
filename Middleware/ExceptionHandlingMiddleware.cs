using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //_next＝次の処理（今回でいうController）　処理結果をawaitし、正常終了すればそのまま、エラーあればcatchして例外処理＆httpレスポンスとして500を返す
            await _next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "サーバーエラーが発生しました。",
                errorCode = "INTERNAL_SERVER_ERROR"
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}