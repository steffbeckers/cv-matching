using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace RJM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<TestController> logger;
        //private readonly FileService fileService;

        public TestController(
            IConfiguration configuration,
            ILogger<TestController> logger//,
                                          //FileService fileService
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            //this.fileService = fileService;
        }

        [HttpGet("puppeteer")]
        public async Task<IActionResult> TestPuppeteerPDFGeneration()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            Page page = await browser.NewPageAsync();

            //await page.GoToAsync("http://localhost:5000/templates/resumes/github-mnjul-html-resume/index.html");
            await page.GoToAsync("http://localhost:5000/templates/resumes/srt/index.html");
            //await page.GoToAsync("http://localhost:8080/resumes/cede8784-3f52-44fe-c955-08d7ed22a16b/print");

            await page.PdfAsync("puppeteer-test.pdf");

            return Ok();
        }

        [HttpGet("puppeteer-2")]
        public async Task<IActionResult> TestPuppeteer2PDFGeneration()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            Page page = await browser.NewPageAsync();

            await page.SetContentAsync("<div>My Receipt</div>");

            await page.PdfAsync("puppeteer-2-test.pdf");

            return Ok();
        }

        //[HttpGet("aws-s3-bucket-read")]
        //public async Task<ActionResult> TestAWSS3BucketRead()
        //{
        //    string res = string.Empty;

        //    // Upload to storage
        //    switch (this.configuration.GetSection("Uploads").GetValue<string>("Location"))
        //    {
        //        case "AWS-S3":
        //            try
        //            {
        //                ListObjectsRequest request = new ListObjectsRequest
        //                {
        //                    BucketName = this.configuration.GetSection("Uploads").GetSection("AWS-S3").GetSection("Bucket").GetValue<string>("Name"),
        //                    MaxKeys = 10
        //                };

        //                do
        //                {
        //                    ListObjectsResponse response = await this.awsS3Client.ListObjectsAsync(request);

        //                    // Process the response.
        //                    foreach (S3Object entry in response.S3Objects)
        //                    {
        //                        res += entry.Key + "\t" + entry.Size + Environment.NewLine;
        //                    }

        //                    // If the response is truncated, set the marker to get the next 
        //                    // set of keys.
        //                    if (response.IsTruncated)
        //                    {
        //                        request.Marker = response.NextMarker;
        //                    }
        //                    else
        //                    {
        //                        request = null;
        //                    }
        //                } while (request != null);
        //            }
        //            catch (AmazonS3Exception e)
        //            {
        //                Console.WriteLine("Error encountered on server. Message: '{0}'.", e.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Unknown encountered on server. Message: '{0}'.", e.Message);
        //            }
        //            break;
        //    }

        //    return Ok(res);
        //}

        //[HttpGet("aws-s3-bucket-upload")]
        //public async Task<ActionResult> TestAWSS3BucketUpload()
        //{
        //    // Upload to storage
        //    switch (this.configuration.GetSection("Uploads").GetValue<string>("Location"))
        //    {
        //        case "AWS-S3":
        //            try
        //            {
        //                var fileTransferUtility = new TransferUtility(this.awsS3Client);

        //                await fileTransferUtility.UploadAsync("aws-s3-bucket-upload.txt", this.configuration.GetSection("Uploads").GetSection("AWS-S3").GetSection("Bucket").GetValue<string>("Name"), "aws-s3-bucket-upload.txt");
        //            }
        //            catch (AmazonS3Exception e)
        //            {
        //                Console.WriteLine("Error encountered on server. Message: '{0}'.", e.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Unknown encountered on server. Message: '{0}'.", e.Message);
        //            }
        //            break;
        //    }

        //    return Ok();
        //}

        //[HttpGet("aws-s3-bucket-download")]
        //public async Task<ActionResult> TestAWSS3BucketDownload()
        //{
        //    string res = string.Empty;

        //    // Upload to storage
        //    switch (this.configuration.GetSection("Uploads").GetValue<string>("Location"))
        //    {
        //        case "AWS-S3":
        //            try
        //            {
        //                GetObjectRequest request = new GetObjectRequest
        //                {
        //                    BucketName = this.configuration.GetSection("Uploads").GetSection("AWS-S3").GetSection("Bucket").GetValue<string>("Name"),
        //                    Key = "aws-s3-bucket-upload.txt"
        //                };

        //                using (GetObjectResponse response = await this.awsS3Client.GetObjectAsync(request))
        //                using (Stream responseStream = response.ResponseStream)
        //                using (StreamReader reader = new StreamReader(responseStream))
        //                {
        //                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
        //                    string contentType = response.Headers["Content-Type"];
        //                    Console.WriteLine("Object metadata, Title: {0}", title);
        //                    Console.WriteLine("Content type: {0}", contentType);

        //                    res = reader.ReadToEnd(); // Now you process the response body.
        //                }
        //            }
        //            catch (AmazonS3Exception e)
        //            {
        //                Console.WriteLine("Error encountered on server. Message: '{0}'.", e.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Unknown encountered on server. Message: '{0}'.", e.Message);
        //            }
        //            break;
        //    }

        //    return Ok(res);
        //}
    }
}