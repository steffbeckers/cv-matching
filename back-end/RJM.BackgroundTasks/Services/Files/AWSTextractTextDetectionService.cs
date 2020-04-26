using Amazon.Textract;
using Amazon.Textract.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RJM.BackgroundTasks.Services.Files
{
    public class AWSTextractService
    {
        private IAmazonTextract amazonTextract;

        public AWSTextractService(IAmazonTextract amazonTextract)
        {
            this.amazonTextract = amazonTextract;
        }

        public async Task<string> StartDocumentTextDetection(string bucketName, string key)
        {
            StartDocumentTextDetectionRequest request = new StartDocumentTextDetectionRequest();

            request.DocumentLocation = new DocumentLocation
            {
                S3Object = new S3Object
                {
                    Bucket = bucketName,
                    Name = key
                }
            };

            StartDocumentTextDetectionResponse response = await this.amazonTextract.StartDocumentTextDetectionAsync(request);

            return response.JobId;
        }

        public async Task WaitForJobCompletion(string jobId, int delay = 5000)
        {
            while (!await IsJobComplete(jobId))
            {
                this.Wait(delay);
            }
        }

        public async Task<bool> IsJobComplete(string jobId)
        {
            GetDocumentTextDetectionResponse response = await this.amazonTextract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });

            return !response.JobStatus.Equals("IN_PROGRESS");
        }

        public async Task<List<GetDocumentTextDetectionResponse>> GetJobResult(string jobId)
        {
            List<GetDocumentTextDetectionResponse> result = new List<GetDocumentTextDetectionResponse>();

            // Wait for response of Amazon Textract
            GetDocumentTextDetectionResponse response = await this.amazonTextract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
            {
                JobId = jobId
            });

            // Add response to the result
            result.Add(response);

            // If there is a next token in the result, we need to check the data again
            string nextToken = response.NextToken;
            while (nextToken != null)
            {
                this.Wait();
                response = await this.amazonTextract.GetDocumentTextDetectionAsync(new GetDocumentTextDetectionRequest
                {
                    JobId = jobId,
                    NextToken = response.NextToken
                });

                // Add next result to the response
                result.Add(response);

                // If there is a next token in the result, we need to check the data again
                nextToken = response.NextToken;
            }

            return result;
        }

        private void Wait(int delay = 5000)
        {
            Task.Delay(delay).Wait();
            Console.Write(".");
        }
    }
}
