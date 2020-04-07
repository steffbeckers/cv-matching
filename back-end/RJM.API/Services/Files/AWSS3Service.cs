using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RJM.API.Services.Files
{
    public class AWSS3Service
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AWSS3Service> logger;
        private readonly AmazonS3Client awsS3Client;

        public AWSS3Service(IConfiguration configuration, ILogger<AWSS3Service> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.awsS3Client = new AmazonS3Client(
                this.configuration.GetSection("FileService")
                    .GetSection("AWSS3Service")
                    .GetValue<string>("AccessKey"),
                this.configuration.GetSection("FileService")
                    .GetSection("AWSS3Service")
                    .GetValue<string>("SecretAccessKey"),
                RegionEndpoint.EUCentral1
            );
        }

        public async Task UploadFile(Stream fileStream, string key, string mimeType)
        {
            TransferUtility fileTransferUtility = new TransferUtility(this.awsS3Client);

            TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest() {
                InputStream = fileStream,
                BucketName = this.configuration.GetSection("FileService")
                                .GetSection("AWSS3Service")
                                .GetSection("Bucket")
                                .GetValue<string>("Name"),
                Key = key,
                ContentType = mimeType,
                AutoCloseStream = true
            };

            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }
}
