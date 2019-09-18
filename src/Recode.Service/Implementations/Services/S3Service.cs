using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;
using Recode.Core.ConfigModels;
using Recode.Core.Models;
using Recode.Service.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VigiPay.Org.Service.EntityService
{

    public interface IS3Service
    {
        Task<ExecutionResponse<object>> CreateBucketAsync(string buckectName);
        Task<ExecutionResponse<string>> UploadFile(Stream stream, string fileName, string contentType);
    }

    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;
        private readonly AWSSettings _awsOptions;

        public S3Service(IAmazonS3 client, IOptions<AWSSettings> awsOptions)
        {
            _client = client;
            _awsOptions = awsOptions.Value;

            var result = CreateBucketAsync(_awsOptions.BucketName).Result.ResponseCode;
        }

        public async Task<ExecutionResponse<object>> CreateBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new ExecutionResponse<object>
                    {
                        ResponseCode = ResponseCode.Ok,
                        ResponseData = response.ResponseMetadata.RequestId
                    };
                }

                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.Ok,
                    ResponseData = "Bucket already exist"
                };
            }
            catch (AmazonS3Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<object>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = $"An error occured - {ex.Message}"
                };
            }

            return new ExecutionResponse<object>
            {
                ResponseCode = ResponseCode.ServerException,
                Message = "Something went wrong"
            };
        }

        public async Task<ExecutionResponse<string>> UploadFile(Stream stream, string fileName, string contentType)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

                var fileTransferRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _awsOptions.BucketName,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead,
                    InputStream = stream,
                    Key = $"{_awsOptions.ApplicationFolder}/{fileName}",
                    StorageClass = S3StorageClass.ReducedRedundancy
                };

                await fileTransferUtility.UploadAsync(fileTransferRequest);

                return new ExecutionResponse<string>
                {
                    ResponseCode = ResponseCode.Ok,
                    ResponseData = $"{_awsOptions.ServiceUrl}/{_awsOptions.BucketName}/{fileTransferRequest.Key}"
                };
            }
            catch (AmazonS3Exception ex)
            {
                Log.Error(ex);

                return new ExecutionResponse<string>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new ExecutionResponse<string>
                {
                    ResponseCode = ResponseCode.ServerException,
                    Message = ex.Message
                };
            }
        }
    }
}
