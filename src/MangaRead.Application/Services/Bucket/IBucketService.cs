public interface IBucketService
{

    public Task<string> UploadFileToBucket(string filePath, string key);

}
