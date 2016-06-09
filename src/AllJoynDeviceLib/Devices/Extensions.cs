using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceProviders;

namespace AllJoynClientLib.Devices
{
    /// <summary>
    /// Extension methods for simplifying executing operations on devices
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Invokes a method on an interface
        /// </summary>
        /// <param name="i">A reference to the interface to invoke</param>
        /// <param name="method">The name of the method to invoke on the interface</param>
        /// <param name="p">Parameters to parse to the method</param>
        /// <returns>The return values from the operation</returns>
        /// <exception cref="InvalidOperationException">Member was not found on the interface.</exception>
        /// <exception cref="AllJoynServiceException">The operation on the interface could not be completed.</exception>
        public static async Task<IList<object>> InvokeMethodAsync(this IInterface i, string method, params object[] p)
        {
            var m = i.GetMethod(method);
            if (m == null)
            {
                throw new InvalidOperationException($"Method {method} not found on {i.Name}");
            }

            var result = await m.InvokeAsync(new List<object>(p)).AsTask().ConfigureAwait(false);
            if (result.Status.IsFailure)
            {
                throw new AllJoynServiceException(result.Status, i, method + "(...)");
            }

            return result.Values;
        }

        /// <summary>
        /// Gets the property value on an interface
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="i">A reference to the interface holding the property</param>
        /// <param name="property">The name of the property</param>
        /// <returns>The property value</returns>
        /// <exception cref="InvalidOperationException">Member was not found on the interface.</exception>
        /// <exception cref="AllJoynServiceException">The operation on the interface could not be completed.</exception>
        public static async Task<T> GetPropertyAsync<T>(this IInterface i, string property)
        {
            var m = i.GetProperty(property);
            if (m == null)
            {
                throw new InvalidOperationException($"Property {property} not found on {i.Name}");
            }

            var result = await m.ReadValueAsync().AsTask().ConfigureAwait(false);
            if (result.Status.IsFailure)
            {
                throw new AllJoynServiceException(result.Status, i, "get " + property);
            }

            return (T)result.Value;
        }

        /// <summary>
        /// Gets the property value on an interface
        /// </summary>
        /// <param name="i">A reference to the interface holding the property</param>
        /// <param name="property">The name of the property</param>
        /// <returns>The property value</returns>
        /// <exception cref="InvalidOperationException">Member was not found on the interface.</exception>
        /// <exception cref="AllJoynServiceException">The operation on the interface could not be completed.</exception>
        public static Task<object> GetPropertyAsync(this IInterface i, string property)
        {
            return GetPropertyAsync<object>(i, property);
        }

        /// <summary>
        /// Sets a value on the property of an interface
        /// </summary>
        /// <param name="i">A reference to the interface holding the property</param>
        /// <param name="property">The name of the property</param>
        /// <param name="newValue">THe value to set on the device</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Member was not found on the interface.</exception>
        /// <exception cref="AllJoynServiceException">The operation on the interface could not be completed.</exception>
        public static async Task SetPropertyAsync(this IInterface i, string property, object newValue)
        {
            var m = i.GetProperty(property);
            if (m == null)
            {
                throw new InvalidOperationException($"Property {property} not found on {i.Name}");
            }

            var result = await m.SetValueAsync(newValue).AsTask().ConfigureAwait(false);
            if (result.IsFailure)
            {
                throw new AllJoynServiceException(result, i, "set " + property);
            }
        }
    }
}
