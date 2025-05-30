// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Gefco.CipQuai.ApiClient
{
    using Models;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for DeclarationBonnePratiqueOperations.
    /// </summary>
    public static partial class DeclarationBonnePratiqueOperationsExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='userId'>
            /// </param>
            public static ApplicationUserListServiceResult GetActeurs(this IDeclarationBonnePratiqueOperations operations, string appVersion, string userId)
            {
                return operations.GetActeursAsync(appVersion, userId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='userId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ApplicationUserListServiceResult> GetActeursAsync(this IDeclarationBonnePratiqueOperations operations, string appVersion, string userId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetActeursWithHttpMessagesAsync(appVersion, userId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='userId'>
            /// </param>
            /// <param name='declaration'>
            /// </param>
            public static BooleanServiceResult AddDeclarationBonnePratique(this IDeclarationBonnePratiqueOperations operations, string appVersion, string userId, DeclarationBonnePratique declaration)
            {
                return operations.AddDeclarationBonnePratiqueAsync(appVersion, userId, declaration).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='userId'>
            /// </param>
            /// <param name='declaration'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<BooleanServiceResult> AddDeclarationBonnePratiqueAsync(this IDeclarationBonnePratiqueOperations operations, string appVersion, string userId, DeclarationBonnePratique declaration, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddDeclarationBonnePratiqueWithHttpMessagesAsync(appVersion, userId, declaration, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='pictureType'>
            /// </param>
            /// <param name='declarationId'>
            /// </param>
            /// <param name='fileName'>
            /// </param>
            /// <param name='file'>
            /// Upload picture
            /// </param>
            public static PictureServiceResult UploadPictureBP(this IDeclarationBonnePratiqueOperations operations, string appVersion, string id, int pictureType, string declarationId, string fileName, Stream file)
            {
                return operations.UploadPictureBPAsync(appVersion, id, pictureType, declarationId, fileName, file).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='pictureType'>
            /// </param>
            /// <param name='declarationId'>
            /// </param>
            /// <param name='fileName'>
            /// </param>
            /// <param name='file'>
            /// Upload picture
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<PictureServiceResult> UploadPictureBPAsync(this IDeclarationBonnePratiqueOperations operations, string appVersion, string id, int pictureType, string declarationId, string fileName, Stream file, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UploadPictureBPWithHttpMessagesAsync(appVersion, id, pictureType, declarationId, fileName, file, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='pictureType'>
            /// </param>
            /// <param name='declarationId'>
            /// </param>
            /// <param name='fileName'>
            /// </param>
            /// <param name='fileContent'>
            /// </param>
            public static PictureServiceResult UploadPicBP(this IDeclarationBonnePratiqueOperations operations, string appVersion, string id, int pictureType, string declarationId, string fileName, string fileContent)
            {
                return operations.UploadPicBPAsync(appVersion, id, pictureType, declarationId, fileName, fileContent).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='appVersion'>
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='pictureType'>
            /// </param>
            /// <param name='declarationId'>
            /// </param>
            /// <param name='fileName'>
            /// </param>
            /// <param name='fileContent'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<PictureServiceResult> UploadPicBPAsync(this IDeclarationBonnePratiqueOperations operations, string appVersion, string id, int pictureType, string declarationId, string fileName, string fileContent, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UploadPicBPWithHttpMessagesAsync(appVersion, id, pictureType, declarationId, fileName, fileContent, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
