using Castle.DynamicProxy;

namespace JHExercise.API.Initialization.Interceptors;

public class AccountServiceClientCachingInterceptor: IInterceptor
{
    object _data;
    DateTime _timestamp = DateTime.UtcNow;

    public void Intercept(IInvocation invocation)
    {
        if (_data == null ||  (DateTime.UtcNow - _timestamp).TotalDays > 7)
        {
            invocation.Proceed();
            _data = invocation.ReturnValue;
            _timestamp = DateTime.UtcNow;
        }
        else
        {
            invocation.ReturnValue = _data;
        }
    }
}