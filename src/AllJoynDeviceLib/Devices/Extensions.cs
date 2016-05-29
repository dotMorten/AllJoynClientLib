using DeviceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllJoynClientLib.Devices
{
    internal static class Extensions
    {
        public static async Task<IList<object>> InvokeMethodAsync(this IInterface i, string method, params object[] p)
        {
            var m = i.GetMethod(method);
            if (m == null)
                throw new InvalidOperationException($"Method {method} not found on {i.Name}");
            var result = await m.InvokeAsync(new List<object>(p)).AsTask().ConfigureAwait(false);
            if (result.Status.IsFailure)
                throw new AllJoynServiceException(result.Status, i, method + "(...)");
            return result.Values;
        }

        public static async Task<T> GetPropertyAsync<T>(this IInterface i, string property)
        {
            var m = i.GetProperty(property);
            if (m == null)
                throw new InvalidOperationException($"Property {property} not found on {i.Name}");
            var result = await m.ReadValueAsync().AsTask().ConfigureAwait(false);
            if (result.Status.IsFailure)
                throw new AllJoynServiceException(result.Status, i, "get " + property);
            return (T)result.Value;
        }

        public static Task<object> GetPropertyAsync(this IInterface i, string property)
        {
            return GetPropertyAsync<object>(i, property);
        }

        public static async Task SetPropertyAsync(this IInterface i, string property, object newValue)
        {
            var m = i.GetProperty(property);
            if (m == null)
                throw new InvalidOperationException($"Property {property} not found on {i.Name}");
            var result = await m.SetValueAsync(newValue).AsTask().ConfigureAwait(false);
            if (result.IsFailure)
                throw new AllJoynServiceException(result, i, "set " + property);
        }
    }
}
