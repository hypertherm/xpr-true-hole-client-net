using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Hypertherm.XprTrueHoleHttpClient.unittests
{
    [TestFixture]
    public class TestTHApiClient
    {
        THApiClient thAPiClient;
        string uploadbadfile;
        string uploadgoodfile;

        Mock<ITHHttpClient> thHttpClientMock;

        [SetUp]
        protected void Setup()
        {
            thHttpClientMock = new Mock<ITHHttpClient>();
            thAPiClient = new THApiClient(thHttpClientMock.Object, new THClientConfiguration("subscriptionKey"));
            uploadbadfile = "bad file content";
            uploadgoodfile = "good file content";
        }

        [Test]
        public void UploadFailureNotJson()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("");

            thHttpClientMock.Setup(thcm => thcm.PostText("upload", uploadbadfile, It.IsAny<HttpClient>())).Returns(response);

            string message = null;

            try
            {
                thAPiClient.Upload(uploadbadfile);
            }
            catch (THApiClientException e)
            {
                message = e.Message;
            }

            Assert.AreEqual("Response did not contain Content-Type: application/json.", message);
        }

        [Test]
        public void UploadFailureApiMessage()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            string errorMessage = "Api error message.";
            string errorMessageJson = $"{{'errorMsg': '{errorMessage}'}}";
            response.Content = new StringContent(errorMessageJson, Encoding.UTF8, "application/json");

            thHttpClientMock.Setup(thcm => thcm.PostText("upload", uploadbadfile, It.IsAny<HttpClient>())).Returns(response);

            string message = null;

            try
            {
                thAPiClient.Upload(uploadbadfile);
            }
            catch (THApiClientException e)
            {
                message = e.Message;
            }

            Assert.AreEqual(errorMessage, message);
        }

        [Test]
        public void UploadSuccess()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string fileID = "fileidguid";
            string fileIDJson = $"{{'fileID': '{fileID}'}}";
            response.Content = new StringContent(fileIDJson, Encoding.UTF8, "application/json");

            thHttpClientMock.Setup(thcm => thcm.PostText("upload", uploadgoodfile, It.IsAny<HttpClient>())).Returns(response);

            var responseFileID = thAPiClient.Upload(uploadgoodfile);
            Assert.AreEqual(fileID, responseFileID);
        }

        [Test]
        public void ConvertSuccess()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string settings = "settingsFileIDGUID";
            string part = "partFileIDGUID";
            string convertJson = $"{{\"settingsFileID\":\"{settings}\",\"partFileID\":\"{part}\"}}";

            string fileID = "fileidguid";
            string fileIDJson = $"{{'convertedPartFileID': '{fileID}'}}";
            response.Content = new StringContent(fileIDJson, Encoding.UTF8, "application/json");

            thHttpClientMock.Setup(thcm => thcm.PostJson("convert", It.IsAny<string>(),
                It.IsAny<HttpClient>())).Returns(response);

            var responseFileID = thAPiClient.Convert(settings, part);

            thHttpClientMock.Verify(thcm => thcm.PostJson("convert", convertJson, It.IsAny<HttpClient>()), Times.Once);
            Assert.AreEqual(fileID, responseFileID);
        }

        [Test]
        public void DownloadSuccess()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string convertedFileID = "partFileIDGUID";
            string downloadJson = $"{{\"fileID\":\"{convertedFileID}\"}}";

            string convertedFileContent = "converted part file";
            response.Content = new StringContent(convertedFileContent);

            thHttpClientMock.Setup(thcm => thcm.PostJson("download", It.IsAny<string>(),
                It.IsAny<HttpClient>())).Returns(response);

            var responseContent = thAPiClient.Download(convertedFileID);

            thHttpClientMock.Verify(thcm => thcm.PostJson("download", downloadJson, It.IsAny<HttpClient>()), Times.Once);
            Assert.AreEqual(convertedFileContent, responseContent);
        }
    }
}
