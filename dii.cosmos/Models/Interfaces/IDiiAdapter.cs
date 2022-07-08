﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dii.cosmos.Models.Interfaces
{
    public interface IDiiAdapter<T> where T : IDiiEntity, new()
    {
        #region Fetch APIs
        /// <summary>
        /// Reads an entity from the service as an asynchronous operation.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="partitionKey">The partition key for the entity.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        /// <remarks>
        /// 
        /// </remarks>
        Task<T> GetAsync(string id, string partitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads multiple entities from a container using Id and PartitionKey values.
        /// </summary>
        /// <param name="idAndPks">List of ids and partition keys.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// A collection of entities.
        /// </returns>
        /// <remarks>
        /// This is meant to perform better latency-wise than a query with IN statements to fetch
        /// a large number of independent entities.
        /// </remarks>
        Task<ICollection<T>> GetManyAsync(IReadOnlyList<(string id, string partitionKey)> idAndPks, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method creates a query for entities in the same partition using a SQL-like statement.
        /// </summary>
        /// <param name="queryText">The SQL query text.</param>
        /// <param name="continuationToken">(Optional) The continuation token for subsequent calls.</param>
        /// <returns>
        /// A PagedList of entities.
        /// </returns>
        /// <remarks>
        /// Only supports single partition queries.
        /// </remarks>
        Task<PagedList<T>> GetPagedAsync(string queryText = null, string continuationToken = null);
        #endregion Fetch APIs

        #region Create APIs
        /// <summary>
        /// Creates an entity as an asynchronous operation.
        /// </summary>
        /// <param name="diiEntity">The <see cref="T"/> to create.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entity that was created.
        /// </returns>
        /// <remarks>
        /// 
        /// </remarks>
        Task<T> CreateAsync(T diiEntity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates multiple entities as an asynchronous operation.
        /// </summary>
        /// <param name="diiEntities">The list of <see cref="IReadOnlyList{T}"/> to create.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entities that were created.
        /// </returns>
        /// <remarks>
        /// When <see cref="CosmosClientOptions.AllowBulkExecution"/> is set to <see langword="true"/>, allows optimistic batching of requests
        /// to the service. This option is recommended for non-latency sensitive scenarios only as it trades latency for throughput.
        /// </remarks>
        Task<ICollection<T>> CreateBulkAsync(IReadOnlyList<T> diiEntities, CancellationToken cancellationToken = default);
        #endregion Create APIs

        #region Replace APIs
        /// <summary>
        /// Replaces an entity as an asynchronous operation.
        /// </summary>
        /// <param name="diiEntity">The <see cref="T"/> to replace.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entity that was updated.
        /// </returns>
        /// <remarks>
        /// The entity's partition key value is immutable. To change an entity's partition key
        /// value you must delete the original entity and insert a new entity.
        /// <para>
        /// This operation does not work on entities that use the same value for both the id and parition key.
        /// </para>
        /// </remarks>
        Task<T> ReplaceAsync(T diiEntity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces multiple entities as an asynchronous operation.
        /// </summary>
        /// <param name="diiEntities">The list of <see cref="IReadOnlyList{T}"/> to replace.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entities that were updated.
        /// </returns>
        /// <remarks>
        /// When <see cref="CosmosClientOptions.AllowBulkExecution"/> is set to <see langword="true"/>, allows optimistic batching of requests
        /// to the service. This option is recommended for non-latency sensitive scenarios only as it trades latency for throughput.
        /// <para>
        /// The entity's partition key value is immutable. To change an entity's partition key
        /// value you must delete the original entity and insert a new entity.
        /// </para>
        /// <para>
        /// This operation does not work on entities that use the same value for both the id and parition key.
        /// </para>
        /// </remarks>
        Task<ICollection<T>> ReplaceBulkAsync(IReadOnlyList<T> diiEntities, CancellationToken cancellationToken = default);
        #endregion Replace APIs

        #region Upsert APIs
        /// <summary>
        /// Upserts an entity as an asynchronous operation.
        /// </summary>
        /// <param name="diiEntity">The <see cref="T"/> to upsert.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entity that was upserted.
        /// </returns>
        /// <remarks>
        /// 
        /// </remarks>
        Task<T> UpsertAsync(T diiEntity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upserts multiple entities as an asynchronous operation.
        /// </summary>
        /// <param name="diiEntities">The list of <see cref="IReadOnlyList{T}"/> to upsert.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The entities that were upserted.
        /// </returns>
        /// <remarks>
        /// When <see cref="CosmosClientOptions.AllowBulkExecution"/> is set to <see langword="true"/>, allows optimistic batching of requests
        /// to the service. This option is recommended for non-latency sensitive scenarios only as it trades latency for throughput.
        /// </remarks>
        Task<ICollection<T>> UpsertBulkAsync(IReadOnlyList<T> diiEntities, CancellationToken cancellationToken = default);
        #endregion Upsert APIs

        #region Delete APIs
        /// <summary>
        /// Delete an entity as an asynchronous operation.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="partitionKey">The partition key for the entity.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The success status of the operation.
        /// </returns>
        /// <remarks>
        /// 
        /// </remarks>
        Task<bool> DeleteAsync(string id, string partitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete multiple entities as an asynchronous operation.
        /// </summary>
        /// <param name="idAndPks">List of ids and partition keys.</param>
        /// <param name="cancellationToken">(Optional) <see cref="CancellationToken"/> representing request cancellation.</param>
        /// <returns>
        /// The success status of the operation.
        /// </returns>
        /// <remarks>
        /// When <see cref="CosmosClientOptions.AllowBulkExecution"/> is set to <see langword="true"/>, allows optimistic batching of requests
        /// to the service. This option is recommended for non-latency sensitive scenarios only as it trades latency for throughput.
        /// </remarks>
        Task<bool> DeleteBulkAsync(IReadOnlyList<(string id, string partitionKey)> idAndPks, CancellationToken cancellationToken = default);
        #endregion Delete APIs
    }
}