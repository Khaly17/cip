// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.IO;

namespace Gefco.CipQuai.ApiClient.Models
{
    /// <summary> The PathsSk51N0UploadpicturencPutRequestbodyContentApplicationXWwwFormUrlencodedSchema. </summary>
    internal partial class PathsSk51N0UploadpicturencPutRequestbodyContentApplicationXWwwFormUrlencodedSchema
    {
        /// <summary> Initializes a new instance of PathsSk51N0UploadpicturencPutRequestbodyContentApplicationXWwwFormUrlencodedSchema. </summary>
        /// <param name="file"> Upload picture. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> is null. </exception>
        internal PathsSk51N0UploadpicturencPutRequestbodyContentApplicationXWwwFormUrlencodedSchema(Stream file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            File = file;
        }

        /// <summary> Upload picture. </summary>
        public Stream File { get; }
    }
}
