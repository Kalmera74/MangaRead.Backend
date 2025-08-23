using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.Extensions.Options;

public class HetznerBucketService : IBucketService
{
    private readonly IAmazonS3 _client;
    private readonly BucketOptions _bucketInfo;

    public HetznerBucketService(IOptions<BucketOptions> bucketOpitons)
    {
        _bucketInfo = bucketOpitons.Value;

        var config = new AmazonS3Config
        {
            ServiceURL = _bucketInfo.ServiceURL,
            ForcePathStyle = true
        };

        _client = new AmazonS3Client(_bucketInfo.AccessKey, _bucketInfo.SecretKey, config);
    }

    public async Task<string> UploadFileToBucket(string filePath, string? key = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }


        key ??= Path.GetFileName(filePath);
        key = Uri.EscapeDataString(key);

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketInfo.BucketName,
            Key = key,
            FilePath = filePath,
            ContentType = "application/octet-stream"
        };

        try
        {
            var response = await _client.PutObjectAsync(putRequest);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                string service = new Uri(_client.Config.ServiceURL).Host;
                return $"https://{_bucketInfo.BucketName}.{service}/{key}";
            }
            else
            {
                throw new Exception($"Failed to upload: {response.HttpStatusCode}");
            }
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception("S3 error occurred. " + ex.Message, ex);
        }
    }
}

