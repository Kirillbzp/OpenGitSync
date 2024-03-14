using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OpenGitSync.Client.Services
{
    public static class HttpClientExtensions
    {
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Type parameters:
        //   TValue:
        //     The target type to deserialize to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<TValue?> GetFromJsonAsyncSafe<TValue>(this HttpClient client, Uri? requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync<TValue>(requestUri, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   jsonTypeInfo:
        //     Source generated JsonTypeInfo to control the behavior during deserialization.
        //
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Type parameters:
        //   TValue:
        //     The target type to deserialize to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        public static async Task<TValue?> GetFromJsonAsyncSafe<TValue>(this HttpClient client, Uri? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync<TValue>(requestUri, jsonTypeInfo, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   options:
        //     Options to control the behavior during deserialization. The default options are
        //     those specified by System.Text.Json.JsonSerializerDefaults.Web.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Type parameters:
        //   TValue:
        //     The target type to deserialize to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<TValue?> GetFromJsonAsyncSafe<TValue>(this HttpClient client, Uri? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   options:
        //     Options to control the behavior during deserialization. The default options are
        //     those specified by System.Text.Json.JsonSerializerDefaults.Web.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Type parameters:
        //   TValue:
        //     The target type to deserialize to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<TValue?> GetFromJsonAsyncSafe<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Type parameters:
        //   TValue:
        //     The target type to deserialize to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<TValue?> GetFromJsonAsyncSafe<TValue>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync<TValue>(requestUri, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   type:
        //     The type of the object to deserialize to and return.
        //
        //   context:
        //     Source generated JsonSerializerContext used to control the deserialization behavior.
        //
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        public static async Task<object?> GetFromJsonAsyncSafe(this HttpClient client, Uri? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync(requestUri, type, context, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   type:
        //     The type of the object to deserialize to and return.
        //
        //   options:
        //     Options to control the behavior during deserialization. The default options are
        //     those specified by System.Text.Json.JsonSerializerDefaults.Web.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<object?> GetFromJsonAsyncSafe(this HttpClient client, Uri? requestUri, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync(requestUri, type, options, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   type:
        //     The type of the object to deserialize to and return.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<object?> GetFromJsonAsyncSafe(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync(requestUri, type, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   type:
        //     The type of the object to deserialize to and return.
        //
        //   context:
        //     Source generated JsonSerializerContext used to control the deserialization behavior.
        //
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        public static async Task<object?> GetFromJsonAsyncSafe(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync(requestUri, type, context, cancellationToken);
            }
            catch
            {
                return default;
            }
        }
        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   type:
        //     The type of the object to deserialize to and return.
        //
        //   options:
        //     Options to control the behavior during deserialization. The default options are
        //     those specified by System.Text.Json.JsonSerializerDefaults.Web.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<object?> GetFromJsonAsyncSafe(this HttpClient client, [StringSyntax("Uri")] string? requestUri, Type type, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync(requestUri, type, options, cancellationToken);
            }
            catch
            {
                return default;
            }
        }

        //
        // Summary:
        //     Sends a GET request to the specified Uri and returns the value that results from
        //     deserializing the response body as JSON in an asynchronous operation.
        //
        // Parameters:
        //   client:
        //     The client used to send the request.
        //
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   type:
        //     The type of the object to deserialize to and return.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext.")]
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        public static async Task<object?> GetFromJsonAsyncSafe(this HttpClient client, Uri? requestUri, Type type, CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.GetFromJsonAsync(requestUri, type, cancellationToken);
            }
            catch
            {
                return default;
            }
        }

    }
}
