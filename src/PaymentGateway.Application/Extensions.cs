using System;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application
{
    public static class Extensions
    {
        /// <summary>
        /// Masks all but the last 4 digits of a card nubmer
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string MaskCardNumber(this Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if (string.IsNullOrEmpty(card.Number))
            {
                return string.Empty;
            }

            var length = card.Number.Length - 4;
            return new string('*', length) + card.Number.Substring(length);
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
                throw new ArgumentNullException(nameof(func));
            }

            if (errorHandler == null)
            {
                throw new ArgumentNullException(nameof(errorHandler));
            }

            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
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