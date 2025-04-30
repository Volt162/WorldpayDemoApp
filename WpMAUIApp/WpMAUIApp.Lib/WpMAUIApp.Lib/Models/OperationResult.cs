namespace WpMAUIApp.Lib.Models;

public static class OperationResult
{
    public static OperationResult<TResult> Create<TResult>(bool operationSucceeded, TResult result)
    {
        return new OperationResult<TResult>(operationSucceeded, result);
    }

    public static OperationResult<TResult> Unsucceed<TResult>()
    {
        return new OperationResult<TResult>(false);
    }
}

public class Operation
{
    public bool OperationSucceeded { get; }

    public Operation() : this(true)
    {

    }

    public Operation(bool operationSucceeded)
    {
        OperationSucceeded = operationSucceeded;
    }
}

public class OperationResult<TResult>
{
    public bool OperationSucceeded { get; set; }
    public TResult? Result { get; set; }
    public string Message { get; set; }

    public OperationResult()
    {

    }

    public OperationResult(bool operationSucceeded, TResult? result = default)
    {
        OperationSucceeded = operationSucceeded;
        Result = result;
    }

    public void SetSuccess(TResult result)
    {
        SetResult(true, result, null);
    }

    public void SetFailure(string message = null)
    {
        SetResult(false, default(TResult), message);
    }

    protected void SetResult(bool operationSucceeded, TResult result, string message)
    {
        Result = result;
        Message = message;
        OperationSucceeded = operationSucceeded;
    }
}
