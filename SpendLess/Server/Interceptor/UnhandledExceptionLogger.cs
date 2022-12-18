using Castle.DynamicProxy;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SpendLess.Server.Interceptor
{
    public class UnhandledExceptionLogger : IInterceptor
    {
        //public void Intercept(IInvocation invocation)
        //{
        //    try
        //    {
        //        invocation.Proceed();
        //        //Log.Logger.Information($"Method {invocation.Method.Name} " +
        //        //    $"called with these parameters: {JsonConvert.SerializeObject(invocation.Arguments)}" +
        //        //    $"returned this response: {JsonConvert.SerializeObject(invocation.ReturnValue)}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.Error($"Error happened in method: {invocation.Method}. Error: {JsonConvert.SerializeObject(ex)}");
        //        throw;
        //    }
        //}
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                var method = invocation.MethodInvocationTarget;
                var isAsync = method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
                if (isAsync && typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    invocation.ReturnValue = InterceptAsync((dynamic)invocation.ReturnValue);
                }
            }

            catch (Exception ex)
            {
                Log.Logger.Error($"Error happened in method: {invocation.Method}. Error: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        public async Task InterceptAsync(Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error: {JsonConvert.SerializeObject(ex)}");
                throw;
            }

        }

        public async Task<T> InterceptAsync<T>(Task<T> task)
        {
            try
            {
                T result = await task.ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

    }
}


//using Castle.DynamicProxy;
//using Newtonsoft.Json;

//namespace SpendLess.Server.Interceptor
//{
//    public class UnhandledExceptionLogger : IAsyncInterceptor
//    {

//        public void InterceptAsynchronous(IInvocation invocation)
//        {
//            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
//        }

//        private async Task InternalInterceptAsynchronous(IInvocation invocation)
//        {
//            // Step 1. Do something prior to invocation.
//            try
//            {
//                invocation.Proceed();
//                var task = (Task)invocation.ReturnValue;
//                await task;
//            }
//            catch (Exception ex)
//            {
//                Log.Error($"\nException message: {ex.Message}\nMethod: {invocation.Method}.\nException stack trace: {ex.StackTrace}");
//                throw;
//            }

//            // Step 2. Do something after invocation.
//        }

//        public void InterceptAsynchronous<TResult>(IInvocation invocation)
//        {
//            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
//        }

//        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
//        {
//            // Step 1. Do something prior to invocation.
//            try
//            {
//                invocation.Proceed();
//                var task = (Task<TResult>)invocation.ReturnValue;
//                TResult result = await task;
//                return result;
//            }
//            catch (Exception ex)
//            {
//                Log.Error($"\nException message: {ex.Message}\nMethod: {invocation.Method}.\nException stack trace: {ex.StackTrace}");
//                throw;
//            }

//            // Step 2. Do something after invocation.

//        }

//        public void InterceptSynchronous(IInvocation invocation)
//        {
//            try
//            {
//                invocation.Proceed();
//            }
//            catch (Exception ex)
//            {
//                Log.Error($"\nException message: {ex.Message}\nMethod: {invocation.Method}.\nException stack trace: {ex.StackTrace}");
//                throw;
//            }
//        }


//    }
//}

