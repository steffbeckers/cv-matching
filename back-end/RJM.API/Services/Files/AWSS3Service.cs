using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace RJM.API.Services.Files
{
    public class AWSS3Service
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AWSS3Service> logger;
        private readonly IAmazonS3 amazonS3;

        public AWSS3Service(IConfiguration configuration, ILogger<AWSS3Service> logger, IAmazonS3 amazonS3)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.amazonS3 = amazonS3;
        }

        public async Task UploadFile(Stream fileStream, string key, string mimeType)
        {
            TransferUtility fileTransferUtility = new TransferUtility(this.amazonS3);

            TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = fileStream,
                BucketName = this.configuration.GetSection("AWS")
                                .GetSection("S3")
                                .GetSection("Bucket")
                                .GetValue<string>("Name"),
                Key = key,
                ContentType = mimeType,
                AutoCloseStream = true
            };

            this.logger.LogInformation("Starting file upload to AWS S3 storage bucket: " + key + " (" + mimeType + ")");

            await fileTransferUtility.UploadAsync(uploadRequest);

            this.logger.LogInformation("File uploaded to AWS S3 storage bucket: " + key + " (" + mimeType + ")");
        }

        public async Task<GetObjectResponse> DownloadFile(string key)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = this.configuration.GetSection("AWS")
                                .GetSection("S3")
                                .GetSection("Bucket")
                                .GetValue<string>("Name"),
                Key = key
            };

            this.logger.LogInformation("Starting download from AWS S3 storage bucket: " + key);

            GetObjectResponse response = await this.amazonS3.GetObjectAsync(request);

            this.logger.LogInformation("Downloaded file from AWS S3 storage bucket: " + response.Key + " (" + response.Headers["Content-Type"] + ")");

            return response;
        }
    }
}
