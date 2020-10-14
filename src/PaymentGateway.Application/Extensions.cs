using System;

namespace PaymentGateway.Application
{
    public static class Extensions
    {
        public static T SafeInvoke<T>(this Func<T> func, Action<Exception> errorHandler)
        {
            if (func == null)
            {
                throw new ArgumentException(nameof(func));
            }

            if (errorHandler == null)
            {
                throw new ArgumentNullException(nameof(errorHandler));
            }

            try
            {
                return func();
            }
            catch (Exception e)
            {
                errorHandler(e);
                throw;
            }
        }

        /// <summary>
        /// Invokes the given delegate and calls the errorHandler if an exception is thrown
        /// </summary>
        /// <param name="func">The delegate to execute</param>
        /// <param name="errorHandler">The action to take on error</param>
        /// <param name="arg">The argument to pass to the delegate</param>
        /// <typeparam name="TIn">Type of the delegate argument</typeparam>
        /// <typeparam name="T">The return type of the delegate</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T SafeInvoke<TIn, T>(this Func<TIn, T> func, Action<Exception> errorHandler, TIn arg)
        {
            if (func == null)
            {
                throw new ArgumentException(nameof(func));
            }

            if (errorHandler == null)
            {
                throw new ArgumentNullException(nameof(errorHandler));
            }

            try
            {
                return func(arg);
            }
            catch (Exception e)
            {
                errorHandler(e);
                throw;
            }
        }
    }
}