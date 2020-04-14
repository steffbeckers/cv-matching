using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RJM.API.DAL.Repositories;
using RJM.API.Framework.Exceptions;
using RJM.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RJM.API.Services.Files
{
    public class FileService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<FileService> logger;
        private readonly AWSS3Service awsS3Service;

        public FileService(
            IConfiguration configuration,
            ILogger<FileService> logger,
            AWSS3Service awsS3Service
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.awsS3Service = awsS3Service;
        }

        public async Task<Document> UploadDocument(Document document, Stream stream)
        {
            // Validation
            if (document == null || stream == null)
            {
                // TODO: Check for better exception/error handling implementation?
                throw new FileServiceException("Document info and stream should be present");
            }

            string uploadLocation = this.configuration.GetSection("FileService")
                                        .GetValue<string>("Location");
            switch (uploadLocation)
            {
                case "AWSS3":
                    string pathInBucket = this.configuration.GetSection("FileService")
                                              .GetSection("AWSS3Service")
                                              .GetSection("Bucket")
                                              .GetValue<string>("DocumentsPath");
                    pathInBucket += "/" + document.Id.ToString().ToUpper();
                    document.Path = pathInBucket;

                    await this.awsS3Service.UploadFile(stream, document.Path, document.MimeType);
                    break;
                case "AzureStorage":
                    // TODO
                    break;
            }

            return document;
        }

        public async Task<Stream> DownloadDocument(Document document)
        {
            // Validation
            if (document == null)
            {
                // TODO: Check for better exception/error handling implementation?
                throw new FileServiceException("Document info should be present");
            }

            string downloadLocation = this.configuration.GetSection("FileService")
                                        .GetValue<string>("Location");
            switch (downloadLocation)
            {
                case "AWSS3":
                    string pathInBucket = this.configuration.GetSection("FileService")
                                              .GetSection("AWSS3Service")
                                              .GetSection("Bucket")
                                              .GetValue<string>("DocumentsPath");
                    pathInBucket += "/" + document.Id.ToString().ToUpper();

                    GetObjectResponse response = await this.awsS3Service.DownloadFile(pathInBucket);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(stream);
                        return stream;
                    }
                case "AzureStorage":
                    // TODO
                    break;
            }

            return null;
        }
    }
}
